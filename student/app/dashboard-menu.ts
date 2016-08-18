import {Course} from "./course";
import {DateFormats} from "./date";
let moment = require('../js/moment.min.js');

export class DashboardMenu {
    menu: Course;
    streamingOnAir: string;
    subMenu: DateFormats[] = [];
    isSelected: boolean = false;

    constructor(course: Course, courseDates: string[] = []) {
        this.menu = course;
        courseDates.forEach(date => this.subMenu.push(
            new DateFormats(date, moment(date, ['DDMMYYYY']).format("DD/MM/YYYY"))
        ));
        //this.subMenu = ;
        if(this.menu.streaming.isAvailable) {
            this.streamingOnAir = "./images/streaming-on.svg";
        }
        else {
            this.streamingOnAir = "./images/streaming-off.svg";
        }
    }

    private sortDates() {

    }


}