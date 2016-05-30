import {Course} from "./course";
export class Student {
    id: number;
    name: string;
    lastName: string;
    courses: Course[] = [];
}