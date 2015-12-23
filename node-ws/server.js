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
* Async Function Test
 */
var async = function(data, callback) {
    callback(null, data);
}

/*
 / Decode Base64 Route
 */
app.get('/decode64/:base64/:filename', function (req,res)  {
    console.log('incoming: ' + req.params.filename)
    /* Set Headers */
    app.set('json spaces', 4);
    res.set("Content-Type", "application/json");
    res.status(200);
    var answer = {};
    /* Executing The Query via FTP */
    decoder.decode(req.params, function(response) {
        console.log('response recieved: '+ response);
        answer.status = 'success';
        if(answer.status != 'error') {
            answer.link = response;
            res.json(answer);
        }
        else {
            res.json(answer);
        }
    });
    /*app.set('json space',4);
    res.set('Content-Type', 'text/html');
    res.send('<html><body> <h1>Welcome, you can choose 3 routing ways : </h1>'+
        '<h2> https://authorsws.herokuapp.com/bestSellers </h2>'+
        '<h2> https://authorsws.herokuapp.com/bookById/number </h2>'+
        '<h2> https://authorsws.herokuapp.com/bookByYear/number</h2> <p></p>'+
        '<h2> Enjoy </h2></body></html>');; */
});


// Set The Port
app.set('port', process.env.PORT || 3000);
// Start the server
var server = app.listen(app.get('port'), function() {
    console.log('Express server listening on port ' + server.address().port + ' ..');
});