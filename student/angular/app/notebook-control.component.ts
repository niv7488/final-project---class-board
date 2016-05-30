import { Component, OnInit, OnDestroy } from '@angular/core';
import {Subscription} from 'rxjs/Subscription';
import { ROUTER_DIRECTIVES, Router }
    from '@angular/router-deprecated';

import { NavElement } from './nav-element';
import { NavElementService } from './nav-element.service';
import {CanvasComponent} from "./canvas.component";
import {NotebookGalleryComponent} from "./notebook-gallery.component";
import {StreamingComponent} from "./streaming.component";
import {StreamingService} from "./streaming.service";
import {NotebookGalleryService} from "./notebook-gallery.service";
import {NotebookService} from "./notebook.service";

@Component({
    selector: 'bc-notebook-control-component',
    templateUrl: 'app/notebook-control.component.html',
    styleUrls: ['app/nav-element.component.css'],
    providers: [
        NavElementService,
        StreamingService,
        NotebookGalleryService
    ],
    directives: [
        CanvasComponent,
        NotebookGalleryComponent, 
        StreamingComponent,
        ROUTER_DIRECTIVES
    ]
})

export class NotebookControlComponent implements OnInit, OnDestroy{
    currentNavElement: NavElement;
    private navElements: NavElement[] = [];
    private navElementSubscription: Subscription;
    //private notebookServiceSubscription: Subscription;
    
    constructor(private navElementService: NavElementService,
                private streamingService: StreamingService,
                private galleryService: NotebookGalleryService,
                private notebookService: NotebookService,
                private router: Router) {
        this.navElementSubscription = navElementService.changedSelected$.subscribe(
            navElement => {
                this.currentNavElement = navElement;
                console.log("GotIt from control " + this.currentNavElement.name);
            }
        );
    }
    
    clickAnotherNavElement(navElement: NavElement) {
        switch (navElement.name) {
            case "menu":
                console.log("HERE");
                this.router.parent.navigate(['/Dashboard']);
                break;
            case "gallery":
                this.galleryService.changeOpenEmitter();
                break;
            case "board":
                this.streamingService.openStreamingEmitter();
                break;
        }
        this.navElementService.changedSelectedEmitter(navElement);
    }

    ngOnInit() {
        this.navElements = this.navElementService.getMenuNavElements();
    }

    ngOnDestroy() {
        this.navElementSubscription.unsubscribe();
    }
}

