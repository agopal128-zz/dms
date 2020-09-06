"use strict";
define(['angularAMD'], function (angularAMD) {
    angularAMD.service('scorecardService', ['configService', '$http', '$log', '$q',
        function (configService, $http, $log, $q) {

            var baseUrl = configService.getApiBaseUrl() + "ScorecardAdmin/";
            var self = this;
            self.hierarchyDropdownDto = {
                kpiOwnerScorecards: [],
                teamMemberScorecards: [],
                rootScorecards: [],
                scorecardExists: false
            };

            this.addOrRemoveFromExistingList = function (scorecardItemList, updatedScorecardList) {

                var updatedMenuItemList = updatedScorecardList.map(function (scorecard) {
                    return {
                        id: scorecard.id,
                        name: scorecard.namePrefix ? scorecard.namePrefix + ' - ' + scorecard.name : scorecard.name,
                        scorecardName: scorecard.name,
                        topScorecardId: scorecard.topScorecardId,
                        parentScorecardId: scorecard.parentScorecardId
                    }
                });
                scorecardItemList.splice(0, scorecardItemList.length);
                scorecardItemList.push.apply(scorecardItemList, updatedMenuItemList);
            }

            this.updateExistingDropDownDto = function (updatedDropdownDto) {
                self.hierarchyDropdownDto.scorecardExists = updatedDropdownDto.scorecardExists;
                self.addOrRemoveFromExistingList(self.hierarchyDropdownDto.kpiOwnerScorecards, updatedDropdownDto.kpiOwnerScorecards);
                self.addOrRemoveFromExistingList(self.hierarchyDropdownDto.teamMemberScorecards, updatedDropdownDto.teamMemberScorecards);
                self.addOrRemoveFromExistingList(self.hierarchyDropdownDto.rootScorecards, updatedDropdownDto.rootScorecards);
            }
            //get Scorecard by id
            this.getScorecard = function (id) {
                var deferred = $q.defer();
                var url = baseUrl + id;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            //Get scorecard hierarchy data
            this.getScorecardHierarchyData = function (rootNodeId, scorecardId) {

                var deferred = $q.defer();
                var url = baseUrl + "GetScorecardHierarchy/" + rootNodeId + "/" + scorecardId;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

            //gets the list of root nodes available to be viewed by the user
            this.getHierarchyDropdownList = function () {

                var deferred = $q.defer();
                var url = baseUrl + "GetHierarchyDropdownList";
                $http.get(url)
                    .success(function (data) {

                        self.updateExistingDropDownDto(data.data)
                        deferred.resolve(self.hierarchyDropdownDto);
                    }).error(function (msg, code) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            };

            //Post Add scorecard
            this.addScorecard = function (scorecard) {

                var deferred = $q.defer();
                var url = baseUrl + "AddScorecard";
                $http.post(url, scorecard)
                  .success(function (data) {
                      deferred.resolve(data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);

                  });
                return deferred.promise;
            };
            //Update scorecard
            this.updateScorecard = function (scorecard) {

                var deferred = $q.defer();
                var url = baseUrl + "UpdateScorecard";
                $http.put(url, scorecard)
                  .success(function (data) {
                      deferred.resolve(data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;
            };

            // change sort order
            this.changeSortOrder = function (swappedScorecards) {
                var deferred = $q.defer();
                var url = baseUrl + 'ChangeSortOrder';
                $http.put(url, swappedScorecards)
                    .success(function (data) {
                        deferred.resolve(data);
                    }).error(function (msg, code) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            };
            //Get Facilities
            this.getFacilities = function (regionId) {
                var deferred = $q.defer();
                var url = baseUrl + "GetFacilities/" + regionId;
                $http.get(url, regionId)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

            //Get Divisions
            this.getDivisions = function (businessSegmentId) {
                var deferred = $q.defer();
                var url = baseUrl + "GetDivisions/" + businessSegmentId;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });
                return deferred.promise;

            };

            // Service to load all active directory users
            this.loadADUsers = function (query) {
                var deferred = $q.defer();
                var url = configService.getApiBaseUrl() + "User/GetADUsersWithLastNameOrAccountName?name=" + query;
                $http.get(url)
                     .success(function (data) {
                         deferred.resolve(data.data);
                     }).error(function (msg, code) {
                         deferred.reject(msg);
                     });
                return deferred.promise;
            };

            // Service to load all NDMS users
            this.loadNDMSUsers = function (query) {
                var deferred = $q.defer();
                var url = configService.getApiBaseUrl() + "User/GetNDMSUsersWithLastName/" + query;
                $http.get(url)
                     .success(function (data) {
                         deferred.resolve(data.data);
                     }).error(function (msg, code) {
                         deferred.reject(msg);
                     });
                return deferred.promise;
            };

            this.addCounterMeasure = function (counterMeasureObject) {
                var deferred = $q.defer();
                var url = configService.getApiBaseUrl() + "CounterMeasure/AddCounterMeasure";
                $http.post(url, counterMeasureObject)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });

                return deferred.promise;
            };

            this.updateCounterMeasure = function (counterMeasureObject) {
                var deferred = $q.defer();
                var url = configService.getApiBaseUrl() + "CounterMeasure/EditCounterMeasure";
                $http.put(url, counterMeasureObject)
                  .success(function (data) {
                      deferred.resolve(data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });

                return deferred.promise;
            };

            this.getCounterMeasureById = function (id) {
                var deferred = $q.defer();
                var url = configService.getApiBaseUrl() + "CounterMeasure/GetCounterMeasure/" + id;
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });

                return deferred.promise;
            };

            this.getCounterMeasurePriority = function () {
                var deferred = $q.defer();
                var url = configService.getApiBaseUrl() + "CounterMeasure/GetAllCounterMeasurePriority";
                $http.get(url)
                  .success(function (data) {
                      deferred.resolve(data.data);
                  }).error(function (msg, code) {
                      deferred.reject(msg);
                  });

                return deferred.promise;
            }

            // Retrieves the scorecard and target status belongs to the current logged in user
            this.getScorecardAndTargetStatus = function () {
                var deferred = $q.defer();
                var url = configService.getApiBaseUrl() + "User/GetScorecardIdAndTargetStatus";

                $http.get(url)
                 .success(function (data) {
                     deferred.resolve(data.data);
                 }).error(function (msg, code) {
                     if (code === 404) {
                         var message = "There is no scorecard available for current user!";
                         deferred.reject(message);
                     }
                     else {
                         deferred.reject(msg);
                     }
                 });
                return deferred.promise;
            };

        }]);
});