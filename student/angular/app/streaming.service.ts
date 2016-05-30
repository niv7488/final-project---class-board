import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { Subject } from 'rxjs/Subject';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Course} from "./course";
import {Streaming} from "./streaming";

/**
 * Streaming Service
 * Looking for streaming service in open notebook
 * Got listener of streaming to update externals
 * every streaming change
 */
@Injectable()
export class StreamingService {
    private getStreamingCourseChanelUrl = 'http://52.34.153.216:3000/getCourseStreaming';
    private headers = new Headers({ 'Content-Type': 'application/json'});
    private options = new RequestOptions({ headers: this.headers});
    private streaming: Streaming;
    private streamingSubject = new Subject<Streaming>();

    /**
     * External listener of streaming behavior
     * @type {Observable<Streaming>}
     */
    streamingListener$ = this.streamingSubject.asObservable();

    constructor(private http: Http) {
        this.streaming = new Streaming();
    }

    getStreamingChanel(course: Course) : Observable<any> {
        return this.http.post(this.getStreamingCourseChanelUrl, JSON.stringify({
            "course_id": course.id
        }), this.options)
            .map(this.extractStreamingChanel)
            .catch(this.handleError);
    }

    private extractStreamingChanel(res: Response) {
        let body = res.json();
        if(body.src === "") {
            this.streaming.isAvailable = false;
        }
        else {
            this.streaming.isAvailable = true;
        }
        this.streaming.channel = body.src;
        return this.streaming;
    }
    
    openStreamingEmitter() {
        this.streaming.isOpen = true;
        console.log(this.streaming);
        this.streamingSubject.next(this.streaming);
    }
    
    closeStreamingEmitter(streaming:Streaming) {
        console.log("Emitter got close");
        streaming.isOpen = false;
        this.streaming.isOpen = false;
        this.streamingSubject.next(streaming);
    }

    getStreaming() {
        return this.streaming;
    }

    private handleError(error:any) {
        let errMsg = (error.message) ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
        console.error(errMsg);
        return Observable.throw(errMsg);
    }
}