'use strict';
define(['angularAMD', 'scorecardService', 'utilityService'], function (angularAMD) {

    angularAMD.controller('counterMeasureController', ['$scope', 'configService', 'notificationService',
        'validationService', 'scorecardService', '$uibModalInstance', 'actualData', 'counterMeasure',
        '$filter', 'utilityService', 'authService', '$q', 'metricDetails', function ($scope, configService, notificationService, validationService,
            scorecardService, $uibModalInstance, actualData, counterMeasure, $filter, utilityService, authService, $q, metricDetails) {

            var self = this,
            actualResources = configService.getUIResources('Actuals');
            self.actualData = {};
            self.actualData = actualData;
            var scorecardId = self.actualData.scorecardId;
            var counterMeasureObject = {};
            self.status = 0;
            self.dateOptions = {};
            self.counterMeasureStatusId = counterMeasure ? counterMeasure.counterMeasureStatusId : null;
            self.iseditableView = authService.isAuthorized() && self.actualData.isUserAuthorisedToEdit && self.counterMeasureStatusId != 4;
            self.metricOptions = metricDetails;
            self.metricId;
            self.isCounterMeasureSaveDisabled = false;
            self.counterMeasurePriority = {};

            // utility service used to avoid date time zone miss match with UTC time
            var convertToDateWithoutTimezone = function (date) {
                return utilityService.convertToDateWithoutTimezone(date);
            };
           
            //ng-tags-input directive : service call to get autocomplete feature
            self.seletedListCount = 1000;
            self.loadTags = function (query) {
                var deferred = $q.defer();
                scorecardService.loadADUsers(query).then(function (data) {
                    if (data) {
                        self.seletedListCount = data ? data.length : 1000;
                        deferred.resolve(data);
                    }
                    else {
                        deferred.reject();
                    }
                });
                return deferred.promise;
            };

            //formatting before displaying on counter measure popup.
            var formatCounterMeasureData = function () {
                if (self.actualData !== undefined) {
                    if (self.actualData.isPrimary) {
                        self.actualData.date = $filter('date')(actualData.date, "fullDate");
                    }
                    else {
                        self.actualData.date = $filter('date')(actualData.date, "MMMM, y");
                    }
                }

                if (counterMeasure !== undefined) {

                    if (self.iseditableView) {
                        self.isEditFromTable = counterMeasure.isEditFromTable;
                        self.priorityName = counterMeasure.counterMeasurePriorityName;
                        self.actualData = {};
                        self.actualData.metricName = counterMeasure.metricName;
                        self.assignedTo =
                        [{
                            fullName: counterMeasure.assignedUserName,
                            accountName: counterMeasure.assignedTo
                        }];
                    }
                    else {
                        self.assignedTo = counterMeasure.assignedUserName;
                        self.actualData.metricName = counterMeasure.metricName;
                        self.priorityName = counterMeasure.counterMeasurePriorityName;
                    }

                    self.openedDate = counterMeasure.openedDate;
                    self.issue = counterMeasure.issue;
                    self.action = counterMeasure.action;
                    self.dueDate = convertToDateWithoutTimezone(counterMeasure.dueDate);
                    self.status = counterMeasure.counterMeasureStatusId;
                    self.priority = counterMeasure.counterMeasurePriorityId;                    
                    self.comment = counterMeasure.comments;
                }
                else {
                    self.iseditableView = true;
                }
            };

            //method from UI button click
            self.saveCounterMeasure = function () {
                if (self.isEditFromTable) {
                    updateCounterMeasure();
                }
                else {
                    addCounterMeasure();
                }
            };

            //form object for add or edit counter measure service call
            var formCounterMeasureObj = function (action) {
                if (action == 'add') {
                    counterMeasureObject = {
                        "scorecardId": self.actualData.scorecardId,
                        "kpiId": self.actualData.kpiId,
                        "metricId": self.actualData.metricId,
                        "issue": self.issue,
                        "action": self.action,
                        "dueDate": $filter('date')(self.dueDate, "dd MMM yyyy"),
                        "assignedTo": self.assignedTo[0].accountName,
                        "counterMeasureStatusId": self.status,
                        "counterMeasurePriorityId": self.priority,
                        "comment": self.newComment
                    };
                }
                else {
                    counterMeasureObject = {
                        "scorecardId": scorecardId,
                        "counterMeasureId": counterMeasure.id,
                        "action": self.action,
                        "dueDate": $filter('date')(self.dueDate, "dd MMM yyyy"),
                        "assignedTo": self.assignedTo[0].accountName,
                        "counterMeasureStatusId": self.status,
                        "counterMeasurePriorityId": self.priority,
                        "comment": self.newComment
                    };
                }
                return counterMeasureObject;
            };

            var validateCounterMeasureAddOrUpdate = function (errors) {
                if (!self.assignedTo || self.assignedTo.length === 0) {
                    errors.push(actualResources.RequiredWho);
                }
                if (!self.dueDate) {
                    errors.push(actualResources.RequiredDueDate);
                }
                if (!self.action || self.action.length === 0) {
                    errors.push(actualResources.RequiredAction);
                }
                if (!self.issue || self.issue.length === 0) {
                    errors.push(actualResources.RequiredIssue);
                }
                return errors;
            };

            // Add counter measure
            var addCounterMeasure = function () {
                var errors = [];
                errors = validateCounterMeasureAddOrUpdate(errors);
                notificationService.close();
                if (errors.length === 0) {
                    counterMeasureObject = formCounterMeasureObj('add');

                    scorecardService.addCounterMeasure(counterMeasureObject).then(function (data) {
                        notificationService.notify(actualResources.AddedSuccessfully,
                                 { type: 'success', timeout: 500 }).then(function () {
                                     $uibModalInstance.close();
                                     self.isCounterMeasureSaveDisabled = false;
                                 });
                    },
                    function (err) {
                        notificationService.notify(err.errors.join("<br/>"),
                        { autoClose: false, type: "danger" });
                        self.isCounterMeasureSaveDisabled = false;
                    });
                }
                else {
                    notificationService.notify(errors.join("<br/>"), { autoClose: false, type: "danger" });
                    self.isCounterMeasureSaveDisabled = false;
                }
            };

            // Get counterMeasure details after Update
            var getCounterMeasureById = function (counterMeasureId) {
                scorecardService.getCounterMeasureById(counterMeasureId).then(function (data) {
                    $uibModalInstance.close(data.data);
                    self.isCounterMeasureSaveDisabled = false;
                }, function (err) {
                    notificationService.notify(err.errors.join("<br/>"),
                    { autoClose: false, type: "danger" });
                    self.isCounterMeasureSaveDisabled = false;
                });
            };

            // Update counter measure
            var updateCounterMeasure = function () {
                var errors = [];
                errors = validateCounterMeasureAddOrUpdate(errors);
                notificationService.close();
                if (errors.length === 0) {
                    counterMeasureObject = formCounterMeasureObj('update');

                    scorecardService.updateCounterMeasure(counterMeasureObject).then(function (data) {
                        notificationService.notify(actualResources.UpdatedSuccessfully,
                              { type: 'success', timeout: 500 }).then(function () {
                                  if (counterMeasure.isEditFromTable) {
                                      getCounterMeasureById(counterMeasureObject.counterMeasureId);
                                  }
                                  else {
                                      $uibModalInstance.close(data.data);
                                      self.isCounterMeasureSaveDisabled = false;
                                  }

                              });
                    },
                      function (err) {
                          notificationService.notify(err.errors.join("<br/>"),
                          { autoClose: false, type: "danger" });
                          self.isCounterMeasureSaveDisabled = false;
                      });
                }
                else {
                    notificationService.notify(errors.join("<br/>"), { autoClose: false, type: "danger" });
                    self.isCounterMeasureSaveDisabled = false;
                }
            };

            var modifyStatus = function (id) {
                if (self.status == 4) {
                    self.status = 0;
                }
                else {
                    self.status += 1;
                }
            };

            var getPriorityList = function () {
                scorecardService.getCounterMeasurePriority().then(function (data) {
                    self.counterMeasurePriority = data;
                },
                function (err) {
                    notificationService.notify(err.errors.join("<br/>"),
                    { autoClose: false, type: "danger" });
                });
            };

            // Change status
            self.changeStatus = function () {
                var id = $("#status");
                modifyStatus(id);
            };

            self.skipClick = function () {
                self.isSkip = true;
                $uibModalInstance.close(counterMeasure);
                self.isCounterMeasureSaveDisabled = false;
            };

            self.cancel = function(){
                $uibModalInstance.dismiss(counterMeasure);
            };

            var init = function () {
                formatCounterMeasureData();
                getPriorityList();
            }();
        }]);
});
