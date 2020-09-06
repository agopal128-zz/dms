'use strict';
define(['angularAMD', 'metricsService', 'uiSelect', 'uiSelectWrap'], function (angularAMD) {

    angularAMD.controller('MetricsController', ['metricsService', 'validationService', 'notificationService',
    'configService', '$scope', '$q', '$rootScope',
        function (metricsService, validationService, notificationService, configService, $scope, $q, $rootScope) {
            var self = this,
                metricsResources = configService.getUIResources('Metrics');


            self.metric = [];
            self.metricData = [];
            self.goalTypes = [];
            self.dataTypes = [];
            self.metricLoaded = false;
            $rootScope.title = "Add  Metric";
            $rootScope.kpiOwners = [];
            $rootScope.scorecardId = "";

            self.isActiveOptions = [{ id: 1, name: 'Active' }, { id: 2, name: 'Inactive' }];
            //add  new row at top of the grid
            self.addNewRow = function () {
                self.gridOptions.data.unshift({
                    id: "",
                    name: '',
                    dataType: '',
                    goalType: '',
                    isActive: ''
                });
            };
            //refresh button click
            self.refreshData = function () {
                self.gridOptions.data.length = 0;
                self.metricLoaded = false;
                loadMetricData();
            };

            //ui-grid column specifications
            self.metricColumnDefs = [
                 { name: 'id', displayName: 'Id', field: 'id', enableCellEdit: false, visible: false },
                 {
                     name: 'name', displayName: 'Metric Name', field: 'name', enableCellEdit: true,
                     validators: { required: true }, enableHiding: false, width: "35%", enableSorting: true,
                     filter: {
                         placeholder: 'Search'
                     },
                 },
                 {
                     name: 'dataType', displayName: 'Data Type', enableCellEdit: true, width: "30%",
                     cellFilter: "griddropdown:editDropdownOptionsArray:editDropdownIdLabel:editDropdownValueLabel:row.entity.dataType.name",
                     editableCellTemplate: 'ngViews/metrics/templates/uiSelect.html',
                     editDropdownIdLabel: 'id',
                     editDropdownValueLabel: 'name',
                     enableSorting: false, enableColumnMenu: false, enableFiltering: false

                 },
                 {
                     name: 'goalType', displayName: 'Goal Type', enableCellEdit: true, width: "20%",
                     cellFilter: "griddropdown:editDropdownOptionsArray:editDropdownIdLabel:editDropdownValueLabel:row.entity.goalType.name",
                     editableCellTemplate: 'ngViews/metrics/templates/uiSelect.html',
                     editDropdownValueLabel: 'name',
                     editDropdownIdLabel: 'id',
                     enableSorting: false, enableColumnMenu: false, enableFiltering: false

                 },
                 {
                     name: 'isActive', displayName: 'Status', enableCellEdit: true, width: "15%",
                     cellFilter: "griddropdown:editDropdownOptionsArray:editDropdownIdLabel:editDropdownValueLabel:row.entity.isActive.name",
                     editableCellTemplate: 'ngViews/metrics/templates/uiSelect.html',
                     editDropdownValueLabel: 'name',
                     editDropdownIdLabel: 'id',
                     enableSorting: false, enableColumnMenu: false, enableFiltering: false
                 }
            ];

            //setting validation rules
            self.validationRules = {
                Name: [new ValidationRule(validationType.required, true, metricsResources.RequiredMetricName)]
            };

            //initial load grid data
            var loadMetricData = function () {
                //Get All Metrics
                metricsService.getAllMetrics()
                   .then(
                       function (data) {
                           self.metricData = data.reverse();
                           self.metricData = $.grep(self.metricData, function (elm) {
                               elm.isActive = {
                                   id: elm.isActive === true ? 1 : 2,
                                   name: elm.isActive === true ? 'Active' : 'Inactive'
                               };
                               return elm.isActive;
                           });
                           self.gridOptions.data = self.metricData;
                           self.metricLoaded = true;
                       },
                       function (err) {
                           notificationService.notify(err.message[0], { type: "danger" });
                       });
            };

            // getting the inital lookup data
            self.getLookupData = function () {
                //Get all lookup data (goaType & dataType)
                metricsService.getMetricTemplateData()
                    .then(
                        function (data) {
                            self.dataTypes = data.dataTypes;
                            self.goalTypes = data.goalTypes;
                            setDropdownOptionsArray();
                        },
                        function (err) {
                            notificationService.notify(err.Errors[0], { type: "danger" });
                        }
                    );
            };

            // Setting the [editDropdownOptionsArray] for grid dropdown columns
            var setDropdownOptionsArray = function () {
                self.gridOptions.columnDefs[2].editDropdownOptionsArray = self.dataTypes;
                self.gridOptions.columnDefs[3].editDropdownOptionsArray = self.goalTypes;
                self.gridOptions.columnDefs[4].editDropdownOptionsArray = self.isActiveOptions;
            };

            //ui-grid specifications
            self.gridOptions = {
                rowHeight: 38,
                data: self.metricData,
                columnDefs: self.metricColumnDefs,
                enableRowSelection: true,
                enableRowHeaderSelection: false,
                enableFiltering: true
            };

            //setting the row object to metric array before addOrUpdate service call
            self.setMetricData = function (rowEntity) {
                self.metric.push({
                    id: rowEntity.id,
                    dataType: {
                        id: rowEntity.dataType.id
                    },
                    goalType: {
                        id: rowEntity.goalType.id
                    },
                    name: rowEntity.name,
                    isActive: parseInt(rowEntity.isActive.id) === 2 ? false : true
                });
            };

            var checkAddorUpdate = function (id) {
                self.addOperation = id === "" ? true : false;
            };
            //ui-grid saveRow function
            self.saveRow = function (rowEntity) {
                self.metricLoaded = false;
                self.setMetricData(rowEntity);
                checkAddorUpdate(self.metric[0].id);
                var deffered = $q.defer();

                self.gridApi.rowEdit.setSavePromise(rowEntity, deffered.promise);
                //service call for add or update function
                metricsService.addOrUpdateMetric(self.metric).then(function (data) {
                    deffered.resolve();
                    if (self.addOperation) {
                        notificationService.notify(metricsResources.AddedSuccessfully, {
                            type: 'success'
                        }).then(function () {
                            self.gridOptions.data.length = 0;
                            loadMetricData();
                        }, function (err) {
                        });
                    }
                    else {
                        notificationService.notify(metricsResources.UpdatedSuccessfully, {
                            type: 'success'
                        }).then(function () {
                            self.gridOptions.data.length = 0;
                            loadMetricData();
                        }, function (err) {
                        });
                    }
                }, function (err) {
                    deffered.resolve();
                    notificationService.notify(err.errors.join("<br/>"), {
                        autoClose: false, type: "danger"
                    });
                });
                self.metric = [];

            };

            //ui-grid specifications
            self.gridOptions.onRegisterApi = function (gridApi) {
                self.gridApi = gridApi;
                gridApi.rowEdit.on.saveRow($scope, self.saveRow);
            };

            //initial load functions
            var init = function () {
                self.getLookupData();
                loadMetricData();
            }();

        }]);
});
