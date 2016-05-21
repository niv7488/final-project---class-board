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
var NavElementsComponent = (function () {
    function NavElementsComponent(_navElementService) {
        var _this = this;
        this._navElementService = _navElementService;
        _navElementService.changeSelected.subscribe(function (navElement) { return _this.changeSelect(navElement); });
    }
    NavElementsComponent.prototype.onSelect = function (navElement) {
        this._navElementService.selectAnother(navElement);
    };
    NavElementsComponent.prototype.changeSelect = function (navElement) {
        this.selectedNavElement = navElement;
    };
    NavElementsComponent.prototype.ngOnInit = function () {
        this.navElements = this._navElementService.getMenuNavElements();
    };
    NavElementsComponent = __decorate([
        core_1.Component({
            selector: 'bc-nav-elements',
            templateUrl: 'app/nav-element.component.html',
            styleUrls: ['app/nav-element.component.css'],
            providers: [nav_element_service_1.NavElementService]
        }), 
        __metadata('design:paramtypes', [nav_element_service_1.NavElementService])
    ], NavElementsComponent);
    return NavElementsComponent;
}());
exports.NavElementsComponent = NavElementsComponent;
//# sourceMappingURL=nav-element.component.js.map