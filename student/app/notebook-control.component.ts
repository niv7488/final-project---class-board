import { Component, OnInit, OnDestroy } from '@angular/core';
import {Subscription} from 'rxjs/Subscription';
import { ROUTER_DIRECTIVES, Router, RouteParams }
    from '@angular/router-deprecated';

import { NavElement } from './nav-element';
import { NavElementService } from './nav-element.service';
import {CanvasComponent} from "./canvas.component";
import {NotebookGalleryComponent} from "./notebook-gallery.component";
import {NotebookGalleryService} from "./notebook-gallery.service";
import {NotebookService} from "./notebook.service";
import {Streaming} from "./streaming";
import {StreamingService} from "./streaming.service";

@Component({
    selector: 'bc-notebook-control-component',
    templateUrl: 'html/notebook-control.component.html',
    styleUrls: [
        'css/nav-element.component.css',
        'css/streaming2.component.css'
    ],
    providers: [
        NavElementService,
        NotebookGalleryService
    ],
    directives: [
        CanvasComponent,
        NotebookGalleryComponent,
        ROUTER_DIRECTIVES
    ]
})

export class NotebookControlComponent implements OnInit, OnDestroy{
    currentNavElement: NavElement;
    streaming: Streaming = new Streaming;
    errorMessage: string;
    private navElements: NavElement[] = [];
    private navElementSubscription: Subscription;
    
    constructor(private streamingService: StreamingService,
                private navElementService: NavElementService,
                private galleryService: NotebookGalleryService,
                private notebookService: NotebookService,
                private router: Router,
                private params: RouteParams) {
    }

    navElementSubscriptionInit() {
        this.navElementSubscription = this.navElementService.changedSelected$.subscribe(
            navElement => {
                this.currentNavElement = navElement;
                console.log("GotIt from control " + this.currentNavElement.name);
            }
        );
    }
    
    clickAnotherNavElement(navElement: NavElement) {
        switch (navElement.name) {
            case "coursesMenu":
                console.log("HERE");
                //this.router.parent.navigate(['/Dashboard']);
                this.router.parent.navigateByUrl('/dashboard/' + this.params.get('id'));
                break;
            case "gallery":
                this.galleryService.changeOpenEmitter();
                break;
            case "board":
                this.streaming.isOpen = !this.streaming.isOpen;
                this.getStreaming();
                break;
            default:
                this.navElementService.changedSelectedEmitter(navElement);
        }
    }

    private closeStreaming() {
        this.streaming.isOpen = false;
        this.streaming.isAvailable = true;
    }

    ngOnInit() {
        this.navElements = this.navElementService.getMenuNavElements();
        this.navElementSubscriptionInit();
        this.getStreaming();
        this.currentNavElement = this.navElements.find(element => element.name === "cursor");
        this.navElementService.changedSelectedEmitter(this.currentNavElement);
    }

    getImage() {
        console.log("Try to import current image");
        this.notebookService.saveCurrentCanvas(this.streaming.channel+'/ScreenTask.jpg');
    }

    getStreaming() {
        this.streamingService.getStreamingChannel()
            .subscribe(
                channel => {
                    console.log(channel);
                    if (channel !== typeof ("")) {
                        this.streaming.isAvailable = true;
                        this.streaming.channel = channel;
                    }
                    else
                        this.streaming.channel = "";
                    console.log(this.streaming)
                },
                    error =>  this.errorMessage = <any>error
                );
    }

    
    
    ngOnDestroy() {
        this.navElementSubscription.unsubscribe();
    }
}

