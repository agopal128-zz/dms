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
    var columnWidth = ($('.sqdicp-wrapper').width() / widthParameter) - 3;
    if (numberOfKpis <= 6) {
        columnWidth = columnWidth - 6;
    }
    if (window.innerWidth > 991) {
        $('.column').css('width', columnWidth);
        $('.column').parent().css('width', columnWidth * numberOfKpis).css('margin', '0 auto');
    }
    else {
        $('.column').css('width', '100%');
        $('.column').parent().css('width', '100%');
    }
    if (numberOfKpis <= 6) {
        $('.metric-wrapper').css({ 'margin': '5px 0px 10px' });
    }
    else{
        $('.metric-wrapper').css({'margin':'10px 0 20px' });
    }

    function setResponsiveStyle() {
        var headerTop = $('.top-header').height();
        $('.header').animate({
            'top': headerTop
        }, 1, function () {
            var contentTop = $('.top-header').height() + $('.header').height();
            $('.content-wrapper').css({
                'margin-top': contentTop
            });
        });
    }
    $(window).resize(function () {
        setResponsiveStyle();
    });
    setResponsiveStyle();
};