'use strict';
define(['angularAMD', 'organizationalDataService', 'uiSelect', 'uiSelectWrap'],
function (angularAMD) {

    angularAMD.controller('OrganizationalDataController', ['organizationalDataService', 'notificationService', 'configService', '$scope', '$rootScope', '$q',
        function (organizationalDataService, notificationService, configService, $scope, $rootScope, $q) {
            var self = this,

            organizationDataResources = configService.getUIResources('OrganizationData');

            $rootScope.kpiOwners = [];
            $rootScope.title = 'List Maintenance';
            self.selectedType = 0;
            self.dataLoaded = false;
            self.organizationData = [];
            self.organization = [];

            self.isActiveOptions = [{ id: 1, name: 'Active' }, { id: 2, name: 'Inactive' }];

            //add  new row at top of the grid
            self.addNewRow = function () {
                self.gridOptions.data.unshift({
                    id: "",
                    name: '',
                    isActive: ''
                });
            };

            // UI Grid column defentions for holiday pattern
            self.OrganizationalDataColumnDefs = [
                {
                    name: 'id', displayName: 'Id', field: 'id', enableCellEdit: false, visible: false
                },
                {
                    name: 'name', displayName: 'Name', field: 'name', enableCellEdit: true,
                    validators: {
                        required: true
                    },
                    enableHiding: false, width: "50%", enableSorting: true,
                    filter: {
                        placeholder: 'Search'
                    }
                },
                {
                    name: 'isActive', displayName: 'Status', enableCellEdit: true, width: "50%",
                    cellFilter: "griddropdown:editDropdownOptionsArray:editDropdownIdLabel:editDropdownValueLabel:row.entity.isActive.name",
                    editableCellTemplate: 'ngViews/metrics/templates/uiSelect.html',
                    editDropdownValueLabel: 'name', editDropdownIdLabel: 'id', enableSorting: false, enableColumnMenu: false,
                    enableFiltering: false
                }
            ];

            //ui-grid specifications
            self.gridOptions = {
                rowHeight: 43,
                data: self.organizationData,
                columnDefs: self.OrganizationalDataColumnDefs,
                enableRowSelection: true,
                enableRowHeaderSelection: false,
                enableFiltering: true
            };
                         
            self.onListChange = function (selectedTypeId) {
                self.selectedType = selectedTypeId;
                switch (selectedTypeId) {
                    case 0: // Business Segments
                        loadBusinessSegments();
                        break;
                    case 1: // Divisions
                        loadDivisions();
                        break;
                    case 2: // Facilities
                        loadFacilities();
                        break;
                    case 3: // Product Lines
                        loadProductLines();
                        break;
                    case 4: // Departments
                        loadDepartments();
                        break;
                    case 5: // Process
                        loadProcess();
                        break;
                }
            }

            // set all Dropdown options array
            var setDropdownOptionsArray = function () {
                self.gridOptions.columnDefs[2].editDropdownOptionsArray = self.isActiveOptions;
            };

            // load business segments
            var loadBusinessSegments = function () {
                self.dataLoaded = false;
                organizationalDataService.getBusinessSegments()
                .then(function (data) {
                    self.organizationData = data;
                    angular.forEach(self.organizationData, function (item) {
                        item.isActive = {
                            id: item.isActive === true ? 1 : 2,
                            name: item.isActive === true ? 'Active' : 'Inactive'
                        };
                    });
                    self.gridOptions.data = self.organizationData;
                    self.dataLoaded = true;
                }, function (err) {
                    notificationService.notify(err.errors.join("<br>"), {
                        autoclose: false, type: 'danger'
                    });
                    self.dataLoaded = true;
                });
            }

            // load all departments
            var loadDepartments = function () {
                self.dataLoaded = false;
                organizationalDataService.getDepartments()
                .then(function (data) {
                    self.organizationData = data;
                    angular.forEach(self.organizationData, function (item) {
                        item.isActive = {
                            id: item.isActive === true ? 1 : 2,
                            name: item.isActive === true ? 'Active' : 'Inactive'
                        };
                    });
                    self.gridOptions.data = self.organizationData;
                    self.dataLoaded = true;
                }, function (err) {
                    notificationService.notify(err.errors.join("<br>"), {
                        autoclose: false, type: 'danger'
                    });
                    self.dataLoaded = true;
                });
            }

            // load all divisions
            var loadDivisions = function () {
                self.dataLoaded = false;
                organizationalDataService.getDivisions()
                .then(function (data) {
                    self.organizationData = data;
                    angular.forEach(self.organizationData, function (item) {
                        item.isActive = {
                            id: item.isActive === true ? 1 : 2,
                            name: item.isActive === true ? 'Active' : 'Inactive'
                        };
                    });
                    self.gridOptions.data = self.organizationData;
                    self.dataLoaded = true;
                }, function (err) {
                    notificationService.notify(err.errors.join("<br>"), {
                        autoclose: false, type: 'danger'
                    });
                    self.dataLoaded = true;
                });
            }

            // load all product lines
            var loadProductLines = function () {
                self.dataLoaded = false;
                organizationalDataService.getProductLines()
                .then(function (data) {
                    self.organizationData = data;
                    angular.forEach(self.organizationData, function (item) {
                        item.isActive = {
                            id: item.isActive === true ? 1 : 2,
                            name: item.isActive === true ? 'Active' : 'Inactive'
                        };
                    });
                    self.gridOptions.data = self.organizationData;
                    self.dataLoaded = true;
                }, function (err) {
                    notificationService.notify(err.errors.join("<br>"), {
                        autoclose: false, type: 'danger'
                    });
                    self.dataLoaded = true;
                });
            }

            // load all facilities
            var loadFacilities = function () {
                self.dataLoaded = false;
                organizationalDataService.getFacilities()
                .then(function (data) {
                    self.organizationData = data;
                    angular.forEach(self.organizationData, function (item) {
                        item.isActive = {
                            id: item.isActive === true ? 1 : 2,
                            name: item.isActive === true ? 'Active' : 'Inactive'
                        };
                    });
                    self.gridOptions.data = self.organizationData;
                    self.dataLoaded = true;
                }, function (err) {
                    notificationService.notify(err.errors.join("<br>"), {
                        autoclose: false, type: 'danger'
                    });
                    self.dataLoaded = true;
                });
            }

            // load all processes
            var loadProcess = function () {
                self.dataLoaded = false;
                organizationalDataService.getProcesses()
                .then(function (data) {
                    self.organizationData = data;
                    angular.forEach(self.organizationData, function (item) {
                        item.isActive = {
                            id: item.isActive === true ? 1 : 2,
                            name: item.isActive === true ? 'Active' : 'Inactive'
                        };
                    });
                    self.gridOptions.data = self.organizationData;
                    self.dataLoaded = true;
                }, function (err) {
                    notificationService.notify(err.errors.join("<br>"), {
                        autoclose: false, type: 'danger'
                    });
                    self.dataLoaded = true;
                });
            }

            var fillOrganizationalData = function () {
                organizationalDataService.getOrganizationalData()
                .then(function (data) {
                    self.organizationDataList = data;
                }, function (err) {
                    notificationService.notify(err.errors.join("<br>"), {
                        autoclose: false, type: 'danger'
                    });
                });
            }
                       
            //refresh button click
            self.refreshData = function () {
                self.gridOptions.data.length = 0;
                self.dataLoaded = false;
                switch (self.selectedType) {
                    case 0: // Business Segments
                        loadBusinessSegments();
                        break;
                    case 1: // Divisions
                        loadDivisions();
                        break;
                    case 2: // Facilities
                        loadFacilities();
                        break;
                    case 3: // Product Lines
                        loadProductLines();
                        break;
                    case 4: // Departments
                        loadDepartments();
                        break;
                    case 5: // Process
                        loadProcess();
                        break;
                }
            };

            //ui-grid specifications
            self.gridOptions.onRegisterApi = function (gridApi) {
                self.gridApi = gridApi;
                gridApi.rowEdit.on.saveRow($scope, self.saveRow);
            };

            //setting the row object to organization data array before addOrUpdate service call
            self.setOrganizationData = function (rowEntity) {
                self.organization.push({
                    id: rowEntity.id,
                    name: rowEntity.name,
                    isActive: parseInt(rowEntity.isActive.id) === 2 ? false : true
                });
            };

            var checkAddorUpdate = function (id) {
                self.addOperation = id === "" ? true : false;
            };

            //ui-grid saveRow function
            self.saveRow = function (rowEntity) {
                debugger;
                self.dataLoaded = false;
                self.organizationName = getOrganizationName(self.selectedType);
                self.setOrganizationData(rowEntity);
                checkAddorUpdate(self.organization[0].id);
                var deffered = $q.defer();

                self.gridApi.rowEdit.setSavePromise(rowEntity, deffered.promise);
                //service call for add or update function
                switch (self.selectedType) {
                    case 0: // Business Segments                    

                        organizationalDataService.addOrUpdateBusinessSegments(self.organization).then(function (data) {
                            debugger;
                            deffered.resolve();
                            if (self.addOperation) {
                                notificationService.notify(self.organizationName +" "+ organizationDataResources.AddedSuccessfully, {
                                    type: 'success'
                                }).then(function () {
                                    self.gridOptions.data.length = 0;
                                    loadBusinessSegments();
                                }, function (err) {
                                });
                            }
                            else {
                                notificationService.notify(self.organizationName +" "+ organizationDataResources.UpdatedSuccessfully, {
                                    type: 'success'
                                }).then(function () {
                                    self.gridOptions.data.length = 0;
                                    loadBusinessSegments();
                                }, function (err) {
                                });
                            }
                        }, function (err) {
                            debugger;
                            deffered.resolve();
                            notificationService.notify(err.errors.join("<br/>"), {
                                autoClose: false, type: "danger"
                            });
                        });
                        self.organization = [];
                        break;
                    case 1: // Divisions
                        organizationalDataService.addOrUpdateDivisions(self.organization).then(function (data) {
                            debugger;
                            deffered.resolve();
                            if (self.addOperation) {
                                notificationService.notify(self.organizationName + " " + organizationDataResources.AddedSuccessfully, {
                                    type: 'success'
                                }).then(function () {
                                    self.gridOptions.data.length = 0;
                                    loadDivisions();
                                }, function (err) {
                                });
                            }
                            else {
                                notificationService.notify(self.organizationName + " " + organizationDataResources.UpdatedSuccessfully, {
                                    type: 'success'
                                }).then(function () {
                                    self.gridOptions.data.length = 0;
                                    loadDivisions();
                                }, function (err) {
                                });
                            }
                        }, function (err) {
                            debugger;
                            deffered.resolve();
                            notificationService.notify(err.errors.join("<br/>"), {
                                autoClose: false, type: "danger"
                            });
                        });
                        self.organization = [];
                        break;
                    case 2: // Facilities
                        organizationalDataService.addOrUpdateFacilities(self.organization).then(function (data) {
                            debugger;
                            deffered.resolve();
                            if (self.addOperation) {
                                notificationService.notify(self.organizationName + " " + organizationDataResources.AddedSuccessfully, {
                                    type: 'success'
                                }).then(function () {
                                    self.gridOptions.data.length = 0;
                                    loadFacilities();
                                }, function (err) {
                                });
                            }
                            else {
                                notificationService.notify(self.organizationName + " " + organizationDataResources.UpdatedSuccessfully, {
                                    type: 'success'
                                }).then(function () {
                                    self.gridOptions.data.length = 0;
                                    loadFacilities();
                                }, function (err) {
                                });
                            }
                        }, function (err) {
                            debugger;
                            deffered.resolve();
                            notificationService.notify(err.errors.join("<br/>"), {
                                autoClose: false, type: "danger"
                            });
                        });
                        self.organization = [];
                        break;
                    case 3: // Product Lines
                        organizationalDataService.addOrUpdateProductLines(self.organization).then(function (data) {
                            debugger;
                            deffered.resolve();
                            if (self.addOperation) {
                                notificationService.notify(self.organizationName + " " + organizationDataResources.AddedSuccessfully, {
                                    type: 'success'
                                }).then(function () {
                                    self.gridOptions.data.length = 0;
                                    loadProductLines();
                                }, function (err) {
                                });
                            }
                            else {
                                notificationService.notify(self.organizationName + " " + organizationDataResources.UpdatedSuccessfully, {
                                    type: 'success'
                                }).then(function () {
                                    self.gridOptions.data.length = 0;
                                    loadProductLines();
                                }, function (err) {
                                });
                            }
                        }, function (err) {
                            debugger;
                            deffered.resolve();
                            notificationService.notify(err.errors.join("<br/>"), {
                                autoClose: false, type: "danger"
                            });
                        });
                        self.organization = [];
                        break;
                    case 4: // Departments
                        organizationalDataService.addOrUpdateDepartments(self.organization).then(function (data) {
                            debugger;
                            deffered.resolve();
                            if (self.addOperation) {
                                notificationService.notify(self.organizationName + " " + organizationDataResources.AddedSuccessfully, {
                                    type: 'success'
                                }).then(function () {
                                    self.gridOptions.data.length = 0;
                                    loadDepartments();
                                }, function (err) {
                                });
                            }
                            else {
                                notificationService.notify(self.organizationName + " " + organizationDataResources.UpdatedSuccessfully, {
                                    type: 'success'
                                }).then(function () {
                                    self.gridOptions.data.length = 0;
                                    loadDepartments();
                                }, function (err) {
                                });
                            }
                        }, function (err) {
                            debugger;
                            deffered.resolve();
                            notificationService.notify(err.errors.join("<br/>"), {
                                autoClose: false, type: "danger"
                            });
                        });
                        self.organization = [];
                        break;
                    case 5: // Process
                        organizationalDataService.addOrUpdateProcess(self.organization).then(function (data) {
                            debugger;
                            deffered.resolve();
                            if (self.addOperation) {
                                notificationService.notify(self.organizationName + " " + organizationDataResources.AddedSuccessfully, {
                                    type: 'success'
                                }).then(function () {
                                    self.gridOptions.data.length = 0;
                                    loadProcess();
                                }, function (err) {
                                });
                            }
                            else {
                                notificationService.notify(self.organizationName + " " + organizationDataResources.UpdatedSuccessfully, {
                                    type: 'success'
                                }).then(function () {
                                    self.gridOptions.data.length = 0;
                                    loadProcess();
                                }, function (err) {
                                });
                            }
                        }, function (err) {
                            debugger;
                            deffered.resolve();
                            notificationService.notify(err.errors.join("<br/>"), {
                                autoClose: false, type: "danger"
                            });
                        });
                        self.organization = [];
                        break;
                }
            };
         
            var getOrganizationName = function (id) {
                                   var organizationPage = $.grep(self.organizationDataList, function (obj) {
                        if (obj.id === id) {
                            return obj;
                        }
                    });
                    if (organizationPage.length) {
                        return organizationPage[0].name;
                    }
                    else {
                        self.selectedType = '';
                        return '';
                    }                  
            };
                                 
            var init = function () {
                setDropdownOptionsArray();
                fillOrganizationalData();
                loadBusinessSegments();
            }();
                        
        }]);

})

