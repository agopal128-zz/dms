"use strict";
define(['angularAMD'], function (angularAMD) {
    angularAMD.service('organizationalDataService', ['configService', '$http', '$log', '$q',
        function (configService, $http, $log, $q) {

            var baseUrl = configService.getApiBaseUrl() + "OrganizationalData/";

            //Post Add business segment 
            this.addOrUpdateBusinessSegments = function (organization) {
                var deferred = $q.defer();
                var url = baseUrl + "AddOrUpdateBusinessSegments";
                $http.post(url, organization)
                  .success(function (data) {
                      deferred.resolve(data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);

                  });
                return deferred.promise;
            };

             //Post Add divisions 
            this.addOrUpdateDivisions = function (organization) {
                var deferred = $q.defer();
                var url = baseUrl + "AddOrUpdateDivisions";
                $http.post(url, organization)
                  .success(function (data) {
                      deferred.resolve(data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);

                  });
                return deferred.promise;
            };

             //Post Add facilities 
            this.addOrUpdateFacilities = function (organization) {
                var deferred = $q.defer();
                var url = baseUrl + "AddOrUpdateFacilities"; 
                $http.post(url, organization)
                  .success(function (data) {
                      deferred.resolve(data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);

                  });
                return deferred.promise;
            };

            //Post Add product lines 
            this.addOrUpdateProductLines = function (organization) {
                var deferred = $q.defer();
                var url = baseUrl + "AddOrUpdateProductLines";
                $http.post(url, organization)
                  .success(function (data) {
                      deferred.resolve(data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);

                  });
                return deferred.promise;
            };

            //Post Add departments 
            this.addOrUpdateDepartments = function (organization) {
                var deferred = $q.defer();
                var url = baseUrl + "AddOrUpdateDepartments";
                $http.post(url, organization)
                  .success(function (data) {
                      deferred.resolve(data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);

                  });
                return deferred.promise;
            };

            //Post Add process 
            this.addOrUpdateProcess = function (organization) {
                var deferred = $q.defer();
                var url = baseUrl + "AddOrUpdateProcess";
                $http.post(url, organization)
                  .success(function (data) {
                      deferred.resolve(data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);

                  });
                return deferred.promise;
            };

            //Get all scorecard details
            this.getScorecardTemplateData = function (id) {
                var deferred = $q.defer();
                var url = baseUrl + "GetScorecardTemplateData/" + id;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            //Get Business Segments
            this.getBusinessSegments = function () {
                var deferred = $q.defer();
                var url = baseUrl + "GetBusinessSegments";
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

            //Get Facilities
            this.getFacilities = function () {
                var deferred = $q.defer();
                var url = baseUrl + "GetFacilities";
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

            //Get Divisions
            this.getDivisions = function () {
                var deferred = $q.defer();
                var url = baseUrl + "GetDivisions";
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

             //Get Departments
            this.getDepartments = function () {
                var deferred = $q.defer();
                var url = baseUrl + "GetDepartments";
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

            //Get ProductLines
            this.getProductLines = function () {
                var deferred = $q.defer();
                var url = baseUrl + "GetProductLines";
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

            //Get Processes
            this.getProcesses = function () {
                var deferred = $q.defer();
                var url = baseUrl + "GetProcesses";
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

            //Get Organizational data
            this.getOrganizationalData = function () {
                var deferred = $q.defer();
                var url = baseUrl + "GetOrganizationalData";
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

            //Get org data for assign metric page 
            this.getMetricMappingTemplateData = function () {
                var deferred = $q.defer();
                var url = baseUrl + "GetMetricMappingTemplateData";
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