import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { Observable} from 'rxjs/Observable';
import { Http, Response, Headers, RequestOptions} from '@angular/http';
import {Student} from "./student";
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import {Course} from "./course";

@Injectable()
export class LoginService {
    private loginUrl = '';
    private headers = new Headers({ 'Content-Type': 'application/json'});
    private options = new RequestOptions({ headers: this.headers});

    private student = new Subject<Student>();

    studentLoggedIn = this.student.asObservable();

    constructor(private http: Http){};

    studentLoggedInEmitter(student: Student) {
        this.student.next(student);
    }

    studentAuthentification(id: number, password: string) {
        return this.http.post(this.loginUrl,JSON.stringify({
            "student_id":id,
            "password": password
        }),this.options)
            .map(this.checkAuthtification)
            .catch(this.handleError);
    }

    checkAuthtification(res: Response) {
        let student: Student = new Student();
        let body = res.json();
        student.id = body.student_id;
        student.name = body.firstname;
        student.lastName = body.lastname;
        for(var course of body.courses) {
            student.courses.push(new Course(course,"Static Test"));
        }

        this.studentLoggedInEmitter(student);
    }

    private handleError(error:any) {
        let errMsg = (error.message) ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
        console.error(errMsg);
        return Observable.throw(errMsg);
    }
}