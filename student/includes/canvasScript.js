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
	paint = true;
	addClick(e.pageX - this.offsetLeft, e.pageY - this.offsetTop, false);
	redraw();
});

$('#noteCanvas').mousemove( function (e) {
	if(paint) {
		addClick(e.pageX - this.offsetLeft, e.pageY-this.offsetTop, true);
		redraw();
	}
});

$('#noteCanvas').mouseup( function(e) {
	paint = false;
});

$('#noteCanvas').mouseleave( function(e) {
	paint = false;
});

function addClick(x, y, dragging)
{
  clickX.push(x);
  clickY.push(y);
  clickDrag.push(dragging);
  clickColor.push(curColor);
}

function redraw(){
  //context.clearRect(0, 0, context.canvas.width, context.canvas.height); // Clears the canvas
  context.lineJoin = "round";
  context.lineWidth = 5;
			
  for(var i=0; i < clickX.length; i++) {		
    context.beginPath();
    if(clickDrag[i] && i){
      context.moveTo(clickX[i-1], clickY[i-1]);
     }else{
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