export class Course {
    id: number;
    name: string;
    streaming: string;

    constructor(id: number, name:string, streaming: string = "") {
        this.id = id;
        this.name = name;
        this.streaming = streaming;
    }
}