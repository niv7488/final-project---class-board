import {Course} from "./course";
import {User} from "./student";
export class Notebook {
    course: Course;
    date: string;
    
    constructor(course: Course, date: string) {
        this.course = course;
        this.date = date;
    }
}