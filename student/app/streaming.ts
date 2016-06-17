export class Streaming {
    isAvailable: boolean;
    isOpen: boolean;
    channel: string;

    constructor(available:boolean = false,
                open:boolean = false,
                channel: string = ""){
        this.isAvailable = available;
        this.isOpen = open;
        this.channel = channel;
    }
}