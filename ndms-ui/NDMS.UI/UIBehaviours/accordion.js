"use strict";

var Accordion = function () {

    var init = function () {
        registerEvents();
    },

    registerEvents = function () {

        $(document).on('click', '.accordionHead', function () {
            var $this = $(this),
                item = $this.siblings('.accordionItem');

            //$('.accordionItem').not(item).hide('slow');
            $('.accordionItem:visible').not(item).slideToggle('slow');
            $('.expandIcon').not($('.expandIcon', $this)).addClass('faplus').removeClass('faminus');
            if (item.is(':visible')) {
                $('.expandIcon', $this).removeClass('faminus').addClass('faplus');
            }
            else {
                $('.expandIcon', $this).removeClass('faplus').addClass('faminus');
            }
            
            item.slideToggle('slow');

        });
    };
    return {
        register: init
    };
}();
