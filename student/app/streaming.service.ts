import { Injectable }     from '@angular/core';
import { Http, Response, RequestOptions, Headers} from '@angular/http';
import { Streaming }           from './streaming';
import { Observable }     from 'rxjs/Observable';
import { WEBSERVICE_STREAMING_GET_STREAMING_CHANNEL, OPTION } from './constants';
import { CourseContent } from './course-content';
import {DB_SOURCE_ENUM} from "./db-source";
let moment = require('../js/moment.min.js');

@Injectable()
export class StreamingService {
    private getServerTimeUrl: string = "https://boardcast-ws.herokuapp.com/getCurrentTime";
    private option = OPTION;
    //private streamingChannelUrl: string = WEBSERVICE_STREAMING_GET_STREAMING_CHANNEL;

    constructor (private http: Http) {
    }

    createContentFromUrl(courseId,imgurl: string) : Observable<CourseContent> {
        console.debug("[createContentFromUrl] Create new courseContent");
        return this.http.post(this.getServerTimeUrl,"",this.option)
            .map(function(res: Response) {
                console.debug("[createContentFromUrl] Got server time " + res);
                console.log(res);
                let body = res.json();
                console.log(body);
                let temp = moment(body.time,"YYYYMMDDHHmmss");
                console.log(temp);
                let courseCnt =  new CourseContent(courseId,temp.format("DDMMYYYY"),temp.format("YYYYMMDDHHmmss")
                ,imgurl,temp,DB_SOURCE_ENUM.Localstorage);
                console.log(courseCnt);
                return courseCnt;
            })
            .catch(this.handleError);
    }

    getStreamingChannel (courseId: number): Observable<Streaming> {
        console.debug("[getStreamingChannel] Request for streaming channel");
        let options = OPTION;
        return this.http.post(WEBSERVICE_STREAMING_GET_STREAMING_CHANNEL,JSON.stringify({
            "course_id": courseId
        }),options)
            .map(this.extractStreamingData)
            .catch(this.handleError);
    }

    
    private extractStreamingData(res: Response) {
        let body = res.json();
        console.debug("[extractStreamingData] Streaming channel is: ");
        console.log(body);
        if(body.src === "") {
            return new Streaming();
        }
        else {
            return new Streaming(true,false,body.src);
        }
    }

    private handleError (error: any) {
        // In a real world app, we might use a remote logging infrastructure
        // We'd also dig deeper into the error to get a better message
        let errMsg = (error.message) ? error.message :
            error.status ? `${error.status} - ${error.statusText}` : 'Server error';
        console.error(errMsg); // log to console instead
        return Observable.throw(errMsg);
    }

}
