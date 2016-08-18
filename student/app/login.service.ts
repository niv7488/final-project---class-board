import { Injectable } from '@angular/core';
import { Observable} from 'rxjs/Observable';
import { Http, Response} from '@angular/http';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';

import {User} from "./student";
import { WEBSERVICE_LOGIN_PATH, OPTION } from './constants';

@Injectable()
export class LoginService {
    private loginUrl = WEBSERVICE_LOGIN_PATH;
    private options = OPTION;
    userLoggedIn: User;
    
    //private user = new Subject<User>();

    //UserLoggedIn = this.student.asObservable();

    constructor(private http: Http) {}
/*
    studentLoggedInEmitter(student: User) {
        this.user.next(student);
    }
*/
    /**
     * Authothentifiacate user
     * @param id - User ID
     * @param password - User Password
     * @returns {Observable<R>}
     */
    authentification(id: string, password: string) {
        console.debug("[authentification] Identify user id " + id );
        return this.http.post(this.loginUrl,JSON.stringify({
            "user_id" : id,
            "password" : password
        }),this.options)
            .map(this.returnUser)
            .catch(this.loginError);
    }

    /**
     * Return response to login authentiication caller
     * @param res
     * @returns {any}
     */
    private returnUser(res: Response) {
        console.debug("[returnUser] Successfully login and got user");
        let body = res.json();
        console.log(body);
        return body;
    }

    /**
     * Catch authentification error
     * @param error
     * @returns {ErrorObservable}
     */
    private loginError(error: any) {
        let errMsg = (error.message) ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
        console.error("[loginError] An error occured when identifying user. Reason: " + errMsg);
        console.error(error);
        return Observable.throw(errMsg);
    }
}