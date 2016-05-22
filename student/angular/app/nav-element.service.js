/*import { Injectable, EventEmitter, Input, Output } from '@angular/core';

import { NavElement } from './nav-element';
import { NAV_ELEMENTS } from './nav-element-list';

@Injectable()
export class NavElementService {
    private _allNavElements = NAV_ELEMENTS;
    @Input() selectedNavElement:NavElement;
    @Output() changeSelected:EventEmitter<NavElement> = new EventEmitter();

    selectAnother(navElement:NavElement) {
        this.selectedNavElement = navElement;
        console.log("Event Emitted");
        this.changeSelected.emit(navElement);
    }

    getSelectedNavElement() {
        return this.selectedNavElement;
    }

    getNavElements() {
        return NAV_ELEMENTS;
    }

    getMenuNavElements() {
        let menuNavComponent: NavElement[] = [];
        menuNavComponent.push(this._allNavElements.filter(element => element.name == "menu")[0]);
        menuNavComponent.push(this._allNavElements.filter(element => element.name == "gallery")[0]);
        menuNavComponent.push(this._allNavElements.filter(element => element.name == "board")[0]);
        menuNavComponent.push(this._allNavElements.filter(element => element.name == "cursor")[0]);
        menuNavComponent.push(this._allNavElements.filter(element => element.name == "pen")[0]);
        menuNavComponent.push(this._allNavElements.filter(element => element.name == "circle")[0]);
        menuNavComponent.push(this._allNavElements.filter(element => element.name == "text")[0]);
        menuNavComponent.push(this._allNavElements.filter(element => element.name == "picture")[0]);
        menuNavComponent.push(this._allNavElements.filter(element => element.name == "eraser")[0]);
        menuNavComponent.push(this._allNavElements.filter(element => element.name == "undo")[0]);
        menuNavComponent.push(this._allNavElements.filter(element => element.name == "redo")[0]);
        return menuNavComponent;
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
var Subject_1 = require('rxjs/Subject');
var nav_element_list_1 = require('./nav-element-list');
var NavElementService = (function () {
    function NavElementService() {
        this.navElementChangeSource = new Subject_1.Subject();
        this.changedSelected$ = this.navElementChangeSource.asObservable();
    }
    NavElementService.prototype.changedSelectedEmitter = function (navElementCurrent) {
        console.log("Emit current " + navElementCurrent.name);
        this.navElementChangeSource.next(navElementCurrent);
    };
    NavElementService.prototype.getMenuNavElements = function () {
        var menuNavComponent = [];
        menuNavComponent.push(nav_element_list_1.NAV_ELEMENTS.filter(function (element) { return element.name == "menu"; })[0]);
        menuNavComponent.push(nav_element_list_1.NAV_ELEMENTS.filter(function (element) { return element.name == "gallery"; })[0]);
        menuNavComponent.push(nav_element_list_1.NAV_ELEMENTS.filter(function (element) { return element.name == "board"; })[0]);
        menuNavComponent.push(nav_element_list_1.NAV_ELEMENTS.filter(function (element) { return element.name == "cursor"; })[0]);
        menuNavComponent.push(nav_element_list_1.NAV_ELEMENTS.filter(function (element) { return element.name == "pen"; })[0]);
        menuNavComponent.push(nav_element_list_1.NAV_ELEMENTS.filter(function (element) { return element.name == "circle"; })[0]);
        menuNavComponent.push(nav_element_list_1.NAV_ELEMENTS.filter(function (element) { return element.name == "text"; })[0]);
        menuNavComponent.push(nav_element_list_1.NAV_ELEMENTS.filter(function (element) { return element.name == "picture"; })[0]);
        menuNavComponent.push(nav_element_list_1.NAV_ELEMENTS.filter(function (element) { return element.name == "eraser"; })[0]);
        menuNavComponent.push(nav_element_list_1.NAV_ELEMENTS.filter(function (element) { return element.name == "undo"; })[0]);
        menuNavComponent.push(nav_element_list_1.NAV_ELEMENTS.filter(function (element) { return element.name == "redo"; })[0]);
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