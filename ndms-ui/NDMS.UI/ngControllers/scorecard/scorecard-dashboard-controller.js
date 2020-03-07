'use strict';
define(['angularAMD', 'kpiLetter', 'kpiService', 'highchart', 'graphDirective', 'dashboardHelper', 'utilityService'],
    function (angularAMD) {
        angularAMD.controller('scorecardDashboardController', ['$routeParams', 'kpiService', 'authService',
            'notificationService', '$location', 'configService', '$scope', 'sessionStorageService', '$rootScope',
            'configurationService', '$filter', '$q', 'utilityService', 'monthNavigationService', '$timeout',
            function ($routeParams, kpiService, authService, notificationService, $location, configService,
            $scope, sessionStorageService, $rootScope, configurationService, $filter, $q, utilityService, monthNavigationService, $timeout) {

                var self = this,
                id = sessionStorageService.get('scorecardId');
                self.scorecardId = parseInt($routeParams.id) ? parseInt($routeParams.id) : id;
                var scorecardResources = configService.getUIResources('Scorecard');
                if (!id && self.scorecardId) {
                    sessionStorageService.set('scorecardId', self.scorecardId);
                }

                self.scorecardData = {
                    scorecardId: '',
                    scorecardName: '',
                    kpiOwners: '',
                };

                self.kpiOwners = [];
                self.graphPlottingPage = 'scorecard';
                self.isGraphDataLoaded = {};
                self.isGraphData = {};

                $rootScope.disableHeader = true;

                var loadScorecardDataOfMonth = function (scorecardId, yearId, month) {
                    self.isGraphDataLoaded = {};
                    self.isGraphData = {};
                    kpiService.getScorecardData(scorecardId, yearId, month).then(function (data) {
                        self.kpiDashboardData = data;
                        self.scorecardData = {
                            scorecardId: parseInt(data.scorecardId),
                            scorecardName: data.scorecardName,
                            kpiOwners: data.kpiOwners,
                            daysWithoutRecordables: data.daysWithoutRecordables
                        };

                        getFiscalMonthStatusForScorecard(self.scorecardId,yearId);
                        loadGraphData(self.scorecardId, yearId, month);

                        if (!data.isPatternAssigned) {
                            $timeout(function () {
                                notificationService.notify(scorecardResources.PatternNotSetError.format($rootScope.displayMonth.name, $rootScope.displayMonth.year)
                                , { autoClose: false, type: "danger" })
                            }, 1000);
                        }
                    },
                    function (err) {
                        notificationService.notify(err.errors.join("<br/>"),
                        { autoClose: false, type: "danger" });
                    });
                };

                // Function to load current date and year
                var loadScorecardData = function (scorecardId, yearId, month) {
                    kpiService.getScorecardData(scorecardId, yearId, month).then(function (data) {
                        self.kpiDashboardData = data;
                        self.scorecardData = {
                            scorecardId: parseInt(data.scorecardId),
                            scorecardName: data.scorecardName,
                            kpiOwners: data.kpiOwners,
                            daysWithoutRecordables: data.daysWithoutRecordables
                        };
                    
                        $rootScope.title = "NDMS Scorecard : " + data.scorecardName;
                        $rootScope.kpiOwners = self.scorecardData.kpiOwners;
                        $rootScope.daysWithoutRecordables = self.scorecardData.daysWithoutRecordables;
                        $rootScope.disableHeader = false;
                        sessionStorageService.set('scorecardName', self.scorecardData.scorecardName);
                        getFiscalMonthStatusForScorecard(self.scorecardId, self.yearId);
                        loadGraphData(self.scorecardId, self.yearId, self.monthId);
                        if (!data.isPatternAssigned) {
                            $timeout(function () {
                                notificationService.notify(scorecardResources.PatternNotSetError.format($rootScope.displayMonth.name, $rootScope.displayMonth.year)
                                , { autoClose: false, type: "danger" })
                            }, 1000);
                        }
                    },
                    function (err) {
                        $rootScope.disableHeader = false;
                        notificationService.notify(err.errors.join("<br/>"),
                        { autoClose: false, type: "danger" });
                    });
                };

                var getFiscalMonthStatusForScorecard = function (scorecardId, yearId) {
                    kpiService.getFiscalMonthStatusForScorecard(scorecardId, yearId).then(
                        function (data) {
                            self.fiscalMonthData = data;
                        },
                    function (err) {
                        notificationService.notify(err.errors.join("<br/>"),
                        { autoClose: false, type: "danger" });
                    });
                };

                //navigation to individual kpi
                self.navigateToKpi = function (kpiId) {
                    var displayMonth = monthNavigationService.getSelectedMonthDetails();
                    if (displayMonth.yearId == self.yearId && displayMonth.month == self.monthId) {
                        $rootScope.scorecardYearId = parseInt(self.yearId);
                        $rootScope.scorecardMonthId = parseInt(self.monthId);
                        $location.url('KPI/' + kpiId + '/' + self.scorecardData.scorecardId + '/' + self.yearId + '/' + self.monthId + '/' + self.day).replace();
                    }
                    else {
                        $rootScope.scorecardYearId = parseInt(displayMonth.yearId);
                        $rootScope.scorecardMonthId = parseInt(displayMonth.id);
                        $location.url('KPI/' + kpiId + '/' + self.scorecardData.scorecardId + '/' + displayMonth.yearId + '/' + displayMonth.id + '/' + self.day).replace();
                    }
                    if (!$scope.$$phase) $scope.$apply();
                };

                //Show or hide back button
                self.showBackButton = function () {

                    if ($rootScope.referrer.endsWith('/NDMSUI/Login/') || !authService.isAuthorized())
                        return false;
                    else return true;

                };

                //catch the broadcast event from parent controller
                $scope.$on('monthChange', function () {
                    var displayMonth = monthNavigationService.getSelectedMonthDetails();
                    loadScorecardDataOfMonth(self.scorecardId, displayMonth.yearId, displayMonth.id);
                });

                //formatting data to plot graph
                var formatDataForGraph = function (scorecardGraphData, index) {

                    var tempData = {
                        series: [{ data: [] }],
                        yAxis: { min: '', max: '', tickInterval: '' },
                        xAxis: { min: '', max: '', tickInterval: '', categories: [], trackingMethode: '' },
                    };
                    var graphData;

                    if (scorecardGraphData.dailyGraphData) {
                        graphData = scorecardGraphData.dailyGraphData;
                        tempData.xAxis.max = graphData.length - 1;
                        tempData.xAxis.trackingMethode = 'daily';
                    }
                    else if (scorecardGraphData.monthlyGraphData) {
                        graphData = scorecardGraphData.monthlyGraphData;
                        tempData.xAxis.max = graphData.length - 1;
                        tempData.xAxis.trackingMethode = 'monthly';
                    }
                    tempData.series.push({ data: [] });
                    tempData.series.push({ data: [] });
                    angular.forEach(graphData, function (item, index) {
                        tempData.series[0].data.push(item.actualValue);
                        tempData.series[1].data.push(item.stretchGoalValue);
                        tempData.series[2].data.push(item.goalValue);
                        tempData.xAxis.categories.push(index + 1);
                    });

                    tempData.yAxis.min = scorecardGraphData.minValue;
                    tempData.yAxis.max = scorecardGraphData.maxValue;

                    self.graphData[index] = tempData;
                    self.isGraphDataLoaded[index] = true;
                };

                //service for loading and formatting graph data
                var loadGraphData = function (scorecardId, yearId, monthId) {
                    self.graphData = [];

                    kpiService.getScorecardGraphData(scorecardId, yearId, monthId).then(function (data) {
                        var scorecardGraphData = data;
                        angular.forEach(scorecardGraphData.data, function (item, index) {
                            self.isGraphDataLoaded[index] = true;
                            if (item) {
                                if (!item.dailyGraphData && !item.monthlyGraphData) {
                                    self.isGraphData[index] = false;
                                }
                                else {
                                    self.isGraphData[index] = true;
                                    formatDataForGraph(item, index);
                                }
                            }
                            else {
                                self.isGraphData[index] = false;
                            }
                        });
                    },
                    function (err) {
                        notificationService.notify(err.errors.join("<br/>"),
                        { autoClose: false, type: "danger" });
                    });
                };

                // Retrieves current year and date
                var getYearAndMonth = function () {
                    var deferred = $q.defer();
                    configurationService.getCurrentDateAndYearId().then(
                        function (data) {
                            var currentDate = data.currentDate.slice(0, 10);
                            self.monthId = parseInt($routeParams.monthId) ? parseInt($routeParams.monthId) : parseInt($filter('date')(currentDate, 'M'), 10);
                            self.yearId = !isNaN(parseInt($routeParams.yearId)) ? parseInt($routeParams.yearId) : data.yearId;
                            self.day = $filter('date')(currentDate, 'dd');
                            deferred.resolve();
                        },
                        function (msg) {
                            deferred.reject(msg);
                        });
                    return deferred.promise;
                };

                var loadCurrentAndPreviousYearMonthData = function () {
                    configurationService.getAllYearMonthsList().then(function (data) {
                        monthNavigationService.setMonthDetails(data, self.monthId, self.yearId);
                        $rootScope.displayMonth = monthNavigationService.getSelectedMonthDetails();
                    },
                    function (err) {
                        notificationService.notify(err.errors.join("<br/>"),
                        { autoClose: false, type: "danger" });
                    });
                }

                var loadInitalData = function () {
                    // Scorecard numbers can only integers for time being
                    if (isNaN(self.scorecardId) !== true) {
                        loadCurrentAndPreviousYearMonthData();
                        loadScorecardData(self.scorecardId, self.yearId, self.monthId);
                    }
                    self.isBackButtonVisible = self.showBackButton();

                };


                // DOM ready function
                var init = function () {
                    getYearAndMonth().then(function () {
                        loadInitalData();
                    });
                }();
            }]);
    });