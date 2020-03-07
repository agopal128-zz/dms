"use strict";
define(['angularAMD'], function (angularAMD) {
    angularAMD.directive('workdayPattern', function () {
        return {
            restrict: 'E',
            scope: {
                isReadonly: '=',
                workdayPattern: '='
            },
            controller: function ($scope) {
                var self = this;
                $scope.$watch('workdayPattern', function () { self.workdayPattern = $scope.workdayPattern; });
            },
            controllerAs: 'ctrl',
            templateUrl: 'ngViews/scorecard/templates/workday-pattern.tpl.min.html',
            link: function (scope, elm, attr) {
            }
        }
    });
});
