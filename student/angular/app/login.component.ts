import { Component } from '@angular/core';
import {Student} from "./student";
import {LoginService} from "./login.service";

@Component ({
    selector: 'bc-login',
    template: `ID: <input><br>
                Password <input><br>
                <button>Login</button>`
})

export class LoginComponent {
    public student: Student;

    constructor(private loginService: LoginService) {}
}