"use strict";
define(['angularAMD'], function (angularAMD) {

    angularAMD.directive('isDataLoaded', ['$parse', function ($parse) {
        return {
            scope: {
                kpis: '=isDataLoaded'
            },
            restrict: 'A',
            link: function (scope, element, attrs) {
                scope.$watch('kpis', function () {                   
                    if (scope.kpis) {
                        dashboardHelper(scope.kpis);
                    }
                });

            }
        };

    }]);
});