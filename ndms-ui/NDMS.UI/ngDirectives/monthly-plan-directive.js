"use strict";
define(['angularAMD', 'dailyTargetController', 'metricType'], function (angularAMD) {

    angularAMD.directive('monthlyPlan', ['$uibModal', 'targetService', function ($uibModal, targetService) {
        return {
            restrict: 'E',
            scope: {
                monthlyTarget: '=',
                isEditable: '=',
                isPrimary: '=',
                mode: '=',
                isCascadeEnabled: '=',
                isCascaded: '=',
                isStretchGoalEnabled: '=',
                hasDifferentTargetEntry: '=',
                targetEntryMethod: '=',
                effectiveStartDate: '=',
                effectiveEndDate: '=',
                trackingMethod: '=',
                metricDataType: '=',
                submitDailyTarget: '&'
            },
            controller: function ($scope) {
                var self = this;
                self.isShowingTarget = true;
                self.isDailyTargetAccessible = false;
                self.isInDateRange = false;
                self.isTargetEnteredDaily = false;

                var setDailyTargetAccess = function () {
                    self.isDailyTargetAccessible = $scope.trackingMethod === 0;
                },
                setTargetEntryMethod = function () {
                    self.isTargetEnteredDaily = $scope.targetEntryMethod === 0;

                    //target entry method is passed to daily target view
                    angular.forEach($scope.monthlyTarget, function (item) {
                        item.targetEntryMethod = $scope.targetEntryMethod;
                    });
                },
    
                showTotalColumn = function () {

                    if ($scope.isBowlingChartApplicable || ($scope.isBowlingChartApplicable &&
                        ($scope.metricDataType || $scope.metricDataType === 0) &&
                        $scope.metricDataType !== 2)) {
                        $scope.isShowTotalColumn = true;
                    }
                    if (!$scope.isBowlingChartApplicable || ($scope.isBowlingChartApplicable &&
                    ($scope.metricDataType && $scope.metricDataType === 2))) {
                        $scope.isShowTotalColumn = false;
                    }
                };
                showTotalColumn();
                var setMonthlyTargetRange = function () {
                    if ($scope.effectiveStartDate && $scope.monthlyTarget) {
                        $scope.effectiveStartMonth = $.grep($scope.monthlyTarget, function (obj) {
                            return obj.month.year === $scope.effectiveStartDate.getFullYear() && obj.month.id == $scope.effectiveStartDate.getMonth() + 1;
                        })[0];
                        $scope.effectiveStartMonthIndex = $scope.monthlyTarget.indexOf($scope.effectiveStartMonth);
                    }

                    if ($scope.effectiveEndDate && $scope.monthlyTarget) {
                        $scope.effectiveEndMonth = $.grep($scope.monthlyTarget, function (obj) {
                            return obj.month.year === $scope.effectiveEndDate.getFullYear() && obj.month.id == $scope.effectiveEndDate.getMonth() + 1;
                        })[0];
                        $scope.effectiveEndMonthIndex = $scope.monthlyTarget.indexOf($scope.effectiveEndMonth);
                    }
                    angular.forEach($scope.monthlyTarget, function (item, index) {
                        if (!(index >= $scope.effectiveStartMonthIndex && index <= $scope.effectiveEndMonthIndex)) {
                            item.goalValue = item.stretchGoalValue = item.dailyRateValue = "";
                        }
                    });
                };

                setMonthlyTargetRange();
                $scope.$watch('effectiveStartDate', function () { setMonthlyTargetRange(); });
                $scope.$watch('effectiveEndDate', function () { setMonthlyTargetRange(); });
                $scope.$watch('isCascadeEnabled', function () { self.isCascadeEnabled = $scope.isCascadeEnabled; });
                $scope.$watch('isCascaded', function () { self.isCascaded = $scope.isCascaded; });
                $scope.$watch('hasDifferentTargetEntry', function () { self.hasDifferentTargetEntry = $scope.hasDifferentTargetEntry; });             
                $scope.$watch('monthlyTarget', function () { self.monthlyTarget = $scope.monthlyTarget; setMonthlyTargetRange(); });
                $scope.$watch('trackingMethod', function () { setDailyTargetAccess(); });
                $scope.$watch('metricDataType', function () { showTotalColumn(); });
                $scope.$watch('targetEntryMethod', function () { setTargetEntryMethod(); });
                $scope.$watch('metricDataType',function() { self.metricDataTypeName = $scope.metricDataType === 0 ? "wholeNumber" : "decimal"; });
            },
            controllerAs: 'ctrl',
            templateUrl: 'ngViews/targets/templates/monthlyPlan.tpl.min.html',
            link: function (scope, elm, attr) {

                scope.showDailyTarget = function (mt) {
                    scope.submitDailyTarget({ target: mt, mode: scope.mode, showDailyAndMonthly: self.hasDifferentTargetEntry, metricDataType: scope.metricDataType });
                };

                scope.calculateMaxAllowedTotal = function (monthlyTargets) {
                    var maxAllowedTotal = 0;
                    if ((scope.metricDataType || scope.metricDataType === 0)
                        && scope.metricDataType !== 2) {
                        for (var i = 0; i < scope.monthlyTarget.length; i++) {
                            if (scope.monthlyTarget[i].maximumAllowedGoal) {
                                maxAllowedTotal = maxAllowedTotal +
                                    Math.round(parseFloat(scope.monthlyTarget[i].maximumAllowedGoal) * 100) / 100;
                            }
                        }
                        maxAllowedTotal = Math.round(maxAllowedTotal * 100) / 100;
                    }
                    else {
                        maxAllowedTotal = '';
                    }
                    return maxAllowedTotal;
                };

                scope.calculateRollupTotal = function (monthlyTargets) {
                    var rolledupTotal = 0;
                    if ((scope.metricDataType || scope.metricDataType === 0)
                        && scope.metricDataType !== 2) {
                        for (var i = 0; i < scope.monthlyTarget.length; i++) {
                            if (scope.monthlyTarget[i].rolledupGoalValue) {
                                rolledupTotal = rolledupTotal +
                                    Math.round(parseFloat(scope.monthlyTarget[i].rolledupGoalValue) * 100) / 100;
                            }
                        }
                        rolledupTotal = Math.round(rolledupTotal * 100) / 100;
                    }
                    else {
                        rolledupTotal = '';
                    }
                    return rolledupTotal;
                };

                scope.clearDailyTargets = function(monthTarget){
                    monthTarget.dailyTargets = [];
                    monthTarget.isManualTarget = true;
                };
                scope.calculateGoalTotal = function (monthlyTargets) {
                    var goalTotal = 0;
                    var hasValue = false;
                    if ((scope.metricDataType || scope.metricDataType === 0) && scope.metricDataType !== 2) {
                        for (var i = 0; i < scope.monthlyTarget.length; i++) {
                            if (scope.monthlyTarget[i].goalValue) {
                                goalTotal = goalTotal +
                                Math.round(parseFloat(scope.monthlyTarget[i].goalValue) * 100) / 100;
                            }

                            hasValue = hasValue || scope.monthlyTarget[i].goalValue != null;
                        }

                        // copied metrics should show blank 
                        if (hasValue) { 
                            goalTotal = Math.round(goalTotal * 100) / 100;
                        }
                        else {
                            goalTotal = '';
                        }
                    }
                    else {
                        goalTotal = '';
                    }
                    return goalTotal;
                };

                scope.calculateStretchGoalTotal = function (monthlyTargets) {
                    var stretchGoalTotal = 0;
                    var hasValue = 0;
                    if ((scope.metricDataType || scope.metricDataType === 0) && scope.metricDataType !== 2) {
                        for (var i = 0; i < scope.monthlyTarget.length; i++) {
                            if (scope.monthlyTarget[i].stretchGoalValue) {                                
                                stretchGoalTotal = stretchGoalTotal +
                                Math.round(parseFloat(scope.monthlyTarget[i].stretchGoalValue) * 100) / 100;
                            }
                            hasValue = hasValue || scope.monthlyTarget[i].stretchGoalValue != null;
                        }

                        // copied metrics should show blank 
                        if (hasValue) {
                            stretchGoalTotal = Math.round(stretchGoalTotal * 100) / 100;
                        }
                        else {
                            stretchGoalTotal = '';
                        }
                    }
                    else {
                        stretchGoalTotal = '';
                    }
                    return stretchGoalTotal;
                };
            }
        };
    }]);
});