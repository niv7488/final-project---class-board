import { Injectable }     from '@angular/core';
import { Http, Response, RequestOptions, Headers} from '@angular/http';
import { Streaming }           from './streaming';
import { Observable }     from 'rxjs/Observable';

@Injectable()
export class StreamingService {
    
    private streamingChannelUrl: string = 'http://52.34.153.216:3000/getCourseStreaming';

    constructor (private http: Http) {
    }


    getStreamingChannel (): Observable<string> {
        console.log("Request for streaming");
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });
        return this.http.post(this.streamingChannelUrl,JSON.stringify({
            "course_id": 123456
        }),options)
            .map(this.extractData)
            .catch(this.handleError);
    }

    
    private extractData(res: Response) {
        let body = res.json();
        return body.src || { };
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
