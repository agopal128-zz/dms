"use strict";
define(['angularAMD'], function (angularAMD) {

    angularAMD.service('holidayPatternService', ['configService', '$http', '$q',
        function (configService, $http, $q) {

            var baseUrl = configService.getApiBaseUrl() + 'HolidayPattern/';

            this.getAllHolidayPatterns = function () {
                var deferred = $q.defer();
                var url = baseUrl + 'GetAllHolidayPatterns';
                $http.get(url)
                    .success(function (data) {
                        deferred.resolve(data.data);
                    }).error(function (msg, code) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            };

            this.getHolidayPatternInfo = function(id){
                var deferred = $q.defer();
                var url = baseUrl + 'GetHolidayPattern/'+id;
                $http.get(url)
                    .success(function(data){
                        deferred.resolve(data.data);
                    }).error(function(msg,code){
                        deferred.reject(msg);
                    });
                    return deferred.promise;
            };

            this.addOrUpdateHolidayPattern = function (holidayPattern) {
                var deferred = $q.defer();
                var url = baseUrl + 'AddOrUpdateHolidayPattern';
                $http.post(url, holidayPattern)
                    .success(function (data) {
                        deferred.resolve(data);
                    }).error(function (msg, code) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            };

            this.setHolidayPatternInfo = function (holidayPatternInfo) {
                var deferred = $q.defer();
                var url= baseUrl + 'UpdateHolidayPatternInfo';
                $http.post(url,holidayPatternInfo)
                    .success(function(data){
                        deferred.resolve(data);
                    }).error(function(msg,code){
                        deferred.reject(msg);
                    });
                return deferred.promise;;
            };

            this.copyHolidayPattern = function(holidayPatternId){
                var deferred = $q.defer();
                var url= baseUrl + 'CopyHolidayPatternInfo/'+ holidayPatternId;
                $http.post(url)
                    .success(function(data){
                        deferred.resolve(data);
                    }).error(function(msg,code){
                        deferred.reject(msg);
                    });
                return deferred.promise;;
            };

        }]);


});