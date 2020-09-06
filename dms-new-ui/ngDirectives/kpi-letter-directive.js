"use strict";
define(['angularAMD', 'metricType'], function (angularAMD) {

    angularAMD.directive('kpiLetter', function () {
        return {
            restrict: 'E',
            scope: {
                kpiId: "=",
                dialyData: "=",
                monthlyData: "=",
                metricDetails: "=",
                displayPopup: '&'
            },
            controller: function ($scope) {
                var self = this;
                self.kpiId = $scope.kpiId;
                $scope.$watch('dialyData', function () {
                    self.dialyData = $scope.dialyData;
                    self.showDays = (self.dialyData && self.dialyData.length > 0) ? true : false;
                });
                $scope.$watch('monthlyData', function () {
                    self.monthlyData = $scope.monthlyData;
                    self.showMonths = (self.monthlyData && self.monthlyData.length > 0) ? true : false;
                });
                $scope.$watch('metricDetails', function () {
                    self.metricDetails = $scope.metricDetails;
                });
                $scope.$watch('kpiId', function () {
                    self.kpiId = $scope.kpiId;
                    switch (self.kpiId) {
                        case 0:
                            self.url = 'ngViews/kpIs/templates/safety.tpl.min.html';
                            break;
                        case 1:
                            self.url = 'ngViews/kpIs/templates/quality.tpl.min.html';
                            break;
                        case 2:
                            self.url = 'ngViews/kpIs/templates/delivery.tpl.min.html';
                            break;
                        case 3:
                            self.url = 'ngViews/kpIs/templates/innovation.tpl.min.html';
                            break;
                        case 4:
                            self.url = 'ngViews/kpIs/templates/cost.tpl.min.html';
                            break;
                        case 5:
                            self.url = 'ngViews/kpIs/templates/people.tpl.html';
                            break;
                        case 6:
                            self.url = 'ngViews/kpIs/templates/revenue.tpl.min.html';
                            break;
                        case 7:
                            self.url = 'ngViews/kpIs/templates/net-working-capital.tpl.min.html';
                            break;
                    }
                });

            },
            controllerAs: 'ctrl',
            template: '<div data-ng-include="ctrl.url"></div>',

            link: function (scope, elm, attr) {
                scope.getStatusClass = function (status) {
                    switch (status) {
                        case 0: return 'inactive';
                        case 1: return 'green';
                        case 2: return 'red';
                        case 3: return 'blue';
                        case 4: return 'nottracked';
                        case 5: return 'grey';
                    }
                }
            }

        };
    });
});