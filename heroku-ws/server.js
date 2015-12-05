var express = require('express');
var app = express();
var Decoder64 = require('./Decoder64');
var decoder = Decoder64();

/**
 / Main Test Rout
 */
app.get('/', function(req,res) {
    res.set('Content-Type', 'text/html');
    res.send('<html><body> <h1>Welcome, This is Base64 Decode Example </h1>'+
        '<h2> Enjoy </h2></body></html>');
});


app.get('/hello', function(req,res) {
    res.set('Content-Type', 'text/html');
    res.send('<html><body> <h1>Hello Test </h1>'+
        '<h2> Enjoy </h2></body></html>');
});

/*
 / Decode Base64 Rout
 */
app.get('/decode64', function (req,res)  {
    console.log('inside localhost:3000/decode64 \n');
    //console.log('req.params: ' + req.param('base64'));
    //console.log('base coming: ' +req.param('base64') + '\n');

    var img = decoder.decode(req.param('base64'));
    app.set('json space',4);
    res.set('Content-Type', 'text/html');
    res.send('<html><body> <h1>Welcome, you can choose 3 routing ways : </h1>'+
        '<h2> https://authorsws.herokuapp.com/bestSellers </h2>'+
        '<h2> https://authorsws.herokuapp.com/bookById/number </h2>'+
        '<h2> https://authorsws.herokuapp.com/bookByYear/number</h2> <p></p>'+
        '<h2> Enjoy </h2></body></html>');;
});


// Set The Port
app.set('port', process.env.PORT || 3000);
// Start the server
var server = app.listen(app.get('port'), function() {
    console.log('Express server listening on port ' + server.address().port + ' ..');
});