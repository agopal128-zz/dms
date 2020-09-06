"use strict";
define(['angularAMD'], function (angularAMD) {

    angularAMD.directive('hierarchyChart',['$location','scorecardService','notificationService', function ($location, scorecardService, notificationService) {
        return {
            scope: {
                hierarchyData: '=',
                showActiveOnly: '=',
                isRootNodeInactive: '='
            },
            link: function (scope, elem, attrs) {
                scope.$watch('hierarchyData', function () {
                    var data = angular.copy(scope.hierarchyData);
                    if (data && data.children) {
                        hierarchyChartHelper(data, scope, $location, scorecardService, notificationService).draw(scope.showActiveOnly);
                    }
                });              
            }
        };
    }]);
});
