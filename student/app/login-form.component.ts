import {Component, OnInit} from '@angular/core';
import {CORE_DIRECTIVES, FORM_DIRECTIVES} from '@angular/common';
import { Router } from '@angular/router-deprecated';

import {LoginService} from './login.service';
import { User } from './student';
import {Course} from "./course";
import {CourseListService} from "./course-list.service";

@Component({
    selector: 'bc-login-form',
    templateUrl: 'html/login-form.component.html',
    styleUrls: ['css/login-form.component.css'],
    directives: [CORE_DIRECTIVES, FORM_DIRECTIVES]
})

export class LoginFormComponent implements OnInit {
    private isInitialized : boolean = false;
    private data: any;
    private error: boolean = false;
    private errorMsg: string = "";
    
    constructor(private loginService: LoginService,
                private router: Router,
                private courseListService: CourseListService) {}

    onSubmit(data) {
        this.data = JSON.stringify(data,null,2);
        this.loginService.authentification(data.id, data.password)
            .subscribe(
                res => {
                    console.debug("[onSubmit] Successfully login");
                    console.debug(res);
                    localStorage.setItem("loggedIn",JSON.stringify({
                        id: data.id,
                        name: res.name,
                        permission: res.permission,
                        image: res.user_picture
                    }));
                    let courses : Course[] = [];
                    for(var course of res.CoursesList) {
                        courses.push(new Course(course.id, course.name, course.streaming));
                    }
                    this.loginService.userLoggedIn =
                        new User(data.id,res.name, res.user_picture, courses);
                    this.router.navigate(['Dashboard']);
                },
                error => {
                    console.error("[onSubmit] Got error from authentification");
                    console.error(error);
                    this.error = true;
                    this.errorMsg = "Unable to login. Please try again";
                }
            );
    }

    ngOnInit() {
        console.debug("[ngOnInit] Checking if the user is already checked in");
        if(localStorage.getItem("loggedIn") !== null) {
            var  localStorageUser = JSON.parse(localStorage.getItem("loggedIn"));
            if( localStorageUser.permission === "secretariat") {
                console.debug("[ngOnInit] Admin user was found logged in");
                this.router.navigate(['Admin']);
            }
            else {
                console.debug("[ngOnInit] User id " +  localStorageUser.id + " was found");
                var localStorageUser = JSON.parse(localStorage.getItem("loggedIn"));
                this.courseListService.getCoursesByUserId(localStorageUser.id)
                    .subscribe(
                        res => {
                            console.debug("[ngOnInit] LocalStorageUser:");
                            console.debug(localStorageUser);
                            this.loginService.userLoggedIn = new User(localStorageUser.id, localStorageUser.name, localStorageUser.image, res);
                            this.router.navigate(['Dashboard']);
                        },
                        error => {
                            console.error("[initializeUser] Unable to get courses for student " + localStorageUser.id +
                                ". Reason: " + error.message);
                        }
                    );
            }
        }
        else {
            console.debug("[ngOnInit] No student was found");
            this.isInitialized = true;
        }
    }
}