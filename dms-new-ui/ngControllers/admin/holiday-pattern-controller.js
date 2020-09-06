'use strict';
define(['angularAMD', 'holidayPatternService', 'uiSelect', 'uiSelectWrap'],
function (angularAMD) {

    angularAMD.controller('HolidayPatternController', ['holidayPatternService', 'notificationService', 'configService', '$scope', '$rootScope', '$q',
        function (holidayPatternService, notificationService, configService, $scope, $rootScope, $q) {
            var self = this,
                holidayResources = configService.getUIResources('Holiday');

            self.holidayPatterns = [];
            self.patternsLoaded = false;
            $rootScope.kpiOwners = [];
            $rootScope.title = 'Holiday Schedule';

            self.isActiveOptions = [{ id: 1, name: 'Active' }, { id: 2, name: 'Inactive' }];

            //add  new row at top of the grid
            self.addNewRow = function () {
                self.gridOptions.data.unshift({
                    id: "",
                    name: '',
                    isActive: ''
                });
            };

            //refresh button click
            self.refreshData = function () {
                self.gridOptions.data.length = 0;
                self.patternsLoaded = false;
                loadHolidayPatterns();
            };

            // UI Grid column defentions for holiday pattern
            self.HolidayPatternColumnDefs = [
                {
                    name: 'id', displayName: 'Id', field: 'id', enableCellEdit: false, visible: false
                },
                {
                    name: 'name', displayName: 'Holiday Schedule', field: 'name', enableCellEdit: true,
                    validators: {
                        required: true
                    },
                    cellTemplate: '<div><a ng-href ="Admin/HolidaySchedule/{{row.entity.id}}">{{row.entity.name}}</a></div>',
                    enableHiding: false, width: "50%", enableSorting: true,
                    filter: {
                        placeholder: 'Search'
                    }
                },
                {
                    name: 'isActive', displayName: 'Status', enableCellEdit: true, width: "30%",
                    cellFilter: "griddropdown:editDropdownOptionsArray:editDropdownIdLabel:editDropdownValueLabel:row.entity.isActive.name",
                    editableCellTemplate: 'ngViews/metrics/templates/uiSelect.html',
                    editDropdownValueLabel: 'name', editDropdownIdLabel: 'id', enableSorting: false, enableColumnMenu: false,
                    enableFiltering: false
                },
                {
                    name: 'copy', displayName: '', enableCellEdit: false, width: "20%",
                    cellTemplate: '<button class="btn button-add" ng-show="row.entity.id || row.entity.id === 0" ng-click="grid.appScope.copyPattern(row)">Copy</button>',
                    enableHiding: false, enableSorting: false, enableColumnMenu: false, enableFiltering: false
                }
            ];

            //setting validation rules
            self.validationRules = {
                Name: [new ValidationRule(validationType.required, true, holidayResources.RequiredHolidayPatternName)]
            };

            //ui-grid specifications
            self.gridOptions = {
                rowHeight: 43,
                data: self.metricData,
                columnDefs: self.HolidayPatternColumnDefs,
                enableRowSelection: true,
                enableRowHeaderSelection: false,
                enableFiltering: true
            };

            //setting the row object to holiday pattern before addOrUpdate service call
            self.setHolidayPatternData = function (rowEntity) {
                self.holidayPattern = {
                    id: rowEntity.id,
                    name: rowEntity.name,
                    isActive: rowEntity.isActive.id === 2 ? false : true
                };
            };

            self.checkAddorUpdate = function (id) {
                self.addOperation = id === "" ? true : false;
            };


            // copies pattern and create a new item named copied pattern
            
            $scope.copyPattern = function (row) {

                holidayPatternService.copyHolidayPattern(row.entity.id)
                 .then(function (data) {
                     notificationService.notify(holidayResources.CopiedSuccessfully.format(row.entity.name) , {
                         type: 'success'
                     }).then(function () {
                         self.refreshData();
                     });
                 }, function (err) {
                     notificationService.notify(err.errors.join("<br>"), {
                         autoclose: false, type: 'danger'
                     });
                 });
            };
            

            //ui-grid saveRow function
            self.saveRow = function (rowEntity) {
                self.patternLoaded = false;
                self.setHolidayPatternData(rowEntity);
                self.checkAddorUpdate(self.holidayPattern.id);
                var deffered = $q.defer();

                self.gridApi.rowEdit.setSavePromise(rowEntity, deffered.promise);
                //service call for add or update function
                holidayPatternService.addOrUpdateHolidayPattern(self.holidayPattern)
                    .then(function (data) {
                        deffered.resolve();
                        if (self.addOperation) {
                            notificationService.notify(holidayResources.AddedSuccessfully, {
                                type: 'success'
                            }).then(function () {
                                self.gridOptions.data.length = 0;
                                loadHolidayPatterns();
                            });
                        }
                        else {
                            notificationService.notify(holidayResources.UpdatedSuccessfully, {
                                type: 'success'
                            }).then(function () {
                                self.gridOptions.data.length = 0;
                                loadHolidayPatterns();
                            });
                        }
                    }, function (err) {
                        deffered.resolve();
                        notificationService.notify(err.errors.join("<br/>"), {
                            autoClose: true, type: "danger"
                        }).then(function(){
                            self.gridOptions.data.length = 0;
                            loadHolidayPatterns();
                        });
                    }); 
                self.holidayPattern = {};
            };

            //ui-grid specifications
            self.gridOptions.onRegisterApi = function (gridApi) {
                self.gridApi = gridApi;
                gridApi.rowEdit.on.saveRow($scope, self.saveRow);
            };

            // set all Dropdown options array
            var setDropdownOptionsArray = function () {
                self.gridOptions.columnDefs[2].editDropdownOptionsArray = self.isActiveOptions;
            };


            //initial grid data
            var loadHolidayPatterns = function () {
                holidayPatternService.getAllHolidayPatterns()
                        .then(function (data) {
                            self.holidayPatterns = data.reverse();
                            self.holidayPatterns = $.grep(self.holidayPatterns, function (elm) {
                                elm.isActive = {
                                    id: elm.isActive === true ? 1 : 2,
                                    name: elm.isActive === true ? 'Active' : 'Inactive'
                                };
                                return elm.isActive;
                            });
                            self.gridOptions.data = self.holidayPatterns;
                            self.patternsLoaded = true;
                        }, function (err) {
                            notificationService.notify(err.message[0], { type: 'danger' });
                        });

            };

            var init = function () {
                setDropdownOptionsArray();
                loadHolidayPatterns();
            }();
        }]);
})