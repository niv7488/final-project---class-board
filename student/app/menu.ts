import {Course} from "./course";
import {CourseContent} from "./course-content";
export class Menu {
    menu: Course;
    subMenu: CourseContent[];
    
    constructor(course: Course, courseContent: CourseContent[] = []) {
        this.menu = course;
        this.subMenu = courseContent;
    }
}