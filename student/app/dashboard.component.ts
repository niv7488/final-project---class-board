import { Component, OnInit, OnDestroy } from '@angular/core';
import {Subscription} from "rxjs/Subscription";
import { Router, RouteParams }
    from '@angular/router-deprecated';

import {LoginService} from './login.service';
import { CourseListService } from './course-list.service'
import {Course} from "./course";
import {CourseContent} from "./course-content";
import {DashboardMenu} from "./dashboard-menu";
import {DashboardMenuDirective} from "./dashboard-menu.directive";
import {DB_SOURCE_ENUM} from "./db-source";
var java = require('js/bs_leftnavi.js');


@Component({
    selector: 'bc-dashboard',
    templateUrl: 'html/dashboard.component.html',
    styleUrls: ['css/dashboard.component.css',
        'styles/bootstrap.min.css',
        'styles/bootstrap-theme.min.css',
        'styles/bs_leftnavi.css'
    ],
    directives: [
        DashboardMenuDirective
    ]
})

export class DashboardComponent implements OnInit, OnDestroy {
    private isInitialized: boolean = false;
    coursesMenu: DashboardMenu[] =[];
    notebooksMenu: DashboardMenu[] =[];
    courseListSubscription: Subscription;
    private chosenCourse: Course;
    private chosenDate: string;
    courseList: Course[] =[];
    imageList: CourseContent[] = [];
    private fullScreenImage: CourseContent;
    private initialized: boolean = false;
    private fullScreen: boolean = false;

    error: string;
    
    constructor(private courseListService: CourseListService,
                private loginService: LoginService,
                private router: Router) {}

    getImages(course: Course, date: string) {
        if(this.chosenCourse != course) {
            this.chosenCourse = course;
            this.imageList = [];
        }
        this.chosenDate = date;
        this.courseListService.getImagesByCourseIdAndDate(course.id, date)
            .subscribe(
                images => {
                    console.log("got images:");
                    console.log(images);
                    this.imageList = images;
                }
            );

    }

    menuAnimation() {
        if(!this.initialized)
            java();
        this.initialized = true;
    }
    
    openFullScreen(image: CourseContent, pageDiff: number) {
        console.log(image);
        if(image != null) {
            this.fullScreenImage = image;
        }
        else {
            this.fullScreenImage = this.imageList.find(
                img => img.id == this.fullScreenImage.id+pageDiff
            ) || this.fullScreenImage;
        }
        this.fullScreen = true;
    }
    
    closeFullScreen() {
        this.fullScreen = false;
        this.fullScreenImage = null;
    }

    openNotebook() {
        console.debug("[openNotebook] It will open note book at date");
        this.courseListService.changeDbSourceEmitter(DB_SOURCE_ENUM.Localstorage);
        //this.router.parent.navigate(['/Notebook']);
        this.router.navigate(['/notebook/'+this.loginService.userLoggedIn.id+'/'+
            this.chosenCourse.id +'/'+ this.chosenDate]);
    }

    ngOnInit() {
        this.courseListService.changeDbSourceEmitter(DB_SOURCE_ENUM.External);
        console.log( this.loginService.userLoggedIn);
        if(typeof this.loginService.userLoggedIn === "undefined") {
            console.log('[ngOnInit] No current user connected');
            this.router.navigate(['Login']);
        }
        else {
            this.isInitialized = true;
            if(this.loginService.userLoggedIn.courses !== []) {
                this.courseListService.getCoursesByUserId(this.loginService.userLoggedIn.id)
                    .subscribe(res => {
                        this.loginService.userLoggedIn.courses = res;
                        this.initializeMenu();
                    });
            }
            else
                this.initializeMenu();
        }

    }

    private initializeMenu() {
        this.courseList = this.loginService.userLoggedIn.courses;
        for (let course of this.courseList) {
            console.log("Course: " + course.name);
            this.courseListService.getCourseDates(course.id)
                .subscribe(
                    dates => {
                        console.log("Get dates for " + course.name);
                        this.coursesMenu.push(new DashboardMenu(course, dates));
                    }
                );
        }
    }


    ngOnDestroy() {
    }
}