import {DB_SOURCE_ENUM} from "./db-source";
let moment = require('../js/moment.min.js');

export class CourseContent {
    course_id: number;
    date: string;
    name: string;
    imgSrc = new Image();
    timestamp:any;
    dbSrc: DB_SOURCE_ENUM;
    
    constructor(courseId: number, date: string, name: string, image: string, timestamp: string, dbSrc: DB_SOURCE_ENUM) {
        this.course_id = courseId;
        this.date=date;
        this.name=name;
        this.imgSrc.setAttribute('crossOrigin', 'anonymous');
        this.imgSrc.src = image;
        this.timestamp = moment.parseZone(moment(timestamp, ["YYYYMMDDHHmmss"])).local().format();
        this.dbSrc = dbSrc;
    }
    
    public static sort(courseContent1:CourseContent, courseContent2:CourseContent) {
        if(courseContent1.name > courseContent2.name)
            return 1;
        else if(courseContent1.name < courseContent2.name) return -1;
        else {
            if(courseContent1.dbSrc == DB_SOURCE_ENUM.External)
                return -1;
            else 
                return 1;
        }
    }
}