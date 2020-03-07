"use strict";
define(['angularAMD'], function (angularAMD) {
    angularAMD.service('actualsService', ['configService', '$http', '$log', '$q',
        function (configService, $http, $log, $q) {

            var baseUrl = configService.getApiBaseUrl() + "Actuals/",
            formUrlForActualStatusRequest = function (actualStatusRequest) {

                var url = configService.getApiBaseUrl() +
                    "CounterMeasure/GetActualandCounterMeasureStatus?";

                if (actualStatusRequest.targetId !== undefined) {
                    url = url + "TargetId=" + actualStatusRequest.targetId + "&";
                }
                if (actualStatusRequest.goalValue !== undefined) {
                    url = url + "GoalValue=" + actualStatusRequest.goalValue + "&";
                }
                if (actualStatusRequest.actualValue !== undefined) {
                    url = url + "ActualValue=" + actualStatusRequest.actualValue + "&";
                }
                if (actualStatusRequest.selectedDate !== undefined) {
                    url = url + "SelectedDate=" + actualStatusRequest.selectedDate;
                }
                return url;
            };

            this.addActuals = function (actualObject) {
                var deferred = $q.defer();
                var url = baseUrl + "AddActual";
                $http.post(url, actualObject)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            this.updateActuals = function (actualObject) {
                var deferred = $q.defer();
                var url = baseUrl + "EditActual";
                $http.put(url, actualObject)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };


            this.getActualStatusandCounterMeasure = function (actualStatusRequestItem) {
                var deferred = $q.defer();
                var url = formUrlForActualStatusRequest(actualStatusRequestItem);
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            this.getTargets = function (targetId, month, day) {
                var deferred = $q.defer();
                var url;
                if (day)
                    url = baseUrl + "GetDailyTarget/" + targetId + "/" + month + "/" + day;
                else
                    url = baseUrl + "GetMonthlyTarget/" + targetId + "/" + month;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            this.changeHoliday = function (scorecardId, targetId, day, status) {
                var deferred = $q.defer();
                var url;
                if (status == 3)
                    url = baseUrl + "UnmarkHoliday/" + scorecardId + "/" + targetId + "/" + day ;
                else
                    url = baseUrl + "MarkHoliday/" + scorecardId + "/" + targetId + "/" + day;
                $http.post(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };
            
            this.getDrillDownHierarchy = function (scorecardId, kpiId, monthId, yearId) {
                var deferred = $q.defer();
                var url = baseUrl + "GetDrillDownHierarchy/" + scorecardId + "/" + kpiId + "/" + monthId + "/" + yearId;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });

                return deferred.promise;
            };

            this.getDrillDownHierarchyOnDate = function (scorecardId, kpiId, date) {
                var deferred = $q.defer();
                var url = baseUrl + "GetDrillDownHierarchyOnDate/" + scorecardId + "/" + kpiId + "/" + date;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });

                return deferred.promise;
            };

            this.isUserAuthorizedToAlterScorecardEntries = function (scorecardId) {
                var deferred = $q.defer();
                var url = baseUrl + "IsUserAuthorizedToAlterScorecardEntries/" + scorecardId;
                $http.get(url)
                    .success(function (data) {
                        deferred.resolve(data.data);
                    }).error(function (msg, code) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            };

            this.getScorecardKPIs = function (scorecardId, monthId, yearId) {
                var deferred = $q.defer();
                var url = baseUrl + "GetScorecardKPIs/" + scorecardId + "/" + monthId + "/" + yearId;
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