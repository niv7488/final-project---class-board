import { Component, OnDestroy } from '@angular/core';
import {Subscription} from 'rxjs/Subscription';

import { NavElementService} from './nav-element.service';
import {NavElement} from './nav-element';
import { CanvasDirective } from './canvas.directive';
import {CourseListService} from "./course-list.service";

@Component({
    selector: 'bc-canvas',
    templateUrl: 'app/canvas.component.html',
    styleUrls: ['app/canvas.component.css'],
    directives: [CanvasDirective]
})

/**
 * Canvas Component
 * Listen to different notebook events
 * Canvas behavior of notebook
 */
export class CanvasComponent implements OnDestroy {
    current: NavElement;
    imgBackground: string;
    navSubscription:Subscription;
    notebookSubscription:Subscription;

    constructor(private navElementService: NavElementService,
        courseListService: CourseListService) {
        this.navSubscription = navElementService.changedSelected$.subscribe(
            current => {
                this.current = current;
                console.log("Canvas component got it " + this.current.name);
            }
        );
        this.notebookSubscription = courseListService.changeImageBackground$.subscribe(
            imageSrc => {
                this.imgBackground = imageSrc;
                console.log("Canvas component got it " + this.imgBackground);
            }
        );
    }

    ngOnDestroy() {
        this.navSubscription.unsubscribe();
        this.notebookSubscription.unsubscribe();
    }
}