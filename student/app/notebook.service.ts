import { Injectable, OnInit } from '@angular/core';
import {Subject} from 'rxjs/Subject';
import {Subscription} from 'rxjs/Subscription';
import {Observable} from 'rxjs/Observable';

import {Notebook} from "./notebook";
import {CourseListService} from "./course-list.service";
import {DB_SOURCE_ENUM} from "./db-source";
import {NotebookGalleryService} from "./notebook-gallery.service";
import {CourseContent} from "./course-content";
var base64 = require("../js/base64-converter.js");
let moment = require('../js/moment.min.js');

/**
 * Notebook Service:
 * Contains notebook details and manage page switching
 * to save content on local storage
 */
@Injectable()
export class NotebookService {
    /**
     * Private members of the service
     */
    private currentDbSource:DB_SOURCE_ENUM = DB_SOURCE_ENUM.Localstorage;

    /**
     * Subscription to external services
     */
    private changeBackgroundSubscription:Subscription;
    private dbSourceSubscription:Subscription;
    private galleryServiceSubscription:Subscription;

    /**
     * Service listeners for externals components
     * @type {Subject<Notebook>}
     */
    private notebookSubject = new Subject<Notebook>();
    notebookListener$ = this.notebookSubject.asObservable();
    
    constructor(private courseListService:CourseListService,
                private notebookGalleryService:NotebookGalleryService) {
        this.dbSourceSubscription = courseListService.changeDbSource$
            .subscribe(
                source => {
                    this.currentDbSource = source;
                }
            );
    }

    /**
     * Save current opened canvas before switching to another
     */

    saveCurrentCanvas(courseContent: CourseContent) {
        base64.convertFileToDataURLviaFileReader(courseContent, this.base64Response);
            /*
            console.debug("[saveCurrentCanvas] Save current canvas with imgSrc " + imgSrc);
            var source;
            if (typeof imgSrc === typeof "")
                source = imgSrc;
            else
                source = imgSrc.src;
            //imgSrc = imgSrc.src || imgSrc;
            console.log(imgSrc);

            let localContent = JSON.parse(localStorage.getItem("course_123456"));
            let res:CourseContent;
            //console.log(localContent.)
            if (localContent.content.find(content => content.name == imgSrc.name))
                console.log("GOT MATCH! NO NEED TO SAVE");
            else
                base64.convertFileToDataURLviaFileReader(imgSrc, this.base64Response);
                */
    }

    /**
     * Save current opened canvas before switching to another
     */
    saveCurrentCanvasAsCanvas(canvas: HTMLCanvasElement, courseContent: CourseContent) {
        this.base64Response(canvas.toDataURL(), courseContent);
    }

    base64Response(res: any, courseContent: CourseContent) {
        console.debug("[base64Response] Successfully convert to base64");
        let courseKey = "course_" + courseContent.course_id + "_" + courseContent.date ;
        console.debug("[base64Response] Notebook will be save with " + courseKey + " key");
        console.log(courseContent);
        let localContent = JSON.parse(localStorage.getItem(courseKey));
        console.debug("Local content is:");
        console.log(localContent);
        if(localContent === null) {
            localContent = [];
            console.debug("[base64Response] Notebook doesn't exist, now creating it");
            console.debug("Local content is:");
            console.log(localContent);
            localContent.push(
            {
                "course_id": courseContent.course_id,
                "date": courseContent.date,
                "name": courseContent.name,
                "image": res,
                "timestamp": courseContent.timestamp
            });
            console.debug("Local content is:");
            console.log(localContent);
            localStorage.setItem(courseKey,JSON.stringify(localContent));
            console.debug("Local content is:");
            console.log(JSON.parse(localStorage.getItem(courseKey)));
        }
        else {
            console.debug("[base64Response] Notebook already exists");
            console.debug("Local content is:");
            console.log(localContent);
            let index = localContent.indexOf(localContent.find(time => time.name === courseContent.name));
            console.log("index is " + index);
            if(index < 0) {
                console.debug("[base64Response] Page was not found. Saving new one");
                localContent.push(
                    {
                        "course_id": courseContent.course_id,
                        "date": courseContent.date,
                        "name": courseContent.name,
                        "image": res,
                        "timestamp": courseContent.timestamp
                    });
            }
            else {
                console.debug("[base64Response] Page was found. Updating image content");
                localContent[index].image = res;
            }
            console.debug("Local content is:");
            console.log(localContent);
            localStorage.setItem(courseKey,JSON.stringify(localContent));
            console.debug("[base64Response] Content for key " + courseKey + " in local storage: ");
            console.log(JSON.parse(localStorage.getItem(courseKey)));
        }
    }
}