import { Directive, Output, EventEmitter } from '@angular/core';

@Directive({
    selector: '[reload]'
})
    
export class AutoReloadDirective {
    @Output() reload = new EventEmitter();
    constructor() {
        setInterval(() => this.reload.emit("event"), 10000);
    }
}