"use strict";

var AppMenu = function () {

    var init = function () {
        registerEvents();
    },

    registerEvents = function () {

        $(document).on('click', '.menuIcon', function (e) {

            //Enable sidebar push menu          
            if ($("body").hasClass('sidebar-expand')) {
                $("body").removeClass('sidebar-expand');
                $('.sidebar-nav .treeview-menu').hide();
            }
            else {
                $("body").addClass('sidebar-expand');
            }
        });        
        
        $(document).on('click', '.sidebar-expand .sidebar-nav li a', function (e) {
            //Get the clicked link and the next element
            var $this = $(this);
            var checkElement = $this.next();
            var animationSpeed = 500;

            if (checkElement.children().length === 0) {
                $this.parent("li").parent('ul.treeview-menu').hide();
                $("body").removeClass('sidebar-expand');
            }
            //Check if the next element is a menu and is visible
            if ((checkElement.is('.treeview-menu')) && (checkElement.is(':visible'))) {
                //Close the menu
                checkElement.slideUp(animationSpeed, function () {
                    checkElement.removeClass('menu-open');
                });
                checkElement.parent("li").removeClass("active");
            }
                //If the menu is not visible
            else if ((checkElement.is('.treeview-menu')) && (!checkElement.is(':visible'))) {
                //Get the parent menu
                var parent = $this.parents('ul').first();
                //Close all open menus within the parent
                var ul = parent.find('ul:visible').slideUp(animationSpeed);
                //Remove the menu-open class from the parent
                ul.removeClass('menu-open');
                //Get the parent li
                var parent_li = $this.parent("li");
                parent.find('li.active').removeClass('active');
                //Open the target menu and add the menu-open class
                checkElement.slideDown(animationSpeed, function () {
                    //Add the class active to the parent li
                    checkElement.addClass('menu-open');
                    parent_li.addClass('active');
                });
            }
            //if this isn't a link, prevent the page from being redirected
            if (checkElement.is('.treeview-menu')) {
                e.preventDefault();
            }
        });      
        
    };


    return {
        register: init
    };
}();
