import { Component } from '@angular/core';
import { Router } from '@angular/router-deprecated';

@Component({
    selector: 'bc-streaming',
    template: '<h1>Streaming</h1>'
})

export class StreamingComponent {
    constructor(
        private _router: Router
    ) { }
}