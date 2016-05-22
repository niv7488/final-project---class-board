import { Component } from '@angular/core';
import { RouteConfig, ROUTER_DIRECTIVES, ROUTER_PROVIDERS } 
  from '@angular/router-deprecated';

//import { NavElementsComponent } from './nav-element.component';
import { NotebookComponent } from './notebook.component';
import { StreamingComponent } from './streaming.component';
import {NotebookControlComponent} from  './notebook-control.component';
import { CanvasComponent } from './canvas.component';
import { NavElementService } from './nav-element.service';

@Component({
  selector: 'bc-app',
  templateUrl: 'app/app.component.html',
  styleUrls: ['app/app.component.css'],
  directives: [NotebookControlComponent, CanvasComponent, ROUTER_DIRECTIVES],
    providers: [
        ROUTER_PROVIDERS, NavElementService
    ]
})
/*
@RouteConfig([
  {
    path:'/notebook',
    name: 'Notebook',
    component: NotebookComponent,
    useAsDefault: true    
  },
  {
  	path:'/streaming',
  	name: 'Streaming',
  	component: StreamingComponent
  }
])*/

export class AppComponent { 
}
