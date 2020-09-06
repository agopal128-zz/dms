"use strict"
define(['angularAMD'], function (angularAMD) {
    angularAMD.directive('scorecardDropdown', function () {
        return {
            restrict: 'E',
            scope: {
                isHierarchyView: '=',
                isScorecardView:'=',
                scorecardOptions: '=',
                disableOptions:'=',
                changeHierarchyScorecard:'&'
            },
            replace: true,
            templateUrl: 'ngViews/scorecard/templates/scorecard-dropdown.tpl.min.html',
            controller: function ($scope) {

            },
            controllerAs: 'ctrl',
            link: function (scope, elem, attrs) {
                scope.$watch('scorecardOptions');
            }

        }
    });
});