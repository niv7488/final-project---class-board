import { Injectable }     from '@angular/core';
import { Http, Response, RequestOptions, Headers} from '@angular/http';
import { Streaming }           from './streaming';
import { Observable }     from 'rxjs/Observable';
import { WEBSERVICE_STREAMING_GET_STREAMING_CHANNEL, OPTION } from './constants';

@Injectable()
export class StreamingService {
    
    //private streamingChannelUrl: string = WEBSERVICE_STREAMING_GET_STREAMING_CHANNEL;

    constructor (private http: Http) {
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
