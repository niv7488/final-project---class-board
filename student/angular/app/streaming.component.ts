// import { Component, OnDestroy, OnInit } from '@angular/core';
// import {StreamingService} from "./streaming.service";
// import { Subscription } from 'rxjs/Subscription';
// import {Streaming} from "./streaming";
// import {Course} from "./course";
//
// @Component({
//     selector: 'bc-streaming',
//     templateUrl: 'app/streaming.component.html',
//     styleUrls: ['app/streaming.component.css']
// })
//
// export class StreamingComponent implements OnInit {
//     private streaming: Streaming = new Streaming();
//     private streamingSubscription: Subscription;
//   
//     constructor(private streamingService: StreamingService) {
//         this.streamingSubscription = this.streamingService.streamingListener$.subscribe(
//             streaming => {
//                 console.log("streaming component got new streaming");
//                 console.log(streaming);
//                 this.streaming = streaming;
//             }
//         )
//     }
//
//     checkStreamingAvailability() {
//         console.log("Check streaming availability");
//         //this.streamingService.getStreamingChanel(new Course(123456,""));
//         this.streamingService.getStreamingChanel(new Course(123456,""));
//     }
//
//     ngOnInit() {
//         //this.streaming = this.streamingService.getStreaming();
//     }
//
// }