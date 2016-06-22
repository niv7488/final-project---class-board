import {Streaming} from "./streaming";
export class Course {
    id: number;
    name: string;
    streaming: Streaming;

    constructor(id: number, name:string, streaming: string = "") {
        this.id = id;
        this.name = name;
        if(streaming === "") 
            this.streaming = new Streaming();
        else 
            this.streaming = new Streaming(true, false, streaming);

    }
}