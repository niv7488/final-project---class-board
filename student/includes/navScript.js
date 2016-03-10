/** Color Const */
const RED = "#ff0000";
const BLACK = "#000000";
const GREEN = "#00ff00";
const BLUE = "#0000ff";
const PURPLE = "#ff00ff";
const WHITE = "#ffffff";

var lastColor = "#000000";
var page = 0;


const folderPath = "ftp://board-cast.com/";
var courseName = "";/*
const folderPath = "http://localhost/websites/student/data/";
var courseName = "Try";*/

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

$("#next").click( function() {
	console.log("[Event_Click] Next was clicked");
	try {
		setCanvasBackground(courseName+(++page));
	}
	catch(err) {
		console.log("[Event_Click Next] "+err);
		--page;
	}
});

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

$("#save").click( function() {
	alert("Save");	
});

function changeColorAndUpdateLast(color) {
	changeColor(color);
	lastColor=color;
}

function initialize() {
	console.log("[initialize] Start Function");
	//$("#noteCanvas").width("700px");
	//$("#noteCanvas").height("800px");
	var canvas = document.getElementById("noteCanvas");
	canvas.width = window.innerWidth*0.95;
	canvas.height = window.innerHeight*0.95;
	if(typeof(Storage) !== "undefined") {
		localStorage.setItem(courseName+"0", "1.jpg");
		localStorage.setItem(courseName+"1", "2.jpg");
		localStorage.setItem(courseName+"2", "3.jpg");
		localStorage.setItem(courseName+"3", "4.jpg");
		localStorage.setItem(courseName+"4", "5.jpg");
    	openNotebook(courseName);
	} else {
	    alert("Your brower doesn't support local storage.");
	}
	console.log("[initialize] End Function");
}

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
		image = folderPath+image;
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
