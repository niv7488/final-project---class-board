import {ElementRef,Directive,OnDestroy} from '@angular/core';
import { Subscription } from 'rxjs/Subscription';

import {NavElementService} from './nav-element.service';
import {NavElement} from "./nav-element";
import {CourseListService} from "./course-list.service";


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
    navSubscription: Subscription;
    courseListSubscribtion: Subscription;
    currentNavElement: NavElement;
    canvas:CanvasRenderingContext2D;
    private el:HTMLElement;

    constructor(el: ElementRef, private navElementService: NavElementService,
        private courseListService: CourseListService) {
        this.navSubscription = navElementService.changedSelected$.subscribe(
            navElement => {
                console.log("Canvas got it!");
                this.currentNavElement = navElement;
                if(navElement.name === "eraser")
                    this.makeAction(0,0);
            }
        );
        this.courseListSubscribtion = courseListService.changeImageBackground$.subscribe(
            imgSrc => {
                console.log("Canvas directive got it! " + imgSrc);
                this.changeBackgroundHandler(imgSrc);
            }
        );
        this.el = el.nativeElement;
        this.canvas = (<HTMLCanvasElement>this.el).getContext('2d');
    }

    changeBackgroundHandler(imgSrc: any) {
        //var background = new Image();
        //background.src = imgSrc;
        //background.onload = (() => this.changeBackground(background));
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
        this.navSubscription.unsubscribe();
        this.courseListSubscribtion.unsubscribe();
    }
}
