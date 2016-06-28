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
        NavElementService
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
                this.getStreaming(!this.streaming.isOpen);
                break;
            default:
                this.navElementService.changedSelectedEmitter(navElement);
        }
    }

    private closeStreaming() {
        this.streaming.isOpen = false;
    }

    ngOnInit() {
        this.navElements = this.navElementService.getMenuNavElements();
        this.navElementSubscriptionInit();
        this.currentNavElement = this.navElements.find(element => element.name === "cursor");
        this.navElementService.changedSelectedEmitter(this.currentNavElement);
    }

    getImage() {
        console.log("Try to import current image");
        this.streamingService.createContentFromUrl(this.params.get('course'),this.streaming.channel+'/ScreenTask.jpg')
            .subscribe(
                res => {
                    console.debug("[getImage] Closing streaming");
                    this.closeStreaming();
                    console.debug("[getImage] Save res to localStorage");
                    console.log(res);
                    this.notebookService.saveCurrentCanvas(res);
                }
            );
        //this.notebookService.saveCurrentCanvas(this.streaming.channel+'/ScreenTask.jpg');
        this.closeStreaming();
    }

    getStreaming(openStreaming: boolean = false) {
        this.streamingService.getStreamingChannel(parseInt(this.params.get('course')))
            .subscribe(
                streaming => {
                    this.streaming.channel = streaming.channel;
                    this.streaming.isAvailable = streaming.isAvailable;
                    this.streaming.isOpen = openStreaming;
                    console.debug("[getStreaming] Successfully got streaming:");
                    console.log(this.streaming);
                },
                    error =>  this.errorMessage = <any>error
                );
    }



    ngOnDestroy() {
        this.navElementSubscription.unsubscribe();
    }
}

