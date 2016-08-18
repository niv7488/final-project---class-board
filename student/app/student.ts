import {Course} from "./course";
export class User {
    id: number;
    name: string;
    courses: Course[] = [];
    image: string;

    constructor(id: number, name: string, image:string = "./images/avatar.png", courses: Course[] = []) {
        this.id = id;
        this.name = name;
        this.courses = courses;
        this.image = image;
    }
}