"use strict";
define(['angularAMD'], function (angularAMD) {
    angularAMD.service('configurationService', ['configService', '$http', '$log', '$q',
        function (configService, $http, $log, $q) {

            var baseUrl = configService.getApiBaseUrl() + "Configuration/";

            this.getCurrentDateAndYearId = function () {
                var deferred = $q.defer();
                var url = baseUrl + "GetCurrentDateAndYearId";
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            this.getAllYearMonthsList = function () {
                var deferred = $q.defer();
                var url = baseUrl + "GetAllYearMonthsList";
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            this.getAll = function () {
                var deferred = $q.defer();
                var url = baseUrl + "GetAll";
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

        }]);

});