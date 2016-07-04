var modifyCanvasSize = function () {
    var myCanvas = document.getElementById('notebook-canvas');
    console.log(myCanvas);
    var container = $('#canvas-img').parent();
    console.log(container);
    console.log($(container).width());
    myCanvas.width = $(container).width();
    myCanvas.height = $(container).height();
};

var modifyIframeSize = function () {
    var myIframe = document.getElementById('streamingIframe');
    console.log(myIframe);
    var container = $('.iframeContainer');
    console.log(container);
    console.log($(container).width());
    myCanvas.width = $(container).width();
    myCanvas.height = $(container).height();
};