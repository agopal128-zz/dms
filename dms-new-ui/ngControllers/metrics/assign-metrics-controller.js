'use strict';
define(['angularAMD', 'metricsService', 'organizationalDataService', 'uiSelect', 'uiSelectWrap'], function (angularAMD) {

    angularAMD.controller('AssignMetricsController', ['metricsService', 'organizationalDataService',
        'validationService', 'notificationService', 'configService', '$scope', '$q', '$rootScope',
    function (metricsService, organizationalDataService, validationService,
        notificationService, configService, $scope, $q, $rootScope) {
        var self = this,
            metricsResources = configService.getUIResources('Metrics');

        self.assignMetricRow = [];
        self.assignedMetricData = [];
        self.businessSegment = [];
        self.division = [];
        self.facility = [];
        self.productline = [];
        self.department = [];
        self.process = [];
        self.kpi = [];
        self.metricLoaded = false;
        self.metrics = [];
        self.activeMetrics = [];
        $rootScope.title = "Assign Metrics";
        $rootScope.kpiOwners = [];
        $rootScope.scorecardId = "";

        //add new row function
        self.addNewRow = function () {

            self.gridOptions.data.unshift({
                id: "",
                businessSegment: {
                    id: self.businessSegment[0].id,
                    name: self.businessSegment[0].name
                },
                division: {
                    id: self.division[0].id,
                    name: self.division[0].name
                },
                facility: {
                    id: self.facility[0].id,
                    name: self.facility[0].name
                },
                productline: {
                    id: self.productline[0].id,
                    name: self.productline[0].name
                },
                department: {
                    id: self.department[0].id,
                    name: self.department[0].name
                },
                process: {
                    id: self.process[0].id,
                    name: self.process[0].name
                },
                kpi: '',
                metrics: '',
                isActive: true
            });

        };

        //refresh button click
        self.refreshData = function () {
            self.gridOptions.data.length = 0;
            self.metricLoaded = false;
            loadAssignedMetricData();
        };

        //ui-grid column specifications
        self.assignMetricColumnDefs = [
             { name: 'id', displayName: 'Id', field: 'id', enableCellEdit: false, visible: false },
             {
                 name: 'businessSegment', displayName: 'Business Segment', type: 'object', enableCellEdit: true,
                 cellFilter: "griddropdown:editDropdownOptionsArray:editDropdownIdLabel:editDropdownValueLabel:row.entity.businessSegment.name",
                 editableCellTemplate: 'ngViews/metrics/templates/uiSelect.html',
                 editDropdownIdLabel: 'id',
                 editDropdownValueLabel: 'name',
                 width: "14%", enableHiding: false
             },
             {
                 name: 'division', displayName: 'Division', type: 'object', enableCellEdit: true,
                 cellFilter: "griddropdown:editDropdownOptionsArray:editDropdownIdLabel:editDropdownValueLabel:row.entity.division.name",
                 editableCellTemplate: 'ngViews/metrics/templates/uiSelect.html',
                 editDropdownIdLabel: 'id',
                 editDropdownValueLabel: 'name',
                 width: "14%", enableHiding: false

             },
             {
                 name: 'facility', displayName: 'Facility', type: 'object', enableCellEdit: true,
                 cellFilter: "griddropdown:editDropdownOptionsArray:editDropdownIdLabel:editDropdownValueLabel:row.entity.facility.name",
                 editableCellTemplate: 'ngViews/metrics/templates/uiSelect.html',
                 editDropdownIdLabel: 'id',
                 editDropdownValueLabel: 'name',
                 width: "11%", enableHiding: false
             },
             {
                 name: 'productline', displayName: 'Product Line', type: 'object', enableCellEdit: true,
                 cellFilter: "griddropdown:editDropdownOptionsArray:editDropdownIdLabel:editDropdownValueLabel:row.entity.productline.name",
                 editableCellTemplate: 'ngViews/metrics/templates/uiSelect.html',
                 editDropdownIdLabel: 'id',
                 editDropdownValueLabel: 'name',
                 width: "12%", enableHiding: false
             },
             {
                 name: 'department', displayName: 'Department', type: 'object', enableCellEdit: true,
                 cellFilter: "griddropdown:editDropdownOptionsArray:editDropdownIdLabel:editDropdownValueLabel:row.entity.department.name",
                 editableCellTemplate: 'ngViews/metrics/templates/uiSelect.html',
                 editDropdownIdLabel: 'id',
                 editDropdownValueLabel: 'name',
                 enableHiding: false, width: "11%"
             },
             {
                 name: 'process', displayName: 'Process', type: 'object', enableCellEdit: true,
                 cellFilter: "griddropdown:editDropdownOptionsArray:editDropdownIdLabel:editDropdownValueLabel:row.entity.process.name",
                 editableCellTemplate: 'ngViews/metrics/templates/uiSelect.html',
                 editDropdownIdLabel: 'id',
                 editDropdownValueLabel: 'name',
                 enableHiding: false, width: "10%"
             },
             {
                 name: 'kpi', displayName: 'KPI', type: 'object', enableCellEdit: true,
                 cellFilter: "griddropdown:editDropdownOptionsArray:editDropdownIdLabel:editDropdownValueLabel:row.entity.kpi.name",
                 editableCellTemplate: 'ngViews/metrics/templates/uiSelect.html',
                 editDropdownIdLabel: 'id',
                 editDropdownValueLabel: 'name',
                 enableHiding: false, width: "10%"
             },
             {
                 name: 'metrics', displayName: 'Metric', type: 'object', enableCellEdit: true,
                 cellFilter: "griddropdown:editDropdownOptionsArray:editDropdownIdLabel:editDropdownValueLabel:row.entity.metrics.name",
                 editableCellTemplate: 'ngViews/metrics/templates/uiSelect.html',
                 editDropdownIdLabel: 'id',
                 editDropdownValueLabel: 'name',
                 enableHiding: false, width: "18%"
             },
             {
                 name: 'isActive', displayName: 'Status', type: 'boolean',
                 enableCellEdit: false, visible: false
             }
        ];

        //setting validation rules
        self.validationRules = {
            Name: [new ValidationRule(validationType.required, true, metricsResources.RequiredMetricName)]
        };

        var setGridOptionsData = function (data) {
            for (var i = 0; i < data.length; i++) {
                self.assignedMetricData.push({
                    id: data[i].id,
                    isActive: data[i].isActive,
                    businessSegment: {
                        id: data[i].businessSegmentId,
                        name: data[i].businessSegmentName
                    },
                    division: {
                        id: data[i].divisionId,
                        name: data[i].divisionName
                    },
                    facility: {
                        id: data[i].facilityId,
                        name: data[i].facilityName
                    },
                    productline: {
                        id: data[i].productLineId,
                        name: data[i].productLineName
                    },
                    department: {
                        id: data[i].departmentId,
                        name: data[i].departmentName
                    },
                    process: {
                        id: data[i].processId,
                        name: data[i].processName
                    },
                    kpi: {
                        id: data[i].kpiId,
                        name: data[i].kpiName
                    },
                    metrics: {
                        id: data[i].metricId,
                        name: data[i].metricName
                    }

                });
            }
            self.assignedMetricData.reverse();
        };

        //initial load of grid with data
        var loadAssignedMetricData = function () {
            //Get All Metrics
            metricsService.getAllAssignedMetricData()
               .then(
                   function (data) {
                       setGridOptionsData(data);
                       self.gridOptions.data = self.assignedMetricData;
                       self.metricLoaded = true;
                   },
                   function (err) {
                       notificationService.notify(err.errors[0].join("<br/>"), { type: "danger" });
                   });
        };

        //setting the editDropdownOptionsArray for drop down columns
        var setDropdownOptionsArray = function () {
            self.gridOptions.columnDefs[1].editDropdownOptionsArray = self.businessSegment;
            self.gridOptions.columnDefs[2].editDropdownOptionsArray = self.division;
            self.gridOptions.columnDefs[3].editDropdownOptionsArray = self.facility;
            self.gridOptions.columnDefs[4].editDropdownOptionsArray = self.productline;
            self.gridOptions.columnDefs[5].editDropdownOptionsArray = self.department;
            self.gridOptions.columnDefs[6].editDropdownOptionsArray = self.process;
            self.gridOptions.columnDefs[7].editDropdownOptionsArray = self.kpi;
        };

        //initial lookup data for dropdown options
        self.getLookupData = function () {
            //Get all lookup data
            organizationalDataService.getMetricMappingTemplateData().then(
                    function (data) {
                        self.businessSegment = data.businessSegments;
                        self.division = data.divisions;
                        self.facility = data.facilities;
                        self.productline = data.productLines;
                        self.department = data.departments;
                        self.process = data.processes;
                        self.kpi = data.kpIs;
                        setDropdownOptionsArray();
                    },
                    function (err) {
                        notificationService.notify(err.errors[0].join("<br/>"), { type: "danger" });
                    }
                );

            metricsService.getAllMetrics().then(
                function (data) {
                    self.metrics = data;

                    angular.forEach(self.metrics,function (elm) {
                        if (elm.isActive === true) {
                            self.activeMetrics.push({
                                id: elm.id,
                                name: elm.name
                            });
                        }
                    });

                    self.gridOptions.columnDefs[8].editDropdownOptionsArray = self.activeMetrics;
                },
                function (err) {
                    notificationService.notify(err.errors[0].join("<br/>"), { type: "danger" });
                });
        };

        //ui-grid specification
        self.gridOptions = {
            rowHeight: 38,
            data: self.assignedMetricData,
            columnDefs: self.assignMetricColumnDefs,
            enableRowSelection: false,
            enableRowHeaderSelection: false,
            enableCellEditOnFocus: false,
            enableSorting: false,
            enableColumnMenus: false
        };

        //setting the mapping data before add or update service call
        self.setMetricMappingData = function (rowEntity) {
            self.assignMetricRow.push({
                id: rowEntity.id,
                businessSegmentId: rowEntity.businessSegment.id,
                divisionId: rowEntity.division.id,
                facilityId: rowEntity.facility.id,
                productLineId: rowEntity.productline.id,
                departmentId: rowEntity.department.id,
                processId: rowEntity.process.id,
                kpiId: rowEntity.kpi.id,
                metricId: rowEntity.metrics.id,
                isActive: rowEntity.isActive
            });
        };

        var checkAddorUpdate = function (id) {
            self.addOperation = id === "" ? true : false;
        };

        //ui-grid saveRow function
        self.saveRow = function (rowEntity) {
            self.metricLoaded = false;
            self.setMetricMappingData(rowEntity);
            var deffered = $q.defer();
            self.gridApi.rowEdit.setSavePromise(rowEntity, deffered.promise);
            //service call for add or update function
            checkAddorUpdate(self.assignMetricRow[0].id);
            metricsService.addOrUpdateMetricMapping(self.assignMetricRow).then(
                function (data) {
                    deffered.resolve();
                    if (self.addOperation) {
                        notificationService.notify(metricsResources.AddedSuccessfully, {
                            type: 'success'
                        }).then(function () {
                            self.gridOptions.data.length = 0;
                            loadAssignedMetricData();
                        }, function (err) {
                        });
                    }
                    else {
                        notificationService.notify(metricsResources.UpdatedSuccessfully, {
                            type: 'success'
                        }).then(function () {
                            self.gridOptions.data.length = 0;
                            loadAssignedMetricData();
                        }, function (err) {
                        });
                    }
                },
                function (err) {
                    deffered.resolve();
                    notificationService.notify(err.errors.join("<br/>"), {
                        autoClose: false, type: "danger"
                    });
                });
            self.assignMetricRow = [];
        };

        //ui-grid specification
        self.gridOptions.onRegisterApi = function (gridApi) {
            self.gridApi = gridApi;
            gridApi.rowEdit.on.saveRow($scope, self.saveRow);
        };

        //initial load data function
        var init = function () {
            self.getLookupData();
            loadAssignedMetricData();
        }();

    }]);
});
