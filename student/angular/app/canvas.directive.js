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
    function CanvasDirective(el, _navElementService) {
        var _this = this;
        this._navElementService = _navElementService;
        this.el = el.nativeElement;
        this.draw = false;
        this.canvas = this.el.getContext("2d");
        _navElementService.changeSelected.subscribe(function (navElement) {
            _this.currentNavElement = navElement;
            console.log("Catch it on canvas");
        });
    }
    CanvasDirective.prototype.onMouseEnter = function () {
        this.highlight("yellow");
    };
    CanvasDirective.prototype.onMouseLeave = function () {
        this.highlight(null);
    };
    CanvasDirective.prototype.onClick = function (event) {
        this.canvasAction(event.clientX, event.clientY);
        this.draw = true;
    };
    CanvasDirective.prototype.onUnclick = function () {
        this.draw = false;
        this.highlight(null);
    };
    CanvasDirective.prototype.highlight = function (color) {
        this.el.style.backgroundColor = color;
    };
    CanvasDirective.prototype.canvasAction = function (eventX, eventY) {
        console.log("Current selected is ");
        console.log(this._navElementService.getSelectedNavElement());
        if (this._navElementService.getSelectedNavElement().name === "circle") {
            this.canvas.fillStyle = "red";
            this.canvas.arc(eventX, eventY, 50, 0, Math.PI * 2);
        }
    };
    CanvasDirective.prototype.ngOnInit = function () {
        this.navElements = this._navElementService.getMenuNavElements();
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