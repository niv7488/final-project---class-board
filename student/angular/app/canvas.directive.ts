/*import { Directive, ElementRef, OnInit } from '@angular/core';

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
}*/

import {ElementRef,Directive,OnDestroy} from '@angular/core';

import {NavElementService} from './nav-element.service';
import { Subscription } from 'rxjs/Subscription';
import {NavElement} from "./nav-element";


@Directive({
    selector: '[myCanvas]',
    host: {
        '(mouseenter)': 'onMouseEnter()',
        '(mouseleave)': 'onMouseLeave()',
        '(mousedown)' : 'onClick($event)',
        '(mouseup)' : 'onUnclick()'
    }
})

export class CanvasDirective implements OnDestroy {
    subscription: Subscription;
    currentNavElement: NavElement;
    canvas:CanvasRenderingContext2D;
    private el:HTMLElement;

    constructor(el: ElementRef, private navElementService: NavElementService) {
        this.subscription = navElementService.changedSelected$.subscribe(
            navElement => {
                console.log("Canvas got it!");
                this.currentNavElement = navElement;
                if(navElement.name === "eraser")
                    this.makeAction(0,0);
            }
        );
        this.el = el.nativeElement;
        this.canvas = (<HTMLCanvasElement>this.el).getContext('2d');
    }

    onMouseEnter() {

    }
    onMouseLeave() {

    }

    onClick(event: any) {
        console.log("Click on canvas");
        this.makeAction(event.clientX, event.clientY)
    }

    onUnclick() {
        //this.draw = false;
        //this.highlight(null);
    }

    private makeAction(x: number, y: number) {
        if(this.currentNavElement.name === "circle") {
            this.canvas.fillStyle = "red";
            this.canvas.arc(x, y, 10, 0, Math.PI * 2);
            this.canvas.fill();
        }
        if(this.currentNavElement.name === "eraser") {
            this.canvas.clearRect(0,0,400,400);
        }
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}
