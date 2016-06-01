import { Component, OnInit, OnDestroy } from '@angular/core';
import {Subscription} from "rxjs/Subscription";
import { ROUTER_DIRECTIVES, Router }
    from '@angular/router-deprecated';


import { CourseListService } from './course-list.service'
import {Course} from "./course";
import {CourseContent} from "./course-content";
import {Menu} from "./menu";
import {DashboardMenu} from "./dashboard-menu";
import {DashboardMenuDirective} from "./dashboard-menu.directive";
import {DB_SOURCE_ENUM} from "./db-source";
var java = require('js/bs_leftnavi.js');


@Component({
    selector: 'bc-dashboard',
    templateUrl: 'app/dashboard.component.html',
    styleUrls: ['app/dashboard.component.css',
        'styles/bootstrap.min.css',
        'styles/bootstrap-theme.min.css',
        'styles/bs_leftnavi.css'
    ],
    directives: [
        DashboardMenuDirective,
        ROUTER_DIRECTIVES
    ]
})

export class DashboardComponent implements OnInit, OnDestroy {
    menu: DashboardMenu[] =[];
    courseListSubscription: Subscription;
    private chosenCourse: Course;
    courseList: Course[] =[];
    imageList: CourseContent[] = [];
    private fullScreenImage: CourseContent;
    private initialized: boolean = false;
    private fullScreen: boolean = false;

    error: string;
    
    constructor(private courseListService: CourseListService,
                private router: Router) {
        this.courseListService.changeDbSourceEmitter(DB_SOURCE_ENUM.External);
    }
/*
    getDates(course: Course) {
        this.imageList = [];
        this.chosenCourse = course;
        this.courseListSubscription = this.courseListService.getCourseDates(course.id)
            .subscribe(
                dates => this.datesList = dates
            )
        ;
    }
*/
    getImages(course: Course, date: string) {
        if(this.chosenCourse != course) {
            this.chosenCourse = course;
            this.imageList = [];
        }

        this.courseListSubscription = this.courseListService.getImagesByCourseIdAndDate(course.id, date)
            .subscribe(
                images => {
                    console.log("got images:");
                    console.log(images);
                    this.imageList = images;
                }
            );

    }

    menuAnimation() {
        console.log("Got my try");
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
        console.log("It will open note book at date");
        this.courseListService.changeDbSourceEmitter(DB_SOURCE_ENUM.Localstorage);
        this.router.parent.navigate(['/Notebook']);
    }

    ngOnInit() {
        this.courseListSubscription = this.courseListService.getCoursesByStudent(11521)
            .subscribe(
                courses => {
                    this.courseList = courses;
                    for(let course of this.courseList) {
                        console.log("Course: " + course.name);
                        this.courseListService.getCourseDates(course.id)
                            .subscribe(
                                dates => {
                                    //this.getDatesCounter++;
                                    console.log("Get dates for " + course.name);
                                    this.menu.push(new DashboardMenu(course,dates));
                                }
                            );
                    }
                }
            );
    }


    ngOnDestroy() {
        this.courseListSubscription.unsubscribe();
    }
}