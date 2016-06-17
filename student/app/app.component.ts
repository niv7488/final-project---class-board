import { Component } from '@angular/core';
import { RouteConfig, ROUTER_DIRECTIVES }
  from '@angular/router-deprecated';

import {NotebookControlComponent} from  './notebook-control.component';
import { CanvasComponent } from './canvas.component';
import { NavElementService } from './nav-element.service';
import {CourseListService} from "./course-list.service";
import {DashboardComponent} from './dashboard.component';
import {NotebookService} from "./notebook.service";
import {NotebookGalleryService} from "./notebook-gallery.service";
import {StreamingService} from "./streaming.service";
import {LoginFormComponent} from './login-form.component';
import {LoginService} from './login.service';

@Component({
  selector: 'bc-app',
  templateUrl: 'html/app.component.html',
  styleUrls: ['css/app.component.css'],
  directives: [
      NotebookControlComponent,
      CanvasComponent,
      DashboardComponent,
      LoginFormComponent,
      ROUTER_DIRECTIVES
  ],
    providers: [
        LoginService,
        NotebookService,
        NotebookGalleryService,
        NavElementService,
        CourseListService,
        StreamingService
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
    path:'/notebook/:id/:course/:date',
    name: 'Notebook',
    component: NotebookControlComponent
  },
  {
  	path:'/dashboard/',
  	name: 'Dashboard',
  	component: DashboardComponent
  },
    {
        path:'/login/',
        name: 'Login',
        component: LoginFormComponent,
        useAsDefault: true
    },
])

/**
 * Boardcast application container
 */
export class AppComponent {
    
}
