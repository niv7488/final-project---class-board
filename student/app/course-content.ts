export class CourseContent {
    id: number;
    name: string;
    imgSrc = new Image();
    
    constructor(id: number, name: string, image: string) {
        this.id=id;
        this.name=name;
        this.imgSrc.src = image;
    }
}