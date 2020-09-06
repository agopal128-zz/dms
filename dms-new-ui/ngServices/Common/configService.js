'use strict';
define(['angularAMD', 'constants'], function (angularAMD) {

    angularAMD.service('configService', ['$location', function ($location) {

        var self = this;

        this.getAppHostUrl = $location.protocol() + "://" + $location.host() + ':' + $location.port() + '/' ;
        
        this.getApiBaseUrl = function () {
            return self.getAppHostUrl + AppConfig.apiBaseUri;
        };

        this.getUIResources = function (key) {
            return Resource[AppConfig.ThreadCulture][key];
        };

        this.getAppUrl = function () {
            return self.getAppHostUrl + AppConfig.appHomePage;
        };

    }]);

});