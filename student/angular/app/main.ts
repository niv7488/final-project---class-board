import { bootstrap }    from '@angular/platform-browser-dynamic';
import { HTTP_PROVIDERS } from '@angular/http';
import { ROUTER_PROVIDERS } from '@angular/router';

import { NavElementService } from './nav-element.service';

import { AppComponent } from './app.component';
import {StreamingService} from "./streaming.service";


bootstrap(AppComponent, [
    HTTP_PROVIDERS,
    ROUTER_PROVIDERS
]);