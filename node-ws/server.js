var express = require('express');
var bodyParser = require('body-parser');
var app = express();
var Decoder64 = require('./Decoder64');
var decoder = Decoder64();

//Here we are configuring express to use body-parser as middle-ware.
app.use(bodyParser.urlencoded({ extended: false }));
app.use(bodyParser.json());



/**
 / Main Test Rout
 */
app.get('/', function(req,res) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    res.set('Content-Type', 'text/html');
    res.send('<html><body> <h1>Welcome, This is Heroku Test Server </h1>'+
        '<h2> 1) Route To hello/:name To Test your name with JSON Response </h2>'+
        '<h2> 2) Route To /decode64/:base64/:filename To Upload Base64 Image with JSON Response </h2></body></html>');
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
 / Params: base64, filename
 */
app.post('/decode64', function (req,res)  {
    /* Set Headers */
    app.set('json spaces', 4);
    res.set("Content-Type", "application/json");
    res.status(200);
    var answer = {};
    /* Executing The Query via FTP */
    decoder.decode(req.body, function(response) {
        console.log('response recieved: '+ response);

        if(response != 'error') {
            answer.status = 'success';
            answer.link = response;
            res.json(answer);
        }
        else {
            answer.status = 'error';
            answer.idea = 'try change your mime-type';
            res.json(answer);
        }
    });
});


// Set The Port
app.set('port', process.env.PORT || 3000);
// Start the server
var server = app.listen(app.get('port'), function() {
    console.log('Express server listening on port ' + server.address().port + ' ..');
});