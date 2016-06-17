export class Course {
    id: number;
    name: string;
    streaming: string;
    streamingOnAir: string;

    constructor(id: number, name:string, streaming: string = "") {
        this.id = id;
        this.name = name;
        this.streaming = streaming;
        if(streaming === "")
            this.streamingOnAir = "./images/streaming-off.svg";
        else
            this.streamingOnAir = "./images/streaming-on.svg";
    }
}