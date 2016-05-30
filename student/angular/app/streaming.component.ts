import { Component, OnDestroy, OnInit } from '@angular/core';
import {StreamingService} from "./streaming.service";
import { Subscription } from 'rxjs/Subscription';
import {Streaming} from "./streaming";

@Component({
    selector: 'bc-streaming',
    templateUrl: 'app/streaming.component.html',
    styleUrls: ['app/streaming.component.css']
})

export class StreamingComponent implements OnInit {
    private streaming: Streaming;
    private streamingSubscription: Subscription;
    
    constructor(private streamingService: StreamingService) {
        this.streamingSubscription = this.streamingService.streamingListener$.subscribe(
            streaming => {
                this.streaming = streaming;
                console.log("streaming component got new streaming");
                console.log(streaming);
            }
        )
    }

    ngOnInit() {
        this.streaming = this.streamingService.getStreaming();
    }

}