import { Directive, ElementRef, OnInit } from '@angular/core';

import { NavElementService } from './nav-element.service';
import { NavElement } from './nav-element';

@Directive({
    selector: '[myCanvas]',
    host: {
        '(mouseenter)': 'onMouseEnter()',
        '(mouseleave)': 'onMouseLeave()',
        '(mousedown)' : 'onClick($event)',
        '(mouseup)' : 'onUnclick()'
    }
})

export class CanvasDirective implements OnInit {
    navElements: NavElement[];
    private el:HTMLElement;
    private draw: boolean;
    canvas:CanvasRenderingContext2D;
    currentNavElement: NavElement;

    constructor(el: ElementRef, public _navElementService: NavElementService) {
        this.el = el.nativeElement;
        this.draw = false;
        this.canvas = (<HTMLCanvasElement>this.el).getContext("2d");
        _navElementService.changeSelected.subscribe((navElement) => {
            this.currentNavElement = navElement;
            console.log("Catch it on canvas");
        });
    }

    onMouseEnter() {
        this.highlight("yellow");
    }
    onMouseLeave() {
        this.highlight(null);
    }

    onClick(event: any) {
        this.canvasAction(event.clientX, event.clientY);
        this.draw = true;
    }
    onUnclick() {
        this.draw = false;
        this.highlight(null);
    }
    private highlight(color: string) {
        this.el.style.backgroundColor = color;
    }

    private canvasAction(eventX: number, eventY: number) {
        console.log("Current selected is ");
        console.log(this._navElementService.getSelectedNavElement());
        if(this._navElementService.getSelectedNavElement().name === "circle") {
            this.canvas.fillStyle = "red";
            this.canvas.arc(eventX, eventY, 50, 0, Math.PI * 2);
        }
    }

    ngOnInit() {
        this.navElements = this._navElementService.getMenuNavElements();
    }
}
