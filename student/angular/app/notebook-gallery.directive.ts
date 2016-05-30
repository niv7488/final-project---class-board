import { Directive, Output, EventEmitter } from '@angular/core';

@Directive({
    selector: '[reloadList]'
})
    
export class NotebookGalleryDirective {
    @Output() reload = new EventEmitter();
    constructor() {
        setInterval(() => this.reload.emit("event"), 60000);
    }
}