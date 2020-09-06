"use strict";
define(['angularAMD', 'rangeSlider', 'actualsService', 'ionSlider', 'counterMeasureController', 'metricType'],
function (angularAMD) {
    angularAMD.controller('ActualsController', ['configService',
        'validationService', 'notificationService', 'confirmationModalService', 'actualsService', '$scope', '$uibModal',
         '$uibModalInstance', 'modalData', '$filter', '$location', '$timeout', function (configService,
     validationService, notificationService,confirmationModalService, actualsService, $scope, $uibModal, $uibModalInstance,
     modalData, $filter, $location, $timeout) {
             var self = this,
             selectedDate = modalData.date;
             self.metricData = {};
             self.metricData = modalData;
             self.metricData.outstandingCounterMeasureExists = false;
             self.inputMinValue = 0;
             self.sliderMinValue = 0;
             self.actualEntered = 0;
             self.sliderMaxValue = 0;
             self.sliderFromValue = 0;
             self.isShowingActualPopup = true;
             self.changedValues = {};
             self.isActualSaveDisabled = false;
             self.hasGoalValue = false;

             var actualsResources = configService.getUIResources('Actuals');

             var formatDataForActualandHolidaypopup = function () {

                 //date formatting for primary and secondary in actual popups.
                 if (self.metricData.isMonthlyTracking === true && self.metricData.isPrimary === true) {
                     self.metricData.date = $filter('date')(self.metricData.date, "MMMM, y");
                 }
                 else if (self.metricData.isPrimary === true) {
                     self.metricData.date = $filter('date')(self.metricData.date, 'fullDate');
                 }

                 //enabling slider only for percentage data type metric
                 if (self.metricData.metricDataTypeId === 2) {
                     self.isSliderEnabled = true;
                 }
                 else {
                     self.isSliderEnabled = false;
                 }

                 //status 3 (holiday),status 1 (inactive)
                 if (self.metricData.status === 3) {
                     self.actionForHoliday = "Mark as Workday";
                 }
                 else {
                     self.actionForHoliday = "Mark as Non-Workday";
                 }

                 //amount type.
                 if (self.metricData.metricDataTypeId == 3) {
                     self.inputMinValue = '';
                 }

                 if (self.metricData.goalValue !== null && self.metricData.goalValue !== '') {
                     self.hasGoalValue = true;
                 }
             };

             var initailizeSlider = function () {

                 //for percentage type.
                 if (self.metricData.metricDataTypeId === 2) {
                     self.sliderMaxValue = 100;
                 }
                 else if (self.metricData.goalValue) {
                     self.sliderMaxValue = self.metricData.goalValue;
                 }

                 if (self.metricData.currentActual || self.metricData.currentActual === 0) {
                     self.sliderFromValue = self.metricData.currentActual;
                     self.actualEntered = self.metricData.currentActual;
                 }
                 else {
                     self.sliderFromValue = self.metricData.goalValue;
                     self.actualEntered = self.metricData.goalValue;
                 }
             };

             //add actuals service call
             var addActual = function (actualObject) {
                 if (self.metricData.metricDataTypeId == 0) {
                     if (actualObject.actualValue != null) {
                         actualObject.actualValue = Math.floor(actualObject.actualValue);
                     }
                 }

                 actualsService.addActuals(actualObject)
                     .then(function (data) {
                         if (self.isTargetAchieved) {
                             self.isActualSaveDisabled = false;
                             notificationService.notify(actualsResources.AddedSuccessfully,
                                        { type: 'success', timeout: 500 }).then(function () {
                                        });
                         }
                         self.changedValues.id = data;
                         $uibModalInstance.close(self.changedValues);
                     },
                     function (err) {
                         self.isActualSaveDisabled = false;
                         notificationService.notify(err.errors.join("<br/>"),
                         { autoClose: false, type: "danger" });
                     });
             };

             //update actuals service call 
             var updateActual = function (actualObject) {
                 if (self.metricData.metricDataTypeId == 0) {
                     if (actualObject.actualValue != null) {
                         actualObject.actualValue = Math.floor(actualObject.actualValue);
                     }
                 }
                 actualsService.updateActuals(actualObject)
                     .then(function (data) {
                         if (self.isTargetAchieved) {
                             self.isActualSaveDisabled = false;
                             notificationService.notify(actualsResources.AddedSuccessfully,
                                        { type: 'success', timeout: 500 }).then(function () {
                                        });
                         }
                         $uibModalInstance.close(self.changedValues);
                     },
                     function (err) {
                         self.isActualSaveDisabled = false;
                         notificationService.notify(err.errors.join("<br/>"),
                         { autoClose: false, type: "danger" });
                     });
             };

             //add or update actuals by checking if actual id already exists.
             var addorUpdateActual = function (status) {

                 var actualObject = {
                     "id": self.metricData.id,
                     "scorecardId": self.metricData.scorecardId,
                     "targetId": self.metricData.targetId,
                     "date": $filter('date')(selectedDate, "dd MMM yyyy"),
                     "actualValue": self.actualEntered,
                     "goalValue": self.metricData.goalValue,
                 };
                 notificationService.close();

                 // Round to two decimal places
                 actualObject.actualValue = Math.round(actualObject.actualValue * 100) / 100;

                 // Values back to actual - modal popup controller
                 self.changedValues = {
                     id: self.metricData.id,
                     newActual: actualObject.actualValue,
                     day: self.metricData.day,
                     status: status,
                     month: self.metricData.month,
                     isMonthlyTracking: self.metricData.isMonthlyTracking,
                     isPrimary: self.metricData.isPrimary,
                     targetId: self.metricData.targetId,
                     targetEntryMethodId: self.metricData.targetEntryMethodId,
                     dailyRate: self.metricData.dailyRate,
                     graphPlottingMethodId: self.metricData.graphPlottingMethodId
                 };
                 if (self.metricData.id || self.metricData.id === 0) {
                     updateActual(actualObject);
                 }
                 else {
                     addActual(actualObject);
                 }

             };

             //save button on actual popup
             self.save = function () {
                 notificationService.close();

                 var errors = [];
                 if (angular.isUndefined(self.actualEntered)) {
                     errors.push(actualsResources.InvalidActual);
                 }
                 else if (!self.actualEntered && self.actualEntered !== 0) {
                     errors.push(actualsResources.RequiredActual);
                 }

                 if (!errors.length) {

                     //get actual status
                     var actualStatusRequestItem = {
                         targetId: self.metricData.targetId,
                         goalValue: self.metricData.goalValue,
                         actualValue: self.actualEntered,
                         selectedDate: $filter('date')(self.metricData.date, 'dd MMM yyyy')
                     };

                     actualsService.getActualStatusandCounterMeasure(actualStatusRequestItem)
                             .then(function (data) {
                                 if (data) {
                                     self.metricData.outstandingCounterMeasureExists =
                                         data.outstandingCounterMeasureExists;

                                     //status=2 : target not achieved 
                                     if (data.actualStatus === 2) {
                                         addCounterMeasure(data.actualStatus);
                                     }
                                         //status=1 : target achieved Or status=0 : not entered
                                     else if (data.actualStatus === 1 || data.actualStatus === 0) {
                                         self.isTargetAchieved = true;
                                         addorUpdateActual(data.actualStatus);
                                     }
                                 }
                             },function (err) {
                                 self.isActualSaveDisabled = false;
                                 notificationService.notify(err.errors.join("<br/>"),
                                    { autoClose: false, type: "danger" });
                         });
                 }
                 else {
                     self.isActualSaveDisabled = false;
                     notificationService.notify(errors.join("<br/>"), { autoClose: false, type: "danger" });
                 }
             };

             var addCounterMeasure = function (status) {
               
                 var modalInstance = $uibModal.open({
                     templateUrl: 'ngViews/counter-measure/partials/counter-measure-popup.min.html',
                     controller: 'counterMeasureController',
                     controllerUrl: 'ngControllers/counter-measure/counter-measure-controller.min',
                     controllerAs: 'ctrl',
                     windowClass: 'counter-measure-popup',
                     backdrop: 'static',
                     keyboard: false,
                     resolve: {
                         actualData: function () {
                             return self.metricData;
                         },
                         counterMeasure: function () {
                             return;
                         },
                         metricDetails: function () {
                             return;
                         }
                     }
                 }).result.then(function (data) {
                     addorUpdateActual(status);
                     self.isActualSaveDisabled = false;
                 }, function () {
                     // action on popup dismissal.
                     self.isActualSaveDisabled = false;
                 });
             };

             //updating slider according to the actual value entered in input box.
             self.updateSlider = function (sliderValue) {
                 if (sliderValue > self.sliderMaxValue) {
                     self.sliderFromValue = self.sliderMaxValue;
                 }
                 else {
                     self.sliderFromValue = sliderValue;
                 }
             };

             //updating input box according to the actual value entered in slider.
             self.updateInput = function (sliderValue) {
                 $scope.changedSliderValue = sliderValue.from;
                 if (sliderValue) {
                     if (!$scope.$$phase) $scope.$apply(function () {
                         self.actualEntered = sliderValue.from;
                     });
                 }
             };

             //mark as holiday section
             self.markHoliday = function (status) {
                 if (status == 3) { //Mark as Workday 
                     var confirmationMessage='';
                     if (self.metricData.targetEntryMethodId == 0) {
                         confirmationMessage =actualsResources.DailyRateMsg + " " + self.metricData.dailyRate + actualsResources.DailyRateProceedMsg;                        
                     }
                     else {
                         confirmationMessage = actualsResources.MarkasWorkdayMsg;
                     }
                     var confirmationModalInstance =
                             confirmationModalService.openConfirmationModal('md', confirmationMessage);
                     confirmationModalInstance.result.then(function () {
                         notificationService.close();
                         self.changedValues = {
                             id: self.metricData.id,
                             newActual: null,
                             day: self.metricData.day,
                             status: status == 3 ? 0 : 3,
                             month: self.metricData.month,
                             isMonthlyTracking: self.metricData.isMonthlyTracking,
                             isPrimary: self.metricData.isPrimary,
                             targetId: self.metricData.targetId,
                             targetEntryMethodId: self.metricData.targetEntryMethodId,
                             dailyRate: self.metricData.dailyRate,
                             graphPlottingMethodId: self.metricData.graphPlottingMethodId
                         };
                         var date = $filter('date')(selectedDate, "dd MMM yyyy");
                         actualsService.changeHoliday(self.metricData.scorecardId, self.metricData.targetId, date, status)
                         .then(function (data) {
                             self.isActualSaveDisabled = false;
                             notificationService.notify(actualsResources.MarkedSuccessfully,
                                 { type: 'success', timeout: 500 }).then(function () {
                                     if (data) {
                                         self.changedValues.id = data;
                                     }
                                     $uibModalInstance.close(self.changedValues);
                                 });
                         },
                             function (err) {
                                 self.isActualSaveDisabled = false;
                                 notificationService.notify(err.errors.join("<br/>"),
                                 { autoClose: false, type: "danger" });
                             });
                     }, function () {
                     });
                 }
                 else { //Mark as holiday 
                     if (self.metricData.targetEntryMethodId != 0 ) {
                         var confirmationModalInstance =
                            confirmationModalService.openConfirmationModal('md', actualsResources.MarkasWorkdayMsg);
                         confirmationModalInstance.result.then(function () {
                             notificationService.close();
                             self.changedValues = {
                                 id: self.metricData.id,
                                 newActual: null,
                                 day: self.metricData.day,
                                 status: status == 3 ? 0 : 3,
                                 month: self.metricData.month,
                                 isMonthlyTracking: self.metricData.isMonthlyTracking,
                                 isPrimary: self.metricData.isPrimary,
                                 targetId: self.metricData.targetId,
                                 dailyRate: self.metricData.dailyRate, //Check need to take the target entry method
                                 targetEntryMethodId: self.metricData.targetEntryMethodId,
                                 graphPlottingMethodId: self.metricData.graphPlottingMethodId
                             };
                             var date = $filter('date')(selectedDate, "dd MMM yyyy");
                             actualsService.changeHoliday(self.metricData.scorecardId, self.metricData.targetId, date, status)
                             .then(function (data) {
                                 self.isActualSaveDisabled = false;
                                 notificationService.notify(actualsResources.MarkedSuccessfully,
                                     { type: 'success', timeout: 500 }).then(function () {
                                         if (data) {
                                             self.changedValues.id = data;
                                         }
                                         $uibModalInstance.close(self.changedValues);
                                     });
                             },
                                 function (err) {
                                     self.isActualSaveDisabled = false;
                                     notificationService.notify(err.errors.join("<br/>"),
                                     { autoClose: false, type: "danger" });
                                 });
                         }, function () {
                         });
                     }
                     else {
                         notificationService.close();
                         self.changedValues = {
                             id: self.metricData.id,
                             newActual: null,
                             day: self.metricData.day,
                             status: status == 3 ? 0 : 3,
                             month: self.metricData.month,
                             isMonthlyTracking: self.metricData.isMonthlyTracking,
                             isPrimary: self.metricData.isPrimary,
                             targetId: self.metricData.targetId,
                             dailyRate: self.metricData.dailyRate, //Check need to take the target entry method
                             targetEntryMethodId: self.metricData.targetEntryMethodId,
                             graphPlottingMethodId: self.metricData.graphPlottingMethodId
                         };
                         var date = $filter('date')(selectedDate, "dd MMM yyyy");
                         actualsService.changeHoliday(self.metricData.scorecardId, self.metricData.targetId, date, status)
                         .then(function (data) {
                             self.isActualSaveDisabled = false;
                             notificationService.notify(actualsResources.MarkedSuccessfully,
                                 { type: 'success', timeout: 500 }).then(function () {
                                     if (data) {
                                         self.changedValues.id = data;
                                     }
                                     $uibModalInstance.close(self.changedValues);
                                 });
                         },
                             function (err) {
                                 self.isActualSaveDisabled = false;
                                 notificationService.notify(err.errors.join("<br/>"),
                                 { autoClose: false, type: "danger" });
                             });
                     }
                 }
             };

             self.cancel = function () {
                 notificationService.close();
                 self.isActualSaveDisabled = false;
                 $uibModalInstance.dismiss('cancel');
             };

             var init = function () {
                 formatDataForActualandHolidaypopup();
                 initailizeSlider();
             }();

         }]);
});