import {Directive, Output, EventEmitter} from '@angular/core';

@Directive({
    selector: '[menuanimation]'
})

export class DashboardMenuDirective {
    @Output() reload = new EventEmitter();
    constructor() {
        console.log("Emitted!");
        setInterval(() => this.reload.emit("event"));
    }
}