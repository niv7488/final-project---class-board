import {Component, OnInit, OnDestroy} from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import {RouteParams }
    from '@angular/router-deprecated';

import {CourseListService} from './course-list.service';
import {CourseContent} from './course-content';
import {DB_SOURCE_ENUM} from "./db-source";
import {AutoReloadDirective} from "./auto-reload.directive.ts";
import {Course} from "./course";
import {NotebookGalleryService} from './notebook-gallery.service';
import {Notebook} from "./notebook";

@Component({
    selector:'bc-notebook-gallery',
    templateUrl: 'html/notebook-gallery.component.html',
    styleUrls: ['css/notebook-gallery.component.css'],
    directives: [AutoReloadDirective]
})

export class NotebookGalleryComponent implements OnInit, OnDestroy {
    currentCourse: string;
    contentList: CourseContent[] = [];
    errorMessage: string;
    private isOpen: boolean = false;
    galleryServiceSubscription: Subscription;
    courseServiceSubscription: Subscription;
    backgroundChangeSubscription: Subscription;
    dbSourceSubscription: Subscription;
    dbSource: DB_SOURCE_ENUM = DB_SOURCE_ENUM.Localstorage;

    constructor(private courseListService: CourseListService,
        private notebookListService: NotebookGalleryService,
        private params: RouteParams)
    {
        this.currentCourse = params.get('course');
        this.dbSourceSubscription = this.courseListService.changeDbSource$.subscribe(
            source => {
                this.dbSource = source;
                console.log("NotebookGallery got it: " + source);
                this.importContent();
            }
        );
        this.galleryServiceSubscription = notebookListService.openListener$.subscribe(
            isOpen => {
                this.isOpen = isOpen;
                console.log("Got new gallery value");
            }
        );
    }
    
    addPageToLocalStorage() {
        
    }

    importContent() {
        console.debug("[importContent] Import content");
        this.courseServiceSubscription =
            this.courseListService.getImagesByCourseIdAndDate(parseInt(this.currentCourse), this.params.get('date'))
                .subscribe(
                    content => {
                        if((this.dbSource === DB_SOURCE_ENUM.External) && (this.contentList.length == content.length))
                            return;
                        if((this.dbSource === DB_SOURCE_ENUM.Localstorage) && (content === this.contentList))
                            return;
                        this.contentList = content;
                        console.debug("[importContent] Got new course content!");
                        console.log(this.contentList);
                    },
                    error => this.errorMessage = <any>error
                );
    }

    changeBackground(content: CourseContent) {
        console.debug("[changeBackground] Change background for course content");
        this.courseListService.changedCanvasBackgroundEmitter(content);
    }

    changeImportSource(source: string) {
        console.debug("changeBackground] Change import source to " + source);
        let temp: DB_SOURCE_ENUM;
        if (source == DB_SOURCE_ENUM[0]) {
            temp = DB_SOURCE_ENUM.External;
        }
        else {
            temp = DB_SOURCE_ENUM.Localstorage
        }
        this.courseListService.changeDbSourceEmitter(temp);
    }

    ngOnInit() {
        this.notebookListService.notebook =
            new Notebook(new Course(parseInt(this.params.get('course')),""),this.params.get('date'));
        this.importContent();
    }
    
    ngOnDestroy() {
        this.courseServiceSubscription.unsubscribe();
    }
    
    
}