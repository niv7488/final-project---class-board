/*import { Component, OnInit } from '@angular/core';

import { NavElement } from './nav-element';
import { NavElementService } from './nav-element.service';

@Component({
    selector: 'bc-nav-elements',
    templateUrl: 'app/nav-element.component.html',
    styleUrls: [ 'app/nav-element.component.css']
})

export class NavElementsComponent implements OnInit {
    navElements: NavElement[];
    selectedNavElement: NavElement;

    constructor(public _navElementService: NavElementService) {
        _navElementService.changeSelected.subscribe(navElement => this.changeSelect(navElement));
    }
/*
    onSelect(navElement: NavElement) {
        this._navElementService.selectAnother(navElement);
    }*/ /*

changeSelect(navElement: NavElement) {
    console.log("Got " + navElement.name);
    this.selectedNavElement = navElement;
}

ngOnInit() {
    this.navElements = this._navElementService.getMenuNavElements();
}
}*/ 
//# sourceMappingURL=nav-element.component.js.map