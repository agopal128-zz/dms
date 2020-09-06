"use strict";
define(['angularAMD'], function (angularAMD) {
    angularAMD.service('metricsService', ['configService', '$http', '$log', '$q',
        function (configService, $http, $log, $q) {

            var baseUrl = configService.getApiBaseUrl() + "Metric/";
        
            //Post Add scorecard 
            this.addOrUpdateMetric = function (metric) {
                var deferred = $q.defer();
                var url = baseUrl + "AddorUpdateMetric";
                $http.post(url, metric)
                  .success(function (data) {
                      deferred.resolve(data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);

                  });
                return deferred.promise;
            };

            //Get All Metrics
            this.getAllMetrics = function () {
                var deferred = $q.defer();
                var url = baseUrl + "GetAll";
               // var url ="http://localhost/NDMS.UI/ngServices/Json/metricData.json";
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };
            //Get All LookUp Data(dataType, goalType)
            this.getMetricTemplateData = function () {
                var deferred = $q.defer();
                var url = baseUrl + "GetMetricTemplateData";
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };
            //Get All metric mapping info
            this.getAllAssignedMetricData = function () {
                var deferred = $q.defer();
                var url = baseUrl + "GetAllMetricMappings";
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };
            //load all metrics for colomn metrics
             this.getAllMetrics = function () {
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
             //add or update metric mapping data                                                                       
             this.addOrUpdateMetricMapping = function (mappingInfo) {
                 var deferred = $q.defer();
                 var url = baseUrl + "AddorUpdateMetricMapping";
                 $http.post(url, mappingInfo)
                   .success(function (data) {
                       deferred.resolve(data);
                   }).error(function (msg, code) {
                       deferred.reject(msg);

                   });
                 return deferred.promise;
             };


             
        }]);

});