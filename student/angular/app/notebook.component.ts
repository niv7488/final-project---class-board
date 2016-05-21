import { Component } from '@angular/core';
import { Router } from '@angular/router-deprecated';

@Component({
    selector: 'bc-notebook',
    template: '<h1>notebook</h1>'
})

export class NotebookComponent {
    constructor(
        private _router: Router
    ) { }
}