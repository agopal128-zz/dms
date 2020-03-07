'use strict';
define(['angularAMD', 'fullYearCalendar', 'holidayPatternService'],
function (angularAMD) {

    angularAMD.controller('HolidaySelectorController', ['holidayPatternService', 'notificationService', 'confirmationModalService', 'configService', 'configurationService',
        'utilityService','$routeParams', '$rootScope', '$filter', '$window', '$scope',
            function (holidayPatternService, notificationService, confirmationModalService, configService, configurationService, utilityService,
                $routeParams, $rootScope, $filter, $window, $scope) {
                var self = this,
                    holidayResources = configService.getUIResources('Holiday');


                self.holidayPatternId = $routeParams.id;
                self.holidayPattern = {};
                self.isPatternChanged = false;
                self.currentDate = utilityService.convertToDateWithoutTimezone(new Date());
                self.calendarOptions = {
                    dayNamesLength: 1
                };

                self.loadPreviousYear = function () {
                    self.selectedYear--;
                };
                self.loadNextYear = function () {
                    self.selectedYear++
                };

                self.goBack = function () {
                    if (self.isPatternChanged) {
                        var confirmationModalInstance = confirmationModalService.openConfirmationModal('md', holidayResources.NavigateConfirmation);
                        confirmationModalInstance.result.then(function () {
                            $window.history.back();
                        }, function () {
                        });
                    }
                    else {
                        $window.history.back();
                    }

                };

                self.markOrUnmarkHoliday = function (date) {
                    var eventDate = new Date(date.year, date.month, date.day);
                    if (+eventDate.getDatePart() <= +self.currentDate.getDatePart()) {
                        notificationService.notify(holidayResources.PastDateError, {
                            autoclose: false, type: 'danger'
                        });
                    }
                    else {
                        var index = isHoliday(eventDate);
                        if (index > -1) {
                            self.holidayPattern.holidaysList.splice(index, 1);
                        }
                        else if (index === -1) {
                            self.holidayPattern.holidaysList.push(eventDate);
                        }
                        self.lastChange = eventDate;
                        self.isPatternChanged = true;
                    }
                };

                self.updateHolidays = function () {

                    var holidayPatternInfo = {
                        holidayPatternId: parseInt(self.holidayPatternId),
                        holidays: []
                    };
                    self.holidayPattern.holidaysList.forEach(function (date) {
                        holidayPatternInfo.holidays.push($filter('date')(date, "dd MMM yyyy"));
                    });
                    
                    saveHolidays(holidayPatternInfo);
                };

                var saveHolidays = function (holidayPatternInfo) {
                    holidayPatternService.setHolidayPatternInfo(holidayPatternInfo)
                    .then(function (data) {
                        notificationService.notify(holidayResources.ListUpdatedSuccessfully, {
                            type: 'success'
                        }).then(function () {
                            self.isPatternChanged = false;
                        });
                    }, function (err) {
                        notificationService.notify(err.errors.join("<br>"), {
                            autoclose: false, type: 'danger'
                        }).then(function(){
                            loadCurrentHolidayPattern();
                        });
                    });
                };

                var loadCurrentHolidayPattern = function () {
                    holidayPatternService.getHolidayPatternInfo(self.holidayPatternId)
                    .then(function (data) {
                        self.holidayPattern = data;
                        self.holidayPattern.holidaysList = [];
                        data.holidays.forEach(function (holiday) {
                            
                            self.holidayPattern.holidaysList.push(utilityService.convertToDateWithoutTimezone(holiday));
                        });
                        self.currentDate = utilityService.convertToDateWithoutTimezone(data.currentDate);
                        self.calendarOptions.currentDate = self.currentDate;
                        self.selectedYear = self.currentDate.getFullYear();
                        $rootScope.title = 'Holiday Schedule : ' + self.holidayPattern.name;

                    }, function (err) {
                        notificationService.notify(err.errors.join("<br>"), {
                            autoclose: false, type: 'danger'
                        });
                    });
                };

                

                var isHoliday = function (eventDate) {
                    var holidayIndex = -1;
                    self.holidayPattern.holidaysList.forEach(function (holiday) {
                        if (+holiday.getDatePart() === +eventDate.getDatePart()) {
                            holidayIndex = self.holidayPattern.holidaysList.indexOf(holiday);
                        }
                    });
                    return holidayIndex;
                };

                
                var init = function () {
                    loadCurrentHolidayPattern();
                }();

            }
    ]);
});