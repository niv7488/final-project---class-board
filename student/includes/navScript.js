/** Color Const */
const RED = "#ff0000";
const BLACK = "#000000";
const GREEN = "#00ff00";
const BLUE = "#0000ff";
const PURPLE = "#ff00ff";
const WHITE = "#ffffff";

var lastColor = "#000000";
var page = 0;
var imageFormat = ".png";


const folderPath = "http://board-cast.com/images/courses/";
var courseName = "123456";/*
const folderPath = "http://localhost/websites/student/data/";
var courseName = "Try";*/

/**** Handelers */
/** Colors handelers */
$("#penColorRed").click( function() {
	changeColorAndUpdateLast(RED);	
});
$("#penColorBlack").click( function() {
	changeColorAndUpdateLast(BLACK);	
});
$("#penColorGreen").click( function() {
	changeColorAndUpdateLast(GREEN);	
});
$("#penColorBlue").click( function() {
	changeColorAndUpdateLast(BLUE);	
});
$("#penColorPurple").click( function() {
	changeColorAndUpdateLast(PURPLE);	
});


/** Save canvas as base 64 and save in local host */
function saveImageAsBase64(name) {
	console.log('[saveImageAsBase64] function called with ');
	console.log(name);
	var imgAsBase64 = $('#noteCanvas').getCanvasImage('jpeg');
	console.log('Canvas as 64: ' + imgAsBase64)
	localStorage.setItem(name, imgAsBase64);
}


/** Next page button handler */
$("#next").click( function() {
	console.log("[Event_Click] Next was clicked");
	try {
		saveImageAsBase64((courseName+page));
		setCanvasBackground(courseName+(++page));
	}
	catch(err) {
		console.log("[Event_Click Next] "+err);
		--page;
	}
});

/** Previous page button handler */
$("#previous").click( function() {
	console.log("[Event_Click] Previous was clicked");
	try {
		setCanvasBackground(courseName+(--page));	
	}
	catch(err) {
		console.log("[Event_Click Previous] "+err);
		++page;
	}
});

/** Save button handler */
$("#save").click( function() {
	alert("Save");	
});

/**
 *	Backup last color before changing color
 */
function changeColorAndUpdateLast(color) {
	changeColor(color);
	lastColor=color;
}

/**
 *	Initialize clien side
 * 	1/ on window resize handler
 * 	2/ Validate local storage validity on browser 
 */
function initialize() {
	console.log("[initialize] Start Function");
	window.onresize = adaptSize();
	if(typeof(Storage) !== "undefined") {
    	openNotebook(courseName);
	} else {
	    alert("Your brower doesn't support local storage.");
	}
	console.log("[initialize] End Function");
}

/**
 *	Adapt canvas size to window size 
 */
function adaptSize() {
	var canvas = document.getElementById("noteCanvas");
	canvas.width = window.innerWidth*0.95;
	canvas.height = window.innerHeight*0.95;
}

/**
 *	Upload from local storage the wanted course 
 *	@param {Object} notebookName - Name of the course
 */
function openNotebook(notebookName) {
	console.log("[openNotebook] Start Function");
	console.log("[openNotebook] Notebook to open is ["+ notebookName +"]");
	try {
		setCanvasBackground(notebookName+page);
	}
	catch(err) {
		console.log("[openNotebook] "+err);
	}
	console.log("[openNotebook] End Function");
}

$("#pen").click( function() {
	changeColor(lastColor);
});

$("#erase").click( function() {
	changeColor(WHITE);
});

/**
 *	Configurate background canvas via URL
 *	@param {Object} path - Full URL of picture from server
 */
function setCanvasBackground(path) {
	console.log("[setCanvasBackground] Start Function");
	console.log("[setCanvasBackground] Path to set as background is ["+ path +"]");
	$('#noteCanvas').clearCanvas().drawLayers();;
	var image = localStorage.getItem(path);
	if(image == null) {
		throw "No page named ["+ path +"] could be found";
	}
	else {
		console.log("[setCanvasBackground] Found key ["+ path +"] with value ["+ image +"]");
		image = folderPath+courseName+'/'+image+imageFormat;
	}
	$('#noteCanvas').drawImage({
		layer: true,
		source: image,
		width: $('#noteCanvas').width(),
	  	height: $('#noteCanvas').height(),
		fromCenter: false
	});
	$('#noteCanvas').addLayer({
		type: 'rectangle',
	  	width: $('#noteCanvas').width(),
	  	height: $('#noteCanvas').height()
	})
	.drawLayers();
	console.log("[setCanvasBackground] End Function");
}

var context;
var paint = false;
var clickX = new Array();
var clickY = new Array();
var clickDrag = new Array();
var paint = false;
var curColor = "#000000";
var clickColor = new Array();

context = document.getElementById('noteCanvas').getContext("2d");

$('#noteCanvas').mousedown( function(e) {
	console.log("[Event_Handler] Mouse down on canvas");
	paint = true;
	addClick(e.pageX - this.offsetLeft, e.pageY - this.offsetTop, false);
	redraw();
});

$('#noteCanvas').mousemove( function (e) {
	if(paint) {
		addClick(e.pageX - this.offsetLeft, e.pageY-this.offsetTop, true);
		redraw();
	}
	else {
		redraw();
	}
});

$('#noteCanvas').mouseup( function(e) {
	paint = false;
});
/*
$('#noteCanvas').mouseleave( function(e) {
	paint = false;
});*/

function addClick(x, y, dragging)
{
	console.log("[addClick] Mouse down on canvas");
	clickX.push(x);
	clickY.push(y);
	clickDrag.push(dragging);
	clickColor.push(curColor);
}

function redraw(){
	console.log("[addClick] Mouse down on canvas");
  //context.clearRect(0, 0, context.canvas.width, context.canvas.height); // Clears the canvas
	context.lineJoin = "round";
  	context.lineWidth = 5;
			
  for(var i=0; i < clickX.length; i++) {		
    context.beginPath();
    if(clickDrag[i] && i){
      	context.moveTo(clickX[i-1], clickY[i-1]);
     } else {
       context.moveTo(clickX[i]-1, clickY[i]);
     }
     context.lineTo(clickX[i], clickY[i]);
     context.closePath();
     context.strokeStyle =  clickColor[i];
     context.stroke();
  }
}

function changeColor(color) {
	curColor = color;
}

(function() {
	$('#buttonOk').click(function() {
		courseName = $("input").val();
		console.log("Course is now: " + courseName);
		page = 0;
		openNotebook(courseName);
	});
})();


/*
function onCourseChange() {
    var evt = new CustomEvent('onCourseChange', {});
    course.dispatchEvent(evt);
}

course.addEventListener('onCourseChange', function() {
	console.log('[Event] onCourseChange');
	switch(course) {
		case "123456":
			courseContent = {
				page1: "033532",
				page2: "033713",
				page3: "065304",
				page4: "073810",
				page5: "074107"
			};
			break;
		case "99999":
			courseContent = {
				page1:: "033042",
				page2: "033129"
			};
			break;
	}
	page = 0;
	openNotebook(courseName);
});
*/
(function() {
	if(typeof(Storage) !== "undefined") {
		localStorage.setItem("999990", "033042");
		localStorage.setItem("999991", "033129");
		localStorage.setItem("1234560", "033532");
		localStorage.setItem("1234561", "033713");
		localStorage.setItem("1234562", "065304");
		localStorage.setItem("1234563", "073810");
		localStorage.setItem("1234564", "074107");
	}
}())