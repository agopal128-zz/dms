"use strict";
var dashboardHelper = function (data) {
    var numberOfKpis = data.length;
    var widthParameter = numberOfKpis;
    if (numberOfKpis < 6) {
        widthParameter = 6;
    }
    else if (numberOfKpis > 10) {
        widthParameter = 10;
    }

    var columnWidth = ( 100 / widthParameter);

    if (window.innerWidth > 1023) {
        $('.letter-column').css('width', columnWidth + '%');
    }
    else {
        $('.letter-column').css('width', '100%');
        $('.letter-column').parent().css('width', '100%');
    }
    if (numberOfKpis <= 6) {
        $('.metric-wrapper').css({ 'margin': '5px 10px 10px' });
    }
    else{
        $('.metric-wrapper').css({'margin':'10px 7px 7px' });
    }
};