'use strict';
define(['angularAMD', 'confirmationModalService', 'targetService'], function (angularAMD) {
    angularAMD.controller('copyTargetController', ['confirmationModalService', 'targetService',
     'notificationService', 'configService', 'kpiTargets', 'targetParams', '$uibModalInstance', function (confirmationModalService, targetService,
      notificationService, configService, kpiTargets, targetParams, $uibModalInstance) {

         var self = this;
         self.kpiTargets = kpiTargets;
         self.scorecardId = targetParams.scorecardId;
         self.fromYear = targetParams.fromYear;
         self.toYear = targetParams.toYear;
         self.selectedTargets = [];
         self.isCopyVisible = false;
         self.isCopying = false;

         //set target resources
         var targetResources = configService.getUIResources('Target');


         var copyTargets = function () {
             var copiedMetrics = {
                 targets: self.selectedTargets,
                 calendarYearId: self.toYear.id
             };

             targetService.copyTargets(self.scorecardId, copiedMetrics)
             .then(function (data) {
                 notificationService.notify(targetResources.CopiedSuccessfully,
                 {
                     type: 'success'
                 }).then(function () {
                     var changedValues = {
                         calendarYearId: self.toYear.id
                     };
                     self.isCopying = false;
                     $uibModalInstance.close(changedValues);
                 });
             }, function (err) {
                 self.isCopying = false;
                 notificationService.notify(err.errors.join("<br/>"),
                 {
                     autoClose: false, type: "danger"
                 });
             });
         }
         

         self.setSelectedTargets = function (target) {
             target.isSelected = true;
             self.selectedTargets.push(target);
             self.isCopyVisible = true;
         }

         self.updateSelectedTargets = function (target) {
             if (target.isSelected) {
                 self.selectedTargets.push(target);
             }
             else{
                 var index = self.selectedTargets.indexOf(target);
                 self.selectedTargets.splice(index, 1);
             }

         }

         self.copy = function () {
             self.errors = [];
             if (!self.selectedTargets.length) {
                 self.errors.push(targetResources.RequiredMetricSelection);
             }

             if (!self.errors.length) {
                 var confirmationModalInstance =
                              confirmationModalService.openConfirmationModal('md', targetResources.CopyTargetConfirmationMsg);

                 confirmationModalInstance.result.then(function () {
                     self.isCopying = true;

                     copyTargets();                     
                 }, function () {
                     self.isCopying = false;
                 });
             }
             else {
                 //show error if no metrics selected
                 notificationService.notify(self.errors.join("<br/>"),
                 {
                     autoClose: false, type: "danger"
                 });
             }
         }


         self.cancel = function () {
             notificationService.close();
             $uibModalInstance.dismiss('cancel');
         }
     }]);
});