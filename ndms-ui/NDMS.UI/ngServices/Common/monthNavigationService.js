'use strict';
define(['angularAMD'], function (angularAMD) {
    // service to share month details 
    angularAMD.service('monthNavigationService', function () {
        var monthDetails = {};
        var displayMonth = {
            id: '',
            details: ''
        };
        var monthArrayIndex = 0;
        var monthArrayLength = 0;
        
        // sets the month details
        this.setMonthDetails = function (data, monthId, yearId) {
            monthDetails = data;
            displayMonth = $.grep(monthDetails, function (obj) {
                return obj.yearId === yearId && obj.id == monthId;
            })[0];

            monthArrayIndex = monthDetails.indexOf(displayMonth);
            monthArrayLength = monthDetails.length - 1;
        };

        // get month details
        this.getMonthList = function () {
            return monthDetails;
        }

        // get the selected month for display
        this.getSelectedMonthDetails = function () {
            return displayMonth;
        };

        // update the month details when Next button is clicked
        this.setNextMonthDetails = function () {
            if (monthArrayIndex >= monthArrayLength)
            {
                return false;
            }

            monthArrayIndex = monthArrayIndex + 1;
            displayMonth = monthDetails[monthArrayIndex];
            return true;
        }

        // update the month details when Previous button is clicked
        this.setPreviousMonthDetails = function () {
            if (monthArrayIndex <= 0) {
                return false;
            }

            monthArrayIndex = monthArrayIndex - 1;
            displayMonth = monthDetails[monthArrayIndex];
            return true;
        }

    });

});