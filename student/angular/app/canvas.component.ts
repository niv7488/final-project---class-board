/*import { Component } from '@angular/core';

import { NavElementService } from './nav-element.service';
import { CanvasDirective } from './canvas.directive';

@Component({
    selector: 'bc-canvas',
    templateUrl: 'app/canvas.component.html',
    styleUrls: ['app/canvas.component.css'],
    providers: [NavElementService],
    directives: [ CanvasDirective ]
})

export class CanvasComponent {
    
}
*/

import { Component, OnDestroy } from '@angular/core';

import { NavElementService} from './nav-element.service';
import {NavElement} from './nav-element';
import {Subscription} from 'rxjs/Subscription';
import { CanvasDirective } from './canvas.directive';

@Component({
    selector: 'bc-canvas',
    templateUrl: 'app/canvas.component.html',
    styleUrls: ['app/canvas.component.css'],
    directives: [CanvasDirective]
})

export class CanvasComponent implements OnDestroy {
    current: NavElement;
    subscription:Subscription;

    constructor(private navElementService: NavElementService) {
        this.subscription = navElementService.changedSelected$.subscribe(
            current => {
                this.current = current;
                console.log("Canvas got it " + this.current.name);
            }
        )
    }

    ngOnDestroy() {
        this.subscription.unsubscribe();
    }
}