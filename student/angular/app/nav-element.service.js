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
var nav_element_list_1 = require('./nav-element-list');
var NavElementService = (function () {
    function NavElementService() {
        this._allNavElements = nav_element_list_1.NAV_ELEMENTS;
        this.changeSelected = new core_1.EventEmitter();
    }
    NavElementService.prototype.selectAnother = function (navElement) {
        this.selectedNavElement = navElement;
        this.changeSelected.emit(navElement);
    };
    NavElementService.prototype.getSelectedNavElement = function () {
        return this.selectedNavElement;
    };
    NavElementService.prototype.getNavElements = function () {
        return nav_element_list_1.NAV_ELEMENTS;
    };
    NavElementService.prototype.getMenuNavElements = function () {
        var menuNavComponent = [];
        menuNavComponent.push(this._allNavElements.filter(function (element) { return element.name == "menu"; })[0]);
        menuNavComponent.push(this._allNavElements.filter(function (element) { return element.name == "gallery"; })[0]);
        menuNavComponent.push(this._allNavElements.filter(function (element) { return element.name == "board"; })[0]);
        menuNavComponent.push(this._allNavElements.filter(function (element) { return element.name == "cursor"; })[0]);
        menuNavComponent.push(this._allNavElements.filter(function (element) { return element.name == "pen"; })[0]);
        menuNavComponent.push(this._allNavElements.filter(function (element) { return element.name == "circle"; })[0]);
        menuNavComponent.push(this._allNavElements.filter(function (element) { return element.name == "picture"; })[0]);
        menuNavComponent.push(this._allNavElements.filter(function (element) { return element.name == "eraser"; })[0]);
        menuNavComponent.push(this._allNavElements.filter(function (element) { return element.name == "undo"; })[0]);
        menuNavComponent.push(this._allNavElements.filter(function (element) { return element.name == "redo"; })[0]);
        console.log(menuNavComponent);
        return menuNavComponent;
    };
    NavElementService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [])
    ], NavElementService);
    return NavElementService;
}());
exports.NavElementService = NavElementService;
//# sourceMappingURL=nav-element.service.js.map