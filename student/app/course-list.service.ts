import { Injectable, Directive } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {CourseContent} from './course-content';
import {Course} from "./course";
import {DB_SOURCE_ENUM} from "./db-source";

import {
    OPTION,
    WEBSERVICE_COURSE_LIST_GET_DATES_BY_COURSE_ID_PATH,
    WEBSERVICE_COURSE_LIST_GET_IMAGES_BY_DATE_PATH, WEBSERVICE_COURSE_LIST_GET_COURSE_BY_USER_ID_PATH
} from './constants';

@Injectable()
export class CourseListService {
    private courseId: number;
    private dbSource: DB_SOURCE_ENUM = DB_SOURCE_ENUM.Localstorage;
    private getCoursesByStudentIdUrl = WEBSERVICE_COURSE_LIST_GET_COURSE_BY_USER_ID_PATH;
    private getImagesByDateUrl = WEBSERVICE_COURSE_LIST_GET_IMAGES_BY_DATE_PATH;
    private getDateByCourseIdUrl = WEBSERVICE_COURSE_LIST_GET_DATES_BY_COURSE_ID_PATH;

    private options = OPTION;
    private imgBackground = new Subject<CourseContent>();
    private dbSourceSubject = new Subject<DB_SOURCE_ENUM>();

    constructor(private http: Http) {
    }

    changeImageBackground$ = this.imgBackground.asObservable();
    changeDbSource$ = this.dbSourceSubject.asObservable();
    
    changedCanvasBackgroundEmitter(imgBackground: CourseContent) {
        this.imgBackground.next(imgBackground);
    }
    
    getDbSource() {
        return this.dbSource;
    }
    
    changeDbSourceEmitter(source: DB_SOURCE_ENUM) {
        console.log("CourseListService now emitter: " + source);
        if(this.dbSource != source) {
            this.dbSource = source;
            this.dbSourceSubject.next(this.dbSource);
        }
    }

    getCoursesByUserId(userId: number) : Observable<Course[]> {
        return this.http.post(this.getCoursesByStudentIdUrl,
            JSON.stringify({
                "user_id": userId
            }),this.options)
            .map(this.extractCoursesListData)
            .catch(this.handleError);
    }
    
    private extractCoursesListData(res: Response) {
        let courses: Course[] = [];
        let body = res.json();
        for (var course of body) {
            courses.push(new Course(course.course_id, course.course_name,course.streaming));
        }
        console.log(courses);
        return courses || [];
    }
    
    getImagesByCourseIdAndDate(courseId: number, date: string) : Observable<CourseContent[]> {
        if(this.dbSource === DB_SOURCE_ENUM.External) {
            return this.http.post(this.getImagesByDateUrl, JSON.stringify({
                "course_id": courseId,
                "date": date
            }), this.options)
                .map(this.extractImageListData)
                .catch(this.handleError);
        }
        if(this.dbSource === DB_SOURCE_ENUM.Localstorage) {
            this.courseId = courseId;
            return this.http.get('#',null)
                .map(this.extractLocalStorageData);
        }
    }

    private extractLocalStorageData() {
        let localContent: any;
        let courses: CourseContent[] = [];
        localContent = JSON.parse(localStorage.getItem("course_123456"));
        for(var image of localContent.content) {
            courses.push(new CourseContent(image.id,image.name,image.imgSource));
        }
        return courses || [];
    }

    private extractImageListData(res: Response) {
        let images: CourseContent[]=[];
        let counter:number = 0;
        let body = res.json();
        for(var image of body) {
            console.log("image: ");
            console.log(image);
            images.push(new CourseContent(counter++,image.filename,image.src));
        }
        return images || { };
    }

    getCourseDates(course: number) : Observable<string[]> {
        console.debug("[getCoursesDates] Get date dates for course " + course);
        return this.http.post(this.getDateByCourseIdUrl,JSON.stringify({
            "course_id": course
        }),this.options)
            .map(this.extractCourseDatesData)
            .catch(this.handleError);
    }

    private extractCourseDatesData(res: Response) {
        return res.json() || [];
    }
    
    private handleError(error:any) {
        let errMsg = (error.message) ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
        console.error(errMsg);
        return Observable.throw(errMsg);
    }
}