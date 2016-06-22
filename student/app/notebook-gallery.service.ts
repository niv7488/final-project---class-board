import { Injectable } from '@angular/core';
import {Subject} from 'rxjs/Subject';
import {Notebook} from "./notebook";

/**
 * Gallery Service
 * Contains boolean listener of gallery status (shown or hide)
 */
@Injectable()
export class NotebookGalleryService {
    notebook: Notebook;
    private isOpen: boolean = false;
    private isOpenSubject = new Subject<boolean>();
    
    openListener$ = this.isOpenSubject.asObservable();
    
    changeOpenEmitter() {
        this.isOpen = !this.isOpen;
        this.isOpenSubject.next(this.isOpen);
    }
}
