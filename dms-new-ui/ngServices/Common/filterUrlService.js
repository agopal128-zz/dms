'use strict';
define(['angularAMD'], function (angularAMD) {

    angularAMD.service('filterUrlService', [function (sessionStorageService) {
        // Check if urls are allowed without authentication
        this.needAuthorization = function (url) {
            // urls excluded from authentication
            if (url.indexOf('.html') != -1 || 
                url.indexOf('/token') != -1 ||
                url.indexOf('api/Scorecard/') != -1 ||
                url.indexOf('api/CounterMeasure/GetCounterMeasures') != -1 ||
                url.indexOf('api/CounterMeasure/GetAllCounterMeasureStatus') != -1 ||
                url.indexOf('api/Configuration') != -1) {
                return false;
            }
            return true;
        };

    }]);

});