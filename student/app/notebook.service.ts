import { Injectable, OnInit } from '@angular/core';
import {Subject} from 'rxjs/Subject';
import {Subscription} from 'rxjs/Subscription';
import {Observable} from 'rxjs/Observable';

import {Notebook} from "./notebook";
import {CourseListService} from "./course-list.service";
import {DB_SOURCE_ENUM} from "./db-source";
import {NotebookGalleryService} from "./notebook-gallery.service";
import {CourseContent} from "./course-content";
var base64 = require("js/base64-converter.js");

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
    private notebook:Notebook;
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
                private galleryService:NotebookGalleryService) {
        this.changeBackgroundSubscription = courseListService.changeImageBackground$
            .subscribe(
                courseContent => {
                    console.log("Notebook service go it");
                    if(this.courseListService.getDbSource() == DB_SOURCE_ENUM.External)
                        this.saveCurrentCanvas(courseContent);
                    else
                        console.log("Not saving the image, already in NB");
                }
            );
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
    saveCurrentCanvas(imgSrc: any) {
        var source;
        if(typeof imgSrc === typeof "")
            source = imgSrc;
        else 
            source = imgSrc.src;
        //imgSrc = imgSrc.src || imgSrc;
        console.log(imgSrc);
        
        let localContent = JSON.parse(localStorage.getItem("course_123456"));
        let res: CourseContent;
        //console.log(localContent.)
        if(localContent.content.find(content => content.name == imgSrc.name))
            console.log("GOT MATCH! NO NEED TO SAVE");
        else
            base64.convertFileToDataURLviaFileReader(imgSrc,this.base64Response);
    }

    base64Response(res: any, imgSrc: any) {
        console.log("Successfully convert to base64");
        let localContent: any;
        localContent = JSON.parse(localStorage.getItem("course_123456"));
        console.log("Error is here");
        console.log(localContent);
        if(localContent === null) {
            localStorage.setItem('course_123456',JSON.stringify({
                "date": 31032016,
                "content": []
            }));
            localContent = JSON.parse(localStorage.getItem("course_123456"));
        }
        console.log("local storage amount: " + localContent.content.length);
        let page = localContent.content.length;
        localContent.content.push({
            "id": page,
            "name": imgSrc.name,
            "imgSource": res
        });
        localStorage.setItem('course_123456',JSON.stringify(localContent));
        console.log("All local storage for this NB");
        console.log(JSON.parse(localStorage.getItem("course_123456")));
    }
}