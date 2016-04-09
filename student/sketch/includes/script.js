
"undefined" === typeof eventJs && (eventJs = {});
//Initialize buttons
(function() {
    //onClick icon event (selected CSS change)
    $('.nav-bar li').click(function() {
        console.log('[Click Event]');
        $('.selected').removeClass('selected');
        $(this).addClass('selected');
    });

    //Streaming screen invoke
    $('.icon-boardcast').click(function() {
        if($(".bc-boardcontent").is(':visible')) {
            $(".bc-boardcontent").hide();
            $('#bc-iframe').attr('src', '');
        }
        else {
            $('#bc-iframe').attr('src','http:\\\\192.168.2.103:7070');
            $(".bc-boardcontent").show();
        }
    });

    $(window).resize(function() {
        console.log('[Resize Event]');
        eventJs.resize();
    });
})();


"undefined" === typeof eventJs && (eventJs = {});
//Fit height 
(function(eventJs) {
    eventJs.resize = function() { 
        $('main').height((window.innerHeight - $('.nav-bar').outerHeight()) + "px");
    };
})(eventJs);

//Generate course content list
(function() {

})();