var express = require('express');
var app = express();
var Decoder64 = require('./Decoder64');
var decoder = Decoder64();

/**
 / Main Test Rout
 */
app.get('/', function(req,res) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    res.set('Content-Type', 'text/html');
    res.send('<html><body> <h1>Welcome, This is Heroku Test Server </h1>'+
        '<h2> Route To hello/:name To Test your name with JSON Response </h2></body></html>');
});

/*
* Hello Route to test your Request
 */
app.get('/hello/:name', function(req,res) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    var ua = req.headers['user-agent'];
    var user_details = {
        name: req.params.name,
        browser: ua
    }
    app.set('json spaces', 4);
    res.set("Content-Type", "application/json");
    res.status(200);
    res.json(user_details);
});

/*
 / Decode Base64 Route
 */
app.get('/decode64/:base64', function (req,res)  {
    console.log('inside localhost:3000/decode64 \n');
    //console.log('req.params: ' + req.param('base64'));
    //console.log('base coming: ' +req.param('base64') + '\n');

    var img = decoder.decode(req.params.base64);
    console.log('img link: ' + img);
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