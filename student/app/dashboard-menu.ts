import {Menu} from "./menu";
import {Course} from "./course";
import {CourseContent} from "./course-content";
export class DashboardMenu {
    menu: Course;
    subMenu: string[];

    constructor(course: Course, courseDates: string[] = []) {
        this.menu = course;
        this.subMenu = courseDates;
    }
}