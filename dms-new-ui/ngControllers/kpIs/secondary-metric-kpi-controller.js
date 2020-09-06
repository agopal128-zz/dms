'use strict'
define(['angularAMD','kpiLetter','kpiService','actualsService','configService'], function (angularAMD) {

    angularAMD.controller('secondaryMetricKpiController', ['$scope', 'kpiService', 'actualsService', 'configService', 'notificationService', '$uibModal', '$uibModalInstance', 'metricDetails',
        function ($scope, kpiService, actualsService, configService, notificationService, $uibModal, $uibModalInstance, metricDetails) {

            var self = this;
            self.metricDetails = metricDetails;

            self.isShowingGoal = true;
            self.day = metricDetails.day;
            self.kpiId = metricDetails.kpiId;
            self.yearId = metricDetails.yearId;
            self.kpiname = metricDetails.kpiname;
            self.monthId = metricDetails.monthId;
            self.currentMonthId = metricDetails.currentMonthId;
            self.currentYearId = metricDetails.currentYearId;
            self.targetId = metricDetails.targetId;
            self.metricDetails.metricType = "secondary";
            self.scorecardId = metricDetails.scorecardId;
            self.displayMonth = metricDetails.displayMonth;
            self.initialMonthArrayIndex = metricDetails.initialMonthArrayIndex;
            self.monthList = metricDetails.monthList;
            self.isAuthorizedUser = metricDetails.isAuthorizedUser;
            self.isClickOnMetricDisabled = false;
            self.isUserAuthorizedToAlterScorecardEntries = metricDetails.isUserAuthorizedToAlterScorecardEntries;
            self.isViewMode = metricDetails.isViewMode;
            self.isRefreshGraph = false;
            self.targetEntryMethodId = metricDetails.targetEntryMethodId;
            self.dailyRate = metricDetails.dailyRate;
            self.graphPlottingMethodId = metricDetails.graphPlottingMethodId
            self.mtdGoal = metricDetails.mtdGoal;

            self.changedValues = {};
            self.metricActualsIndicationData = {};
            self.actualModalData = {
                id: '',
                goalValue: '',
                stretchGoalValue: '',
                metricName: '',
                date: '',
                currentActual: '',
                status: '',
                day: '',
                targetId: '',
                month: '',
                year: '',
                metricDataTypeId: '',
                kpiId: self.kpiId,
                scorecardId: self.scorecardId,
                metricId: '',
                isMonthlyTracking: '',
                isPrimary: '',
                targetEntryMethodId: '',
                dailyRate: '',
                graphPlottingMethodId: '',
                mtdGoal: ''
            };

            var actualsResources = configService.getUIResources('Actuals');

            //popup for entering metric actuals
            var openActualsPopup = function (actualData) {
                var modalInstance = $uibModal.open({
                    animation: self.animationsEnabled,
                    templateUrl: 'ngViews/actuals/partials/primary-metric.min.html',
                    controller: 'ActualsController',
                    controllerAs: 'ctrl',
                    windowClass: 'actual-popup',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'md',
                    resolve: {
                        modalData: function () {
                            return self.actualModalData;
                        }
                    }
                }).result.then(function () {
                    displayChangedActualValues();
                    self.isClickOnMetricDisabled = false;
                }, function () {
                    // action on popup dismissal.
                    self.isClickOnMetricDisabled = false;
                });
            };

            //popup for mark holiday
            var openMarkHolidayPopup = function (actualData, primaryMetricDetails) {
                self.holidayModalData = self.actualModalData;
                var modalInstance = $uibModal.open({
                    animation: self.animationsEnabled,
                    templateUrl: 'ngViews/actuals/partials/mark-holiday.min.html',
                    controller: 'ActualsController',
                    controllerAs: 'ctrl',
                    windowClass: 'actual-popup',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'md',
                    resolve: {
                        modalData: function () {
                            self.holidayModalData.status = actualData.status;
                            self.holidayModalData.targetId = metricDetails.targetId;
                            self.holidayModalData.scorecardId = self.scorecardId;
                            self.actualModalData.isPrimary = false;
                            self.actualModalData.isMonthlyTracking = self.isMonthlyTracking;
                            if (!self.isMonthlyTracking)
                                self.actualModalData.day = actualData.day;
                            self.holidayModalData.date =
                            new Date(self.displayMonth.details.year, self.displayMonth.details.id - 1, actualData.day);
                            self.actualModalData.targetEntryMethodId = self.targetEntryMethodId;
                            self.actualModalData.dailyRate = self.dailyRate;
                            self.actualModalData.graphPlottingMethodId = self.graphPlottingMethodId;
                            self.actualModalData.mtdGoal = self.mtdGoal;
                            return self.holidayModalData;
                        }
                    }
                }).result.then(function () {
                    self.isRefreshGraph = true;
                    displayChangedActualValues();
                    self.isClickOnMetricDisabled = false;
                }, function () {
                    // action on popup dismissal.
                    self.isClickOnMetricDisabled = false;
                });
            };

            // fetch actual entry data for secondary kpi letter
            var loadSecondaryMetricDetailedData = function (kpiId, year, month, targetId) {
                kpiService.getDetailedSecondaryMetricData(kpiId, year, month, targetId).then(function (data) {

                    self.metricActualsIndicationData = data;
                    self.metricActualsIndicationData.kpiId = self.kpiId;

                    if (self.metricActualsIndicationData.monthlyActuals) {

                        self.metricDetails.month = self.metricActualsIndicationData.monthlyActuals[0].month;
                        self.isMonthlyTracking = true;
                    }
                    else {
                        self.metricDetails.month = month;
                        self.isMonthlyTracking = false;
                    }
                    self.metricDetails.targetEntryMethodId = data.targetEntryMethodId;
                    self.metricDetails.dailyRate = data.dailyRate;                    
                    self.metricDetails.graphPlottingMethodId = data.graphPlottingMethodId;
                    self.metricDetails.mtdGoal = data.mtdGoal; 
                }, function (err) {
                    notificationService.notify(err.errors.join("<br/>"),
                    { autoClose: false, type: "danger" });
                });

            },

            // validate conditions and open actual entry popup
            validateToOpenMetricActualPopup = function (actualData, metricDetails) {
                if (actualData.status != 4) {

                    var canOpenActualEntryPopup = checkForMetricActualOpenConditions(actualData, metricDetails);
                    if (canOpenActualEntryPopup) {

                        formActualPopupModel(actualData, metricDetails);
                    }
                    else {
                        self.isClickOnMetricDisabled = false;
                    }
                }
                else {
                    self.isClickOnMetricDisabled = false;
                }
            },

            // validate conditions and open holiday popup
            validateToOpenHolidayPopup = function (actualData, metricDetails) {
                //status = 4 means NotApplicable (before or after effective dates).
                if (actualData.status != 4) {

                    var selectedArrayIndex = self.monthList.indexOf(self.displayMonth.details);
                    //we can mark/unmark holiday only for previous month,current month and beyond that.
                    var canOpenHolidayPopup = selectedArrayIndex >= self.initialMonthArrayIndex || selectedArrayIndex == self.initialMonthArrayIndex - 1;
                    canOpenHolidayPopup = canOpenHolidayPopup;
                    if (canOpenHolidayPopup) {
                        openMarkHolidayPopup(actualData, metricDetails);
                    }
                    else {
                        self.isClickOnMetricDisabled = false;
                    }
                }
                else {
                    self.isClickOnMetricDisabled = false;
                }
            },

            // check for actual entry modal opening conditions.
            checkForMetricActualOpenConditions = function (actualData, metricDetails) {
                var isOpen;
                if (actualData.status != 3) {
                    isOpen = true;
                }
                // in case of daily tracking
                if (actualData.day && metricDetails.month) {
                    if (actualData.day > self.day && metricDetails.month == self.currentMonthId) {
                        isOpen = false;
                    }
                }
                    //in case of monthly tracking
                else if (actualData.month) {
                    var previousYearId = self.currentYearId - 1;
                    var prevMonth = self.currentMonthId === 1 ? 12 : self.currentMonthId - 1;
                    if (actualData.month == self.monthId) {
                        isOpen = true;
                    }
                    else if (actualData.month == prevMonth) {
                        // date check added to prevent actual entry for current year December
                        if (prevMonth != 12) {
                            isOpen = true;
                        }
                        else {
                            if (previousYearId == self.yearId) {
                                isOpen = true;
                            }
                            else {
                                isOpen = false;
                            }
                        }
                    }
                    else {
                        isOpen = false;
                    }
                }
                isOpen = (isOpen && !self.isViewMode) ? true : false;
                return isOpen;

            },

            //form actual entry popup data model
            formActualPopupModel = function (actualData, metricDetails) {
                if (!metricDetails.isCascaded) {
                    self.actualModalData.metricName = metricDetails.metricName;
                    self.actualModalData.currentActual = actualData.value;
                    self.actualModalData.status = actualData.status;
                    self.actualModalData.targetId = metricDetails.targetId;
                    self.actualModalData.id = actualData.id;
                    self.actualModalData.day = actualData.day;
                    self.actualModalData.metricDataTypeId = metricDetails.metricDataTypeId;
                    self.actualModalData.isPrimary = false;
                    self.actualModalData.metricId = metricDetails.metricId;
                    self.actualModalData.kpiId = metricDetails.kpiId;
                    self.actualModalData.scorecardId = self.scorecardId;
                    self.actualModalData.month = metricDetails.month;
                    if (self.isMonthlyTracking) {
                        self.actualModalData.goalValue = metricDetails.goalValue;
                        self.actualModalData.isMonthlyTracking = self.isMonthlyTracking;
                        self.actualModalData.month = actualData.month;
                        self.actualModalData.date = new Date(self.displayMonth.details.year, actualData.month - 1, self.day);
                    }
                    else {
                        self.actualModalData.date = new Date(self.displayMonth.details.year, self.displayMonth.details.id - 1, actualData.day);
                    }
                    getTargets(metricDetails, actualData);
                }
                else {
                    self.isClickOnMetricDisabled = false;
                    notificationService.notify(actualsResources.CannotEnterActualForcCscaded,
                        { autoClose: false, type: "danger" });
                }
                self.actualModalData.targetEntryMethodId = metricDetails.targetEntryMethodId; 
                self.actualModalData.dailyRate = metricDetails.dailyRate;
                self.actualModalData.graphPlottingMethodId = metricDetails.graphPlottingMethodId;
                self.actualModalData.mtdGoal = metricDetails.mtdGoal;
            },

            getTargets = function (metricDetails, actualData) {
                var month = self.isMonthlyTracking ? actualData.month : metricDetails.month;
                actualsService.getTargets(metricDetails.targetId, month, actualData.day)
                    .then(function (data) {
                            self.actualModalData.goalValue = data;
                            openActualsPopup();
                    }, function (err) {
                        self.isClickOnMetricDisabled = false;
                        notificationService.notify(err.errors.join("<br/>"),
                        { autoClose: false, type: "danger" });
                    });
            },
            displayChangedActualValues = function () {
                loadSecondaryMetricDetailedData(self.kpiId, self.yearId, self.monthId, self.targetId);
            };
            
            self.cancel = function () {
                notificationService.close();
                self.changedValues.targetId = self.targetId;
                self.changedValues.isRefreshGraph = self.isRefreshGraph;
                $uibModalInstance.close(self.changedValues);
            };
            
            self.showSecondaryMetricActualPopup = function (actualData, type, metricDetails) {
                if(self.isAuthorizedUser && self.isUserAuthorizedToAlterScorecardEntries)
                {
                    if (self.isClickOnMetricDisabled) {
                        return;
                    }
                    self.isClickOnMetricDisabled = true;
                    self.actualModalData = {};
                    switch (type) {
                        case "secondary": validateToOpenMetricActualPopup(actualData, metricDetails);
                            break;
                        case "holiday": validateToOpenHolidayPopup(actualData, metricDetails);
                            break;
                    }
                }
                else if (self.isAuthorizedUser) {
                    notificationService.notify(actualsResources.NotAuthorizedToChange,
                       { autoClose: true, type: "danger" });
                }
            };


            var init = function () {
                loadSecondaryMetricDetailedData(self.kpiId, self.yearId, self.monthId, self.targetId);
            }();

        }]);
});