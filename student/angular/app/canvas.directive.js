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
"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var nav_element_service_1 = require('./nav-element.service');
var CanvasDirective = (function () {
    function CanvasDirective(el, navElementService) {
        var _this = this;
        this.navElementService = navElementService;
        this.subscription = navElementService.changedSelected$.subscribe(function (navElement) {
            console.log("Canvas got it!");
            _this.currentNavElement = navElement;
            if (navElement.name === "eraser")
                _this.makeAction(0, 0);
        });
        this.el = el.nativeElement;
        this.canvas = this.el.getContext('2d');
    }
    CanvasDirective.prototype.onMouseEnter = function () {
    };
    CanvasDirective.prototype.onMouseLeave = function () {
    };
    CanvasDirective.prototype.onClick = function (event) {
        console.log("Click on canvas");
        this.makeAction(event.clientX, event.clientY);
    };
    CanvasDirective.prototype.onUnclick = function () {
        //this.draw = false;
        //this.highlight(null);
    };
    CanvasDirective.prototype.makeAction = function (x, y) {
        if (this.currentNavElement.name === "circle") {
            this.canvas.fillStyle = "red";
            this.canvas.arc(x, y, 10, 0, Math.PI * 2);
            this.canvas.fill();
        }
        if (this.currentNavElement.name === "eraser") {
            this.canvas.clearRect(0, 0, 400, 400);
        }
    };
    CanvasDirective.prototype.ngOnDestroy = function () {
        this.subscription.unsubscribe();
    };
    CanvasDirective = __decorate([
        core_1.Directive({
            selector: '[myCanvas]',
            host: {
                '(mouseenter)': 'onMouseEnter()',
                '(mouseleave)': 'onMouseLeave()',
                '(mousedown)': 'onClick($event)',
                '(mouseup)': 'onUnclick()'
            }
        }), 
        __metadata('design:paramtypes', [core_1.ElementRef, nav_element_service_1.NavElementService])
    ], CanvasDirective);
    return CanvasDirective;
}());
exports.CanvasDirective = CanvasDirective;
//# sourceMappingURL=canvas.directive.js.map