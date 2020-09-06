'use strict';
define(['angularAMD', 'actualsService', 'd3js', 'hierarchyChartDirective', 'hierarchyChartHelper'],
function (angularAMD) {

    angularAMD.controller('drillDownController', ['actualsService', 'validationService',
        'notificationService', 'configService', 'hierarchyChartDirective', '$rootScope',
        '$routeParams', '$filter', 'monthNavigationService', 'configurationService', '$scope', '$location', '$q', 'utilityService', 
    function (actualsService, validationService, notificationService, configService,
        hierarchyChartDirective, $rootScope, $routeParams, $filter, monthNavigationService, configurationService, $scope, $location, $q, utilityService) {
        var self = this,
            scorecardId = $routeParams.scorecardId;
        self.kpiId = $routeParams.kpiId;
        self.monthId = parseInt($routeParams.monthId);
        self.yearId = parseInt($routeParams.yearId);
        self.scorecardKPIs = [];        
        self.selectedDate = '';
        self.isDataLoaded = false;
        self.dateOptions = {};
        self.showTree = false;

        // properties to navigate to KPI page on back button click
        self.previousKPI = $routeParams.kpiId;
        self.previousMonthId = parseInt($routeParams.monthId);
        self.previousYearId = parseInt($routeParams.yearId);

        $rootScope.title = "Drill-Down";
        $rootScope.kpiOwners = [];
        $rootScope.scorecardId = "";

        // Retrieves current year and date
        var getYearAndMonth = function () {
            var deferred = $q.defer();
            configurationService.getCurrentDateAndYearId().then(
                function (data) {
                    var currentDate = data.currentDate.slice(0, 10);
                    self.currentMonthId = parseInt($filter('date')(currentDate, 'M'), 10);
                    self.currentYearId = data.yearId;
                    self.currentDay = $filter('date')(currentDate, 'dd');
                    self.currentYear = parseInt($filter('date')(currentDate, 'yyyy'));
                    deferred.resolve();
                },
                function (msg) {
                    deferred.reject(msg);
                });
            return deferred.promise;
        };

        // load KPIs in Scorecard
        var loadKPIs = function () {
            actualsService.getScorecardKPIs(scorecardId, self.monthId, self.yearId)
            .then(function (data) {
                self.scorecardKPIs = data;
                if (self.scorecardKPIs.length > 0) {
                    if (!self.isKPIExists(self.kpiId)) {
                        self.kpiId = self.scorecardKPIs[0].id;
                    }
                    self.setTab(parseInt(self.kpiId));
                }
                else { // if no KPIs show message
                    self.drillDownData = [];
                    self.showTree = false;
                    self.isDataLoaded = true;
                }
            },
            function (err) {
                notificationService.notify(err.errors.join("<br/>"),
                {
                    autoClose: false, type: "danger"
                });
            });
        }

        // load month list
        var loadCurrentAndPreviousYearMonthData = function () {
            configurationService.getAllYearMonthsList().then(function (data) {
                monthNavigationService.setMonthDetails(data, self.monthId, self.yearId);
                $rootScope.displayMonth = monthNavigationService.getSelectedMonthDetails();
                self.year = $rootScope.displayMonth.year;
                setDateOption(self.year, self.monthId);
            },
            function (err) {
                notificationService.notify(err.errors.join("<br/>"),
                { autoClose: false, type: "danger" });
            });
        }        

        // load drill down data
        var loadDrillDownData = function () {
            self.isDataLoaded = false;
            self.drillDownData = [];
            actualsService.getDrillDownHierarchy(scorecardId, self.kpiId, self.monthId, self.yearId)
                .then(function (data) {
                    self.drillDownData = data;
                    self.isDataLoaded = true;
                    self.showTree = self.scorecardKPIs.length > 0 && self.drillDownData != null;
                });
        }

        // load drill down data of specific day
        var loadDrillDownDataOnDate = function () {
            self.drillDownData = [];
            self.isDataLoaded = false;
            var requestedDate = convertToDateWithoutTimezone(self.selectedDate);
            requestedDate = $filter('date')(requestedDate, "dd MMM yyyy")
            actualsService.getDrillDownHierarchyOnDate(scorecardId, self.kpiId, requestedDate)
                .then(function (data) {
                    self.drillDownData = data;
                    self.isDataLoaded = true;
                    self.showTree = self.scorecardKPIs.length > 0 && self.drillDownData != null;
                });
        }

        // utility service used to avoid date time zone miss match with UTC time
        var convertToDateWithoutTimezone = function (date) {
            return utilityService.convertToDateWithoutTimezone(date);
        };

        // reset to the month to current month
        var resetMonth = function () {
            var monthDetails = monthNavigationService.getMonthList();
            monthNavigationService.setMonthDetails(monthDetails, self.monthId, self.yearId);
            $rootScope.displayMonth = monthNavigationService.getSelectedMonthDetails();
            self.year = $rootScope.displayMonth.year;
            setDateOption(self.year, self.monthId);
        }

        // set date options for date picker
        var setDateOption = function (year, month) {            
            var daysInMonth = new Date(year, month, 0).getDate();
            var minDateOfMonth = new Date(year, month - 1, 1);
            var maxDateOfMonth = new Date(year, month - 1, daysInMonth);
            var selectedDay = daysInMonth;
            if (year === self.currentYear && month === self.currentMonthId) {
                selectedDay = self.currentDay;
            }

            var initDateOfMonth = new Date(year, month - 1, selectedDay);
            self.dateOptions = {
                minDate: minDateOfMonth,
                maxDate: maxDateOfMonth,
                initDate: initDateOfMonth,
                showWeeks: false,
                maxMode: "day"
            };
        }

        //catch the broadcast event from parent controller
        $scope.$on('monthChange', function () {            
            self.monthId = $rootScope.displayMonth.id;
            self.yearId = $rootScope.displayMonth.yearId;
            self.year = $rootScope.displayMonth.year;
            self.selectedDate = '';
            loadKPIs();
            setDateOption(self.year, self.monthId);
        });

        // selected date change
        $scope.$watch('ctrl.selectedDate', function (newDate, oldDate) {
            if (newDate === oldDate || newDate === '') { return; }
            if (self.scorecardKPIs.length > 0) {
                loadDrillDownDataOnDate();
            }
        });
               
        // reset the drill down details
        self.resetDrillDown = function () {
            self.selectedDate = '';
            self.monthId = self.currentMonthId;
            self.yearId = self.currentYearId;
            resetMonth();
            loadKPIs();
        }

        // navigate to KPI page
        self.navigateToKPI = function () {
            $location.url('KPI/' + self.previousKPI + '/' + scorecardId + '/' + self.previousYearId + '/' + self.previousMonthId + '/' + self.currentDay).replace();
            if (!$scope.$$phase) $scope.$apply();
        }

        // tab click
        self.setTab = function (tabIndex) {
            self.kpiId = tabIndex;
            loadDrillDownData();
        }

        // check whether KPI exists
        self.isKPIExists = function (tabIndex) {
            var kpi;
            if (self.scorecardKPIs) {
                kpi = $filter('filter')(self.scorecardKPIs, { id: tabIndex })[0];
            }
            if (kpi && kpi.id !== undefined) {
                return true;
            }
            else {
                return false;
            }
        }

        // Check the selected tab
        self.isSet = function (tabIndex) {
            return self.kpiId === tabIndex;
        }
        
        // initialization
        var init = function () {
            getYearAndMonth().then(function () {
                loadCurrentAndPreviousYearMonthData();
                loadKPIs();
            });                 
        }();
    }]);
});