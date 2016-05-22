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
var canvas_component_1 = require("./canvas.component");
var NotebookControlComponent = (function () {
    function NotebookControlComponent(navElementService) {
        var _this = this;
        this.navElementService = navElementService;
        this.navElements = [];
        navElementService.changedSelected$.subscribe(function (navElement) {
            _this.currentNavElement = navElement;
            console.log("GotIt from control " + _this.currentNavElement.name);
        });
    }
    NotebookControlComponent.prototype.clickAnotherNavElement = function (navElement) {
        this.navElementService.changedSelectedEmitter(navElement);
    };
    NotebookControlComponent.prototype.ngOnInit = function () {
        this.navElements = this.navElementService.getMenuNavElements();
    };
    NotebookControlComponent = __decorate([
        core_1.Component({
            selector: 'bc-notebook-control-component',
            templateUrl: 'app/notebook-control.component.html',
            styleUrls: ['app/nav-element.component.css'],
            providers: [nav_element_service_1.NavElementService],
            directives: [canvas_component_1.CanvasComponent]
        }), 
        __metadata('design:paramtypes', [nav_element_service_1.NavElementService])
    ], NotebookControlComponent);
    return NotebookControlComponent;
}());
exports.NotebookControlComponent = NotebookControlComponent;
//# sourceMappingURL=notebook-control.component.js.map