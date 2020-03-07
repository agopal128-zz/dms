"use strict";
define(['angularAMD', 'confirmationModalController'], function (angularAMD) {

    angularAMD.service('confirmationModalService', ['$uibModal', function ($uibModal) {

        this.openConfirmationModal = function (size, msg) {

            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'ngTemplates/Common/ConfimationModal.tpl.min.html',
                controller: 'ConfirmationModalController',
                controllerUrl: 'ngControllers/Common/ConfirmationModalController.min',
                controllerAs: 'ctrl',
                backdrop: 'static',
                keyboard: false,
                size: size,
                resolve: {
                    message: function () {
                        return msg;
                    }
                }
            });
            return modalInstance;
        };
    }]);
});