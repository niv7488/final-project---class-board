import {Course} from "./course";
let moment = require('../js/moment.min.js');

export class DashboardMenu {
    menu: Course;
    streamingOnAir: string;
    subMenu: any[] = [];
    isSelected: boolean = false;

    constructor(course: Course, courseDates: string[] = []) {
        this.menu = course;
        courseDates.forEach(date => this.subMenu.push({
            original: date,
            dateFormat: moment(date, ['DDMMYYYY']).format("DD/MM/YYYY")
        }));
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