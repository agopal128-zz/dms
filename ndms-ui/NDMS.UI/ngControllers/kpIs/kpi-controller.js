'use strict';
define(['angularAMD', 'kpiLetter', 'counterMeasure', 'kpiService', 'actualsController', 'counterMeasureController', 'secondaryMetricKpiController',
    'actualsService', 'configurationService', 'highchart', 'graphDirective', 'validNumber', 'metricType','notificationModalService'],
    function (angularAMD) {
        angularAMD.controller('kpiController', ['$routeParams', 'kpiService', 'actualsService',
        '$uibModal', 'notificationService', '$location', '$scope', '$filter', '$q',
        'configurationService', 'sessionStorageService', '$rootScope', 'authService', 'configService', '$timeout', 'notificationModalService', 'monthNavigationService',
        function ($routeParams, kpiService, actualsService, $uibModal, notificationService,
            $location, $scope, $filter, $q, configurationService, sessionStorageService,
            $rootScope, authService, configService, $timeout, notificationModalService, monthNavigationService) {

                var self = this;
                self.isShowingGoal = true;
                self.kpiId = parseInt($routeParams.kpiId);
                self.scorecardId = parseInt($routeParams.scorecardId);
                self.yearId = parseInt($routeParams.yearId);
                self.monthId = parseInt($routeParams.monthId);                
                self.day = parseInt($routeParams.day);
                self.isViewMode = false;
                self.prevMonthId = self.monthId - 1;                
                self.isGraphDataLoaded = false;
                self.graphPlottingPage = 'kpi';

                $rootScope.scorecardId = self.scorecardId;
                $rootScope.disableHeader = true;
                $rootScope.navigateToScorecard = function () {
                    if (angular.isUndefined($rootScope.scorecardYearId)) {
                        $rootScope.scorecardYearId = self.yearId;
                    }
                    if (angular.isUndefined($rootScope.scorecardMonthId)) {
                        $rootScope.scorecardMonthId = self.monthId;
                    }
                    if ($rootScope.scorecardId) {
                        $location.url('Scorecard/' + $rootScope.scorecardId + '/' + $rootScope.scorecardYearId + '/' + $rootScope.scorecardMonthId).replace();
                        if (!$scope.$$phase) $scope.$apply();
                    }
                };
            var scorecardName = sessionStorageService.get('scorecardName');
            if (scorecardName) {
                $rootScope.title = "NDMS Scorecard : " + sessionStorageService.get('scorecardName');
            }
            self.goalText = '';
            self.stretchGoalText = '';
            self.animationsEnabled = true;
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
                mtdGoal:''
            };
            self.primaryMetricDetails = {
            };
            self.kpiIndicationData = {};
            self.fiscalMonthData = {};
            self.monthList = {};
            self.currentDate = '';
            self.displayMonth = {
                id: '',
                details: ''
            };
            self.primaryStretchGoalValue = '';
            self.primaryGoalValue = null;
            var actualsResources = configService.getUIResources('Actuals');
            var scorecardResources = configService.getUIResources('Scorecard');

            self.graphData = {};
            self.metricDetails = [];            
            self.actualData = {
                scorecardId: '',
                kpiId: '',
                metricId: ''
            };

            // Graph plotting for primary & secondary metrics
            self.scorecardMetricTargetId = 0;
            self.scorecardKpiMetrics = [];

            //get kpi names from kpiId
            var getKpiName = function (kpiId) {
                switch (kpiId) {
                    case 0: return 'Safety';
                    case 1: return 'Quality';
                    case 2: return 'Delivery';
                    case 3: return 'Innovation';
                    case 4: return 'Cost';
                    case 5: return 'People [Culture]';
                    case 6: return 'Revenue';
                    case 7: return 'Net Working Capital'; 
                }
            };
            self.kpiname = getKpiName(self.kpiId);

            // Load counter measure status legends
            var loadCounterMeasureStatusLegends = function () {
                kpiService.getAllCounterMeasureStatus().then(function (data) {
                    if (data.data) {
                        self.statusLegends = data.data;
                        for (var i = 0; i < self.statusLegends.length; i++) {
                            var statusId = self.statusLegends[i].id;
                            setStatus(i, statusId);
                        }
                    }
                }, function (err) {
                    notificationService.notify(err.errors.join("<br/>"),
                    {
                        autoClose: false, type: "danger"
                    });
                });
            };
            // set counter measure status on individual pages
            var setStatus = function (index, statusId) {
                switch (statusId) {
                    case 0: self.statusLegends[index].counterMeasureStatus = 'status statusnoprogress';
                        break;
                    case 1: self.statusLegends[index].counterMeasureStatus = 'status statusinvestigation';
                        break;
                    case 2: self.statusLegends[index].counterMeasureStatus = 'status statusidentified';
                        break;
                    case 3: self.statusLegends[index].counterMeasureStatus = 'status statusimplemented';
                        break;
                    case 4: self.statusLegends[index].counterMeasureStatus = 'status statusconfirmed';
                        break;
                }
            };

            // Load counter measure on individual pages
            var loadCounterMeasures = function (scorecardId, kpiId, isShowClosed) {
                kpiService.getCounterMeasures(scorecardId, kpiId, isShowClosed).then(function (data) {
                    self.counterMeasureData = data;
                    if (self.counterMeasureData.length > 0) {
                        loadCounterMeasureStatusLegends();
                    }
                }, function (err) {
                    notificationService.notify(err.errors.join("<br/>"),
                    {
                        autoClose: false, type: "danger"
                    });
                });
            };

            //display changed actual values and status directly from modal popup
            var displayChangedActualValues = function (changedValues) {
                loadScorecardKPIData(self.scorecardId, self.kpiId, self.yearId, self.monthId, false);
                
                //fetching updated fiscal month status,graph and counter measure
                getFiscalMonthStatus(self.scorecardId, self.kpiId, self.yearId);
                // fetching updates counter measures
                loadCounterMeasures(self.scorecardId, self.kpiId, false);
                if (changedValues.targetId == self.scorecardMetricTargetId) {
                    self.loadGraphForMetric();
                }
                else if (changedValues.isRefreshGraph) {
                    self.loadGraphForMetric();
                }

                // fetch updates recordables if KPi is Safety
                if (self.kpiId === 0) {
                    loadBasicScorecardData(self.scorecardId, self.yearId, self.monthId);
                }

                if (!$scope.$$phase) $scope.$apply();
            };

            //popup for entering primary actuals
            var openPrimaryActualsPopup = function (actualData) {
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
                }).result.then(function (changedValues) {
                    changedValues.isRefreshGraph = false;
                    displayChangedActualValues(changedValues);
                    self.isClickOnMetricDisabled = false;
                }, function () {
                    // action on popup dismissal.
                    self.isClickOnMetricDisabled = false;
                });
            },
            
             //popup for mark holiday
            openMarkHolidayPopup = function (actualData, primaryMetricDetails) {
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
                            self.holidayModalData.targetId = primaryMetricDetails.targetId;
                            self.holidayModalData.scorecardId = self.scorecardId;
                            self.actualModalData.isPrimary = true;
                            self.actualModalData.isMonthlyTracking = self.isMonthlyTracking;
                            self.actualModalData.dailyRate = primaryMetricDetails.dailyRate;
                            if (!self.isMonthlyTracking)
                                self.actualModalData.day = actualData.day;
                            self.holidayModalData.date =
                            new Date(self.displayMonth.details.year, self.displayMonth.details.id - 1, actualData.day);
                            self.actualModalData.targetEntryMethodId = primaryMetricDetails.targetEntryMethodId;
                            self.actualModalData.graphPlottingMethodId = primaryMetricDetails.graphPlottingMethodId;
                            return self.holidayModalData;
                        }
                    }
                }).result.then(function (changedValues) {
                    changedValues.isRefreshGraph = true;
                    displayChangedActualValues(changedValues);
                    self.isClickOnMetricDisabled = false;
                }, function () {
                    // action on popup dismissal.
                    self.isClickOnMetricDisabled = false;
                });
            },

            //popup for adding counter measure from table
            addCounterMeasure = function () {
                var modalInstance = $uibModal.open({
                    animation: self.animationsEnabled,
                    templateUrl: 'ngViews/counter-measure/partials/counter-measure-popup.min.html',
                    controller: 'counterMeasureController',
                    controllerAs: 'ctrl',
                    windowClass: 'counter-measure-popup',
                    backdrop: 'static',
                    keyboard: false,
                    resolve: {
                        metricDetails: function () {
                            return self.metricDetails;
                        },
                        actualData: function () {
                            return self.actualData;
                        },
                        counterMeasure: function () {
                            return;
                        }
                    }
                }).result.then(function () {
                    loadCounterMeasures(self.scorecardId, self.kpiId, false);
                });
            },

            //To get daily/monthly target for primary metric
            getTargets = function (primaryMetricDetails, actualData) {

                var month = self.isMonthlyTracking ? actualData.month : primaryMetricDetails.month;
                actualsService.getTargets(primaryMetricDetails.targetId, month, actualData.day)
                                           .then(function (data) {
                                                   self.actualModalData.goalValue = data;
                                                   openPrimaryActualsPopup();
                                           }, function (err) {
                                               self.isClickOnMetricDisabled = false;
                                               notificationService.notify(err.errors.join("<br/>"),
                                               { autoClose: false, type: "danger" });
                                           });
            },

             //forming modal data for primary actuals
            formPrimaryPopupModel = function (actualData, primaryMetricDetails) {
                if (!primaryMetricDetails.isCascaded) {
                    self.actualModalData.metricName = primaryMetricDetails.metricName;
                    self.actualModalData.currentActual = actualData.value;
                    self.actualModalData.status = actualData.status;
                    self.actualModalData.targetId = primaryMetricDetails.targetId;
                    self.actualModalData.id = actualData.id;
                    self.actualModalData.day = actualData.day;
                    self.actualModalData.metricDataTypeId = primaryMetricDetails.metricDataTypeId;
                    self.actualModalData.isPrimary = true;
                    self.actualModalData.metricId = primaryMetricDetails.metricId;
                    self.actualModalData.kpiId = self.kpiId;
                    self.actualModalData.scorecardId = self.scorecardId;
                    self.actualModalData.month = primaryMetricDetails.month;
                    self.actualModalData.dailyRate = primaryMetricDetails.dailyRate;

                    if (self.isMonthlyTracking) {
                        self.actualModalData.goalValue = primaryMetricDetails.monthlyGoalValue;
                        self.actualModalData.isMonthlyTracking = self.isMonthlyTracking;
                        self.actualModalData.month = actualData.month;
                        self.actualModalData.date = new Date(self.displayMonth.details.year, actualData.month - 1, self.day);
                    }
                    else {
                        self.actualModalData.date = new Date(self.displayMonth.details.year, self.displayMonth.details.id - 1, actualData.day);
                    }
                    self.actualModalData.mtdGoal = primaryMetricDetails.monthlyGoalValue;
                    getTargets(primaryMetricDetails, actualData);
                }
                else {
                    self.isClickOnMetricDisabled = false;
                    notificationService.notify(actualsResources.CannotEnterActualForcCscaded,
                        { autoClose: false, type: "danger" });
                }
            };

            //to check whether actual popups to be enabled only for current and previous month and disabled if holiday
            var checkForPrimaryMetricActualOpenConditions = function (actualData, primaryMetricDetails) {
                var isOpen;
                if (actualData.status != 3) {
                    isOpen = true;
                }
                //daily tracking 
                if (actualData.day && primaryMetricDetails.month) {
                    if (actualData.day > self.day && primaryMetricDetails.month == self.currentMonthId) {
                        isOpen = false;
                    }
                }
                    //monthly tracking 
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
            };

            //check whether primary actual popup up can be opened 
            var validateToOpenPrimaryMetricActualPopup = function (actualData, metricDetails) {
                if (actualData.status != 4) {
                    var canOpenPrimary = checkForPrimaryMetricActualOpenConditions(actualData, metricDetails);
                    canOpenPrimary = canOpenPrimary;
                    if (canOpenPrimary) {
                        formPrimaryPopupModel(actualData, metricDetails);
                    }
                    else {
                        self.isClickOnMetricDisabled = false;
                    }
                }
                else {
                    self.isClickOnMetricDisabled = false;
                }
            };
           
            //check whether holiday popup up can be opened 
            var validateToOpenHolidayPopup = function (actualData, metricDetails) {
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
            };

            //for choosing which popup to be displayed
            self.showActualsOrHolidayPopup = function (actualData, type, metricDetails) {
                if (self.isAuthorizedUser && self.isUserAuthorizedToAlterScorecardEntries) {

                    // prevent clicking on metric without dismissing popup or entering actuals/mark holiday.
                    if (self.isClickOnMetricDisabled) {
                        return;
                    }
                    // This variable will be set to false in all conditions where popup not showing after setting it true.
                    self.isClickOnMetricDisabled = true;

                    self.actualModalData = {};
                    switch (type) {
                        case "primary":
                            validateToOpenPrimaryMetricActualPopup(actualData, metricDetails);
                            break;
                        case "holiday":
                            validateToOpenHolidayPopup(actualData, metricDetails);
                            break;
                    }
                }
                else if (self.isAuthorizedUser) {
                    notificationService.notify(actualsResources.NotAuthorizedToChange,
                        { autoClose: true, type: "danger" });
                }
            };

            //popup for secondary metric detailed view.
            self.openSecondaryMetricDetailedPopup = function (metricDetails) {
                var modalInstance = $uibModal.open({
                    animation: self.animationsEnabled,
                    templateUrl: 'ngViews/kpIs/partials/secondary-metric-kpi.min.html',
                    controller: 'secondaryMetricKpiController',
                    controllerAs: 'ctrl',
                    windowClass: 'secondary-metric-popup',
                    backdrop: 'static',
                    keyboard: false,
                    resolve: {
                        metricDetails: function () {
                            metricDetails.scorecardId = self.scorecardId;
                            metricDetails.kpiId = self.kpiId;
                            metricDetails.kpiname = self.kpiname;
                            metricDetails.day = self.day;
                            metricDetails.monthId = self.monthId;
                            metricDetails.yearId = self.yearId;
                            metricDetails.monthList = self.monthList;
                            metricDetails.displayMonth = self.displayMonth;
                            metricDetails.currentMonthId = self.currentMonthId;
                            metricDetails.currentYearId = self.currentYearId;
                            metricDetails.initialMonthArrayIndex = self.initialMonthArrayIndex;
                            metricDetails.isAuthorizedUser = self.isAuthorizedUser,
                            metricDetails.isUserAuthorizedToAlterScorecardEntries = self.isUserAuthorizedToAlterScorecardEntries;
                            metricDetails.isViewMode = self.isViewMode;
                            return metricDetails;
                        }
                    }

                }).result.then(function (changedValues) {
                    displayChangedActualValues(changedValues);
                    self.isClickOnMetricDisabled = false;
                }, function () {
                    // action on popup dismissal.
                    self.isClickOnMetricDisabled = false;
                });
            };

            //To add counter measure from table
            self.proceedToAddCounterMeasure = function () {
                self.actualData = {
                    scorecardId: self.scorecardId,
                    kpiId: self.kpiId,
                    metricId: ''
                };
                addCounterMeasure();
            };      

        //data formatting for individual kpi pages
        var extractPrimaryMetricData = function (kpiData, month) {

            if (kpiData && kpiData.primaryMetricData) {
                self.primaryMetricDetails.metricName = kpiData.primaryMetricData.metricName;
                self.primaryMetricDetails.targetId = kpiData.primaryMetricData.targetId;
                self.primaryMetricDetails.metricDataTypeId = kpiData.primaryMetricData.metricDataTypeId;
                self.primaryMetricDetails.metricId = kpiData.primaryMetricData.metricId;
                self.primaryMetricDetails.isCascaded = kpiData.primaryMetricData.isCascaded;
                self.primaryMetricDetails.monthlyGoalValue = kpiData.primaryMetricData.monthlyGoalValue;
                self.primaryMetricDetails.metricType = 'primary';
                self.primaryGoalValue = kpiData.primaryMetricData.monthlyGoalValue;
                self.primaryStretchGoalValue = kpiData.primaryMetricData.monthlyStretchGoalValue;
                self.primaryMetricDetails.dailyRate = kpiData.primaryMetricData.dailyRate;
                self.primaryMetricDetails.targetEntryMethodId = kpiData.primaryMetricData.targetEntryMethodId;
                self.primaryMetricDetails.graphPlottingMethodId = kpiData.primaryMetricData.graphPlottingMethodId;
                if (kpiData.primaryMetricData.monthlyActuals) {
                    self.primaryMetricDetails.month = kpiData.primaryMetricData.monthlyActuals[0].month;
                    self.isMonthlyTracking = true;
                }
                else {
                    self.primaryMetricDetails.month = month;
                    self.isMonthlyTracking = false;
                }                
            }
            else {
                self.primaryStretchGoalValue = self.primaryGoalValue = null;
                self.primaryMetricDetails.metricName = "";
            }
        };

        // populates metric dropdown
        var populateMetricDropdown = function (kpiData) {
            self.scorecardKpiMetrics = [];
            self.scorecardMetricTargetId = 0;
            if (!kpiData) {
                self.isGraphDataLoaded = true;
                self.isGraphData = false;
            }

            if (kpiData) {
                if (kpiData.primaryMetricData) {
                    self.scorecardKpiMetrics.push({
                        id: kpiData.primaryMetricData.targetId,
                        name: kpiData.primaryMetricData.metricName
                    });

                    self.scorecardMetricTargetId = kpiData.primaryMetricData.targetId;
                }

                var sortedSecondaryMetrices = [];
                if (kpiData.secondaryMetricsData) {
                    sortedSecondaryMetrices = $filter('orderBy')(kpiData.secondaryMetricsData, 'metricName');
                }

                angular.forEach(sortedSecondaryMetrices, function (metric) {
                    self.scorecardKpiMetrics.push({
                        id: metric.targetId,
                        name: metric.metricName
                    });
                });

                if (self.scorecardMetricTargetId == 0 && self.scorecardKpiMetrics.length > 0) {
                    self.scorecardMetricTargetId = self.scorecardKpiMetrics[0].id;
                }

                if (self.scorecardMetricTargetId != 0) {
                    self.loadGraphForMetric();
                }
                else {
                    self.isGraphDataLoaded = true;
                    self.isGraphData = false;
                }
            }
        }

        var loadScorecardKPIData = function (scorecardId, kpiId, yearId, month, isRepopulateDropdown) {
            kpiService.getScorecardKPIData(scorecardId, kpiId, yearId, month).then(function (data) {
                self.kpiIndicationData = data;
                $rootScope.disableHeader = false;
                extractPrimaryMetricData(data, month);
                if (isRepopulateDropdown) {
                    populateMetricDropdown(data);
                }
            },
            function (err) {
                $rootScope.disableHeader = false;
                notificationService.notify(err.errors.join("<br/>"),
                { autoClose: false, type: "danger" });
            });
        };

        var loadBasicScorecardData = function (scorecardId, yearId, month) {

            kpiService.getBasicScorecardData(scorecardId, yearId, month).then(function (data) {
                        self.kpiOwners = JSON.stringify(data.kpiOwners);
                        sessionStorageService.set('scorecardName', data.scorecardName);
                        sessionStorageService.set('scorecardId', scorecardId);
                        $rootScope.kpiOwners = JSON.parse(self.kpiOwners);
                        $rootScope.daysWithoutRecordables = data.daysWithoutRecordables;
                        $rootScope.title = "NDMS Scorecard : " + data.scorecardName;
                        if (!data.isPatternAssigned) {
                            $timeout(function () {
                                notificationService.notify(scorecardResources.PatternNotSetError.format(self.displayMonth.details.name, self.displayMonth.details.year)
                                , { autoClose: false, type: "danger" })
                            }, 1000);
                        }
                    },
                    function (err) {
                        notificationService.notify(err.errors.join("<br/>"),
                        { autoClose: false, type: "danger" });
                    });
        };

        var getFiscalMonthStatus = function (scorecardId, kpiId, yearId) {
            kpiService.getFiscalMonthStatusForKPI(scorecardId, kpiId, yearId).then(function (data) {
                self.fiscalMonthData = data;
            },
            function (err) {
                notificationService.notify(err.errors.join("<br/>"),
                { autoClose: false, type: "danger" });
            });
        };

        //service for rendering available months to UI 
        var loadCurrentAndPreviousYearMonthsList = function () {
            configurationService.getAllYearMonthsList().then(function (data) {
                self.monthList = data;

                monthNavigationService.setMonthDetails(data, self.monthId, self.yearId);
                $rootScope.displayMonth = monthNavigationService.getSelectedMonthDetails();

                self.displayMonth.details = monthNavigationService.getSelectedMonthDetails();

                // get current month details
                self.currentMonthDetails = $.grep(self.monthList, function (obj) {
                    return obj.yearId === self.currentYearId && obj.id == self.currentMonthId;
                })[0];

                self.initialMonthArrayIndex = self.monthList.indexOf(self.currentMonthDetails);
                self.monthArrayIndex = self.monthList.indexOf(self.displayMonth.details);
                self.initialMonthArrayLength = self.monthArrayLength = self.monthList.length - 1;
                self.selectedYearId = self.displayMonth.details.yearId;
                changeEditMode();
            },
            function (err) {
                notificationService.notify(err.errors.join("<br/>"),
                { autoClose: false, type: "danger" });
            });
        };

        //load metrics belonging to a kpi in a scorecard
        var loadScorecardKPIMetrics = function (scorecardId, kpiId, yearId, monthId) {
            if (self.isAuthorizedUser) {
                kpiService.getScorecardKPIMetrics(scorecardId, kpiId, yearId, monthId).then(function (data) {
                    self.metricDetails = data;
                },
                function (err) {
                    notificationService.notify(err.errors.join("<br/>"),
                    { autoClose: false, type: "danger" });
                });
            }
        };


        //enable edit for current month and previous month only
        var changeEditMode = function () {
            var clickedMonthIndex = self.monthList.indexOf(self.displayMonth.details);
            if (clickedMonthIndex == self.initialMonthArrayIndex) {
                self.isViewMode = false;
            }
            else if (clickedMonthIndex == self.initialMonthArrayIndex - 1) {
                self.isViewMode = false;
            }
            else {
                self.isViewMode = true;
            }
        };

        //formatting data to plot graph
        var formatDataForGraph = function (data) {
            var tempData = self.graphData;
            var graphData;

            if (data.dailyGraphData) {
                graphData = data.dailyGraphData;
                tempData.xAxis.trackingMethode = 'daily';
            }
            else if (data.monthlyGraphData) {
                graphData = data.monthlyGraphData;
                tempData.xAxis.trackingMethode = 'monthly';
            }

            tempData.xAxis.max = graphData.length - 1;
            tempData.series.push({ data: [] });
            tempData.series.push({ data: [] });

            angular.forEach(graphData, function (item, index) {
                tempData.series[0].data.push(item.actualValue);
                tempData.series[2].data.push(item.goalValue);
                tempData.series[1].data.push(item.stretchGoalValue);
                tempData.xAxis.categories.push(index + 1);
            });

            tempData.yAxis.min = data.minValue;
            tempData.yAxis.max = data.maxValue;
            self.graphData = tempData;
            self.isGraphDataLoaded = true;
        };
        
        // to load graph for the selected metric
        self.loadGraphForMetric = function () {
            self.graphData = {};
            kpiService.getScorecardMetricKPIGraphData(self.scorecardMetricTargetId, self.kpiId, self.yearId, self.monthId)
                .then(function (data) {
                    self.isGraphDataLoaded = true;
                    if (data) {
                        if (!data.dailyGraphData && !data.monthlyGraphData) {
                            self.isGraphData = false;
                        }
                        else {
                            self.graphData = {
                                series: [{ data: [] }],
                                yAxis: { min: '', max: '', tickInterval: '' },
                                xAxis: { min: '', max: '', tickInterval: '', categories: [], trackingMethode: '' },
                            };
                            self.isGraphData = true;
                            formatDataForGraph(data);
                        }
                    }
                    else {
                        self.isGraphData = false;
                    }                    
                },
                function (err) {
                    notificationService.notify(err.errors.join("<br/>"),
                    { autoClose: false, type: "danger" });
                });
        }        
        
            //catch the broadcast event from parent controller
        $scope.$on('monthChange', function () {
            self.selectedYearId = $rootScope.displayMonth.yearId;
            self.displayMonth.details = $rootScope.displayMonth;
            changeEditMode();
            self.monthId = self.displayMonth.details.id;
            self.yearId = self.displayMonth.details.yearId;
            loadScorecardKPIData(self.scorecardId, self.kpiId, self.displayMonth.details.yearId, self.displayMonth.details.id, true);
            getFiscalMonthStatus(self.scorecardId, self.kpiId, self.displayMonth.details.yearId);
            loadCounterMeasures(self.scorecardId, self.kpiId, false);
            loadScorecardKPIMetrics(self.scorecardId, self.kpiId, self.yearId, self.monthId);
        });

        // called when show closed counter measure is clicked
        self.reloadCounterMeasureTable = function () {
            loadCounterMeasures(self.scorecardId, self.kpiId, self.counterMeasureIncludeClosed);
        }

        // check whether a role is available in logged-in user roles
        var isUserInRole = function (role, userRoles) {
            return userRoles.indexOf(role) > -1 ? true : false;
        };

        // validating the user session and roles
        var validateUser = function () {
            self.isAuthorized = authService.isAuthorized();
            if (self.isAuthorized) {
                validateUserAuthenticationAgainstScorecard(self.scorecardId);
                var userData = JSON.parse(sessionStorageService.get('userData'));
                if (userData) {
                    var isAdmin = isUserInRole('Admin', userData.roles),
                                isKPIOwner = isUserInRole('KPIOwner', userData.roles),
                                isTeamMember = isUserInRole('TeamMember', userData.roles);
                    self.isAuthorizedUser = self.isAuthorized && (isAdmin || isKPIOwner || isTeamMember);
                }
            }
        };

        var validateUserAuthenticationAgainstScorecard = function () {
            self.isUserAuthorizedToAlterScorecardEntries = false;
                actualsService.isUserAuthorizedToAlterScorecardEntries(self.scorecardId).then(function (data) {
                    self.isUserAuthorizedToAlterScorecardEntries = data;
            },
            function (err) {
                notificationService.notify(err.errors.join("<br/>"),
                { autoClose: false, type: "danger" });
            });
        };
        // drill down button click
        self.drillDown = function () {
            $location.path('Drilldown/' + self.scorecardId + "/" + self.kpiId + "/" + self.monthId + "/" + self.yearId);
            if (!$scope.$$phase) $scope.$apply();
        };

        // Retrieves current year and date
        var getYearAndMonth = function () {
            var deferred = $q.defer();
            configurationService.getCurrentDateAndYearId().then(
                function (data) {
                    var currentDate = data.currentDate.slice(0, 10);
                    self.currentMonthId = parseInt($filter('date')(currentDate, 'M'), 10);
                    self.currentYearId = data.yearId;
                    self.currentDay = $filter('date')(currentDate, 'dd');
                    deferred.resolve();
                },
                function (msg) {
                    deferred.reject(msg);
                });
            return deferred.promise;
        };

        //initial load function
        var init = function () {
            getYearAndMonth().then(function () {
                loadBasicScorecardData(self.scorecardId, self.yearId, self.monthId);
                loadScorecardKPIData(self.scorecardId, self.kpiId, self.yearId, self.monthId, true);
                getFiscalMonthStatus(self.scorecardId, self.kpiId, self.yearId);
                loadCurrentAndPreviousYearMonthsList();
                loadCounterMeasures(self.scorecardId, self.kpiId, false);
                validateUser();
                loadScorecardKPIMetrics(self.scorecardId, self.kpiId, self.yearId, self.monthId);                
            });
        }();
    }]);
});