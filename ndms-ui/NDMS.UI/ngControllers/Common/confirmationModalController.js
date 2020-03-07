"use strict";
define(['angularAMD'], function (angularAMD) {

    angularAMD.controller('ConfirmationModalController', ['$scope', '$uibModalInstance','message', function ($scope, $uibModalInstance, message) {

        var self=this;
        self.message = message;

        self.yes = function () {
            $uibModalInstance.close();
        };

        self.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    }]);
});