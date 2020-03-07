'use strict';
define(['angularAMD'], function (angularAMD) {

    angularAMD.controller('dailyTargetController', ['$scope', '$uibModalInstance', 'monthTarget', 'targetParams', 'monthlyTargetId', 'mode', 'targetService',
        'notificationService', 'confirmationModalService', 'configService', 'utilityService',
        function ($scope, $uibModalInstance, monthTarget, targetParams, monthlyTargetId, mode, targetService,
            notificationService, confirmationModalService, configService, utilityService) {


            var self = this,
            targetResources = configService.getUIResources('Target');

            self.mode = monthTarget.isPastMonth? "view": mode;
            self.monthlyTarget = {};
            self.showRolledUpTarget = false;
            self.dailyTargets = [];
            self.dynamicPopover = {
                targets: monthTarget,
            };

            self.isEditMode = self.mode === "manage" ? true : false;
            if (targetParams.showDailyAndMonthly) {
                // if daily target entry, daily rate will be visible if monthly goal value is null or empty
                if (monthTarget.targetEntryMethod === 0) {
                    self.isTargetEntryDaily = (monthTarget.goalValue == null || monthTarget.goalValue == '');
                    self.isTargetEntryMonthly = (monthTarget.goalValue != null && monthTarget.goalValue != '');
                }
                else {
                    self.isTargetEntryDaily = (monthTarget.dailyRateValue != null && monthTarget.dailyRateValue != '');
                    self.isTargetEntryMonthly = (monthTarget.goalValue != null && monthTarget.goalValue != '');
                }
            }
            else {
                self.isTargetEntryDaily = monthTarget.targetEntryMethod === 0;
                self.isTargetEntryMonthly = monthTarget.targetEntryMethod === 1;
            }
            
            self.metricDataType = targetParams.metricDataTypeId;
            self.metricDataTypeName = self.metricDataType == 0 ? "wholeNumber" : "decimal";


            self.calculateSumofDailyTargets = function (dailyTargets) {
                if (!dailyTargets) {
                    dailyTargets = self.dailyTargets;
                }
                var sum = 0;

                dailyTargets.forEach(function (targetItem) {
                    sum += targetItem.goalValue;
                });

                return Math.round(sum * 100) / 100;
            };

            self.checkDailyTargetsAgainstMonthlyTargets = function () {

                var sumOfDailyTargets = self.calculateSumofDailyTargets();
                if (sumOfDailyTargets == self.monthlyTarget.goalValue) {
                    return true;

                } else {
                    return false;
                }

            };

            self.applyDailyRate = function () {
                self.dailyTargets.forEach(function (target) {
                    if(!target.isHoliday && !target.isOutofRange){
                        target.goalValue = self.monthlyTarget.dailyRateValue;
                    }
                });
            };

            //calendar popup close
            self.cancel = function () {
                $uibModalInstance.dismiss('cancel');
            };

            //calendar modal popup save 
            self.ok = function () {
                if (self.isTargetEntryMonthly) {
                    // if daily targets total and monthly goals match
                    if (self.checkDailyTargetsAgainstMonthlyTargets()) {
                        self.monthlyTarget.dailyTargets = self.dailyTargets;
                        $uibModalInstance.close(self.monthlyTarget);
                    }
                        // if daily target total does not match monthly goals, ask confirmation
                    else {
                        var confirmationModalInstance = confirmationModalService.openConfirmationModal('md',
                                        targetResources.checkDailyTargetAgainstMonthlyTarget.format(self.calculateSumofDailyTargets()));
                        confirmationModalInstance.result.then(function () {
                            self.monthlyTarget.goalValue = self.calculateSumofDailyTargets()
                            self.monthlyTarget.dailyTargets = self.dailyTargets;

                            $uibModalInstance.close(self.monthlyTarget);
                        }, function () {
                        });
                    }
                }
                else {
                    self.monthlyTarget.dailyTargets = self.dailyTargets;
                    $uibModalInstance.close(self.monthlyTarget);
                }
            };

            //service call to check/get any daily targets
            var loadExistingDailyTargets = function (monthlyTargetId) {
                targetService.getDailyTargets(monthlyTargetId).then(function (data) {
                    if (data) {
                        self.dailyTargets = data;
                        if (self.isEditMode && getNumberOfAvailableTargetDays(self.dailyTargets) != getNumberofTargetDays(monthTarget.month.year, monthTarget.month.id)) {
                            generateDailyTargets(targetParams, monthTarget);
                        }
                    }
                },
                function (err) {
                });
            };

            //generates new daily targets 
            var generateDailyTargets = function (targetParams, monthlyTarget) {

                targetService.generateDailyTargets(targetParams, monthlyTarget).then(function (data) {
                    if (data) {
                        self.dailyTargets = data;
                    }
                },
                    function (err) {
                    });

            };

            var getNumberofTargetDays = function (year, month) {
                var _date = new Date(year, month - 1, 1),
                    _year = _date.getFullYear(),
                    _month = _date.getMonth();

                var effectiveStartDate = utilityService.convertToDateWithoutTimezone(new Date(targetParams.effectiveStartDate));
                var effectiveEndDate = utilityService.convertToDateWithoutTimezone(new Date(targetParams.effectiveEndDate));
                var noOfDays = new Date(_year, _month + 1, 0).getDate();
                if (effectiveStartDate.getMonth() == _month && effectiveStartDate.getFullYear() == _year) {
                    noOfDays = noOfDays - effectiveStartDate.getDate() + 1;
                }
                else if (effectiveEndDate.getMonth() == _month && effectiveEndDate.getFullYear() == _year) {
                    noOfDays = effectiveEndDate.getDate();
                }
                return noOfDays;

            };

            var getNumberOfAvailableTargetDays = function (dailyTargets) {
               var numberOfAvailableDays = $.grep(dailyTargets, function (elt) {
                    return elt.isOutofRange == false;
                }).length;
                return numberOfAvailableDays;
            };


            // load or generate daily targets according to case
            var loadDailyTargets = function () {
                self.monthlyTarget.goalValue = monthTarget.goalValue;
                self.monthlyTarget.rolledupGoalValue = monthTarget.rolledupGoalValue;
                self.monthlyTarget.dailyRateValue = monthTarget.dailyRateValue;
                self.monthlyTarget.hasManualTarget = monthTarget.hasManualTarget;
                self.monthlyTarget.isManualTarget = monthTarget.isManualTarget;
                self.monthlyTarget.hasDailyTarget = monthTarget.hasDailyTarget;
                self.monthlyTarget.hasRolledUpDailyTarget = monthTarget.hasRolledUpDailyTarget;

                if (self.isEditMode) {
                    if (!monthTarget.dailyTargets || monthTarget.dailyTargets.length === 0) {
                        if (monthlyTargetId && self.monthlyTarget.hasDailyTarget && !self.monthlyTarget.isManualTarget) {
                            loadExistingDailyTargets(monthlyTargetId);
                            
                        }
                        else if (!self.monthlyTarget.hasDailyTarget || self.monthlyTarget.isManualTarget) {
                            generateDailyTargets(targetParams, monthTarget);
                        }
                    }
                    else if(getNumberOfAvailableTargetDays(monthTarget.dailyTargets) != getNumberofTargetDays(monthTarget.month.year, monthTarget.month.id)){
                        generateDailyTargets(targetParams, monthTarget);
                    }
                    else if (targetParams.targetEntryMethod == 0) {
                        self.dailyTargets = monthTarget.dailyTargets;
                    }
                    else if (targetParams.targetEntryMethod == 1) {
                        if (monthTarget.goalValue != self.calculateSumofDailyTargets(monthTarget.dailyTargets)) {
                            generateDailyTargets(targetParams, monthTarget);
                        }
                        else {
                            self.dailyTargets = monthTarget.dailyTargets;
                        }
                    }
                }
                else if (monthlyTargetId) {
                    loadExistingDailyTargets(monthlyTargetId);
                }
            };


            //calendar modal popup init
            var init = function () {
                loadDailyTargets();
            }();
        }]);
});