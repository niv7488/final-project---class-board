import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import {Http, Response, Headers, RequestOptions} from '@angular/http';
import {Observable} from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {CourseContent} from './course-content';
import {Course} from "./course";
import {DB_SOURCE_ENUM} from "./db-source";

@Injectable()
export class CourseListService {
    private dbSource: DB_SOURCE_ENUM = DB_SOURCE_ENUM.Localstorage;
    private getCoursesByStudentIdUrl = 'http://52.34.153.216:3000/studentFullCourses';
    private getImagesByDateUrl = 'http://52.34.153.216:3000/getDateImages';
    private getDateByCourseIdUrl = 'http://52.34.153.216:3000/getCoursesContentDates';

    private headers = new Headers({ 'Content-Type': 'application/json'});
    private options = new RequestOptions({ headers: this.headers});
    private imgBackground = new Subject<any>();
    private dbSourceSubject = new Subject<DB_SOURCE_ENUM>();

    constructor(private http: Http) {
    }

    changeImageBackground$ = this.imgBackground.asObservable();
    changeDbSource$ = this.dbSourceSubject.asObservable();
    
    changedCanvasBackgroundEmitter(imgBackground: CourseContent) {
        this.imgBackground.next(imgBackground.imgSrc);
    }
    
    changeDbSourceEmitter(source: DB_SOURCE_ENUM) {
        console.log("CourseListService now emitter: " + source);
        if(this.dbSource != source) {
            this.dbSource = source;
            this.dbSourceSubject.next(this.dbSource);
        }
    }

    getCoursesByStudent(studentId: number) : Observable<Course[]> {
        return this.http.post(this.getCoursesByStudentIdUrl,
            JSON.stringify({
                "student_id": studentId
            }),this.options)
            .map(this.extractCoursesListData)
            .catch(this.handleError);
    }
    
    private extractCoursesListData(res: Response) {
        let courses: Course[] = [];
        let body = res.json();
        for (var course of body) {
            let temp: Course = new Course();
            temp.id = course.course_id;
            temp.name = course.course_name;
            temp.streaming = course.streaming;
            courses.push(temp);
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
            return this.http.get('#',null)
                .map(this.extractLocalStorageData);
        }
    }

    private extractLocalStorageData() {
        let localContent: any;
        let courses: CourseContent[] = [];
        localContent = JSON.parse(localStorage.getItem("course_99999"));
        for(var image of localContent.content) {
            let temp = new CourseContent();
            temp.id = image.id;
            temp.name = image.name;
            temp.imgSrc.src = image.imgSource;
            courses.push(temp);
        }
        return courses || [];
    }

    private extractImageListData(res: Response) {
        let images: CourseContent[]=[];
        let body = res.json();
        for(var image of body) {
            let temp: CourseContent = new CourseContent();
            temp.name = image.filename;
            temp.imgSrc.src = image.src;
            images.push(temp);
        }
        return images || { };
    }

    getCourseDates(date: number) : Observable<string[]> {
        return this.http.post(this.getDateByCourseIdUrl,JSON.stringify({
            "course_id": date
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