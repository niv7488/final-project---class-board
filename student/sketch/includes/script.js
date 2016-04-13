
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
        console.debug('[AddContent] add content to lesson');
        console.log(content);
        var source = content.src.indexOf(".com") == -1 ? content.src : content.src.split(".com")[1];
        console.debug('source = ' + source);
        this.content[content.name] = new LessonContent(content.name, source);
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

class Student {
    constructor(id,name) {
        this.id = id;
        this.name = name || "No name";
        this.coursesList = {
            add: function(courseName, courseId) {
                console.log(this);
                console.log(courseName);
                console.log(courseId);
                var newCourse = new Course(courseName, courseId);
                this[courseId] = newCourse;
            }
        };
    }
}

"undefined" === typeof boardCastObj && (boardCastObj = {});
"undefined" === typeof student && (student = {});
(function(boardCastObj) {
    boardCastObj.urlParse = boardCastObj.urlParse || {};
    var app = document.URL.split('.html')[0];
    var getQuery = document.URL.split('?')[1] || "student=0000&course=00000&date=000000";
    var queryVariable = getQuery.split("&");
    boardCastObj.urlParse.variable =  {
        student: queryVariable[0].split("=")[1],
        course: queryVariable[1].split("=")[1],
        date: queryVariable[2].split("=")[1]
    };
    
    localStorage.clear();
    var jsonVar = {
        "pages" : [
            { "filename" : "033042", "source" : "images/courses/99999/033042.png", "image" : "", "timestamp" : ""},
            { "filename" : "033129", "source" : "images/courses/99999/033129.png", "image" : "", "timestamp" : ""}
        ]
    };
    localStorage.setItem(boardCastObj.urlParse.variable.course, JSON.stringify(jsonVar)) || {
        pages: new Array()
    };
    
    boardCastObj.student = new Student(boardCastObj.urlParse.variable.student);
    student = boardCastObj.student;
    var canvas = boardCastObj.canvas = $('#note-canvas');
    boardCastObj.preview = {
        tab: "none",
        page: "none"
    };
    boardCastObj.canvas.changeBackground = function(backgroundImage) {
        console.log("[setCanvasBackground] Start Function");
        if(backgroundImage.indexOf("data:image")==-1) {
            console.debug(backgroundImage);
        }
        else {
            console.debug(boardCastObj.notebook.pages[boardCastObj.preview.page]);
        }
        //console.log("[setCanvasBackground] Path to set as background is ["+ path +"]");
        $('#note-canvas').clearCanvas();
        //canvas.clearCanvas().drawLayers();
        canvas.drawImage({
            layer: true,
            source: backgroundImage,
            width: canvas.width(),
            height: canvas.height(),
            fromCenter: false
        });
        canvas.addLayer({
            type: 'rectangle',
            width: canvas.width(),
            height: canvas.height()
        })
        .drawLayers();

        console.log("[setCanvasBackground] End Function");
    };
    //var notebook = boardCastObj.notebook = boardCastObj.notebook || {};
    var notebook = boardCastObj.notebook = JSON.parse(localStorage.getItem(boardCastObj.urlParse.variable.course));
    notebook.loadNotebook = function() {
        console.debug('[Load notebook]');
        console.debug(notebook.pages);
        $.each(notebook.pages, function(key, value) {
            var background = value.image == "" ? value.source : value.image;
            var li = $('<li/>')
                .addClass('board-content')
                .css("background-image", "url("+ background +")")
                .css("background-size","100% 140px")
                .click(function() {
                    var background = value.image == "" ? value.source : value.image;
                    if(boardCastObj.preview.tab == "notebook") {
                        notebook.savePageAs64Base(boardCastObj.preview.page);
                    }
                    console.debug('[notebook] click tab');
                    //console.debug(background);
                    boardCastObj.canvas.changeBackground(background);
                    var background = value.image == "" ? value.source : value.image;
                    $(this).css("background-image",background);
                    //notebook.refreshList();
                    boardCastObj.preview = {
                        tab: "notebook",
                        page: key
                    };
                    console.debug(notebook);
                });
            li.appendTo('#gallery-content');
        })
    };
    notebook.savePageAs64Base = function(page) {
        console.log('[saveImageAsBase64] function called with ');
        console.log(page);
        var imgAsBase64 = $('#note-canvas').getCanvasImage('jpeg');
        //console.log('Canvas as 64: ' + imgAsBase64);
        //console.log(imgAsBase64);
        notebook.pages[page].image = imgAsBase64;
        console.debug('[Json] new json');
        console.debug(notebook.pages);
        localStorage.setItem(boardCastObj.urlParse.variable.course, JSON.stringify(notebook));
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
            $('section.canvas').show();
        }
        else {
            query.getStreamingChanel(function(source) {
                $('section.canvas').hide();
                $('#bc-iframe').attr('src',source);
                $(".bc-boardcontent").show();
            });
            
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
        eventJs.resize();
    });

    $('#board-tab').click(function() {
        $('#gallery-content').empty();
        console.debug('[EventClick] board tab');
        query.createBoardList();
    });

    $('#notebook-tab').click(function() {
        var notebook = boardCastObj.notebook;
        console.debug('[EventClick] notebook tab');
        $('#gallery-content').empty();
        console.debug(notebook);
        notebook.loadNotebook();
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
        var galleryMenuWidth = $("#side-course-gallery").outerWidth();
        $('main').height((window.innerHeight - $('.nav-bar').outerHeight()) + "px");
        if($("#side-course-gallery").is(':visible')) {
            $(".canvas").width((window.innerWidth - galleryMenuWidth) + "px");
            $(".canvas").css( { marginLeft : galleryMenuWidth + "px" } );
        }
        else {
            $(".canvas").width(window.innerWidth + "px");
            $(".canvas").css( { marginLeft : "0px" } );
        }
    };
    eventJs.resize();
})(eventJs);


//Generate queries
"undefined" === typeof query && (query = {});
(function(query) {
    //boardCastObj.urlParse.variable = boardCastObj.urlParse.variable || {};
    var appParameters = boardCastObj.urlParse.variable;
    query.getStudentCoursesList = function(studentId, callback) {
        $.post('http://52.34.153.216:3000/studentFullCourses', { 
                student_id: studentId
            }, function(data) {
                var courseList = $.parseJSON(JSON.stringify(data));
                console.log(courseList);
                $.each(courseList, function(key, value) {
                    console.log(value);
                    console.log(student);
                    student.coursesList.add(value.course_name, value.course_id);
                });
                "undefined" !== typeof callback ? callback() : {} ;
            }
        );
    };
    query.getCourseContent = function(courseId, callback) {
        console.log(courseId);
        console.log(callback);
        $.post('http://52.34.153.216:3000/getCoursesContentDates', {
            course_id: courseId//course.id
        }, function(data) {
            console.log(data);
            var course = student.coursesList[courseId];
            var lessonDates = $.parseJSON(JSON.stringify(data));
            $.each(lessonDates, function(key, value) {
                course.addLesson(value);
            });
            "undefined" !== typeof callback ? callback() : {} ;
        });
    };
    query.getCourseContentInDate = function(courseId, courseDate, callback) {
        $.post('http://52.34.153.216:3000/getDateImages', { 
            course_id: courseId,//course.id,
            date: courseDate
        }, function(data) {
            console.log(data);
            var course = student.coursesList[courseId];
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
        query.getStudentCoursesList(student.id, function() {
            query.getCourseContent(appParameters.course, function() {
                query.getCourseContentInDate(appParameters.course, appParameters.date, function() {
                    createBoardList();
                });
            });
        });
        var createBoardList = function() {
            query.createBoardList();
        };
        eventJs.resize();
    };
    query.createBoardList = function() {
        var courses = courses || student.coursesList || {};
        console.log(student);
        $.each(courses[appParameters.course].lessons[appParameters.date].content, function(key, value) {
            console.debug('[AddContentGallery] add content to gallery');
            console.debug(value);
            var li = $('<li/>')
                .addClass('board-content')
                .css("background-image", "url("+ value.src +")")
                .css("background-size","100% 140px")
                .click(function() {
                    console.debug("[Gallery] board Click object " );
                    console.debug(value);
                    boardCastObj.canvas.changeBackground(value.src);
                    boardCastObj.preview = {
                        tab: "board",
                        page: key
                    };
                });
            li.appendTo('#gallery-content');
        });
    };
    query.getStreamingChanel = function(callback) {
        $.post('http://52.34.153.216:3000/getCourseStreaming', { 
                course_id: boardCastObj.urlParse.variable.course
            }, function(data) {
                var streaming = $.parseJSON(JSON.stringify(data));
                console.log(streaming);
                "undefined" !== typeof callback ? callback(streaming.src) : {} ;
            }
        );
    }
})(query);

"undefined" === typeof notebook && (notebook = {});
//Generate notebook object
(function(notebook) {
    var courses = student.coursesList;
    console.log(student);
    query.initializeApp();
})(notebook);
