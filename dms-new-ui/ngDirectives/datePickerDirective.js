"use strict";
define(['angularAMD'], function (angularAMD) {

    angularAMD.directive('datePicker', function () {
        return {
            scope: {
                dateModel: "=",
                dateOptions: "=",
                opened: "=",
                placeholderText: '=',
                minDate: "=",
                maxDate: "=",
                isDisabled: "=",
                showButtons: "=?",
                changeDate: '&'
            },
            templateUrl: 'ngTemplates/Common/DatePicker.tpl.min.html',
            controller: function ($scope) {
                if (angular.isUndefined($scope.showButtons)) {
                    $scope.showButtons = true
                }
            },
            link: function (scope, elem, attrs) {
                

                scope.open = function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    scope.opened = true;
                };

                scope.clear = function () {
                    scope.ngModel = null;
                };
            }

        };

    });
});


