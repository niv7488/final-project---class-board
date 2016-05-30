import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import {NavElement} from "./nav-element";
import { NAV_ELEMENTS } from './nav-element-list';

@Injectable()
export class NavElementService {
    private navElementChangeSource = new Subject<NavElement>();

    changedSelected$ = this.navElementChangeSource.asObservable();

    changedSelectedEmitter(navElementCurrent: NavElement) {
        console.log("Emit current " + navElementCurrent.name);
        this.navElementChangeSource.next(navElementCurrent);
    }

    getMenuNavElements() {
        let menuNavComponent: NavElement[] = [];
        menuNavComponent.push(NAV_ELEMENTS.filter(element => element.name == "menu")[0]);
        menuNavComponent.push(NAV_ELEMENTS.filter(element => element.name == "gallery")[0]);
        menuNavComponent.push(NAV_ELEMENTS.filter(element => element.name == "board")[0]);
        menuNavComponent.push(NAV_ELEMENTS.filter(element => element.name == "cursor")[0]);
        menuNavComponent.push(NAV_ELEMENTS.filter(element => element.name == "pen")[0]);
        menuNavComponent.push(NAV_ELEMENTS.filter(element => element.name == "circle")[0]);
        menuNavComponent.push(NAV_ELEMENTS.filter(element => element.name == "text")[0]);
        menuNavComponent.push(NAV_ELEMENTS.filter(element => element.name == "picture")[0]);
        menuNavComponent.push(NAV_ELEMENTS.filter(element => element.name == "eraser")[0]);
        menuNavComponent.push(NAV_ELEMENTS.filter(element => element.name == "undo")[0]);
        menuNavComponent.push(NAV_ELEMENTS.filter(element => element.name == "redo")[0]);
        return menuNavComponent;
    }
}