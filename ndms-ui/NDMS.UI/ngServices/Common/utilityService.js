'use strict';
define(['angularAMD'], function (angularAMD) {

    angularAMD.service('utilityService', ['$filter', function ($filter) {

        this.convertToDateWithoutTimezone = function (date) {
            if (date.toString().match(/(\d\d\d\d)-(\d\d)-(\d\d)/)) {
                var year = $filter('date')(date.slice(0, 10), 'yyyy'),
                    month = $filter('date')(date.slice(0, 10), 'MM'),
                    day = $filter('date')(date.slice(0, 10), 'dd');
                return new Date(year, month - 1, day);
            }
            else {
                return new Date(date);
            }
        };

        this.loadExtentionMethods = function () {

            String.prototype.endsWith = function (suffix) {
                return this.indexOf(suffix, this.length - suffix.length) !== -1;
            };

            //Stufff strings in string 
            String.prototype.format = function () {
                var theString = this;
                for (var i = 0; i < arguments.length; ++i) {
                    var reg = new RegExp("\\{" + i + "\\}", "gm");
                    theString = theString.replace(reg, arguments[i]);
                }
                return theString;
            };

            // gets date part of the date
            Date.prototype.getDatePart = function () {
                this.setHours(0, 0, 0, 0);
                return this;
            };

            // check if an element exists in array using a comparer function
            // comparer : function(currentElement)
            Array.prototype.inArray = function (comparer) {
                for (var i = 0; i < this.length; i++) {
                    if (comparer(this[i])) return true;
                }
                return false;
            };

            // adds an element to the array if it does not already exist using a comparer 
            // function
            Array.prototype.pushIfNotExist = function (element, comparer) {
                if (!this.inArray(comparer)) {
                    this.push(element);
                }
            };


        };

    }]);
});