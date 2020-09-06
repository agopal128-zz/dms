"use strict";
define(['angularAMD', 'metricType'], function (angularAMD) {

    angularAMD.directive('viewTarget', function () {
        return {
            restrict: 'E',
            scope: {
                kpiTargetData: '=',
                isViewTarget: '=',
                isPrevYearTarget: '=',
                isBowlingChartApplicable: '=',
                currentExpandedTargetIndex: '=',
                editTarget: '&',
                deleteTarget:'&',
                submitDailyTarget: '&'
            },
            controller: function ($scope) {
                var self = this;
                self.isShowingTarget = true;
                self.submitDailyTarget = function (mt, mode, showDailyAndMonthly,metricDataType) {
                    $scope.submitDailyTarget({ target: mt, mode: 'view', showDailyAndMonthly: showDailyAndMonthly, metricDataType: metricDataType });
                };
                $scope.$watch('currentExpandedTargetIndex');
            },
            controllerAs: 'ctrl',
            templateUrl: 'ngViews/targets/templates/view-target-tpl.min.html',

            link: function (scope, elm, attr) {
            }

        };
    });
});