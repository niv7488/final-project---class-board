import { Component, OnInit, OnDestroy } from '@angular/core';
import { CourseListService } from './course-list.service'
import {Subscription} from "rxjs/Subscription";
import {Course} from "./course";
import {CourseContent} from "./course-content";

@Component({
    selector: 'bc-dashboard',
    templateUrl: 'app/dashboard.component.html',
    styleUrls: ['app/dashboard.component.css']
})

export class DashboardComponent implements OnInit, OnDestroy {
    courseListSubscription: Subscription;
    chosenCourse: Course;
    chosenDate: string;
    courseList: Course[] =[];
    datesList: string[] = [];
    imageList: CourseContent[] = [];
    error: string;
    
    constructor(private courseListService: CourseListService) {
        //this.courseListSubscription = courseListService.
    }

    getDates(course: Course) {
        this.imageList = [];
        this.chosenCourse = course;
        this.courseListSubscription = this.courseListService.getCourseDates(course.id)
            .subscribe(
                dates => this.datesList = dates
            )
        ;
    }

    getImages(date: string) {
        this.chosenDate = date;
        this.courseListSubscription = this.courseListService.getImagesByCourseIdAndDate(this.chosenCourse.id, this.chosenDate)
            .subscribe(
                images => this.imageList = images
            );
    }
    
    ngOnInit() {
        this.courseListSubscription = this.courseListService.getCoursesByStudent(1123)
            .subscribe(
                courses => this.courseList = courses,
                error => this.error = error
            );
    }
    
    ngOnDestroy() {
        this.courseListSubscription.unsubscribe();
    }
}