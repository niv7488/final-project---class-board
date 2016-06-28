import { Component, OnInit, OnDestroy } from '@angular/core';
import {Subscription} from "rxjs/Subscription";
import { Router, RouteParams }
    from '@angular/router-deprecated';
import {Observable} from 'rxjs/Observable';
import 'rxjs/operator/merge';
import 'rxjs/add/operator/zip';

import {LoginService} from './login.service';
import { CourseListService } from './course-list.service'
import {Course} from "./course";
import {CourseContent} from "./course-content";
import {DashboardMenu} from "./dashboard-menu";
import {DashboardMenuDirective} from "./dashboard-menu.directive";
import {DB_SOURCE_ENUM} from "./db-source";
import {CanvasComponent} from "./canvas.component";
import {CanvasDirective} from "./canvas.directive";
import { NavElement } from './nav-element';
import { NavElementService } from './nav-element.service';
import {NAV_ELEMENTS} from "./nav-element-list";


var java = require('../js/bs_leftnavi.js');
let moment = require('../js/moment.min.js');


@Component({
    selector: 'bc-dashboard',
    templateUrl: 'html/dashboard.component.html',
    styleUrls: [
        'css/nav-element.component.css',
        'css/dashboard.component.css',
        'styles/bootstrap.min.css',
        'styles/bootstrap-theme.min.css',
        'styles/bs_leftnavi.css'
    ],
    directives: [
        DashboardMenuDirective,
        CanvasComponent
    ],
    providers: [
        NavElementService
    ]
})

export class DashboardComponent implements OnInit, OnDestroy {
    private isInitialized: boolean = false;
    coursesMenu: DashboardMenu[] =[];
    notebooksMenu: DashboardMenu[] =[];
    courseListSubscription: Subscription;
    private chosenCourse: Course;
    private chosenDate: string;
    private dbContentToDisplay: DB_SOURCE_ENUM = DB_SOURCE_ENUM.External;
    courseList: Course[] =[];
    imageList: CourseContent[] = [];
    private previousBtn = true;
    private nextBtn = true;
    private allImagesList: CourseContent[] = [];
    private fullScreenImage: CourseContent;
    private initialized: boolean = false;
    private fullScreen: boolean = false;
    private navElements: NavElement[] = [];
    private navElementSubscription: Subscription;

    error: string;
    
    constructor(private courseListService: CourseListService,
                private navElementService: NavElementService,
                private loginService: LoginService,
                private router: Router) {
        this.navElements = navElementService.getSubNavElements();
    }

    getImages(course: Course, date: string) {
        console.debug("[getImages] Get images for course " + course.id + " at date:");
        console.log(date);
        console.log(moment(date,["DD/MM/YYYY"]).format("DDMMYYYY"));
        if(this.chosenCourse != course) {
            this.chosenCourse = course;
            this.imageList = [];
        }
        this.chosenDate = date;
        Observable.zip(
            this.courseListService.getImagesByCourseIdAndDate(course.id, date, DB_SOURCE_ENUM.External),
            this.courseListService.getImagesByCourseIdAndDate(course.id,date,DB_SOURCE_ENUM.Localstorage),
            function(res1, res2) {
                return res1.concat(res2);
            }
        )
            .subscribe(
                res => {
                    res.sort(CourseContent.sort);
                    this.allImagesList = res;
                    this.imageList = this.allImagesList.filter(image => image.dbSrc === this.dbContentToDisplay);
                    console.log(res);
                    console.log(this.imageList);
                },
                err => {
                    console.error(err);
                }
            );
    }

    private clickAnotherNavElement(navElement: NavElement) {
        this.navElementService.changedSelectedEmitter(navElement);
    }

    private addFilter(filter: DB_SOURCE_ENUM) {
        if(filter === DB_SOURCE_ENUM.Both)
            this.imageList = this.allImagesList;
        else
            this.imageList = this.allImagesList.filter(image => image.dbSrc === filter);
        this.dbContentToDisplay = filter;
    }

    menuAnimation() {
        if(!this.initialized)
            java();
        this.fullScreen = false;
        this.initialized = true;
    }
    
    openFullScreen(image: CourseContent, pageDiff: number) {
        if(!this.fullScreen)
            this.clickAnotherNavElement(this.navElements[0]);
        console.log("[openFullScreen] Open full screen with pageDiff " + pageDiff + " content");
        console.log(image);
        let index: number;
        this.fullScreen = true;
        index = image != null ?
            this.imageList.findIndex(
                img => img.name === image.name
            )
            : this.imageList.findIndex(
            img => img.name == this.fullScreenImage.name
        );
        index += pageDiff;
        console.log("[openFullScreen] Current index: " + index);

        if (index >= 0)
            this.fullScreenImage = this.imageList[index];
        console.log(this.fullScreenImage);
        this.previousBtn = index == 0 ? false : true;
        this.nextBtn = index == this.imageList.length-1 ? false : true;
        this.courseListService.changedCanvasBackgroundEmitter(this.fullScreenImage);

        //this.courseListService.changedCanvasBackgroundEmitter(image);


    }

    closeFullScreen() {
        console.debug("[closeFullScreen]")
        this.fullScreen = false;
        this.fullScreenImage = null;
    }

    openNotebook() {
        console.debug("[openNotebook] It will open note book at date");
        this.courseListService.changeDbSourceEmitter(DB_SOURCE_ENUM.Localstorage);
        //this.router.parent.navigate(['/Notebook']);
        this.router.navigateByUrl('notebook/'+ this.chosenCourse.id+'/'+ this.chosenDate);
    }

    ngOnInit() {

        //this.courseListService.changeDbSourceEmitter(DB_SOURCE_ENUM.Both);
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
            else {
                this.initializeMenu();
            }
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