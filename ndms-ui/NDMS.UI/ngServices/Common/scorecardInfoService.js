'use strict';
define(['angularAMD'], function (angularAMD) {

    angularAMD.service('scorecardInfoService', function () {

        var scorecardData = {
            scorecardId:'',
            scorecardName:'',
            kpiOwner:'',
        };

        this.get = function () {
            return scorecardData;
        };

        this.set = function (data) {
            scorecardData = {
                scorecardId: data.scorecardId,
                scorecardName: data.scorecardName,
                kpiOwner: data.kpiOwner,
            };
        };


    });

});