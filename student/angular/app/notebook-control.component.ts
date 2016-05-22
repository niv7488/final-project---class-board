import { Component, OnInit } from '@angular/core';

import { NavElement } from './nav-element';
import { NavElementService } from './nav-element.service';

import {CanvasComponent} from "./canvas.component";
import {CanvasDirective} from "./canvas.directive";

@Component({
    selector: 'bc-notebook-control-component',
    templateUrl: 'app/notebook-control.component.html',
    styleUrls: ['app/nav-element.component.css'],
    providers: [ NavElementService ],
    directives: [CanvasComponent]
})

export class NotebookControlComponent implements OnInit{
    currentNavElement: NavElement;
    navElements: NavElement[] = [];
    
    constructor(private navElementService: NavElementService) {
        navElementService.changedSelected$.subscribe(
            navElement => {
                this.currentNavElement = navElement;
                console.log("GotIt from control " + this.currentNavElement.name);
            }
        )
    }
    
    clickAnotherNavElement(navElement: NavElement) {
        this.navElementService.changedSelectedEmitter(navElement);
    }

    ngOnInit() {
        this.navElements = this.navElementService.getMenuNavElements();
    }
}

