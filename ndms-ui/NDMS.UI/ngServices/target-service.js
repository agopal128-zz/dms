"use strict";
define(['angularAMD'], function (angularAMD) {
    angularAMD.service('targetService', ['configService', '$http', '$log', '$q',
        function (configService, $http, $log, $q) {

            var baseUrl = configService.getApiBaseUrl() + "Target/",
            formUrlForCascadeDetails = function (cascadeDetailsRequest) {

                var url = baseUrl + "GetCascadedMetricDetails?";

                if (cascadeDetailsRequest.scorecardId !== undefined) {
                    url = url + "ScorecardId=" + cascadeDetailsRequest.scorecardId + "&";
                }
                if (cascadeDetailsRequest.kpiId !== undefined) {
                    url = url + "KPIId=" + cascadeDetailsRequest.kpiId + "&";
                }
                if (cascadeDetailsRequest.metricId !== undefined) {
                    url = url + "MetricId=" + cascadeDetailsRequest.metricId + "&";
                }
                if (cascadeDetailsRequest.calendarYearId !== undefined) {
                    url = url + "CalendarYearId=" + cascadeDetailsRequest.calendarYearId + "&";
                }
                if (cascadeDetailsRequest.metricType !== undefined) {
                    url = url + "MetricType=" + cascadeDetailsRequest.metricType + "&";
                }
                if (cascadeDetailsRequest.targetId !== undefined) {
                    url = url + "TargetId=" + cascadeDetailsRequest.targetId;
                }
                if (cascadeDetailsRequest.targetEntryMethodId !== undefined) {
                    url = url + "&TargetEntryMethodId=" + cascadeDetailsRequest.targetEntryMethodId;
                }
                if (cascadeDetailsRequest.effectiveStartDate !== undefined) {
                    url = url + "&EffectiveStartDate=" + cascadeDetailsRequest.effectiveStartDate;
                }
                if (cascadeDetailsRequest.effectiveEndDate !== undefined) {
                    url = url + "&EffectiveEndDate=" + cascadeDetailsRequest.effectiveEndDate;
                }
                if (cascadeDetailsRequest.rollUpMethodId !== undefined) {
                    url = url + "&RollUpMethodId=" + cascadeDetailsRequest.rollUpMethodId;
                }
                return url;
            },
            formUrlForDailyTargetList = function (targetParams, monthlyTarget) {
                var url = baseUrl + "GenerateDailyTargets?";

                if(angular.isDefined(targetParams.scorecardId)){
                    url = url + "scorecardId=" + targetParams.scorecardId;
                }
                if(angular.isDefined(targetParams.yearId)){
                    url = url + "&yearId=" + targetParams.yearId;
                }
                if(angular.isDefined(targetParams.monthId)){
                    url = url + "&monthId=" + targetParams.monthId;
                }
                if (angular.isDefined(targetParams.metricId)) {
                    url = url + "&metricId=" + targetParams.metricId;
                }
                if(angular.isDefined(targetParams.effectiveStartDate)){
                    url = url + "&effectiveStartDate=" + targetParams.effectiveStartDate;
                }
                if (angular.isDefined(targetParams.targetEntryMethod)) {
                    url = url + "&targetEntryMethodId=" + targetParams.targetEntryMethod;
                }
                if(angular.isDefined(targetParams.effectiveEndDate)){
                    url = url + "&effectiveEndDate=" + targetParams.effectiveEndDate;
                }
                if(angular.isDefined(monthlyTarget.id)){
                    url= url + "&existingMonthlyTargetId=" + monthlyTarget.id;
                }
                if (angular.isDefined(monthlyTarget.goalValue)) {
                    url = url + "&monthlyGoalValue=" + monthlyTarget.goalValue;
                }
                if (angular.isDefined(monthlyTarget.dailyRateValue)) {
                    url = url + "&dailyRateValue=" + monthlyTarget.dailyRateValue;
                }
                return url;

            };

            //initial load Target data
            this.getTargetsInitialData = function (scorecardId, canEdit) {
                var deferred = $q.defer();
                var url = baseUrl + "GetTargetsInitialData/" + scorecardId + "/" + canEdit;
                $http.get(url)
                 .success(function (data) {
                     deferred.resolve(data.data);
                 }).error(function (msg, code) {
                     deferred.reject(msg);
                 });
                return deferred.promise;
            };

            //Get All KPIs 
            this.getKpis = function (scorecardId, yearId) {
                var deferred = $q.defer();
                var url = baseUrl + "GetScorecardKPIs/" + scorecardId + "/" + yearId;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

            //Get Months List 
            this.getMonthsList = function (yearId) {
                var deferred = $q.defer();
                var url = baseUrl + "GetMonthsListForCalendarYear/" + yearId;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

            //Get kpi's target details
            this.getTargetsForScorecardAndKPI = function (scorecardId, kpiId, yearId) {
                var deferred = $q.defer();

                var url = baseUrl + "GetTargetsForScorecardAndKPI/" + scorecardId + "/" + kpiId + "/" + yearId;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

            //Get metric names for drop down list
            this.getMetrics = function (kpiId, scorecardId) {
                var deferred = $q.defer();
                var url = baseUrl + "GetMetrics/" + kpiId + "/" + scorecardId;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

            //set primary target
            this.setTarget = function (targetData) {
                var deferred = $q.defer();
                var url;
                url = baseUrl + "AddMetricTarget";
                $http.post(url, targetData)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

            //update primary target
            this.updateTarget = function (targetData) {
                var url;
                var deferred = $q.defer();
                    url = baseUrl + "EditMetricTarget";
                $http.put(url, targetData)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

            this.getCascadedMetricDetails = function (cascadeDetailsRequestItem) {

                var deferred = $q.defer();
                var url = formUrlForCascadeDetails(cascadeDetailsRequestItem);
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

            this.getDailyTargets = function (id) {
                var deferred = $q.defer();
                var url = baseUrl + "GetDailyTargets/" + id;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };
            //Generates daily targets evenly distributed in case of montly Target Entry
            //else gets the holiday List
            this.generateDailyTargets = function (targetParams, monthlyTarget) {
                var deferred = $q.defer();
                var url = formUrlForDailyTargetList(targetParams, monthlyTarget);
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            }

            // Retrieves the maximum allowed monthly goals for a child target when metric is
            // getting cascaded
            this.getMaximumAllowedMonthlyGoals = function (parentTargetId, rollupMethodId, childTargetId, targetEntryMethodId) {
                var deferred = $q.defer();
                var url = baseUrl + "GetMaximumAllowedMonthlyGoals/"
                    + parentTargetId + "/" + rollupMethodId + "/" + targetEntryMethodId + "/" + childTargetId;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            // Deleted the specific metric target
            this.deleteMetricTarget = function (targetId, scorecardId) {
                var deferred = $q.defer();
                var url = baseUrl + "DeleteMetricTarget/" + scorecardId + "/" + targetId;
                $http.delete(url)
                    .success(function (data) {
                        deferred.resolve(data);
                    }).error(function (msg, code) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            };            

            // get all targets of scorecard for a selected year
            this.getSelectedYearTargets = function (scorecardId, yearId) {
                var deferred = $q.defer();
                var url = baseUrl + "GetTargetsForScorecard/"
                    + scorecardId + "/" + yearId;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            // copy current year targets to next year
            this.copyTargets = function (scorecardId, copiedMetrics) {
                var deferred = $q.defer();
                var url = baseUrl + "CopyTargets" + "/" + scorecardId;
                $http.post(url, copiedMetrics)
                  .success(function (data) {
                      deferred.resolve(data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);

                  });
                return deferred.promise;
            };

            this.getRolledUpTargets = function(targetId, targetEntryMethodId, mtdPerformanceTrackingId)
            {
                var deferred = $q.defer();
                var url = baseUrl + "GetRolledUpTargets/" + targetId + "/" + targetEntryMethodId
                        + "/" + mtdPerformanceTrackingId;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);

                  });
                return deferred.promise;
            }

        }]);
});