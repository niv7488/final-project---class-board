import { Component } from '@angular/core';

import { NavElementService } from './nav-element.service';
import { CanvasDirective } from './canvas.directive';

@Component({
    selector: 'bc-canvas',
    templateUrl: 'app/canvas.component.html',
    styleUrls: ['app/canvas.component.css'],
    providers: [NavElementService],
    directives: [ CanvasDirective ]
})

export class CanvasComponent {
    
}


