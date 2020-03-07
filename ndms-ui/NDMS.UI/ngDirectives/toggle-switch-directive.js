"use strict";
define(['angularAMD'],function(angularAMD){

    angularAMD.directive('toggleSwitch', function () {
        return {
            restrict: 'E',
            scope: {
                status: '=',
                labelLeft: '=',
                labelRight: '=',
                toggle: '&'
            },
            replace: false,
            templateUrl:'ngTemplates/Common/toggleSwitch.tpl.min.html',
            controller: function ($scope) {
               
            },
            controllerAs: 'ctrl',
            link: function (scope, elem, attr) {
                
                scope.$watch('status', function () {
                    scope.toggle();
                });

            }
        }
    });
});


