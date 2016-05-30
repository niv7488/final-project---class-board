import { Injectable } from '@angular/core';
import {Subject} from 'rxjs/Subject';
import {Subscription} from 'rxjs/Subscription';
import {Observable} from 'rxjs/Observable';

import {Notebook} from "./notebook";
import {CourseListService} from "./course-list.service";
import {DB_SOURCE_ENUM} from "./db-source";
import {NotebookGalleryService} from "./notebook-gallery.service";

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
    private notebook: Notebook;
    private currentDbSource: DB_SOURCE_ENUM = DB_SOURCE_ENUM.Localstorage;

    /**
     * Subscription to external services
     */
    private changeBackgroundSubscription: Subscription;
    private dbSourceSubscription: Subscription;
    private galleryServiceSubscription: Subscription;

    /**
     * Service listeners for externals components
     * @type {Subject<Notebook>}
     */
    private notebookSubject = new Subject<Notebook>();
    notebookListener$ = this.notebookSubject.asObservable();

    constructor(private courseListService:CourseListService,
                private galleryService: NotebookGalleryService) {
        this.changeBackgroundSubscription = courseListService.changeImageBackground$
            .subscribe(
                () => {
                    this.saveCurrentCanvas();
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
    saveCurrentCanvas() {
        //TODO
    }

    
    
    
}