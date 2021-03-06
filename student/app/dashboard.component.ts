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
import {DateFormats} from "./date";

let moment = require('../js/moment.min.js');
var canvasResize = require('../js/responsive-canvas.js');

@Component({
    selector: 'bc-dashboard',
    templateUrl: 'html/dashboard.component.html',
    styleUrls: [
        'css/nav-element.component.css',
        'styles/bootstrap.min.css',
        'styles/bootstrap-theme.min.css',
        'css/dashboard.component.css'
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
    courseListSubscription: Subscription;
    private menuIsOpen: boolean = false;
    private chosenCourse: Course;
    private chosenDate: DateFormats;
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

    getImages(course: Course, date: any) {
        this.menuIsOpen = false;
        console.debug("[getImages] Get images for course " + course.id + " at date:");
        console.log(date.original);
        if(this.chosenCourse != course) {
            this.chosenCourse = course;
            this.imageList = [];
        }
        this.chosenDate = date;
        Observable.zip(
            this.courseListService.getImagesByCourseIdAndDate(course.id, date.original, DB_SOURCE_ENUM.External),
            this.courseListService.getImagesByCourseIdAndDate(course.id,date.original,DB_SOURCE_ENUM.Localstorage),
            function(res1, res2) {
                return res1.concat(res2);
            }
        )
            .subscribe(
                res => {
                    res.sort(CourseContent.sort);
                    this.allImagesList = res;
                    this.imageList = this.allImagesList.filter(image => image.dbSrc === this.dbContentToDisplay);
                    this.addFilter(this.dbContentToDisplay);
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

    openFullTry(image: CourseContent, pageDiff: number) {
        this.clickAnotherNavElement(this.navElements[0]);
        this.fullScreen = true;
        canvasResize.modifyCanvasSize();
        this.openFullScreen(image,pageDiff);
    }
    
    openFullScreen(image: CourseContent, pageDiff: number) {
        console.log("[openFullScreen] Open full screen with pageDiff " + pageDiff + " content");
        console.log(image);
        let index: number;
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
    }

    closeFullScreen() {
        console.debug("[closeFullScreen]");
        this.fullScreen = false;
        this.fullScreenImage = null;
        this.getImages(this.chosenCourse,this.chosenDate);
    }

    openNotebook() {
        console.debug("[openNotebook] It will open note book at date");
        this.courseListService.changeDbSourceEmitter(DB_SOURCE_ENUM.Localstorage);
        this.router.navigateByUrl('notebook/'+ this.chosenCourse.id+'/'+ this.chosenDate.original);
    }

    private selectCourse(course: DashboardMenu) {
        if(course.menu===this.chosenCourse)
            return;
        this.coursesMenu.find(item => item === course).isSelected = !course.isSelected;
        console.log(course);
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
    
    private menuManager() {
        this.menuIsOpen = !this.menuIsOpen;
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