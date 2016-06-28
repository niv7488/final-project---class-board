import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import {Http, Response } from '@angular/http';
import {Observable} from 'rxjs/Rx';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/operator/merge';


import {NotebookGalleryService} from "./notebook-gallery.service";
import {CourseContent} from './course-content';
import {Course} from "./course";
import {DB_SOURCE_ENUM} from "./db-source";
import {
    OPTION,
    WEBSERVICE_COURSE_LIST_GET_DATES_BY_COURSE_ID_PATH,
    WEBSERVICE_COURSE_LIST_GET_IMAGES_BY_DATE_PATH, 
    WEBSERVICE_COURSE_LIST_GET_COURSE_BY_USER_ID_PATH
} from './constants';


@Injectable()
export class CourseListService {
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
        console.log("[changedCanvasBackgroundEmitter] Emitting new image");
        this.imgBackground.next(imgBackground);
     }
    
    getDbSource() {
        return this.dbSource;
    }
    
    changeDbSourceEmitter(source: DB_SOURCE_ENUM) {
        if(this.dbSource != source) {
            console.debug("[changeDbSourceEmitter] CourseListService now emitter: " + source);
            this.dbSource = source;
            this.dbSourceSubject.next(this.dbSource);
        }
        else
            console.debug("[changeDbSourceEmitter] Already base on "+ source +" DB, no need to reload");
    }

    getCoursesByUserId(userId: number) : Observable<Course[]> {
        console.debug("[getCoursesByUserId] Get courses for user id " + userId);
        return this.http.post(this.getCoursesByStudentIdUrl,
            JSON.stringify({
                "user_id": userId
            }),this.options)
            .map(this.extractCoursesListData)
            .catch(this.handleError);
    }
    
    private extractCoursesListData(res: Response) {
        console.debug("[extractCoursesListData] Successfully got courses list");
        console.log(res.json());
        let courses: Course[] = [];
        let body = res.json();
        for (var course of body) {
            courses.push(new Course(course.course_id, course.course_name,course.streaming));
        }
        console.log(courses);
        return courses || [];
    }
    
    getImagesByCourseIdAndDate(courseId: number, date: string, db: DB_SOURCE_ENUM = this.dbSource) : Observable<CourseContent[]> {
        console.debug("[getImagesByCourseIdAndDate] Get course content for course " + courseId + " at date " + date +
            " from source " + DB_SOURCE_ENUM[this.dbSource]);
        if(db === DB_SOURCE_ENUM.External) {
            return this.http.post(this.getImagesByDateUrl, JSON.stringify({
                "course_id": courseId,
                "date": date
            }), this.options)
                .map(function(res:Response) {
                    console.debug("[extractImageListData] Extract image list data received");
                    console.log(res.json());
                    let images: CourseContent[]=[];
                    let body = res.json();
                    for(var image of body) {
                        let timestamp = image.s_timestamp || "";
                        images.push(new CourseContent(courseId,date,image.filename,image.src,timestamp,db));
                    }
                    return images || { };
                })
                .catch(this.handleError);
        }
        if(db === DB_SOURCE_ENUM.Localstorage) {
            let courseKey = "course_" + courseId + "_" + date;
            return Observable.create(function (observer) {
                let localContent:any;
                let courses:CourseContent[] = [];
                localContent = JSON.parse(localStorage.getItem(courseKey));
                console.debug("[extractLocalStorageData] Got local storage " + courseKey + " content:");
                console.log(localContent);
                if (localContent !== null) {
                    for (var image of localContent) {
                        courses.push(new CourseContent(image.course_id, image.date, image.name, image.image, image.timestamp,db));
                    }
                }
                console.debug("[extractLocalStorageData] Return course contents:");
                observer.next(courses);
                observer.complete();
            });
        }
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
        console.debug("[extractCourseDatesData] Extract dates data:");
        console.log(res.json());
        return res.json() || [];
    }
    
    private handleError(error:any) {
        let errMsg = (error.message) ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
        console.error(errMsg);
        return Observable.throw(errMsg);
    }
}