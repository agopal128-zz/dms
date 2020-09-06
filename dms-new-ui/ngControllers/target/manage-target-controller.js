'use strict';
define(['angularAMD', 'targetService', 'monthlyPlan', 'calendar', 'viewTarget', 'toggleSwitch',
    'confirmationModalService', 'dailyTargetController', 'copyTargetController', 'validNumber', 'utilityService'], function (angularAMD) {
        angularAMD.controller('ManageTargetController', ['targetService', 'confirmationModalService',
            'validationService', 'notificationService', 'configService', '$routeParams', '$filter', '$rootScope',
            '$uibModal', '$scope', 'utilityService', function (targetService, confirmationModalService,
                validationService, notificationService, configService, $routeParams, $filter, $rootScope,
                $uibModal, $scope, utilityService) {

                var self = this,
                scorecardId = parseInt($routeParams.scorecardId);
                self.isPrimary = true;
                self.isSaving = false;
                self.goalEntered = false;
                self.canYearChange = true;
                self.isMetricExists = false;
                self.isMetricEditMode = false;
                self.isAddingNewTarget = false;
                self.canAddMetricTarget = true;
                self.isCascadeFromParent = false;
                self.isCascaded = false;
                self.isStretchGoalEnabled = false;
                self.isPrevCalenderYearSelected = false;
                self.isNextCalenderYearSelected = false;
                self.isViewTarget = $routeParams.mode == 'View' ? true : false;

                self.action = '';
                self.yearId = '';
                self.metricId = '';
                self.metricType = '';
                self.annualTarget = '';
                self.selectedYear = '';
                self.scorecardName = '';
                self.rollupMethodId = '';
                self.effectiveEndDate = '';
                self.effectiveStartDate = '';
                self.isBowlingChartApplicable = '';
                self.cascadedMetricsTrackingMethodId = '';

                self.selectedMetricDataType = {};

                self.years = [];
                self.targets = [];
                self.monthlyData = [];
                self.setMatrics = [];
                self.setMetricsInDateRange = [];
                self.kpiTargetData = [];
                self.monthlyTargets = [];
                self.metricTypeOptions = [];
                self.rollupMethodOptions = [];
                self.trackingMethodOptions = [];
                self.targetEntryMethodOptions = [];
                self.trackingMethodOptionsFiltered = [];
                self.mtdTrackingMethodOptions = [];
                self.mtdTrackingMethodOptionsFiltered = []

                self.trackingMethodId = 0;
                self.graphPlottingMethodId = 0;
                self.targetEntryMethodId = 1;
                self.canCopyMetrics = true;
                self.hasMonthlyAndDailyTargets = false;
                self.isDataLoaded = false;


                var targetResources = configService.getUIResources('Target');

                // utility service used to avoid date time zone miss match with UTC time
                var convertToDateWithoutTimezone = function (date) {
                    return utilityService.convertToDateWithoutTimezone(date);
                };

                var setTargetData = function () {
                    self.targetData = {
                        "scorecardId": scorecardId,
                        "kpiId": self.tab,
                        "metricId": self.metricId,
                        "metricType": self.metricType,
                        "cascadeFromParent": self.isCascadeFromParent,
                        "isStretchGoalEnabled": self.isStretchGoalEnabled,
                        "calendarYearId": self.yearId,
                        "effectiveStartDate": $filter('date')(self.effectiveStartDate, "dd MMM yyyy"),
                        "effectiveEndDate": $filter('date')(self.effectiveEndDate, "dd MMM yyyy"),
                        "trackingMethodId": self.trackingMethodId,
                        "targetEntryMethodId": self.targetEntryMethodId,
                        "graphPlottingMethodId": self.graphPlottingMethodId,
                        "mTDPerformanceTrackingMethodId": self.mtdTrackingMethodId,
                        "rollupMethodId": self.rollupMethodId,
                        "annualTarget": self.annualTarget,
                        "monthlyTargets": self.monthlyTargets,
                        "cascadedMetricsTrackingMethodId": self.cascadedMetricsTrackingMethodId
                    };
                    if (self.metricType === 1) {
                        if (self.ParentMetricData) {
                            self.targetData.maximumAllowedMonthlyGoals =
                                self.ParentMetricData.maximumAllowedMonthlyGoals;
                        }
                        else {
                            self.targetData.maximumAllowedMonthlyGoals = '';
                        }
                    }
                };

                //Binding year id on change event of year switch
                self.bindYearId = function (yearId) {
                    switch (yearId) {
                        case self.years[0].id:
                            self.isPrevCalenderYearSelected = true;
                            self.isNextCalenderYearSelected = false;
                            self.yearId = yearId;
                            break;
                        case self.years[1].id: self.yearId = yearId;
                            self.isPrevCalenderYearSelected = false;
                            self.isNextCalenderYearSelected = false;
                            self.dateOptions = {
                                minDate: self.currentYearMinDate,
                                maxDate: self.currentMaxDate
                            };
                            setDefaultEffectiveStartAndEndDate();
                            break;
                        case self.years[2].id: self.yearId = yearId;
                            self.isPrevCalenderYearSelected = false;
                            self.isNextCalenderYearSelected = true;
                            self.dateOptions = {
                                minDate: self.nextYearMinDate,
                                maxDate: self.nextMaxDate
                            };
                            self.effectiveStartDate = self.nextYearMinDate;
                            self.effectiveEndDate = self.nextMaxDate;
                            break;
                    }
                    loadKpi(scorecardId, self.yearId);
                };

                //Displaying primary first then secondary on view summery page.
                var rearrangingPrimaryAndSecondaryTargetsForViewSummery = function (data) {
                    self.targets = [];
                    self.targetsSecondary = [];

                    angular.forEach(data, function (obj) {
                        if (obj.metricType === 0) {
                            self.targets.push(obj);
                        }
                        else {
                            self.targetsSecondary.push(obj);
                        }
                    });

                    self.targets = self.targets.concat(self.targetsSecondary);
                    self.kpiTargetData.push({
                        targets: self.targets
                    });
                };

                var selectYearTargetAmountLabel = function (metricDataTypeId, isChangingMetricName) {

                    //For all metrics except percentage label is 'Full Year Target'.
                    if (!metricDataTypeId || ((metricDataTypeId || metricDataTypeId === 0) && metricDataTypeId !== 2)) {
                        //If the metric data type is changed while editing the metric name, clear the annual target and change label.
                        self.annualTarget = self.targetLabelName == 'Year End Target' ? '' : self.annualTarget;
                        self.targetLabelName = 'Full Year Target';
                    }

                    //If metric type is percentage (data type id 2 )
                    if ((metricDataTypeId && metricDataTypeId === 2)) {
                        //If the metric data type is changed while editing the metric name from UI, clear the annual target and change label.
                        if (isChangingMetricName && self.targetLabelName == 'Full Year Target')
                            self.annualTarget = '';
                        self.targetLabelName = 'Year End Target';
                    }
                };

                // To extract the unallocated values, rollup method name(from id),
                // year target amount label from metric data to display on view summery page.
                var extractDataForSummaryView = function () {

                    if (self.kpiTargetData) {
                        angular.forEach(self.kpiTargetData[0].targets, function (target, index) {

                            //extracting rollup method name using id.
                            if (target.rollupMethodId !== null) {
                                var rollupMethodItem = $.grep(self.rollupMethodOptions,
                                    function (item) {
                                        return item.id === target.rollupMethodId;
                                    });
                                if (rollupMethodItem) {
                                    target.rollupMethodName = rollupMethodItem[0].name;
                                }
                            }
                            target.effectiveStartDate = convertToDateWithoutTimezone(target.effectiveStartDate);
                            target.effectiveEndDate = convertToDateWithoutTimezone(target.effectiveEndDate);

                            //extracting maximum allowed value from monthly targets.
                            angular.forEach(target.monthlyTargets, function (item, index) {
                                if (target.maximumAllowedMonthlyGoals) {
                                    target.monthlyTargets[index].maximumAllowedGoal =
                                    target.maximumAllowedMonthlyGoals[index].value;
                                }
                            });

                            //setting label as 'Year End Target' for decimal and  
                            //percentage(metric data type id 1,2 respectively) otherwise 'Full Year Target'.
                            if (((target.metricDataTypeId || target.metricDataTypeId === 0) &&
                                target.metricDataTypeId !== 2)) {
                                target.targetLabelName = 'Full Year Target';
                            }

                            if (((target.metricDataTypeId && target.metricDataTypeId === 2))) {
                                target.targetLabelName = 'Year End Target';
                            }
                        });
                    }
                };

                //Find the index of last updated or added target to display in expanded mode in view target summery page.
                var getLastAddedorUpdatedTargetIndex = function (currentlyUpdatedTargetId, currentlyAddedTargetMetricType) {

                    if (currentlyUpdatedTargetId || currentlyUpdatedTargetId === 0) {
                        angular.forEach(self.kpiTargetData[0].targets, function (value, index) {
                            if (value.id == currentlyUpdatedTargetId)
                                self.currentExpandedTargetIndex = index;
                        });
                    }
                    else if (currentlyAddedTargetMetricType || currentlyAddedTargetMetricType === 0) {
                        angular.forEach(self.kpiTargetData[0].targets, function (value, index) {
                            if (value.metricType == currentlyAddedTargetMetricType)
                                self.currentExpandedTargetIndex = index;
                        });
                    }
                };

                //load all metric data for accordion view - service call
                var loadMetricDetails = function (kpiId, currentlyUpdatedTargetId, currentlyAddedTargetMetricType) {
                    self.kpiTargetData = [];
                    self.setMatrics = [];
                    self.setMetricsInDateRange = [];
                    self.isDataLoaded = false;
                    self.canYearChange = false;
                    targetService.getTargetsForScorecardAndKPI(scorecardId, kpiId, self.yearId)
                                 .then(function (data) {
                                     self.isDataLoaded = true;
                                     self.canYearChange = true;
                                     //setting accordion view if targets exists.
                                     if (data && data.length) {
                                         self.metricData = data;

                                         rearrangingPrimaryAndSecondaryTargetsForViewSummery(data);

                                         getLastAddedorUpdatedTargetIndex(currentlyUpdatedTargetId,
                                             currentlyAddedTargetMetricType);

                                         extractDataForSummaryView();

                                         self.renderingPageUrl = 'ngViews/targets/partials/view-target.min.html';
                                         self.isMetricExists = true;
                                         //getting already set metrics for the kpi.
                                         angular.forEach(self.metricData, function (value, index) {
                                             self.setMatrics.push({
                                                 targetId: value.id,
                                                 metricName: value.metricName,
                                                 effectiveStartDate: convertToDateWithoutTimezone(value.effectiveStartDate),
                                                 effectiveEndDate: convertToDateWithoutTimezone(value.effectiveEndDate)
                                             });
                                             //self.setMatrics.push(value.metricName);
                                         });
                                     }
                                     else {
                                         self.isMetricExists = false;
                                         self.metricData = null;
                                     }

                                 }, function (err) {
                                     self.isDataLoaded = true;
                                     self.canYearChange = true;
                                     notificationService.notify(err.errors.join("<br />"),
                                     {
                                         autoClose: false, type: "danger"
                                     });
                                 });
                };

                //Display only existing KPI Tabs for the scorecard.
                self.isKPIExists = function (kpiId) {
                    var kpi;
                    if (self.existingKPIs) {
                        kpi = $filter('filter')(self.existingKPIs, { id: kpiId })[0];
                    }
                    if (kpi && kpi.id !== undefined) {
                        return true;
                    }
                    else {
                        return false;
                    }
                };

                // Confirmation for changing tab while editing or adding a target.
                var confirmDisacardUnsavedChanges = function (tabIndex, currentlyUpdatedTargetId,
                    currentlyAddedTargetMetricType) {
                    var confirmationModalInstance =
                          confirmationModalService.openConfirmationModal('md', targetResources.ChangeTabConfirmationMsg);

                    confirmationModalInstance.result.then(function () {
                        self.isFormDirty = false;
                        self.addOrUpdateTargetForm.$setPristine();
                        self.tab = tabIndex;
                        clear();
                        self.canAddMetricTarget = true;
                        loadMetricDetails(tabIndex, currentlyUpdatedTargetId, currentlyAddedTargetMetricType);
                    }, function () {
                    });
                };

                //KPI Tab click
                self.setTab = function (tabIndex, currentlyUpdatedTargetId, currentlyAddedTargetMetricType) {
                    self.isFormDirty = self.addOrUpdateTargetForm.$dirty;

                    if (self.isFormDirty) {
                        confirmDisacardUnsavedChanges(tabIndex, currentlyUpdatedTargetId, currentlyAddedTargetMetricType);
                    }
                    else {
                        self.tab = tabIndex;
                        clear();
                        self.canAddMetricTarget = self.isFormDirty ? false : true;
                        loadMetricDetails(tabIndex, currentlyUpdatedTargetId, currentlyAddedTargetMetricType);
                    }

                    if (!currentlyUpdatedTargetId && !currentlyAddedTargetMetricType) {
                        self.currentExpandedTargetIndex = 0;
                    }
                };

                //To highlight selected tab
                self.isSet = function (tabNum) {
                    return self.tab === tabNum;
                };

                self.resetMetricsList = function () {
                    targetService.getMetrics(self.tab, scorecardId)
                       .then(function (data) {
                           if (data.length) {
                               self.fullMetrics = data;
                               getUnsetMetrics(data);
                           }
                       }, function (err) {
                           notificationService.notify(err.errors.join("<br/>"),
                           {
                               autoClose: false, type: "danger"
                           });
                       });
                }

                // get the metrics active in specific date range
                var getSetMetricsInDateRange = function (isEdit) {
                    self.setMetricsInDateRange = [];
                    self.validMetrics = [];
                    if (isEdit) {
                        // if edit, remove the currently editing item from the set metrics list
                        angular.forEach(self.setMatrics, function (metric) {
                            if (metric.targetId != self.targetId) {
                                self.validMetrics.push(metric);
                            }
                        });
                    }
                    else {
                        self.validMetrics = self.setMatrics;
                    }

                    angular.forEach(self.validMetrics, function (metric) {
                        if (metric.effectiveStartDate <= self.effectiveEndDate && metric.effectiveEndDate >= self.effectiveStartDate) {
                            self.setMetricsInDateRange.push(metric.metricName);
                        }
                    });

                    if (self.metricId) {
                        self.getCascadedMetricDetails();
                    }
                }

                //extracting metrics which are not set while adding target.
                var getUnsetMetrics = function (data) {
                    var unsetMetrics = [];

                    getSetMetricsInDateRange(self.isMetricEditMode);

                    for (var index = 0; index < data.length; index++) {
                        if ($.inArray(data[index].name, self.setMetricsInDateRange) == -1) {
                            var item = $filter('filter')(data, { name: data.name })[index];
                            var hasDuplicateMetric = false;
                            if (unsetMetrics.length) {
                                for (var i = 0; i < unsetMetrics.length; i++) {
                                    if (unsetMetrics[i].id == item.id) {
                                        hasDuplicateMetric = true;
                                        break;
                                    }
                                    else {
                                        hasDuplicateMetric = false;
                                    }
                                }
                            }
                            if (!hasDuplicateMetric) {
                                unsetMetrics.push(item);
                            }
                        }
                    }
                    self.metrics = unsetMetrics;
                };

                //to get available metrics for selected KPI
                var getMetricsForSelectedKpi = function (kpiId, isEditingTargetcascadedFromParent) {
                    self.metrics = [];

                    targetService.getMetrics(kpiId, scorecardId)
                       .then(function (data) {
                           if (data.length) {
                               self.fullMetrics = data;
                               if (!self.isMetricEditMode) {
                                   getUnsetMetrics(data);

                                   //show add target page only if metrics are available for that kpi.
                                   //and setting variables for  add target page.
                                   if (self.metrics.length !== 0) {
                                       clear();
                                       self.isAddingNewTarget = true;
                                       self.canAddMetricTarget = false;
                                       self.action = "Save";
                                       getMonthsList();
                                   }
                               }
                               else {
                                   getUnsetMetrics(data);
                                   //service call to check  if the currently editing target
                                   //has set in parent level.If so make the cascaded details service
                                   //call only after fetching the metrics for kpi.
                                   if (!isEditingTargetcascadedFromParent) {
                                       self.getCascadedMetricDetails();
                                   }
                               }
                               // show error message if all the assigned metrics are already set.
                               if (self.metrics.length === 0) {
                                   notificationService.close();
                                   notificationService.notify(targetResources.assignMetrics,
                                                     { autoClose: false, type: "danger" });
                               }
                                   //navigate to add target page only if metrics are available for that kpi.
                               else {
                                   self.renderingPageUrl = 'ngViews/targets/partials/add-primary-metric.min.html';
                                   self.isMetricExists = true;
                                   self.canYearChange = false;

                               }
                           }
                           else {
                               // show error message if no metrics are assigned.
                               notificationService.close();
                               notificationService.notify(targetResources.assignMetrics,
                               {
                                   autoClose: false, type: "danger"
                               });
                           }
                       }, function (err) {
                           notificationService.notify(err.errors.join("<br/>"),
                           {
                               autoClose: false, type: "danger"
                           });
                       });
                };

                //get metric datatype to disable total column field if metric is percentage or decimal 
                //and to display metric type symbol. 
                var getSelectedMetricDataType = function (metricId) {
                    var item = $.grep(self.metrics, function (e) { return e.id == metricId; });
                    if (item.length) {
                        self.selectedMetricDataType = item[0].dataType;
                    }
                };

                //setting effective start date of the second primary metric from next month onwards 
                //if there is one primary metric exists which ending in middle of a month.
                var changeEffectiveStartDate = function () {
                    notificationService.close();
                    var item;
                    //getting the first primary metric.
                    if (self.metricData) {
                        item = $.grep(self.metricData, function (e) { return e.metricType === 0; });
                    }
                    if (item && item.length == 1) {

                        var firstPrimaryMetricEndDate = convertToDateWithoutTimezone(item[0].effectiveEndDate),
                            firstPrimaryMetricStartDate = convertToDateWithoutTimezone(item[0].effectiveStartDate),
                            firstPrimaryMetricEndYear = firstPrimaryMetricEndDate.getFullYear(),
                            firstPrimaryMetricEndMonth = firstPrimaryMetricEndDate.getMonth(),
                            firstPrimaryMetricStartMonth = firstPrimaryMetricStartDate.getMonth();

                        //do not change effective start date if editing the first primary metric itself and if effective start date is after current date.
                        if ((firstPrimaryMetricStartMonth <= self.currentMonth && firstPrimaryMetricEndMonth <= 10) &&
                           !(self.isMetricEditMode && (self.currentlyEditingTarget.id == item[0].id))) {
                            self.effectiveStartDate = new Date(firstPrimaryMetricEndYear, firstPrimaryMetricEndMonth + 1, 1);
                            // while switching primary to secondary, check whether end date is less than or equal to start date
                            if (self.effectiveStartDate >= self.effectiveEndDate) {
                                self.effectiveEndDate = convertToDateWithoutTimezone(self.years[self.years.length - 2].endDate);
                            }
                        }
                            //prevent showing error message to inform already one primary metric exist from current month to December 31st 
                            //if editing the first primary metric itself.
                        else if ((firstPrimaryMetricStartMonth === self.currentMonth && firstPrimaryMetricEndMonth === 11) &&
                               !(self.isMetricEditMode && (self.currentlyEditingTarget.id == item[0].id))) {
                            notificationService.close();
                            notificationService.notify(targetResources.InvalidSecondPrimaryMetric,
                                                   { autoClose: false, type: "danger" })
                        }
                    }
                };

                //display parent data if target cascading is enabled.
                var displayParentData = function () {

                    //Disable dates outside of parent effective dates

                    self.dateOptions.minDate = convertToDateWithoutTimezone(self.ParentMetricData.effectiveStartDate) > convertToDateWithoutTimezone(self.dateOptions.minDate) ?
                        convertToDateWithoutTimezone(self.ParentMetricData.effectiveStartDate) : convertToDateWithoutTimezone(self.dateOptions.minDate);
                    self.dateOptions.maxDate = convertToDateWithoutTimezone(self.ParentMetricData.effectiveEndDate) < convertToDateWithoutTimezone(self.dateOptions.maxDate) ?
                        convertToDateWithoutTimezone(self.ParentMetricData.effectiveEndDate) : convertToDateWithoutTimezone(self.dateOptions.maxDate);

                    self.isStretchGoalEnabled = self.ParentMetricData.isStretchGoalEnabled;


                    self.trackingMethodId = self.ParentMetricData.trackingMethodId || self.ParentMetricData.trackingMethodId === 0 ?
                        self.ParentMetricData.trackingMethodId : 0;

                    self.graphPlottingMethodId = self.ParentMetricData.graphPlottingMethodId || self.ParentMetricData.graphPlottingMethodId === 0 ?
                        self.ParentMetricData.graphPlottingMethodId : 0;

                    self.onGraphPlottingMethodChanged(self.graphPlottingMethodId);

                    self.mtdTrackingMethodId = self.ParentMetricData.mtdPerformanceTrackingMethodId || self.ParentMetricData.mtdPerformanceTrackingMethodId === 0 ?
                        self.ParentMetricData.mtdPerformanceTrackingMethodId : self.mtdTrackingMethodOptionsFiltered[0].id;

                    angular.forEach(self.monthlyTargets, function (value, index) {
                        self.monthlyTargets[index].maximumAllowedGoal =
                        self.ParentMetricData.maximumAllowedMonthlyGoals[index].value;
                    });

                    // Make Sum of Children as the default roll up method, if roll up is not selected
                    if (self.rollupMethodId === '') {
                        self.rollupMethodId = 0;
                    }
                };

                //on switching cascade radio button while editing target.
                var switchCascadingDataOnEdit = function () {
                    if (self.isCascadeFromParent) {
                        displayParentData();
                    }
                    else {
                        // revert date picker to enable full year
                        self.dateOptions.minDate = !self.isNextCalenderYearSelected ? convertToDateWithoutTimezone(self.currentYearMinDate) : convertToDateWithoutTimezone(self.nextYearMinDate);
                        self.dateOptions.maxDate = !self.isNextCalenderYearSelected ? convertToDateWithoutTimezone(self.currentMaxDate) : convertToDateWithoutTimezone(self.nextMaxDate);
                        //set own target data
                        self.effectiveStartDate = self.effectiveStartDate ? convertToDateWithoutTimezone(self.effectiveStartDate) : convertToDateWithoutTimezone(self.currentlyEditingTarget.effectiveStartDate);
                        self.effectiveEndDate = self.effectiveEndDate ? convertToDateWithoutTimezone(self.effectiveEndDate) : convertToDateWithoutTimezone(self.currentlyEditingTarget.effectiveEndDate);
                        self.isStretchGoalEnabled = self.currentlyEditingTarget.isStretchGoalEnabled;

                        self.trackingMethodId = self.currentlyEditingTarget.trackingMethodId || self.currentlyEditingTarget.trackingMethodId === 0 ?
                            self.currentlyEditingTarget.trackingMethodId : 0;
                        self.targetEntryMethodId = self.currentlyEditingTarget.targetEntryMethodId || self.currentlyEditingTarget.targetEntryMethodId === 0 ?
                            self.currentlyEditingTarget.targetEntryMethodId : 1;
                        self.graphPlottingMethodId = self.currentlyEditingTarget.graphPlottingMethodId || self.currentlyEditingTarget.graphPlottingMethodId === 0 ?
                            self.currentlyEditingTarget.graphPlottingMethodId : 0;
                        self.onGraphPlottingMethodChanged(self.graphPlottingMethodId);
                        self.mtdTrackingMethodId = self.currentlyEditingTarget.mtdPerformanceTrackingMethodId || self.currentlyEditingTarget.graphPlottingMethodId === 0 ?
                            self.currentlyEditingTarget.mtdPerformanceTrackingMethodId : self.mtdTrackingMethodOptionsFiltered[0].id;

                    }
                };

                var ValidateTargetEntryChange = function (targetEntryMethodId) {
                    var currentMonth = self.currentDate.getMonth();
                    var targetStartMonth = self.effectiveStartDate.getMonth();
                    if (self.isMetricEditMode && currentMonth > targetStartMonth &&
                    targetEntryMethodId == 0 && self.trackingMethodId == 1) {
                        notificationService.notify(targetResources.InvalidTargetEntry,
                         { autoClose: false, type: "danger" });
                        return false;
                    }

                    return true;
                }

                var updateUnallocatedGoals = function (targetEntryMethodId) {
                    var parentTargetId;
                    // If present in parent metric data, take it.
                    if (self.ParentMetricData.parentTargetId) {
                        parentTargetId = self.ParentMetricData.parentTargetId;
                    }
                    else {
                        parentTargetId = self.parentTargetId;
                    }

                    // Call service to get the maximum allowed monthly goals
                    targetService.getMaximumAllowedMonthlyGoals(parentTargetId, self.rollupMethodId,
                        self.targetId, targetEntryMethodId).then(function (data) {
                            if (!data.hasError) {
                                angular.forEach(self.monthlyTargets, function (mTarget, index) {
                                    // Find the parent's monthly target
                                    var item = $.grep(data, function (e) {
                                        return e.month.id === mTarget.month.id;
                                    });
                                    if (item && item.length > 0) {                                        
                                        mTarget.maximumAllowedGoal = item[0].value;
                                    }
                                });
                            }
                        },
                    function (err) {
                        notificationService.notify(err.errors.join("<br/>"),
                        {
                            autoClose: false, type: "danger"
                        });
                    });
                }

                var updateRollupTargets = function () {
                    // Call service to get the rolled up goals
                    targetService.getRolledUpTargets(self.targetId, self.targetEntryMethodId, self.mtdTrackingMethodId).then(function (data) {
                            if (!data.hasError) {
                                angular.forEach(self.monthlyTargets, function (mTarget, index) {
                                    // Find the parent's monthly target
                                    var item = $.grep(data, function (e) {
                                        return e.month.id === mTarget.month.id;
                                    });
                                    if (item && item.length > 0) {
                                        
                                        mTarget.rolledupGoalValue = item[0].value;
                                    }
                                });
                            }
                        },
                    function (err) {
                        notificationService.notify(err.errors.join("<br/>"),
                        {
                            autoClose: false, type: "danger"
                        });
                    });
                }

                //service call for getting parent data 
                self.getCascadedMetricDetails = function (isChangingMetricName) {

                    //setting total columns and year end target/full year target according to metricDataType
                    getSelectedMetricDataType(self.metricId);
                    selectYearTargetAmountLabel(self.selectedMetricDataType.id, isChangingMetricName);

                    var cascadeDetailsRequestItem = {
                        scorecardId: scorecardId,
                        kpiId: self.tab,
                        metricId: self.metricId,
                        calendarYearId: self.yearId,
                        metricType: self.metricType,
                        targetId: self.targetId,
                        targetEntryMethodId: self.targetEntryMethodId,
                        rollUpMethodId: self.rollupMethodId,
                        effectiveStartDate: $filter('date')(self.effectiveStartDate, "dd MMM yyyy"),
                        effectiveEndDate: $filter('date')(self.effectiveEndDate, "dd MMM yyyy")
                    };
                    targetService.getCascadedMetricDetails(cascadeDetailsRequestItem)
                                             .then(
                                                 function (data) {
                                                     if (data) {
                                                         self.isCascadeEnabled = true;
                                                         self.isCascadedFromParent = data.cascadeFromParent;
                                                         self.ParentMetricData = data;
                                                         if (self.isMetricEditMode) {
                                                             switchCascadingDataOnEdit();
                                                         }
                                                     }
                                                     else {
                                                         self.isCascadeEnabled = false;
                                                         self.isCascadeFromParent = false;
                                                     }
                                                 },
                                                 function (err) {
                                                     notificationService.notify(err.errors.join("<br/>"),
                                                     { autoClose: false, type: "danger" });
                                                 });
                };

                //display or hide parent metric data using radio button.
                self.changeCascading = function (isCascade) {
                    if (self.isMetricEditMode) {
                        self.getCascadedMetricDetails();
                    }
                    else {
                        if (self.isCascadeFromParent) {
                            displayParentData();
                            if (self.ParentMetricData) {
                                self.targetEntryMethodId = self.ParentMetricData.targetEntryMethodId;
                                self.onTargetEntryMethodChanged(self.targetEntryMethodId);
                            }
                        }
                        else {
                            // revert date picker to enable full year
                            self.dateOptions.minDate = !self.isNextCalenderYearSelected ? convertToDateWithoutTimezone(self.currentYearMinDate) : convertToDateWithoutTimezone(self.nextYearMinDate);
                            self.dateOptions.maxDate = !self.isNextCalenderYearSelected ? convertToDateWithoutTimezone(self.currentMaxDate) : convertToDateWithoutTimezone(self.nextMaxDate);

                            self.isStretchGoalEnabled = '';
                            self.trackingMethodId = 0;
                            self.graphPlottingMethodId = 0;
                            self.targetEntryMethodId = 1;
                            angular.forEach(self.monthlyTargets, function (value, index) {
                                self.monthlyTargets[index].maximumAllowedGoal = '';
                            });
                        }
                    }
                    self.resetMetricsList();
                };

                // reset mtd tracking options on changing graphPlotting method
                self.onGraphPlottingMethodChanged = function (graphPlottingMethodId) {
                    self.mtdTrackingMethodOptionsFiltered = [];
                    if (graphPlottingMethodId === 0) {
                        self.mtdTrackingMethodOptionsFiltered =
                            $.grep(self.mtdTrackingMethodOptions, function (item) {
                                return item.id !== 0;
                            });
                    }
                    else if (graphPlottingMethodId === 1) {
                        self.mtdTrackingMethodOptionsFiltered =
                            $.grep(self.mtdTrackingMethodOptions, function (item) {
                                return item.id === 0;
                            });
                    }
                    if (self.mtdTrackingMethodOptionsFiltered.length > 0) {
                        self.mtdTrackingMethodId = self.mtdTrackingMethodOptionsFiltered[0].id;
                    }
                };

                //allow monthly tracking only for monthly target entry method
                self.onTargetEntryMethodChanged = function (targetEntryMethodId, oldTargetEntryMethodId) {
                    // filter tracking method option on target entry method changed.
                    self.trackingMethodOptionsFiltered = [];
                    var isValid = ValidateTargetEntryChange(targetEntryMethodId);
                    if (isValid) {
                        if (targetEntryMethodId === 0) {
                            self.trackingMethodOptionsFiltered =
                                $.grep(self.trackingMethodOptions, function (item) {
                                    return item.id != 1;
                                });
                            self.trackingMethodId = 0;

                        }
                        else if (targetEntryMethodId === 1) {
                            self.trackingMethodOptionsFiltered = self.trackingMethodOptions;
                            self.trackingMethodId = self.trackingMethodId ? self.trackingMethodId : self.trackingMethodOptionsFiltered[0].id;
                        }

                        var currentMonth = self.currentDate.getMonth() + 1;
                        if (self.monthlyTargets) {
                            self.monthlyTargets.forEach(function (monthlyTarget) {
                                if (self.isMetricEditMode) {
                                    // if edit mode clear next month daily rate/ monthly goal
                                    if (monthlyTarget.month.id > currentMonth) {
                                        monthlyTarget.dailyRateValue = null;
                                        monthlyTarget.goalValue = null;
                                        monthlyTarget.stretchGoalValue = null;
                                        monthlyTarget.dailyTargets = [];
                                        monthlyTarget.hasManualTarget = false;
                                        monthlyTarget.isManualTarget = true;
                                    }
                                    else if ((monthlyTarget.dailyRateValue || monthlyTarget.dailyRateValue == 0) ||
                                             (monthlyTarget.goalValue || monthlyTarget.goalValue == 0)) {
                                        self.hasMonthlyAndDailyTargets = true;
                                        if (monthlyTarget.month.id == currentMonth || monthlyTarget.month.id == currentMonth - 1) {
                                            monthlyTarget.hasManualTarget = true;
                                            if (monthlyTarget.dailyTargets) {
                                                monthlyTarget.dailyTargets.forEach(function (target) {
                                                    target.isManual = true;
                                                });
                                            }
                                        }
                                        
                                    }
                                }
                                else {
                                    monthlyTarget.dailyRateValue = null;
                                    monthlyTarget.stretchGoalValue = null;
                                    monthlyTarget.hasManualTarget = false;
                                    monthlyTarget.goalValue = null;
                                    monthlyTarget.dailyTargets = [];
                                }
                            });
                        }

                        if (self.isCascadeFromParent) {
                            updateUnallocatedGoals(targetEntryMethodId);
                        }

                        if (self.isCascaded) {
                            updateRollupTargets();
                        }
                    }
                    else {
                        self.targetEntryMethodId = oldTargetEntryMethodId;
                        self.trackingMethodOptionsFiltered = self.trackingMethodOptions;
                    }
                };

                self.onMTDPerformanceTrackingChanged = function (mtdTrackingMethodId) {
                    if (self.isCascaded) {
                        updateRollupTargets();
                    }
                }


                // This function will be invoked when roll up method combo box selection is changed
                self.onRollupMethodChanged = function (rollupMethodId) {

                    var parentTargetId;
                    // If present in parent metric data, take it.
                    if (self.ParentMetricData.parentTargetId) {
                        parentTargetId = self.ParentMetricData.parentTargetId;
                    }
                    else {
                        parentTargetId = self.parentTargetId;
                    }

                    // Call service to get the maximum allowed monthly goals
                    targetService.getMaximumAllowedMonthlyGoals(parentTargetId, rollupMethodId,
                        self.targetId, self.targetEntryMethodId).then(function (data) {                        
                            if (!data.hasError && self.monthlyTargets) {
                                var startMonth = self.effectiveStartDate.getMonth();
                                if (self.hasMonthlyAndDailyTargets) {
                                    startMonth = self.currentDate.getMonth() + 1;
                                }
                                self.monthlyTargets.forEach(function (mtarget) {
                                    if (mtarget.month.id > startMonth &&
                                        mtarget.month.id <= self.effectiveEndDate.getMonth() + 1) {

                                        //get the parents monthlyGoal Item
                                        var item = $.grep(data, function (e) {
                                            return e.month.id == mtarget.month.id;
                                        })[0];

                                        if (item) {

                                            //set monthly goal / dialyRate (in case of Same As Child rollup method) 
                                            // and maximum allowed value     
                                            if (rollupMethodId == 3) {
                                                if (self.targetEntryMethodId == 0) {
                                                    mtarget.dailyRateValue = item.value;
                                                }
                                                else if (self.targetEntryMethodId == 1) {
                                                    mtarget.goalValue = item.value;
                                                }
                                            }
                                            mtarget.maximumAllowedGoal = item.value;
                                        }
                                    }

                                });
                            }
                        },
                    function (err) {
                        notificationService.notify(err.errors.join("<br/>"),
                        {
                            autoClose: false, type: "danger"
                        });
                    });
                };


                var setKPiTargetData = function (data) {
                    self.kpiTargetData = [];
                    self.targets = [];
                    self.monthlyData = [];
                    for (var i = 0; i < data.length; i++) {
                        self.monthlyData.push({
                            'month': data[i],
                            'goalValue': '',
                            'stretchGoalValue': '',
                            'maximumAllowedGoal': '',
                            'dailyRateValue': '',
                            'dailyTargets': []
                        });
                    }
                    self.monthlyTargets = self.monthlyData;
                };

                //getting months list from service call
                var getMonthsList = function () {
                    targetService.getMonthsList(self.yearId).then(function (data) {
                        self.monthsList = data;
                        setKPiTargetData(self.monthsList);
                    },
                    function (err) {
                        notificationService.notify(err.errors.join("<br/>"),
                        {
                            autoClose: false, type: "danger"
                        });
                    });
                };

                var setAddEditPageURlandMetrics = function (isEditingTargetcascadedFromParent) {
                    var kpiId = self.tab;
                    getMetricsForSelectedKpi(kpiId, isEditingTargetcascadedFromParent);

                };

                // add goal button click
                self.addNewTarget = function () {
                    setAddEditPageURlandMetrics();
                    self.onGraphPlottingMethodChanged(self.graphPlottingMethodId);
                    self.isAddingNewTarget = true;

                };

                self.copyMetrics = function () {
                    targetService.getSelectedYearTargets(scorecardId, self.yearId)
                    .then(function (data) {
                        if (data) {
                            openCopyMetricPopup(data);
                        }
                    },
                    function (err) {
                        notificationService.notify(err.errors.join("<br/>"),
                        {
                            autoClose: false, type: "danger"
                        });
                    });

                }

                var setCopyTargetYears = function () {
                    self.copyFromYear = '';
                    self.copyToYear = '';
                    if (self.years[0].id == self.yearId) {
                        self.copyFromYear = self.years[0];
                        self.copyToYear = self.years[1];
                    }
                    else if (self.years[1].id == self.yearId) {
                        self.copyFromYear = self.years[1];
                        self.copyToYear = self.years[2];
                    }
                }

                var openCopyMetricPopup = function (kpiTargets) {

                    setCopyTargetYears();
                    var modalInstance = $uibModal.open({
                        animation: true,
                        templateUrl: 'ngViews/targets/templates/CopyTargetPopup.tpl.min.html',
                        controller: 'copyTargetController',
                        controllerAs: 'ctrl',
                        windowClass: 'copy-target-popup',
                        backdrop: 'static',
                        keyboard: false,
                        resolve: {
                            kpiTargets: function () {
                                return kpiTargets;
                            },
                            targetParams: function () {
                                var targetParams = {
                                    'scorecardId': scorecardId,
                                    'kpiId': self.tab,
                                    'kpiName': getKpiName(self.tab),
                                    'fromYear': self.copyFromYear,
                                    'toYear': self.copyToYear
                                };
                                return targetParams;
                            }
                        }
                    });
                    modalInstance.result.then(function (changedValues) {
                        // set the calendar to next year
                        self.bindYearId(changedValues.calendarYearId);
                    },
                    function (err) {

                    });
                    return modalInstance;
                }

                var setPrimary = function (metricType) {
                    if (metricType === 0) {
                        self.isPrimary = true;
                    } else {
                        self.isPrimary = false;
                    }
                };

                var setCascadingAndStretchGoalOnEdit = function (target) {
                    //cascaded metric details enabling/disabling while updating target
                    if (target.cascadeFromParent) {
                        self.isCascadeFromParent = true;
                        self.isCascadeEnabled = true;
                    }
                    else {
                        self.isCascadeFromParent = false;
                        self.isCascadeEnabled = false;
                    }
                };

                // on edit target button click 
                self.editTarget = function (target) {
                    self.currentlyEditingTarget = target;
                    self.isMetricEditMode = true;
                    setAddEditPageURlandMetrics(target.cascadeFromParent);
                    self.action = "Update";
                    self.targetId = target.id;
                    self.parentTargetId = target.parentTargetId;
                    self.metricId = target.metricId;
                    self.metricIdOnEdit = target.metricId;
                    self.metricType = target.metricType;
                    self.metricDataTypeId = target.metricDataTypeId;
                    setPrimary(self.metricType);
                    self.trackingMethodId = target.trackingMethodId;
                    self.targetEntryMethodId = target.targetEntryMethodId;
                    self.graphPlottingMethodId = target.graphPlottingMethodId;
                    self.hasMonthlyAndDailyTargets = target.hasMonthlyAndDailyTargets;
                    var rollupMethodItem;
                    if (target.rollupMethodId !== null)
                        rollupMethodItem =
                         $filter('filter')(self.rollupMethodOptions, { id: target.rollupMethodId })[0];
                    if (rollupMethodItem) {
                        self.rollupMethodId = rollupMethodItem.id;
                    }
                    self.onGraphPlottingMethodChanged(self.graphPlottingMethodId, target.mtdPerformanceTrackingMethodId);
                    self.onTargetEntryMethodChanged(self.targetEntryMethodId);
                    self.mtdTrackingMethodId = target.mtdPerformanceTrackingMethodId;
                    self.cascadedMetricsTrackingMethodId = target.cascadedMetricsTrackingMethodId
                    self.isCascaded = target.isCascaded;

                    self.annualTarget = target.annualTarget;
                    self.monthlyTargets = target.monthlyTargets;
                    self.isStretchGoalEnabled = target.isStretchGoalEnabled;

                    setCascadingAndStretchGoalOnEdit(target);
                    self.effectiveStartDate = convertToDateWithoutTimezone(target.effectiveStartDate);
                    self.effectiveEndDate = convertToDateWithoutTimezone(target.effectiveEndDate);
                    self.canAddMetricTarget = true;
                    self.selectedMetricDataType.id = self.metricDataTypeId;
                    selectYearTargetAmountLabel(self.selectedMetricDataType.id);

                };

                // on delete target button click 
                self.deleteTarget = function (target) {

                    var confirmationModalInstance =
                               confirmationModalService.openConfirmationModal('md', targetResources.DeleteConfirmationMsg);
                    confirmationModalInstance.result.then(function () {
                        targetService.deleteMetricTarget(target.id, target.scorecardId).then(function (data) {
                            if (data) {
                                notificationService.notify(target.metricName + " " + targetResources.DeletedSuccessfully,
                                        {
                                            type: "success"
                                        }).then(function () {
                                            self.setTab(self.tab);
                                        });
                            }
                        }, function (err) {
                            notificationService.notify(err.errors.join("<br/>"),
                                     {
                                         type: "danger"
                                     }).then(function () {
                                         self.setTab(self.tab);
                                     });

                        });
                    }, function () {
                    });
                };

                //effective start date validations while add or update.
                var validateEffectiveStartDate = function (data) {

                    if (data.effectiveStartDate == null || data.effectiveStartDate == "") {
                        self.errors.push(targetResources.RequiredEffectiveStartDate);
                    }
                    else {
                        var effStartDate = convertToDateWithoutTimezone(data.effectiveStartDate);
                        if (convertToDateWithoutTimezone(self.currentDate) != effStartDate && data.effectiveStartDate < self.currentDate) {
                            self.errors.push(targetResources.PastEffectiveStartDate);
                        }
                    }
                };

                //effective end date validations while add or update.
                var validateEffectiveEndDate = function (data) {

                    if (self.effectiveEndDate == null || self.effectiveEndDate == "") {
                        self.errors.push(targetResources.RequiredEffectiveEndDate);
                    }
                    else if (data.effectiveEndDate < self.currentDate) {
                        self.errors.push(targetResources.PastEffectiveEndDate);
                    }
                };
                var validateEffectiveDateRange = function (data) {
                    if ((data.effectiveStartDate != null && data.effectiveStartDate != "") && (data.effectiveEndDate != null && data.effectiveEndDate != "")) {
                        if (convertToDateWithoutTimezone(data.effectiveStartDate) >= convertToDateWithoutTimezone(data.effectiveEndDate)) {
                            self.errors.push(targetResources.InvalidDateRange);
                        }
                    }
                };

                var validateGoalsAndTotalColoumns = function () {

                    //validate goal
                    self.goalEntered = false;
                    self.monthlyTargets.forEach(function (obj) {
                        if (!self.goalEntered) {
                            //if target entry method monthly check if monthly goal list is empty
                            if (self.targetEntryMethodId === 1) {
                                if (obj.goalValue !== null && obj.goalValue !== "") {
                                    self.goalEntered = true;
                                }
                                else {
                                    self.goalEntered = false;
                                }
                            }
                                //if target entry method monthly check if daily rate list is empty
                            else if (self.targetEntryMethodId === 0) {
                                if (obj.dailyRateValue !== null && obj.dailyRateValue !== "") {
                                    self.goalEntered = true;
                                }
                                else {
                                    self.goalEntered = false;
                                }
                            }
                        }
                        if (self.selectedMetricDataType.id === 0) {
                            if (self.targetEntryMethodId === 0 && obj.dailyRateValue !== null && obj.dailyRateValue !== "") {
                                obj.dailyRateValue = Math.floor(obj.dailyRateValue);
                            }
                            else if (self.targetEntryMethodId === 1 && obj.goalValue !== null && obj.goalValue !== "") {
                                obj.goalValue = Math.floor(obj.goalValue);
                            }
                        }
                    });
                    if (!self.goalEntered) {
                        self.errors.push(self.targetEntryMethodId === 1 ? targetResources.RequiredMonthlyTarget : targetResources.RequiredDailyRate);
                    }

                    //validate total columns
                    var goalErrorCount = 0,
                    stretchGoalErrorCount = 0,
                    annualTargetErrorCount = 0;
                    angular.forEach(self.monthlyTargets, function (value, index) {
                        if (value.stretchGoalValue != null) {
                            if (value.stretchGoalValue != "") {
                                if (self.selectedMetricDataType.id == 0) {
                                    value.stretchGoalValue = Math.floor(value.stretchGoalValue);
                                }
                            }
                        }
                        //checking error message count to show  the error message only once.
                        if (angular.isUndefined(value.goalValue) && goalErrorCount === 0) {
                            self.errors.push(targetResources.InvalidGoal);
                            goalErrorCount = goalErrorCount + 1;
                        }
                        if (angular.isUndefined(value.stretchGoalValue) && stretchGoalErrorCount === 0) {
                            self.errors.push(targetResources.InvalidStretchGoal);
                            stretchGoalErrorCount = stretchGoalErrorCount + 1;
                        }
                        if (angular.isUndefined(self.annualTarget) && annualTargetErrorCount === 0) {
                            self.errors.push(targetResources.InvalidAnnualTarget);
                            annualTargetErrorCount = annualTargetErrorCount + 1;
                        }

                    });
                };

                //validations
                var validateInputData = function (data) {
                    self.errors = [];

                    //metric type validation
                    if (data.metricType === "") {
                        self.errors.push(targetResources.RequiredMetricType);
                    }
                    //metric name validation
                    if (data.metricId === "") {
                        self.errors.push(targetResources.RequiredMetricName);
                    }
                    if ((data.effectiveStartDate == null || data.effectiveStartDate == "") && (data.effectiveEndDate == null || data.effectiveEndDate == "")) {
                        self.errors.push(targetResources.RequiredEffectiveStartDateAndEffectiveEndDate);
                    }
                    else {
                        validateEffectiveStartDate(data);
                        validateEffectiveEndDate(data);
                        validateEffectiveDateRange(data);
                    }
                    validateGoalsAndTotalColoumns();
                };

                var validateDailyPopupOpenRequest = function (target, mode) {
                    if (mode !== "view" && (self.metricId === null || self.metricId === "")) {
                        self.errors.push(targetResources.RequiredMetricName);
                    }
                    if (target.targetEntryMethod === 1 && (target.goalValue === null || target.goalValue === "") && (target.dailyRateValue === null || target.dailyRateValue === "")) {
                        self.errors.push(targetResources.SetMonthlyTarget);
                    }
                }

                var getKpiName = function (kpiId) {
                    switch (kpiId) {
                        case 0: return 'Safety';
                        case 1: return 'Quality';
                        case 2: return 'Delivery';
                        case 3: return 'Innovation';
                        case 4: return 'Cost';
                        case 5: return 'People [Culture]';
                        case 6: return 'Revenue';
                        case 7: return 'Net Working Capital';
                    }
                };

                var getMetricName = function (id) {
                    var metric = $.grep(self.metrics, function (obj) {
                        if (obj.id === id) {
                            return obj;
                        }
                    });
                    if (metric.length) {
                        return metric[0].name;
                    }
                    else {
                        self.targetData.metricId = '';
                        return '';
                    }
                };
                //returns yearId from year
                var getYearId = function (year) {
                    var yearObject = $.grep(self.years, function (yearObj) {
                        if (yearObj.name == year)
                            return yearObj;
                    });
                    return yearObject[0].id;
                };

                //set target service call after popup confirmation
                var setTarget = function () {

                    self.canAddMetricTarget = true;
                    self.addOrUpdateTargetForm.$setPristine();
                    self.kpiName = getKpiName(self.targetData.kpiId);
                    self.metricName = getMetricName(self.targetData.metricId);
                    self.targetData.metricDataTypeId = self.selectedMetricDataType.id;
                    targetService.setTarget(self.targetData)
                                 .then(function (data) {
                                     notificationService.notify(self.metricName + " for " + self.kpiName + " " + targetResources.SetSuccessfully,
                                     {
                                         type: 'success'
                                     }).then(function () {
                                         self.setTab(self.tab, self.targetData.id, self.targetData.metricType);
                                         self.isSaving = false;
                                     });
                                 }, function (err) {
                                     self.isSaving = false;
                                     notificationService.notify(err.errors.join("<br/>"),
                                     {
                                         autoClose: false, type: "danger"
                                     });
                                 });
                };

                //update target service call after popup confirmation
                var updateTarget = function () {

                    self.canAddMetricTarget = true;
                    self.addOrUpdateTargetForm.$setPristine();
                    self.targetData.id = self.targetId;
                    self.kpiName = getKpiName(self.targetData.kpiId);
                    self.metricName = getMetricName(self.targetData.metricId);
                    self.targetData.metricDataTypeId = self.selectedMetricDataType.id;

                    targetService.updateTarget(self.targetData)
                                 .then(function (data) {
                                     notificationService.notify(self.metricName + " for " + self.kpiName + " " + targetResources.UpdateSuccessfully,
                                     {
                                         type: 'success'
                                     }).then(function () {
                                         self.setTab(self.tab, self.targetData.id);
                                         self.isSaving = false;
                                     });
                                 }, function (err) {
                                     self.isSaving = false;
                                     notificationService.notify(err.errors.join("<br/>"),
                                     {
                                         autoClose: false, type: "danger"
                                     });
                                 });
                };

                //submit confirmation popup opens
                self.confirmSubmit = function (action) {
                    setTargetData();
                    validateInputData(self.targetData);

                    if (!self.errors.length) {
                        var confirmationModalInstance =
                                confirmationModalService.openConfirmationModal('md', targetResources.SubmitConfirmationMsg);

                        confirmationModalInstance.result.then(function () {
                            if (self.action == "Save") {
                                self.isSaving = true;
                                setTarget();
                            }
                            else if (self.action == "Update") {
                                self.isSaving = true;
                                updateTarget();
                            }
                        }, function () {
                        });
                    }
                    else {
                        notificationService.notify(self.errors.join("<br/>"),
                        {
                            autoClose: false, type: "danger"
                        });
                    }
                };

                //daily target entry logic
                self.submitDailyTarget = function (target, mode, showDailyAndMonthly, metricDataType) {
                    self.errors = [];
                    validateDailyPopupOpenRequest(target, mode);
                    if (self.errors.length) {
                        notificationService.notify(self.errors.join("<br/>"), {
                            type: "danger", autoClose: false
                        });
                    }
                    else {
                        var targetEntryMethod = !showDailyAndMonthly ? self.targetEntryMethodId :
                                                             (target.goalValue || target.goalValue === 0) ? 1 :
                                                             (target.dailyRateValue || target.dailyRateValue === 0) ? 0 : self.targetEntryMethodId;
                        self.targetMonthid = target.month.id;
                        self.monthlyTargetId = target.id ? target.id : '';
                        var modalInstance = $uibModal.open({
                            animation: true,
                            templateUrl: 'ngViews/targets/templates/dailyTargetPopup.tpl.min.html',
                            controller: 'dailyTargetController',
                            controllerUrl: 'ngControllers/target/daily-targets-tpl-controller.min',
                            controllerAs: 'ctrl',
                            backdrop: 'static',
                            keyboard: false,
                            resolve: {
                                monthTarget: function () {
                                    return target;
                                },
                                targetParams: function () {
                                    var targetParams = {
                                        'scorecardId': scorecardId,
                                        'metricId': self.metricId,
                                        'effectiveStartDate': $filter('date')(self.effectiveStartDate, "dd MMM yyyy"),
                                        'effectiveEndDate': $filter('date')(self.effectiveEndDate, "dd MMM yyyy"),
                                        'metricDataTypeId': metricDataType,
                                        'targetEntryMethod': targetEntryMethod,
                                        'kpiId': self.tab,
                                        'monthId': target.month.id,
                                        'yearId': self.yearId,
                                        'showDailyAndMonthly': showDailyAndMonthly
                                    };
                                    return targetParams;
                                },
                                monthlyTargetId: target.id,
                                mode: function () {
                                    return mode;
                                }
                            }
                        });
                        modalInstance.result.then(function (data) {
                            var updatedMonthlyTarget = data;
                            if (targetEntryMethod != self.targetEntryMethodId) {
                                updatedMonthlyTarget.dailyTargets.forEach(function (dailyTarget) {
                                    dailyTarget.isManual = true;
                                });
                            }
                            //self.dailyTargets = data.dailyTargets;
                            var currentlyEditingMonthTarget = $.grep(self.monthlyTargets, function (obj) {
                                return obj.month.id === self.targetMonthid;
                            })[0];
                            currentlyEditingMonthTarget.goalValue = updatedMonthlyTarget.goalValue;
                            currentlyEditingMonthTarget.dailyRateValue = updatedMonthlyTarget.dailyRateValue;
                            currentlyEditingMonthTarget.dailyTargets = updatedMonthlyTarget.dailyTargets;

                            currentlyEditingMonthTarget.hasManualTarget = $.grep(updatedMonthlyTarget.dailyTargets, function (elt) {
                                return elt.isManual === true;
                            }).length > 0;
                        },
                        function (err) {

                        });
                        return modalInstance;
                    }
                };


                var clear = function () {
                    self.canYearChange = true;
                    self.isCascadeEnabled = false;
                    self.isCascadeFromParent = false;
                    self.isCascaded = false;
                    self.isSaving = false;
                    self.isMetricEditMode = false;
                    self.canAddMetricTarget = true;
                    self.isStretchGoalEnabled = false;
                    self.hasMonthlyAndDailyTargets = false;
                    self.isAddingNewTarget = false;

                    self.targetId = '';
                    self.metricId = '';
                    self.metricType = '';
                    self.annualTarget = '';
                    self.monthlyTargets = '';
                    self.rollupMethodId = '';
                    self.cascadedMetricsTrackingMethodId = '';

                    self.effectiveStartDate = self.isNextCalenderYearSelected ?
                        convertToDateWithoutTimezone(self.years[self.years.length - 1].startDate) : convertToDateWithoutTimezone(self.currentMonthStartDate);
                    self.effectiveEndDate = self.isNextCalenderYearSelected ?
                    convertToDateWithoutTimezone(self.years[self.years.length - 1].endDate) : convertToDateWithoutTimezone(self.years[self.years.length - 2].endDate);

                    self.ParentMetricData = {};
                    self.selectedMetricDataType = {};

                    self.trackingMethodId = 0;
                    self.targetEntryMethodId = 1;
                    self.graphPlottingMethodId = 0;

                    self.dateOptions.minDate = !self.isNextCalenderYearSelected ? convertToDateWithoutTimezone(self.currentYearMinDate) : convertToDateWithoutTimezone(self.nextYearMinDate);
                    self.dateOptions.maxDate = !self.isNextCalenderYearSelected ? convertToDateWithoutTimezone(self.currentMaxDate) : convertToDateWithoutTimezone(self.nextMaxDate);

                    selectYearTargetAmountLabel(self.selectedMetricDataType.id);
                    self.onTargetEntryMethodChanged(self.targetEntryMethodId);
                };

                //On cancel button click in set metric page
                self.cancel = function () {
                    self.addOrUpdateTargetForm.$setPristine();
                    clear();
                    if (self.metricData) {
                        self.isMetricExists = true;
                        self.renderingPageUrl = 'ngViews/targets/partials/view-target.min.html';
                        self.setTab(self.tab);
                    }
                    else
                        self.isMetricExists = false;
                };

                //get the kpi's for this scorecard
                var loadKpi = function (scorecardId, yearId) {
                    targetService.getKpis(scorecardId, yearId).then(function (data) {
                        self.existingKPIs = data;
                        if (!self.tab) {
                            self.tab = self.existingKPIs[0].id;
                        }
                        self.setTab(self.tab);
                    }, function (err) {
                        notificationService.notify(err.errors.join("<br/>"),
                        {
                            autoClose: false, type: "danger"
                        });
                    });
                };

                self.resetMonthlyStretchGoals = function () {
                    angular.forEach(self.monthlyTargets, function (value, index) {
                        self.monthlyTargets[index].stretchGoalValue = '';
                    });
                };

                //to show the stretch goal radio button if primary metric selected
                self.metricTypeChange = function (metricType) {

                    if (metricType !== undefined && metricType === 0) {
                        self.isPrimary = true;
                        if (self.action != "Update") {
                            self.isStretchGoalEnabled = false;
                            self.trackingMethodId = self.trackingMethodOptionsFiltered[0].id;
                            self.graphPlottingMethodId = 0;
                            self.targetEntryMethodId = 1;
                        }
                        changeEffectiveStartDate();
                    }

                    if (metricType !== undefined && metricType == 1) {
                        self.isPrimary = false;
                        if (self.action != "Update") {
                            self.isStretchGoalEnabled = false;
                            self.trackingMethodId = self.trackingMethodOptionsFiltered[0].id;
                            self.targetEntryMethodId = 1;
                        }
                        if (self.isMetricEditMode) {
                            self.effectiveStartDate = convertToDateWithoutTimezone(self.currentlyEditingTarget.effectiveStartDate);
                        }
                        else {
                            self.effectiveStartDate = !self.isNextCalenderYearSelected ?
                                convertToDateWithoutTimezone(self.currentMonthStartDate) : convertToDateWithoutTimezone(self.nextYearMinDate);
                        }
                    }
                    if (self.metricId)
                        self.getCascadedMetricDetails();

                    self.resetMetricsList();
                };

                //initial load function
                var loadInitialData = function () {
                    targetService.getTargetsInitialData(scorecardId, !self.isViewTarget).then(function (data) {
                        if (!data.hasError) {
                            self.currentDate = convertToDateWithoutTimezone(data.currentDate);
                            self.currentMonthStartDate = data.currentMonthStartDate;
                            self.years = data.years;
                            setDefaultEffectiveStartAndEndDate();

                            self.currentMaxDate = convertToDateWithoutTimezone(data.years[self.years.length - 2].endDate);
                            self.nextYearMinDate = convertToDateWithoutTimezone(data.years[self.years.length - 1].startDate);
                            self.nextMaxDate = convertToDateWithoutTimezone(data.years[self.years.length - 1].endDate);
                            self.dateOptions = {
                                minDate: self.currentYearMinDate,
                                maxDate: self.currentMaxDate
                            };
                            self.rollupMethodOptions = data.rollupMethods;
                            self.metricTypeOptions = data.metricTypes;
                            self.isBowlingChartApplicable = data.isBowlingChartApplicable;
                            self.trackingMethodOptions = data.trackingMethods;
                            self.trackingMethodOptionsFiltered = self.trackingMethodOptions;
                            self.targetEntryMethodOptions = data.targetEntryMethods;
                            self.graphPlottingMethodOptions = data.graphPlottingMethods;
                            self.mtdTrackingMethodOptions = data.mtdTrackingMethods;
                            self.cascadedMetricsTrackingMethods = data.cascadedMetricsTrackingMethods;
                            self.yearId = getYearId(self.currentYear);
                            $rootScope.title = "Scorecard : " + data.scorecardName;
                            $rootScope.kpiOwners = data.kpiOwners;
                            self.scorecardName = data.scorecardName;
                            selectYearTargetAmountLabel();
                            loadKpi(scorecardId, self.yearId);
                        }
                    }, function (err) {
                        notificationService.notify(err.errors.join("<br/>"),
                        {
                            autoClose: false, type: "danger"
                        });
                    });
                };

                //to set effective end date to Dec 31 of that year by default.
                var setDefaultEffectiveStartAndEndDate = function () {
                    self.currentYear = self.currentDate.getFullYear();
                    self.currentMonth = self.currentDate.getMonth();
                    self.effectiveEndDate = convertToDateWithoutTimezone(self.years[self.years.length - 2].endDate);
                    self.currentYearMinDate = convertToDateWithoutTimezone(self.currentMonthStartDate);
                    self.effectiveStartDate = convertToDateWithoutTimezone(self.currentMonthStartDate);
                };

                var init = function () {
                    loadInitialData();
                }();
            }]);
    });