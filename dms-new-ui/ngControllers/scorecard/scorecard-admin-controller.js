'use strict';
define(['angularAMD', 'scorecardService', 'organizationalDataService', 'titleCase', 'validNumber', 'utilityService', 'workdayPattern'], function (angularAMD) {

    angularAMD.controller('ScorecardAdminController', ['scorecardService', 'organizationalDataService',
        'validationService', 'notificationService', 'configService', '$http', '$routeParams',
        '$location', '$scope', '$q', '$rootScope', 'sessionStorageService', 'utilityService', '$filter',
    function (scorecardService, organizationalDataService, validationService, notificationService,
        configService, $http, $routeParams, $location, $scope, $q, $rootScope, sessionStorageService, utilityService, $filter) {
        var self = this,
            scorecardResources = configService.getUIResources('Scorecard'),
            scorecardId = $routeParams.id,
            mode = $routeParams.mode,
            configInfo = JSON.parse(sessionStorageService.get('configData'));

        self.maxKPIOwnerCount = configInfo ? configInfo.maxKPIOwnerCount : 4;
        self.maxTeamCount = configInfo ? configInfo.maxTeamCount : 15;
        self.scorecardId = parseInt(scorecardId);
        self.mode = $routeParams.mode;
        $rootScope.scorecardId = "";

        //setting parent id based on mode - edit or add
        if (self.mode === "Edit") {
            self.parentId = $routeParams.parentId !== "false" ? $routeParams.parentId : '';
            self.hasParent = self.parentId === "" ? false : true;
            self.displayMode = false;
        }
        else if (self.mode === "Add") {
            self.parentId = self.scorecardId === 0 ? null : self.scorecardId;
            self.hasParent = self.scorecardId === 0 ? false : true;
            self.displayMode = true;
        }

        self.tags = [];
        self.kpiOwners = [];
        self.teams = [];
        self.selected_KPIs = [];
        self.dateOptions = {};
        self.recordable = {};
        self.isSaving = false;
        self.selectedIds = [];
        self.selectedBusinessSegmentIds = [];
        self.selectedBusinessSegments = [];
        self.selectedDivisionIds = [];
        self.selectedDivisions = [];
        self.selectedFacilityIds = [];
        self.selectedFacilities = [];
        self.selectedProductLineIds = [];
        self.selectedProductLines = [];
        self.selectedDepartmentIds = [];
        self.selectedDepartments = [];
        self.selectedProcessIds = [];
        self.selectedProcesses = [];

        ////ng-tags-input directive : service call to get autocomplete feature
        //self.loadTags = function (query) {
        //    return scorecardService.loadADUsers(query);
        //}
        self.seletedListCount = 1000;

        // utility service used to avoid date time zone miss match with UTC time
        var convertToDateWithoutTimezone = function (date) {
            return utilityService.convertToDateWithoutTimezone(date);
        };
        //ng-tags-input directive : service call to get autocomplete feature
        self.loadTags = function (query) {
            var deferred = $q.defer();
            scorecardService.loadADUsers(query).then(function (data) {
                if (data) {
                    self.seletedListCount = data ? data.length : 1000;
                    deferred.resolve(data);
                }
                else {
                    deferred.reject();
                }
            });
            return deferred.promise;
        };

        //scorecard model
        self.scorecard = {
            name: '',
            parentScorecardId: '',
            rootScorecardId: '',
            businessSegment: {},
            division: {},
            facility: {},
            productLine: {},
            department: {},
            process: {},
            kpiOwners: [],
            teams: [],
            isBowlingChartApplicable: false,
            kpIs: [],
            drilldownLevel: 2,
            isActive: true,
            scorecardWorkdayPattern: {},
            scorecardHolidayPattern: {},
            isActiveWorkdayPattern: false,
            isActiveHolidyPattern: false,
            isNextWorkdayPatternAvailable: false,
            isNextHolidayPattrenAvailable: false,
            isEmptyWorkdayPatternAvailable: false,
            nextWorkdayPatternName: '',
            nextHolidayPatternName: ''
        };

        //setting validation rules
        self.validationRules = {
            Name: [new ValidationRule(validationType.required, true, scorecardResources.RequiredScorecardName)],
            KPIOwner: [new ValidationRule(validationType.required, true, scorecardResources.RequiredKPIOwner)],
            Team: [new ValidationRule(validationType.required, true, scorecardResources.RequiredTeam)],
            IsBowlingChart: [new ValidationRule(validationType.required, true, scorecardResources.RequiredIsBowlingChart)],
            KPISelection: [new ValidationRule(validationType.required, true, scorecardResources.RequiredKPISelection)],
            DrilldownLevel: [
                new ValidationRule(validationType.required, true, scorecardResources.RequiredDrilldownLevel),
                new ValidationRule(validationType.max, 99, scorecardResources.InvalidDrilldownLevel),
                new ValidationRule(validationType.valid, true, scorecardResources.InvalidDrilldownLevel)]
        };

        //setting parent data in initial load
        self.setParentData = function (data) {
            if (data.parentScorecardItem) {
                self.scorecard.parentName = data.parentScorecardItem.name;
                self.parentName = self.scorecard.parentName;
                self.scorecard.parentScorecardId = data.parentScorecardItem.id;
            }
        };

        //set scorecard options
        self.setScorecardOptions = function (data) {
            self.businessSegmentOptions = data.businessSegments;
            self.divisionOptions = data.divisions;
            self.facilityOptions = data.facilities;
            self.productLineOptions = data.productLines;
            self.departmentOptions = data.departments;
            self.processOptions = data.processes;
            self.KPIs = data.kpIs;
            self.HolidayPatterns = data.scorecardHolidayPatterns;           
        };
        self.removeOrganizationalDropdowns = function () {
            var businessSegmentDrpdwn = $("#business-segments").data("kendoMultiSelect");
            if (businessSegmentDrpdwn != undefined){
                businessSegmentDrpdwn.destroy();
                $('#business-segments').unwrap('.k-multiselect').show().empty();
                $(".k-multiselect-wrap").remove(); 
            }
            var divisionDrpdwn = $("#divisions").data("kendoMultiSelect");
            if (divisionDrpdwn != undefined) {
                divisionDrpdwn.destroy();
                $('#divisions').unwrap('.k-multiselect').show().empty();
                $(".k-multiselect-wrap").remove();
            }
            var facilityDrpdwn = $("#facilities").data("kendoMultiSelect");
            if (facilityDrpdwn != undefined) {
                facilityDrpdwn.destroy();
                $('#facilities').unwrap('.k-multiselect').show().empty();
                $(".k-multiselect-wrap").remove();
            }
            var productLineDrpdwn = $("#productlines").data("kendoMultiSelect");
            if (productLineDrpdwn != undefined) {
                productLineDrpdwn.destroy();
                $('#productlines').unwrap('.k-multiselect').show().empty();
                $(".k-multiselect-wrap").remove();
            }
            var departmentDrpdwn = $("#departments").data("kendoMultiSelect");
            if (departmentDrpdwn != undefined) {
                departmentDrpdwn.destroy();
                $('#departments').unwrap('.k-multiselect').show().empty();
                $(".k-multiselect-wrap").remove();
            }
            var processDrpdwn = $("#processes").data("kendoMultiSelect");
            if (processDrpdwn != undefined) {
                processDrpdwn.destroy();
                $('#processes').unwrap('.k-multiselect').show().empty();
                $(".k-multiselect-wrap").remove();
            }

        };
        self.CreateOrganizationalDropDowns = function () {
            var businessSegment = $("#business-segments").kendoMultiSelect({
                autoClose: false,
                dataSource: self.businessSegmentOptions,
                dataTextField: "name",
                dataValueField: "id",
                filter: "contains",
                itemTemplate: "<input type='checkbox'/>&nbsp;#:data.name# ",
                dataBound: function () {
                    var items = this.ul.find("li");
                    setTimeout(function () {
                        checkInputs(items);
                    });
                },
                change: function () {
                    var items = this.ul.find("li");
                    checkInputs(items);
                }
            }).data("kendoMultiSelect");

            var division = $("#divisions").kendoMultiSelect({
                autoClose: false,
                dataSource: self.divisionOptions,
                dataTextField: "name",
                dataValueField: "id",
                filter: "contains",
                itemTemplate: "<input type='checkbox'/>&nbsp;#:data.name# ",
                dataBound: function () {
                    var items = this.ul.find("li");
                    setTimeout(function () {
                        checkInputs(items);
                    });
                },
                change: function () {
                    var items = this.ul.find("li");
                    checkInputs(items);
                }
            }).data("kendoMultiSelect");
            
            var facility = $("#facilities").kendoMultiSelect({
                autoClose: false,
                dataSource: self.facilityOptions,
                dataTextField: "name",
                dataValueField: "id",
                filter: "contains",
                itemTemplate: "<input type='checkbox'/>&nbsp;#:data.name# ",
                dataBound: function () {
                    var items = this.ul.find("li");
                    setTimeout(function () {
                        checkInputs(items);
                    });
                },
                change: function () {
                    var items = this.ul.find("li");
                    checkInputs(items);
                }
            }).data("kendoMultiSelect");

            var productLine = $("#productlines").kendoMultiSelect({
                autoClose: false,
                dataSource: self.productLineOptions,
                dataTextField: "name",
                dataValueField: "id",
                filter: "contains",
                itemTemplate: "<input type='checkbox'/>&nbsp;#:data.name# ",
                dataBound: function () {
                    var items = this.ul.find("li");
                    setTimeout(function () {
                        checkInputs(items);
                    });
                },
                change: function () {
                    var items = this.ul.find("li");
                    checkInputs(items);
                }
            }).data("kendoMultiSelect");

            var department = $("#departments").kendoMultiSelect({
                autoClose: false,
                dataSource: self.departmentOptions,
                dataTextField: "name",
                dataValueField: "id",
                filter: "contains",
                itemTemplate: "<input type='checkbox'/>&nbsp;#:data.name# ",
                dataBound: function () {
                    var items = this.ul.find("li");
                    setTimeout(function () {
                        checkInputs(items);
                    });
                },
                change: function () {
                    var items = this.ul.find("li");
                    checkInputs(items);
                }
            }).data("kendoMultiSelect");

            var process = $("#processes").kendoMultiSelect({
                autoClose: false,
                dataSource: self.processOptions,
                dataTextField: "name",
                dataValueField: "id",
                filter: "contains",
                itemTemplate: "<input type='checkbox'/>&nbsp;#:data.name# ",
                dataBound: function () {
                    var items = this.ul.find("li");
                    setTimeout(function () {
                        checkInputs(items);
                    });
                },
                change: function () {
                    var items = this.ul.find("li");
                    checkInputs(items);
                }
            }).data("kendoMultiSelect");
        };

        //set scorecard model
        self.setScorecardTemplateData = function (data) {
           
            if(mode=="Add"){
                self.removeOrganizationalDropdowns();
                self.CreateOrganizationalDropDowns();
            }
            var businessSegmentDrpdwn = $("#business-segments").data("kendoMultiSelect");
            var divisionDrpdwn = $("#divisions").data("kendoMultiSelect");
            var facilityDrpdwn = $("#facilities").data("kendoMultiSelect");
            var productLineDrpdwn = $("#productlines").data("kendoMultiSelect");
            var departmentDrpdwn = $("#departments").data("kendoMultiSelect");
            var processDrpdwn = $("#processes").data("kendoMultiSelect");
            if (data.parentScorecardItem) {
                if (mode == "Add") {
                    var scorecardBusinessSegments = data.parentScorecardItem.businessSegments;
                    if (scorecardBusinessSegments != null && !angular.isUndefined(scorecardBusinessSegments)) {
                        angular.forEach(scorecardBusinessSegments, function (value, key) {
                            self.selectedBusinessSegmentIds.push(value.id);                            
                        });
                        businessSegmentDrpdwn.value(self.selectedBusinessSegmentIds);
                    }
                    var scorecardDivisions = data.parentScorecardItem.divisions;
                    if (scorecardDivisions != null && !angular.isUndefined(scorecardDivisions)) {
                        angular.forEach(scorecardDivisions, function (value, key) {
                            self.selectedDivisionIds.push(value.id);
                        });
                        divisionDrpdwn.value(self.selectedDivisionIds);
                    }
                    var scorecardFacilities = data.parentScorecardItem.facilities;
                    if (scorecardFacilities != null && !angular.isUndefined(scorecardFacilities)) {
                        angular.forEach(scorecardFacilities, function (value, key) {
                            self.selectedFacilityIds.push(value.id);
                        });
                        facilityDrpdwn.value(self.selectedFacilityIds);
                    }
                    var scorecardProductLines = data.parentScorecardItem.productLines;
                    if (scorecardProductLines != null && !angular.isUndefined(scorecardProductLines)) {
                        angular.forEach(scorecardProductLines, function (value, key) {
                            self.selectedProductLineIds.push(value.id);
                        });
                        productLineDrpdwn.value(self.selectedProductLineIds);
                    }
                    var scorecardDepartments = data.parentScorecardItem.departments;
                    if (scorecardDepartments != null && !angular.isUndefined(scorecardDepartments)) {
                        angular.forEach(scorecardDepartments, function (value, key) {
                            self.selectedDepartmentIds.push(value.id);
                        });
                        departmentDrpdwn.value(self.selectedDepartmentIds);
                    }
                    var scorecardProcesses = data.parentScorecardItem.processes;
                    if (scorecardProcesses != null && !angular.isUndefined(scorecardProcesses)) {
                        angular.forEach(scorecardProcesses, function (value, key) {
                            self.selectedProcessIds.push(value.id);
                        });
                        processDrpdwn.value(self.selectedProcessIds);
                    }
                    
                }
                self.scorecard.drilldownLevel = data.parentScorecardItem.drilldownLevel;
                // self.scorecard.scorecardHolidaydayPattern.id = data.parentScorecardItem.scorecardHolidaydayPattern.id;
            }
            else {
                if (mode == "Add") {
                    businessSegmentDrpdwn.value(["0"]);
                    divisionDrpdwn.value(["0"]);
                    facilityDrpdwn.value(["0"]);
                    productLineDrpdwn.value(["0"]);
                    departmentDrpdwn.value(["0"]);
                    processDrpdwn.value(["0"]);
                }
            }

            // keep the root scorecardId
            self.scorecard.rootScorecardId = data.rootScorecardId;
        };
        self.setOrganizationDropdowns = function (data) {
            var businessSegmentDrpdwn = $("#business-segments").data("kendoMultiSelect");
            var businessItems = businessSegmentDrpdwn.ul.find("li");
            setTimeout(function () {
                checkInputs(businessItems);
            });
            var divisionDrpdwn = $("#divisions").data("kendoMultiSelect");
            var divisiondtems = divisionDrpdwn.ul.find("li");
            setTimeout(function () {
                checkInputs(divisiondtems);
            });
            var facilityDrpdwn = $("#facilities").data("kendoMultiSelect");
            var facilityItems = facilityDrpdwn.ul.find("li");
            setTimeout(function () {
                checkInputs(facilityItems);
            });
            var productLineDrpdwn = $("#productlines").data("kendoMultiSelect");
            var productLineItems = productLineDrpdwn.ul.find("li");
            setTimeout(function () {
                checkInputs(productLineItems);
            });
            var departmentDrpdwn = $("#departments").data("kendoMultiSelect");
            var departmentItems = departmentDrpdwn.ul.find("li");
            setTimeout(function () {
                checkInputs(departmentItems);
            });
            var processDrpdwn = $("#processes").data("kendoMultiSelect");
            var processItems = processDrpdwn.ul.find("li");
            setTimeout(function () {
                checkInputs(processItems);
            });
           
            //set Business Segment drop down         
            var scorecardBusinessSegments = data.businessSegments;
            if (scorecardBusinessSegments != null && !angular.isUndefined(scorecardBusinessSegments)) {
                angular.forEach(scorecardBusinessSegments, function (value, key) {
                    self.selectedBusinessSegmentIds.push(value.id);
                });
                businessSegmentDrpdwn.value(self.selectedBusinessSegmentIds);
            }
            //set Division drop down
            var scorecardDivisions = data.divisions;
            if (scorecardDivisions != null && !angular.isUndefined(scorecardDivisions)) {
                angular.forEach(scorecardDivisions, function (value, key) {
                    self.selectedDivisionIds.push(value.id);
                });
                divisionDrpdwn.value(self.selectedDivisionIds);
            }
            //set Facility drop down
            var scorecardFacilities = data.facilities;
            if (scorecardFacilities != null && !angular.isUndefined(scorecardFacilities)) {
                angular.forEach(scorecardFacilities, function (value, key) {
                    self.selectedFacilityIds.push(value.id);
                });
                facilityDrpdwn.value(self.selectedFacilityIds);
            }
            //set Product Line drop down
            var scorecardProductLines = data.productLines;
            if (scorecardProductLines != null && !angular.isUndefined(scorecardProductLines)) {
                angular.forEach(scorecardProductLines, function (value, key) {
                    self.selectedProductLineIds.push(value.id);
                });
                productLineDrpdwn.value(self.selectedProductLineIds);
            }
            //set Department drop down
            var scorecardDepartments = data.departments;
            if (scorecardDepartments != null && !angular.isUndefined(scorecardDepartments)) {
                angular.forEach(scorecardDepartments, function (value, key) {
                    self.selectedDepartmentIds.push(value.id);
                });
                departmentDrpdwn.value(self.selectedDepartmentIds);
            }
            //set Process drop down
            var scorecardProcesses = data.processes;
            if (scorecardProcesses != null && !angular.isUndefined(scorecardProcesses)) {
                angular.forEach(scorecardProcesses, function (value, key) {
                    self.selectedProcessIds.push(value.id);
                });
                processDrpdwn.value(self.selectedProcessIds);
            }
        };

        //setting scorecard model before save/updatekendoMultiSelect
        self.setScorecard = function () {

            self.scorecard.parentScorecardId = self.scorecardId === 0 ? null : scorecardId;

            self.scorecard.kpiOwners = self.kpiOwners;

            self.scorecard.teams = self.teams;

            self.scorecard.recordable = {
                recordableDate: $filter('date')(self.recordable.recordableDate, "dd MMM yyyy")
            };
            self.scorecard.kpIs = $.grep(self.KPIs, function (kpi) { return kpi.isSelected === true; });
            var selectedBusinessSegmentItems = $("#business-segments").data("kendoMultiSelect").dataItems();
            self.scorecard.businessSegments = selectedBusinessSegmentItems;
            var selectedDivisonItems = $("#divisions").data("kendoMultiSelect").dataItems();
            self.scorecard.divisions = selectedDivisonItems;
            var selectedFacilityItems = $("#facilities").data("kendoMultiSelect").dataItems();
            self.scorecard.facilities = selectedFacilityItems;
            var selectedProductLineItems = $("#productlines").data("kendoMultiSelect").dataItems();
            self.scorecard.productLines = selectedProductLineItems;
            var selectedDepartmentItems = $("#departments").data("kendoMultiSelect").dataItems();
            self.scorecard.departments = selectedDepartmentItems;
            var selectedProcessItems = $("#processes").data("kendoMultiSelect").dataItems();
            self.scorecard.processes = selectedProcessItems;           
        };

        //add scorecard : save button click
        self.save = function () {
            self.isSaving = true;
            self.setScorecard();
            notificationService.close();
            validationService.validate('add-scorecard').then(function () {
                scorecardService.addScorecard(self.scorecard)
                    .then(function (data) {
                        if (!data.hasError) {
                            if (data.data) {
                                notificationService.notify(scorecardResources.AddedSuccessfully, { type: 'success' }).then(function () {
                                    self.isSaving = false;
                                    updateDropdownLists();
                                    goToHomePage(self.scorecard.rootScorecardId || data.data);
                                });
                            }
                        }
                    }, function (err) {
                        self.isSaving = false;
                        notificationService.notify(err.errors.join("<br/>"), { autoClose: false, type: "danger" });
                    });

            }, function (err) {
                self.isSaving = false;
                notificationService.notify(err.join("<br/>"), { autoClose: false, type: "danger" });
            });
        };

        //Update Scorecard function : update button click
        self.Update = function () {
            self.isSaving = true;
            self.setScorecard();
            if (self.scorecard.isNextWorkdayPatternAvailable == false) {
                self.scorecard.scorecardWorkdayPattern = null;
            }
            if (self.scorecard.isNextHolidayPattrenAvailable == false) {
                self.scorecard.scorecardHolidayPattern = null;
            }
            notificationService.close();
            validationService.validate('add-scorecard')
                .then(
                    function () {
                        scorecardService.updateScorecard(self.scorecard)
                            .then(function () {
                                notificationService.notify(scorecardResources.UpdatedSuccessfully, { type: 'success' })
                                    .then(function () {
                                        self.isSaving = false;
                                        updateDropdownLists();
                                        goToHomePage(self.scorecard.rootScorecardId || self.scorecard.id);
                                    });
                            },
                                function (err) {
                                    self.isSaving = false;
                                    notificationService.notify(err.errors.join("<br/>"), { autoClose: false, type: "danger" });
                                });
                    },
                    function (err) {
                        self.isSaving = false;
                        notificationService.notify(err.join("<br/>"), { autoClose: false, type: "danger" });
                    });
        };

        //cancel button click : back to home page
        self.cancel = function () {
            goToHomePage(self.scorecard.rootScorecardId || self.scorecard.id);
        };

        // get look up data - load all dropdowns options and parent data
        self.getLookupData = function () {

            var deferred = $q.defer();
            organizationalDataService.getScorecardTemplateData(self.parentId)
                         .then(
                             function (data) {
                                 self.setScorecardOptions(data);
                                 self.setScorecardTemplateData(data);
                                 self.setParentData(data);
                                 deferred.resolve();
                             },
                             function (err) {
                                 notificationService.notify(err.errors.join("<br/>"), { autoClose: false, type: "danger" });
                                 deferred.reject(err.errors);
                             });

            return deferred.promise;
        };
        self.ChangeWorkdayPattern = function () {
            self.scorecard.isNextWorkdayPatternAvailable = true;
        };
        self.ChangeHolidayPattern = function () {
            self.scorecard.isNextHolidayPattrenAvailable = true;
        };

        //load scorecard in edit mode
        var loadScoreCardData = function () {
            //get scorecard by id
            scorecardService.getScorecard(scorecardId)
                                .then(function (data) {
                                    formScoreCardData(data);
                                }, function (err) {
                                    notificationService.notify(err.errors.join("<br/>"), { autoClose: false, type: "danger" });
                                });
        },

        //back to hierarchy page
        goToHomePage = function (rootScorecardId) {
            $rootScope.lastActiveTopLevelScorecardId = rootScorecardId? rootScorecardId : null;
            $location.path("Hierarchy");
        },

        //setting the scorecard model after getScorecard service
        formScoreCardData = function (data) {            
            self.scorecard = data;
            self.kpiOwners = data.kpiOwners;
            self.teams = data.teams;
            self.businessSegments = data.businessSegments;
            self.divisions = data.divisions;
            self.facilities = data.facilities;
            self.productLines = data.productLines;
            self.departments = data.departments;
            self.processes = data.processes;
            self.recordable.recordableDate = data.recordable != null && data.recordable.recordableDate != null ? convertToDateWithoutTimezone(data.recordable.recordableDate) : '';
            self.disableLastRecordableDate = data.recordable != null && data.recordable.recordableDate != null && !data.recordable.isManual;
            setSelectedKpis(data.kpIs);
            self.scorecard.isActiveWorkdayPattern = true;
            self.scorecard.isActiveHolidyPattern = true;
            self.scorecard.scorecardWorkdayPattern = data.scorecardWorkdayPattern;
            self.scorecard.activeScorecardWorkdayPattern = data.activeScorecardWorkdayPattern;
            self.scorecard.scorecardHolidayPattern = data.scorecardHolidayPattern;
            self.scorecard.activeScorecardHolidayPattern = data.activeScorecardHolidayPattern;
            if (self.scorecard.scorecardWorkdayPattern != null) {
                if (self.scorecard.activeScorecardWorkdayPattern != null &&
                    self.scorecard.scorecardWorkdayPattern.effectiveStartDate != self.scorecard.activeScorecardWorkdayPattern.effectiveStartDate) { //Both Active and Next pattern exist
                    self.scorecard.isNextWorkdayPatternAvailable = true;
                    self.scorecard.isEmptyWorkdayPatternAvailable = false;
                }
                else {
                    self.scorecard.isNextWorkdayPatternAvailable = false;
                    self.scorecard.isEmptyWorkdayPatternAvailable = false;
                }
                var effectiveStartDate = convertToDateWithoutTimezone(data.nextWorkdayPatternStartDate);
                var startMonth = $filter('date')(effectiveStartDate, 'MMM');
                var startDay = $filter('date')(effectiveStartDate, 'dd');
                var startYear = $filter('date')(effectiveStartDate, 'yyyy');
                self.scorecard.nextWorkdayPatternName = "Starting " + startDay + "-" + startMonth + "-" + startYear;

            }
            else {
                self.scorecard.scorecardWorkdayPattern = {};
                self.scorecard.scorecardWorkdayPattern.isSunday = false;
                self.scorecard.scorecardWorkdayPattern.isMonday = false;
                self.scorecard.scorecardWorkdayPattern.isTuesday = false;
                self.scorecard.scorecardWorkdayPattern.isWednesday = false;
                self.scorecard.scorecardWorkdayPattern.isThursday = false;
                self.scorecard.scorecardWorkdayPattern.isFriday = false;
                self.scorecard.scorecardWorkdayPattern.isSaturday = false;
                self.scorecard.isActiveWorkdayPattern = false;
                var effectiveStartDate = convertToDateWithoutTimezone(data.currentWorkdayPatternStartDate);//If empty pattern is added initially,the start date of next pattern will be the Current month start date
                var startMonth = $filter('date')(effectiveStartDate, 'MMM');
                var startDay = $filter('date')(effectiveStartDate, 'dd');
                var startYear = $filter('date')(effectiveStartDate, 'yyyy');
                self.scorecard.nextWorkdayPatternName = "Starting " + startDay + "-" + startMonth + "-" + startYear;
                if (self.scorecard.activeScorecardWorkdayPattern == null) { //If  empty pattern is added initially
                    self.scorecard.isEmptyWorkdayPatternAvailable = true;
                    self.scorecard.isNextWorkdayPatternAvailable = false;
                }
                else {
                    self.scorecard.isNextWorkdayPatternAvailable = false;
                }

            }
            if (self.scorecard.scorecardHolidayPattern != null) {
                if (self.scorecard.activeScorecardHolidayPattern != null && self.scorecard.scorecardHolidayPattern.effectiveStartDate != self.scorecard.activeScorecardHolidayPattern.effectiveStartDate) { //Both Active and Next pttern exist 
                    self.scorecard.isNextHolidayPattrenAvailable = true;
                }
                else {
                    self.scorecard.isNextHolidayPattrenAvailable = false;
                }
                var effectiveStartDate = convertToDateWithoutTimezone(data.nextHolidayPatternStartDate);
                var startMonth = $filter('date')(effectiveStartDate, 'MMM');
                var startDay = $filter('date')(effectiveStartDate, 'dd');
                var startYear = $filter('date')(effectiveStartDate, 'yyyy');
                self.scorecard.nextHolidayPatternName = "Starting " + startDay + "-" + startMonth + "-" + startYear;
            }
            else {
                if (self.scorecard.activeScorecardHolidayPattern == null) { //If  empty pattern is added initially
                    self.scorecard.isNextHolidayPattrenAvailable = true;
                }
                else {
                    self.scorecard.isNextHolidayPattrenAvailable = false;
                }
            }
            //Business Segment Options
            self.selectedBusinessSegments = $filter('filter')(data.businessSegments, { isActive: 'false' })
            angular.forEach(self.selectedBusinessSegments, function (item, key) {
                    self.businessSegmentOptions.push(item);
            });
            self.businessSegmentOptions = $filter('orderBy')(self.businessSegmentOptions, "name");
            var firstBusinessSegmentItem = $filter('filter')(self.businessSegmentOptions, { id: 0 })[0];
            self.businessSegmentOptions.splice(self.businessSegmentOptions.indexOf(firstBusinessSegmentItem), 1);
            self.businessSegmentOptions.splice(0, 0, firstBusinessSegmentItem);

            //Division Options
            self.selectedDivisions = $filter('filter')(data.divisions, { isActive: 'false' })
            angular.forEach(self.selectedDivisions, function (item, key) {
                self.divisionOptions.push(item);
            });
            self.divisionOptions = $filter('orderBy')(self.divisionOptions, "name");
            var firstDivisionItem = $filter('filter')(self.divisionOptions, { id: 0 })[0];
            self.divisionOptions.splice(self.divisionOptions.indexOf(firstDivisionItem), 1);
            self.divisionOptions.splice(0, 0, firstDivisionItem);

            //Facility Options
            self.selectedFacilities = $filter('filter')(data.facilities, { isActive: 'false' })
            angular.forEach(self.selectedFacilities, function (item, key) {
                self.facilityOptions.push(item);
            });
            self.facilityOptions = $filter('orderBy')(self.facilityOptions, "name");
            var firstFacilityItem = $filter('filter')(self.facilityOptions, { id: 0 })[0];
            self.facilityOptions.splice(self.facilityOptions.indexOf(firstFacilityItem), 1);
            self.facilityOptions.splice(0, 0, firstFacilityItem);

            //Product Line Options
            self.selectedProductLines= $filter('filter')(data.productLines, { isActive: 'false' })
            angular.forEach(self.selectedProductLines, function (item, key) {
                self.productLineOptions.push(item);
            });
            self.productLineOptions = $filter('orderBy')(self.productLineOptions, "name");
            var firstProductLineItem = $filter('filter')(self.productLineOptions, { id: 0 })[0];
            self.productLineOptions.splice(self.productLineOptions.indexOf(firstProductLineItem), 1);
            self.productLineOptions.splice(0, 0, firstProductLineItem);

            //Department Options
            self.selectedDepartments = $filter('filter')(data.departments, { isActive: 'false' })
            angular.forEach(self.selectedDepartments, function (item, key) {
                self.departmentOptions.push(item);
            });
            self.departmentOptions = $filter('orderBy')(self.departmentOptions, "name");
            var firstDepartmentItem = $filter('filter')(self.departmentOptions, { id: 0 })[0];
            self.departmentOptions.splice(self.departmentOptions.indexOf(firstDepartmentItem), 1);
            self.departmentOptions.splice(0, 0, firstDepartmentItem);

            //Facility Options
            self.selectedProcesses = $filter('filter')(data.processes, { isActive: 'false' })
            angular.forEach(self.selectedProcesses, function (item, key) {
                self.processOptions.push(item);
            });
            self.processOptions = $filter('orderBy')(self.processOptions, "name");
            var firstProcessItem = $filter('filter')(self.processOptions, { id: 0 })[0];
            self.processOptions.splice(self.processOptions.indexOf(firstProcessItem), 1);
            self.processOptions.splice(0, 0, firstProcessItem);
            self.removeOrganizationalDropdowns();
            self.CreateOrganizationalDropDowns();
            self.setOrganizationDropdowns(data);
        },

        //update dropdown list when a scorecard is added or updated
        updateDropdownLists = function () {

            scorecardService.getHierarchyDropdownList().then(function () {
            }, function (err) {
                notificationService.notify(err.errors.join("<br/>"),
               { autoClose: false, type: "danger" });
            });

        },
        //setting seleted kpi's isselected property to true
        setSelectedKpis = function (selectedKpis) {

            var selectedKpiIds = selectedKpis.map(function (k) { return k.id; });
            for (var index = 0; index < self.KPIs.length; index++) {

                if ($.inArray(self.KPIs[index].id, selectedKpiIds) !== -1) {
                    self.KPIs[index].isSelected = true;
                }
                else {
                    self.KPIs[index].isSelected = false;
                }
            }
        },

        //intial load function
        init = function () {
            self.CreateOrganizationalDropDowns();
            self.getLookupData().then(function () {    
                if (mode === "Edit") {
                   loadScoreCardData();                 
                    self.displayMode = false;
                    $rootScope.title = "Edit Scorecard";
                }
                else if (mode === "Add") {                    
                    self.displayMode = true;
                    self.disableLastRecordableDate = false;
                    self.scorecard.scorecardWorkdayPattern.isSunday = false;
                    self.scorecard.scorecardWorkdayPattern.isMonday = true;
                    self.scorecard.scorecardWorkdayPattern.isTuesday = true;
                    self.scorecard.scorecardWorkdayPattern.isWednesday = true;
                    self.scorecard.scorecardWorkdayPattern.isThursday = true;
                    self.scorecard.scorecardWorkdayPattern.isFriday = true;
                    self.scorecard.scorecardWorkdayPattern.isSaturday = false;
                    self.scorecard.isActiveWorkdayPattern = false;
                    self.scorecard.isActiveHolidyPattern = false;
                    self.scorecard.isNextWorkdayPatternAvailable = true;
                    self.scorecard.isNextHolidayPattrenAvailable = true;
                    self.scorecard.isEmptyWorkdayPatternAvailable = false;
                    self.scorecard.scorecardHolidayPattern.holidayPatternId = 1;
                    self.scorecard.nextWorkdayPatternName = "Workday Pattern";
                    self.scorecard.nextHolidayPatternName = "Holiday Schedule";
                    $rootScope.title = "Add Scorecard";
                }
            }, function (err) {
                notificationService.notify(err.join("<br/>"), { autoClose: 'false', type: "danger" });
            });
        }();
        var checkInputs = function (elements) {
            elements.each(function () {
                var element = $(this);
                var input = element.children("input");
                input.prop("checked", element.hasClass("k-state-selected"));
            });
        }
       
    }]);
});
