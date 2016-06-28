var convertImgToDataURLviaCanvas = function (url, callback){
    var img = new Image();
    img.crossOrigin = 'Anonymous';
    img.onload = function(){
        var canvas = document.createElement('CANVAS');
        var ctx = canvas.getContext('2d');
        var dataURL;
        canvas.height = this.height;
        canvas.width = this.width;
        ctx.drawImage(this, 0, 0);
        dataURL = canvas.toDataURL("image/jpeg",1);
        canvas = null;
        callback(dataURL);
    };
    img.src = url;
};

var convertFileToDataURLviaFileReader = function (courseContent, callback){
    //var source;
    //source = url.imgSrc.src || url;
    /*
    console.log(typeof url);
    if(url.indexOf('.jpg') > 1)
        source = url;
    else
        source = url.imgSrc.src;*/
    //console.log("Converting url:"+ source);
    //console.log(url);
    var url = courseContent.imgSrc.src;
    console.debug("Convert image to base 64 via blob");
    var xhr = new XMLHttpRequest();
    xhr.responseType = 'blob';
    xhr.onload = function() {
        var reader  = new FileReader();
        reader.onloadend = function () {
            callback(reader.result, courseContent);
        };
        reader.readAsDataURL(xhr.response);
    };
    xhr.open('GET', url);
    xhr.send();
};

var convertCanvasToUrl = function (canvas, callback) {
    console.debug("[convertCanvasToUrl] Convert the canvas to base64");
    var pngUrl = canvas.toDataURL();
    callback(pngUrl, new Date(milliseconds));
};