"use strict";
var platform_browser_dynamic_1 = require('@angular/platform-browser-dynamic');
// Imports for loading & configuring the in-memory web api
//import { provide }    from '@angular/core';
//import { XHRBackend } from '@angular/http';
//import { InMemoryBackendService, SEED_DATA } from 'angular2-in-memory-web-api/core';
//import { InMemoryDataService } from './in-memory-data.service';
var app_component_1 = require('./app.component');
platform_browser_dynamic_1.bootstrap(app_component_1.AppComponent);
/*
bootstrap(AppComponent, [
    HTTP_PROVIDERS,
    provide(XHRBackend, { useClass: InMemoryBackendService }), // in-mem server
    provide(SEED_DATA,  { useClass: InMemoryDataService })     // in-mem server data
]);*/ 
//# sourceMappingURL=main.js.map