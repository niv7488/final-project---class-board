import { Injectable } from '@angular/core';
import {Observable} from 'rxjs/Observable';
import {Subject} from 'rxjs/Subject';

@Injectable()
export class NotebookGalleryService {
    private isOpen: boolean = false;
    private isOpenSubject = new Subject<boolean>();
    
    openListener$ = this.isOpenSubject.asObservable();
    
    changeOpenEmitter() {
        this.isOpen = !this.isOpen;
        this.isOpenSubject.next(this.isOpen);
    }
}
