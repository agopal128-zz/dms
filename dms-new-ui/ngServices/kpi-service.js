"use strict";
define(['angularAMD'], function (angularAMD) {
    angularAMD.service('kpiService', ['configService', '$http', '$log', '$q',
        function (configService, $http, $log, $q) {

            var baseUrl = configService.getApiBaseUrl() + "Scorecard/";

            this.getScorecardData = function (scorecardId, yearId, month) {
                var deferred = $q.defer();
                var url = baseUrl + "GetScorecardData/" + scorecardId + "/" + yearId + "/" + month;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            this.getBasicScorecardData = function (scorecardId, yearId, month) {
                var deferred = $q.defer();
                var url = baseUrl + "GetBasicScorecardData/" + scorecardId + "/" + yearId + "/" + month;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            this.getScorecardKPIData = function (scorecardId, kpiId, yearId, month) {
                var deferred = $q.defer();
                var url = baseUrl + "GetScorecardKPIData/" + scorecardId + "/" + kpiId + "/" + yearId + "/" + month;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            this.getDetailedSecondaryMetricData = function (kpiId, yearId, month, targetId) {
                var deferred = $q.defer();
                var url = baseUrl + "GetScorecardKPISecondaryMetricData/" + kpiId + "/" + targetId + "/" + yearId + "/" + month;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });

                return deferred.promise;
            };

            this.getFiscalMonthStatusForScorecard = function (scorecardId, yearId) {
                var deferred = $q.defer();
                var url = baseUrl + "GetFiscalMonthStatusForScorecard/" + scorecardId + "/" + yearId;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            this.getFiscalMonthStatusForKPI = function (scorecardId, kpiId, yearId) {
                var deferred = $q.defer();
                var url = baseUrl + "GetFiscalMonthStatusForKPI/" + scorecardId + "/" + kpiId + "/" + yearId;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            this.MarkAsHoliday = function (targetId, date) {
                var deferred = $q.defer();
                var url = baseUrl + "MarkAsHoliday/" + targetId + "/" + date;
                $http.post(url, targetId, date)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            this.getScorecardKPIGraphData = function (scorecardId, kpiId, yearId, monthId) {
                var deferred = $q.defer();
                var url = baseUrl + "GetScorecardKPIGraphData/" + scorecardId + "/" + kpiId + "/" + yearId + "/" + monthId;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            this.getScorecardGraphData = function (scorecardId, yearId, monthId) {
                var deferred = $q.defer();
                var url = baseUrl + "GetScorecardGraphData/" + scorecardId + "/" + yearId + "/" + monthId;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });

                return deferred.promise;
            };

            //Get counter measures
            this.getCounterMeasures = function (scorecardId, kpiId, isShowClosed) {
                var deferred = $q.defer();
                var url = configService.getApiBaseUrl() + "CounterMeasure/GetCounterMeasures/" +
                    scorecardId + "/" + kpiId + "/" + isShowClosed;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });

                return deferred.promise;
            };

            //Get counter measure status 
            this.getAllCounterMeasureStatus = function () {
                var deferred = $q.defer();
                var url = configService.getApiBaseUrl() +
                    "CounterMeasure/GetAllCounterMeasureStatus";
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });

                return deferred.promise;
            };

            this.getScorecardKPIMetrics = function (scorecardId, kpiId, yearId, month) {
                var deferred = $q.defer();
                var url = configService.getApiBaseUrl() + "CounterMeasure/GetScorecardKPIMetrics/"
                    + scorecardId + '/' + kpiId + '/' + yearId + '/' + month;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });

                return deferred.promise;
            };

            this.getScorecardMetricKPIGraphData = function (metricTargetId, kpiId, yearId, monthId) {
                var deferred = $q.defer();
                var url = baseUrl + "GetScorecardMetricKPIGraphData/" + metricTargetId + "/" + kpiId + "/" + yearId + "/" + monthId;
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