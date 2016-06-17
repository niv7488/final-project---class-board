import { Directive, ElementRef, Input } from '@angular/core';

@Directive({
    selector: '[streaming]'
})

export class StreamingDirective {
    private el : HTMLElement;

    constructor(el: ElementRef) {
        this.el = el.nativeElement;
        //el.nativeElement.style.backgroundImage = 'url(./images/streaming-off.svg)';

    }

    setLedColor(isOnAir: boolean) {
        if(isOnAir)
            this.el.style.backgroundImage = 'url(./images/streaming-on.svg)';
        else
            this.el.style.backgroundImage = 'url(./images/streaming-off.svg)';
    }
}
