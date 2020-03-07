"use strict";
define(['angularAMD','metricType'], function (angularAMD) {

    angularAMD.directive("calendar", function () {
        return {
            scope: {
                target: '=',
                dailyTargets: '=',
                metricDataType: '=',
                showRolledUpTargets: '=',
                mode: '='
            },
            templateUrl: 'ngTemplates/Common/calendar.min.html',
            controller: function ($scope) {
                var self = this,
                    dailyTargetCopy = [];
                self.isShowingTarget = true;
                self.mode = $scope.mode;
                self.metricDataType = $scope.metricDataType;

                $scope.metricDataTypeName = $scope.metricDataType === 0 ? "wholeNumber" : "decimal";
                $scope.$watch('mode', function () {
                    $scope.editMode = $scope.mode === 'manage' ? true : false;
                });
                $scope.$watch('target', function () {
                    var date = new Date($scope.target.month.year, $scope.target.month.id - 1, 1),
                    year = date.getFullYear(),
                    month = date.getMonth();

                    var noOfDays = new Date(year, month + 1, 0).getDate();
                    $scope.startDay = new Date(year, month, 1).getDay();
                    $scope.blankDates = Array($scope.startDay);
                    $scope.endDay = new Date(year, month, noOfDays).getDay();
                    $scope.blankDatesLast = Array(6 - $scope.endDay);

                    if (!$scope.dailyTargets || $scope.dailyTargets.length === 0) {
                        for (var index = 0; index < noOfDays; index++) {
                            $scope.dailyTargets.push({
                                day: index + 1,
                            });                            
                        }                      
                    }
                });

                $scope.onValueChange = function (dailytarget) {
                    var ele = angular.element("[data-day=" + dailytarget.day + "]");
                    var originalGoalValue = $scope.metricDataType === 0 ? parseInt(ele.attr("data-original")) :parseFloat(ele.attr("data-original"));
                    dailytarget.isManual = dailytarget.isManual || originalGoalValue !== dailytarget.goalValue;
                    
                };

            }
        };
    });
});

