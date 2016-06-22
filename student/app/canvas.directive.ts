import {ElementRef,Directive,OnDestroy, OnInit} from '@angular/core';
import { Subscription } from 'rxjs/Subscription';

import {NavElementService} from './nav-element.service';
import {NavElement} from "./nav-element";
import {CourseListService} from "./course-list.service";
import {NotebookService} from './notebook.service';
import {CourseContent} from "./course-content";

let moment = require('../js/moment.min.js');

@Directive({
    selector: '[myCanvas]',
    host: {
        '(mouseenter)': 'onMouseEnter()',
        '(mouseleave)': 'onMouseLeave()',
        '(mousedown)' : 'onClick($event)',
        '(mouseup)' : 'onUnclick($event)',
        '(mousemove)' : 'onPaint($event)'
    }
})

export class CanvasDirective implements OnInit, OnDestroy {
    navSubscription: Subscription;
    courseListSubscription: Subscription;
    currentNavElement: NavElement;
    canvas:CanvasRenderingContext2D;
    private isModified: boolean = false;
    private el:HTMLElement;
    private isPaintMode : boolean = false;
    private currentCourseContent: CourseContent;

    constructor(el: ElementRef, 
                private navElementService: NavElementService,
                private courseListService: CourseListService,
                private notebookService: NotebookService)
    {
        this.el = el.nativeElement;
        this.canvas = (<HTMLCanvasElement>this.el).getContext('2d');
    }

    changeBackgroundHandler(imgSrc: any) {
        console.debug("[changeBackgroundHandler] Change background image");
        this.canvas.drawImage(imgSrc,0,0,imgSrc.width,imgSrc.height,0,0,this.canvas.canvas.width,this.canvas.canvas.height);
    }

    changeBackground(imgSrc: any)
    {
        this.canvas.drawImage(imgSrc,0,0,imgSrc.width,imgSrc.height,0,0,this.canvas.canvas.width,this.canvas.canvas.height);
    }

    onMouseEnter() {

    }
    onMouseLeave() {

    }

    onClick(event: any) {
        console.log("Click on canvas");
        this.makeAction(event.x, event.y)
    }

    onUnclick() {
        this.isPaintMode = false;
        //this.el.removeEventListener('mousemove', this.onPaint, false);
    }

    private makeAction(x: number, y: number) {
        this.canvas.beginPath();
        if(!this.isModified) {
            if (["circle", "pen", "text"].find(navElement => navElement === this.currentNavElement.name)) {
                this.isModified = true;
            }
        }
        switch (this.currentNavElement.name) {
            case "circle":
                this.canvas.fillStyle = "red";
                this.canvas.arc(x, y, 10, 0, Math.PI * 2);
                this.canvas.fill();
                this.canvas.save();
                break;
            case "eraser":
                this.canvas.clearRect(0,0,400,400);
                break;
            case "pen":
                this.isPaintMode = true;
                this.canvas.lineWidth = 3;
                this.canvas.lineJoin = 'round';
                this.canvas.lineCap = 'round';
                this.canvas.strokeStyle = '#000';
                this.canvas.moveTo(x,y-43);
                break;
            case "text":
                this.isPaintMode = !this.isPaintMode;
                if(this.isPaintMode) {

                }
                else {

                }
                break;
            case "undo":
                this.canvas.restore();
        }
        this.canvas.closePath();
    }

    onPaint(event: any) {
        if(this.isPaintMode) {
            this.canvas.lineTo(event.x, event.y-43);
            this.canvas.stroke();
        }
    }

    ngOnInit() {
        this.navSubscription = this.navElementService.changedSelected$.subscribe(
            navElement => {
                console.debug("Canvas got it!");
                this.currentNavElement = navElement;
                if(navElement.name === "eraser")
                    this.makeAction(0,0);
            }
        );
        this.courseListSubscription = this.courseListService.changeImageBackground$.subscribe(
            courseContent => {
                console.log("Canvas directive got it! current course content is:");
                console.log(this.currentCourseContent);
                if("undefined" !== typeof this.currentCourseContent) {
                    if(this.isModified) {
                        console.log("This is not the first background change");
                        this.notebookService.saveCurrentCanvasAsCanvas(<HTMLCanvasElement>this.el, this.currentCourseContent);
                        this.isModified = false;
                    }
                    else {
                        console.debug("No modification were made on canvas, no need to save it");
                    }
                }
                this.currentCourseContent = courseContent;
                this.changeBackgroundHandler(courseContent.imgSrc);
            }
        );
    }

    ngOnDestroy() {
        this.navSubscription.unsubscribe();
        this.courseListSubscription.unsubscribe();
    }
}
