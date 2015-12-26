var JSFtp = require("jsftp");
var base64 = require('base-64');
var fs = require("fs")

/**
 * Base64Decoder Object
 */
function Decoder64() {

}

/**
 * Decode Base64 String Into IMG Type
 */
Decoder64.prototype.decode = function(data, callback) {
    /* Create A JSFtp Connection Object */
    var Ftp = new JSFtp({
        host: "lang-develop.comxa.com",
        port: 21, // defaults to 21
        user: "a9879734", // defaults to "anonymous"
        pass: "1q2w3e4r" // defaults to "@anonymous"
    });

    /* Our Returned Link */
    var img_link = null;
    var file_data_01 = null; // only for test

    /* Saves the file FTP Side */
    try {
        var buffer = new Buffer(data.base64,'base64');
        var file_name = data.filename;
        Ftp.put(buffer, '/public_html/images/'+data.filename, function (hadError) {
            if (!hadError) {
                console.log("File transferred successfully!");
                img_link = "http://lang-develop.comxa.com/images/" + file_name;
                callback(img_link);
            }
            else {
                console.log("File transferred failed!: " + hadError);
                callback('error');
            }
        });

    }
    catch(err) {
        console.log('error detected: ' + err);
        callback('error');
    }
};


/**
 / What the module exports on initiate (Creating an instance)
 */
module.exports = function() {
    var decoder;
    decoder = new Decoder64();
    return decoder;
};