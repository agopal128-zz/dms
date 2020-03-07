'use strict';
define(['angularAMD', 'scorecardService', 'd3js', 'hierarchyChartDirective', 'hierarchyChartHelper'],
    function (angularAMD) {

        angularAMD.controller('ScorecardHierarchyController', ['scorecardService', 'validationService', 'notificationService', 'configService', 'hierarchyChartDirective', '$routeParams', '$rootScope', '$location', '$scope',
        function (scorecardService, validationService, notificationService, configService, hierarchyChartDirective, $routeParams, $rootScope, $location, $scope) {
            var self = this;
            self.hierarchyData = [];

            if (parseInt($routeParams.default) === 0) {
                $rootScope.lastActiveTopLevelScorecardId = null;
            }

            self.selectedRootNodeId = $rootScope.lastActiveTopLevelScorecardId;
            self.selectedScorecardId = null;
            self.activeTopLevelScorecardId = null;
            $rootScope.title = "Hierarchy";
            $rootScope.kpiOwners = [];
            $rootScope.scorecardId = "";

            self.loadHierarchy = function (changeTitle) {
                scorecardService.getScorecardHierarchyData(self.selectedRootNodeId, self.selectedScorecardId)
                    .then(function (data) {
                        self.hierarchyData = data;
                        self.activeTopLevelScorecardId = self.hierarchyData.id;


                        var rootNode = findCurrentRootNode(self.hierarchyData);
                        self.isRootNodeInactive = rootNode.isActive ? false : true;

                        if (changeTitle) {
                            var defaultNode = findDefaultNode(self.hierarchyData);
                            $rootScope.title = "Hierarchy : " + defaultNode.name;
                        }
                    }, function (err) {
                        if (err) {
                            notificationService.notify(err.errors.join("</br>"), {
                                autoclose: false,
                                type: 'danger'
                            })
                        }
                    });
            };

            $scope.$on('hierarchyToggle', function (event, args) {
                self.showOnlyActiveScorecards = !args.showAllScorecards;
                if (toggleActiveRef)
                    toggleActiveRef(self.showOnlyActiveScorecards);
            });

            $scope.$on('selectedHierarchyOption', function (event, args) {
                if (args.topScorecardId == self.activeTopLevelScorecardId) {
                    $rootScope.title = "Hierarchy : " + args.scorecardName;
                    setRootRef(args.id, args.parentScorecardId, self.showOnlyActiveScorecards);
               
               } else {
                    self.selectedRootNodeId = args.topScorecardId;
                    self.selectedScorecardId = args.id;
                    $rootScope.lastActiveTopLevelScorecardId = self.selectedRootNodeId;
                    $rootScope.title = "Hierarchy : " + args.scorecardName;
                    self.loadHierarchy();
                }
            });

            var findDefaultNode = function (d) {
                if (d.expandTillDrilldownLevel) {
                    return d;
                }
                if (d.children) {
                    for (var key in d.children) {
                        var obj = d.children[key];
                        obj._parent = d;
                        obj.parent = null;
                        var node = findDefaultNode(obj);
                        if (node) {
                            return node;
                        }
                    }
                }
                return null;
            };

            var findCurrentRootNode = function (d) {
                if (d.isRootNode) {
                    return d;
                }
                if (d.children) {
                    for (var key in d.children) {
                        var obj = d.children[key];
                        obj._parent = d;
                        obj.parent = null;
                        var node = findCurrentRootNode(obj);
                        if (node) {
                            return node;
                        }
                    }
                }
                return null;
            };

            var init = function () {
                self.showOnlyActiveScorecards = true;
                self.isRootNodeInactive = false;
                self.loadHierarchy(true);
            }();
        }]);
    });