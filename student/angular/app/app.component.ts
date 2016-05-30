import { Component } from '@angular/core';
import { RouteConfig, ROUTER_DIRECTIVES, ROUTER_PROVIDERS } 
  from '@angular/router-deprecated';

import {NotebookControlComponent} from  './notebook-control.component';
import { CanvasComponent } from './canvas.component';
import { NavElementService } from './nav-element.service';
import {CourseListService} from "./course-list.service";
import {DashboardComponent} from './dashboard.component';
import {LoginComponent} from "./login.component";
import {NotebookService} from "./notebook.service";
import {NotebookGalleryService} from "./notebook-gallery.service";

@Component({
  selector: 'bc-app',
  templateUrl: 'app/app.component.html',
  styleUrls: ['app/app.component.css'],
  directives: [
      NotebookControlComponent,
      CanvasComponent,
      DashboardComponent,
      ROUTER_DIRECTIVES
  ],
    providers: [
        NotebookService,
        NotebookGalleryService,
        NavElementService,
        CourseListService,
        ROUTER_PROVIDERS
    ]
})

/**
 * Application routes:
 * Login - Login page
 * Dashboard - Manage content
 * Notebook - Notebook area 
 */
@RouteConfig([
  {
    path:'/notebook',
    name: 'Notebook',
    component: NotebookControlComponent
  },
    {
        path:'/login',
        name: 'Login',
        component: LoginComponent
    },
  {
  	path:'/dashboard',
  	name: 'Dashboard',
  	component: DashboardComponent,
    useAsDefault: true
  }
])

/**
 * Boardcast application container
 */
export class AppComponent { 
}
