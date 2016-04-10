
class Course {
    constructor(name, id) {
        console.debug('[Course] new course created');
        this.name = name;
        this.id = id;
        this.lessons = {};
        console.log(this);
    }

    addLesson(date) {
        console.debug('[AddLesson] add lesson at date');
        console.log(date);
        this.lessons[date] = new Lesson(date);
    }

    getLesson(date) {
        console.debug('[GetLesson] get lesson by date' + date);
        return this.lessons[date];
    }

    getLessonContent(date) {
        console.debug('[GetLessonContent] get lesson of course'+ this.name + ' at date' + date);
        return this.lessons[date].content;
    }
}

class Lesson {
    constructor(date) {
        console.debug('[Lesson] new lesson created');
        this.date = date;
        this.content = {};
        console.log(this);
    }

    addContent(content) {
        console.debug('[AddContent] add content to lesson')
        console.log(content);
        this.content[content.name] = new LessonContent(content.name, content.src);
    }
}

class LessonContent {
    constructor(filename, source) {
        console.debug('[LessonContent] new lesson content created')
        this.name = filename;
        this.src = source;
        console.log(this);
    }
}

"undefined" === typeof boardCastObj && (boardCastObj = {});
(function(boardCastObj) {
    boardCastObj.test = boardCastObj.test || {};
    var test = boardCastObj.test;
    test.student = {
        type: "Test",
        name: "Test ForNow",
        id: "1123",
        coursesList: {
            add: function(courseName, courseId) {
                var newCourse = new Course(courseName, courseId);
                test.coursesList[courseId] = newCourse;
            }
        }
    };
})(boardCastObj);

"undefined" === typeof eventJs && (eventJs = {});
"undefined" === typeof query && (query = {});
//Initialize buttons
(function(eventJs) {
    //onClick icon event (selected CSS change)
    $('.nav-bar li').click(function() {
        console.log('[Click Event]');
        if( $(this).parent().attr("id") == "center-menu") {
            console.log($(this).parent());
            $('.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    //onClick sidemenu gallery tab
    $('#gallery-tab li').click(function() {
        console.log('[Click Event]');
        $('.tab-selected').removeClass('tab-selected');
        $(this).addClass('tab-selected');
    });

    //Streaming screen invoke
    $('.icon-boardcast').click(function() {
        if($(".bc-boardcontent").is(':visible')) {
            $(".bc-boardcontent").hide();
            $('#bc-iframe').attr('src', '');
        }
        else {
            $('#bc-iframe').attr('src','http:\\\\192.168.2.107:7070');
            $(".bc-boardcontent").show();
        }
    });

    //icon gallery action
    $('.icon-gallery').click(function() {
        if($("#side-course-gallery").is(':visible')) {
            $("#side-course-gallery").hide();
        }
        else {
            $("#side-course-gallery").show();
        }
    });

    $('#board-tab').click(function() {
        $('#gallery-content').empty();
        console.debug('[EventClick] board tab');
        query.createBoardList();
    });

    $('#notebook-tab').click(function() {
        console.debug('[EventClick] notebook tab');
        $('#gallery-content').empty();
    });

    $(window).resize(function() {
        console.log('[Resize Event]');
        eventJs.resize();
    });
})(eventJs);


"undefined" === typeof eventJs && (eventJs = {});
//Fit height 
(function(eventJs) {
    eventJs.resize = function() { 
        $('main').height((window.innerHeight - $('.nav-bar').outerHeight()) + "px");
    };
    eventJs.resize();
})(eventJs);

//Generate student info
"undefined" === typeof student && (student = {});
(function(student) {
    student.type = "Student";
    student.name = "Test ForNow";
    student.id = "1123";
    student.coursesList = {};
})(student);

//Generate queries
"undefined" === typeof query && (query = {});
(function(query) {
    query.getStudentCoursesList = function(student, callback) {
        console.log(student);
        console.log(callback);
        $.post('http://52.34.153.216:3000/studentFullCourses', { 
                student_id: student.id 
            }, function(data) {
                var coursesList = $.parseJSON(JSON.stringify(data));
                $.each(coursesList, function(key, value) {
                    student.coursesList.add(value.course_name, value.course_id);
                });
                "undefined" !== typeof callback ? callback() : {} ;
            }
        );
    };
    query.getCourseContent = function(course, callback) {
        console.log(course);
        console.log(callback);
        $.post('http://52.34.153.216:3000/getCourseContent', {
            course_id: course.id
        }, function(data) {
            console.log(data);
            var lessonDates = $.parseJSON(JSON.stringify(data));
            $.each(lessonDates, function(key, value) {
                course.addLesson(value);
            });
            "undefined" !== typeof callback ? callback() : {} ;
        });
    };
    query.getCourseContentInDate = function(course, courseDate, callback) {
        $.post('http://52.34.153.216:3000/getDateImages', { 
            course_id: course.id,
            date: courseDate
        }, function(data) {
            console.log(data);
            console.log(course);
            var lesson = course.getLesson(courseDate);
            console.log(lesson);
            var lessonContent = $.parseJSON(JSON.stringify(data));
            $.each(lessonContent, function(key, value) {
                console.log(value);
                lesson.addContent(new LessonContent(value.filename, value.src));
            });
            console.log(student);
            "undefined" !== typeof callback ? callback() : {} ;
        });
    };
    query.initializeApp = function() {
        console.log(student);
        var courses = student.coursesList;
        console.log(courses);
        query.getStudentCoursesList(student, function() {
            query.getCourseContent(courses[99999], function() {
                query.getCourseContentInDate(courses[99999], courses[99999].lessons[31032016].date, function() {
                    createBoardList();
                });
            });
        });
        var createBoardList = function() {
            query.createBoardList();
        };
    };
    query.createBoardList = function() {
        var courses = courses || student.coursesList || {};
        console.log(student);
        $.each(courses[99999].lessons[31032016].content, function(key, value) {
            console.debug('[AddContentGallery] add content to gallery');
            console.debug(value);
            var li = $('<li/>')
                .addClass('board-content')
                .css("background-image", "url("+ value.src +")")
                .css("background-size","100% 140px");
            li.appendTo('#gallery-content');
        });
    };
})(query);

//Generate student courses
"undefined" === typeof student && (student = {});
(function(student) {
    var studentCourses = student.coursesList || {};
    studentCourses.add = function(courseName, courseId) {
        var newCourse = new Course(courseName, courseId);
        studentCourses[courseId] = newCourse;
    }
})(student);

"undefined" === typeof notebook && (notebook = {});
//Generate notebook object
(function(notebook) {
    var courses = student.coursesList;

    console.log(student);
    query.initializeApp();



})(notebook);

/*
"undefined" === typeof boardCastObj && (boardCastObj || {});
(function(boardCastObj) {
    /*boardCastObj.canvas = {
        paint: false,
        addClick: {},
        clickX: new Array(),
        clickY: new Array(),
        clickDrag: new Array(),
        redraw: {},
        context: 
    }*//*
    var bcCanvas = boardCastObj.canvas = boardCastObj.canvas || {};
    var paint = bcCanvas.paint = bcCanvas.paint || false;
    var addClick = bcCanvas.addClick = bcCanvas.addClick || {};
    var clickX = bcCanvas.clickX = bcCanvas.clickX || new Array();
    var clickY = bcCanvas.clickY = bcCanvas.clickY || new Array();
    var clickDrag = bcCanvas.clickDrag = bcCanvas.clickDrag || new Array();
    var redraw = bcCanvas.redraw = bcCanvas.redraw || {};
    var context = boardCastObj.canvas.context = document.getElementById('canvas').getContext("2d");
    boardCastObj.canvas.dom = $('#canvas');
    var canvas = boardCastObj.canvas.dom;
    console.debug($("#canvas"));
    addClick = function(x, y, dragging) {
        clickX.push(x);
        clickY.push(y);
        clickDrag.push(dragging);
    };
    $('#canvas').onmousedown(function(e) {
        console.debug('Canvas mousedown');
        var mouseX = e.pageX - this.offsetLeft;
        var mouseY = e.pageY - this.offsetTop;     
        paint = true;
        addClick(e.pageX - this.offsetLeft, e.pageY - this.offsetTop);
        redraw();
    });
    $('#canvas').on("mousemove", function(e) {
        console.debug('Canvas mousemouve');
        if(paint) {
            addClick(e.pageX - this.offsetLeft, e.pageY - this.offsetTop, true);
            redraw();
        }
    });
    $('#canvas').on("mouseup", function(e) {
        console.debug('Canvas mouseup');
        paint = false;
    });
    $('#canvas').on("mouseleave", function(e) {
        console.debug('Canvas mouseleave');
        paint = false; 
    });
    redraw = function() {
        context.clearRect(0, 0, context.canvas.width, context.canvas.height); // Clears the canvas
  
        context.strokeStyle = "#df4b26";
        context.lineJoin = "round";
        context.lineWidth = 5;
                
        for(var i=0; i < clickX.length; i++) {        
            context.beginPath();
            if(clickDrag[i] && i){
                context.moveTo(clickX[i-1], clickY[i-1]);
            }
            else {
                context.moveTo(clickX[i]-1, clickY[i]);
            }
            context.lineTo(clickX[i], clickY[i]);
            context.closePath();
            context.stroke();
        }
    };
})(boardCastObj);*/