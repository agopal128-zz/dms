"use strict";
define(['angularAMD'], function (angularAMD) {

    angularAMD.directive('fullYearCalendar',['utilityService', function (utilityService) {
        return {
            restrict: 'E',
            scope: {
                year: '=?',
                options: '=?',
                lastChange: '=?',
                events: '=',
                markEvent: '&'

            },
            templateUrl: 'ngViews/admin/templates/full-year-calendar.tpl.min.html',
            controller: function ($scope) {
                var MONTHS = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
                var WEEKDAYS = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
                var loadMonth, loadMonths, calculateWeeks, calculateDefaultDate, allowedDate, bindEvent, loadYear;
                $scope.months = [];
                $scope.selectedYear;

                loadMonths = function () {
                    angular.forEach(MONTHS, function (month) {
                        var newMonth = {
                            name: month,
                            weeks: calculateWeeks(month)
                        }
                        $scope.months.push(newMonth);
                    });
                };

                loadMonth = function (month) {
                    var newMonth = {
                        name: MONTHS[month],
                        weeks: calculateWeeks(MONTHS[month])
                    }
                    $scope.months.splice(month, 1, newMonth);
                }

                bindEvent = function (date) {
                    if (!date || !$scope.events) { return; }
                    date.event = false;
                    $scope.events.forEach(function (event) {
                        event.date = new Date(event);
                        if (date.year === event.date.getFullYear() && date.month === event.date.getMonth() && date.day === event.date.getDate()) {
                            date.event = true;
                        }
                    });
                };

                allowedDate = function (date) {
                    if (!$scope.options.minDate && !$scope.options.maxDate) {
                        return true;
                    }
                    var currDate = new Date([date.year, date.month + 1, date.day]);
                    if ($scope.options.minDate && (currDate < $scope.options.minDate)) { return false; }
                    if ($scope.options.maxDate && (currDate > $scope.options.maxDate)) { return false; }
                    return true;
                };

                $scope.loadNextYear = function () {
                    $scope.selectedYear++;
                    $scope.months = [];
                    loadMonths();
                };

                $scope.loadPreviousYear = function () {
                    $scope.selectedYear--;
                    $scope.months = [];
                    loadMonths();
                };

                calculateWeeks = function (month) {
                    var weeks = [];
                    var week = null;
                    var daysInCurrentMonth = new Date($scope.selectedYear, MONTHS.indexOf(month) + 1, 0).getDate();
                    for (var day = 1; day < daysInCurrentMonth + 1; day += 1) {
                        var dayNumber = new Date($scope.selectedYear, MONTHS.indexOf(month), day).getDay();
                        week = week || [null, null, null, null, null, null, null];
                        week[dayNumber] = {
                            year: $scope.selectedYear,
                            month: MONTHS.indexOf(month),
                            day: day
                        };

                        if (allowedDate(week[dayNumber])) {
                            if ($scope.events) { bindEvent(week[dayNumber]); }
                        } else {
                            week[dayNumber].disabled = true;
                        }

                        if (dayNumber === 6 || day === daysInCurrentMonth) {
                            weeks.push(week);
                            week = undefined;
                        }
                    }

                    return weeks;
                };

                calculateDefaultDate = function () {
                    if ($scope.options.currentDate) {
                        $scope.options._defaultDate = utilityService.convertToDateWithoutTimezone($scope.options.currentDate);
                    } else {
                        $scope.options._defaultDate = utilityService.convertToDateWithoutTimezone(new Date());
                    }
                    
                    $scope.selectedDay = $scope.options._defaultDate.getDate();
                };

                loadYear = function (year) {
                        $scope.selectedYear = year;
                        calculateDefaultDate();
                        $scope.months = [];
                        loadMonths();
                };

                $scope.weekDays = function (size) {
                    return WEEKDAYS.map(function (name) { return name.slice(0, size) });
                };


                $scope.isDefaultDate = function (date) {
                    if (!date) { return; }
                    return date.year === $scope.options._defaultDate.getFullYear() &&
                      date.month === $scope.options._defaultDate.getMonth() &&
                      date.day === $scope.options._defaultDate.getDate()
                };


                $scope.$watch('year', function (year) {
                    if (!angular.isUndefined(year)) {
                       loadYear(year);
                    }
                });
                
                $scope.$watch('lastChange', function (changeDate) {
                    if (!angular.isUndefined(changeDate)) {
                        loadMonth(changeDate.getMonth());
                    }
                });

                $scope.$watch('options.currentDate',function(currentDate){
                    if (!angular.isUndefined(currentDate)) {
                        loadYear(currentDate.getFullYear());
                    }
                });
                
            }
        }
    }]);
});