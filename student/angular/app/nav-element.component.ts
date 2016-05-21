import { Component, OnInit } from '@angular/core';

import { NavElement } from './nav-element';
import { NavElementService } from './nav-element.service';

@Component({
    selector: 'bc-nav-elements',
    templateUrl: 'app/nav-element.component.html',
    styleUrls: [ 'app/nav-element.component.css'],
    providers: [NavElementService]
})

export class NavElementsComponent implements OnInit {
    navElements: NavElement[];
    selectedNavElement: NavElement;

    constructor(private _navElementService: NavElementService) {
        _navElementService.changeSelected.subscribe(navElement => this.changeSelect(navElement));
    }

    onSelect(navElement: NavElement) {
        this._navElementService.selectAnother(navElement);
    }

    changeSelect(navElement: NavElement) {
        this.selectedNavElement = navElement;
    }
    
    ngOnInit() {
        this.navElements = this._navElementService.getMenuNavElements();
    }
}