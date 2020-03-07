"use strict";
define(['angularAMD'], function (angularAMD) {
    angularAMD.directive('counterMeasure', ['$uibModal', '$filter',
        function ($uibModal, $filter) {
               return {
                   restrict: 'E',
                   scope: {
                       counterMeasureData : "=",
                       scorecardId : "=",
                       isEditAuthorised : "="
                   },
                   controller: function ($scope) {
                       var self = this;
                       $scope.counterMeasure = {};

                       $scope.toolTipOptions = {
                           filter: "th:last",
                           position: "bottom"
                       };
            
                       var sortCounterMeasureStatus = function (e, kendoGrid) {
                           var filterMultiCheck = kendoGrid.thead.find("[data-field=\"counterMeasureStatusId\"]").data("kendoFilterMultiCheck")
                           filterMultiCheck.container.empty();
                           filterMultiCheck.checkSource.sort({ field: e.field, dir: "asc" });

                           filterMultiCheck.checkSource.data(filterMultiCheck.checkSource.view().toJSON());
                           filterMultiCheck.createCheckBoxes();                           
                       }
                       var sortCounterMeasureFields = function (e, kendoGrid) {
                           var filterMultiCheck = kendoGrid.thead.find("[data-field=\"" + e.field + "\"]").data("kendoFilterMultiCheck")
                           filterMultiCheck.container.empty();
                           filterMultiCheck.checkSource.sort({ field: e.field, dir: "asc" });

                           filterMultiCheck.checkSource.data(filterMultiCheck.checkSource.view().toJSON());
                           filterMultiCheck.createCheckBoxes();
                       }

                       var initDateFilter = function (e, kendoGrid) {
                           var filterContext = {
                               container: e.container,
                               popup: e.container.data("kendoPopup"),
                               dataSource: kendoGrid.dataSource,
                               field: e.field
                           }

                           // Remove default filtering UI and create custom UI.
                           initDateFilterUI(filterContext);
                       }

                       var initDateFilterUI = function (filterContext) {
                           // Remove default filter UI
                           filterContext.container.off();
                           filterContext.container.empty();

                           // Create custom filter UI using a template
                           var template = kendo.template($("#counterMeasureDateFilterTemplate").html());
                           var result = template({});
                           filterContext.container.html(result);
                           filterContext.container.find('.date-from').kendoDatePicker();
                           filterContext.container.find('.date-to').kendoDatePicker();

                           filterContext.container.find('.date-from').attr("readonly", true);
                           filterContext.container.find('.date-to').attr("readonly", true);

                           filterContext.container
                               .on('submit', $.proxy(onSubmit, filterContext))
                               .on('reset', $.proxy(onReset, filterContext));
                       }

                       function onSubmit(e) {

                           // the context here is the filteringContext 
                           e.preventDefault();
                           e.stopPropagation();
                           
                           var dateFrom = this.container.find("[data-role=datepicker]:eq(0)").data("kendoDatePicker").value();
                           var dateTo = this.container.find("[data-role=datepicker]:eq(1)").data("kendoDatePicker").value();
                           removeFilterForField(this.dataSource, this.field);
                           applyDateRangeFilter(this.dataSource, this.field, dateFrom, dateTo);
                           this.popup.close();
                       };

                       function onReset(e) {

                           // the context here is the filteringContext 
                           e.preventDefault();
                           e.stopPropagation();

                           removeFilterForField(this.dataSource, this.field);
                           this.popup.close();
                       };

                       function applyDateRangeFilter(dataSource, field, dateFrom, dateTo) {

                           // Create custom filter
                           var dateFilter = {
                               field: field,
                               logic: "and",
                               filters:[
                                          { field: field, operator: "gte", value: dateFrom },
                                          { field: field, operator: "lte", value: dateTo }
                                       ]
                           };

                           var masterFilter = dataSource.filter() || { logic: "and", filters: [] };
                           if (masterFilter.logic == "or") {
                               var tempFilter = { logic: "and", filters: [] };
                               tempFilter.filters.push(masterFilter);
                               tempFilter.filters.push(dateFilter);
                               masterFilter = tempFilter;
                           }
                           else {
                               masterFilter.filters.push(dateFilter);
                           }
                           dataSource.filter(masterFilter);
                       };

                       function removeFilterForField(dataSource, field) {

                           var masterFilter = dataSource.filter();

                           if (!masterFilter) {
                               return;
                           }

                           // Get existing filters for the field
                           var existingFilters = $.grep(masterFilter.filters, function (item) {
                               return item.field === field;
                           });

                           angular.forEach(existingFilters, function (item) {
                               var index = masterFilter.filters.indexOf(item);
                               masterFilter.filters.splice(index, 1);
                           });

                           dataSource.filter(masterFilter);
                       };

                       var setGridOptions = function () {
                           self.gridOptions = {
                               sortable: true,
                               filterable: {
                                   operators: {
                                       string: {
                                           startswith: "Starts With",
                                           contains: "Contains"
                                       }
                                   }
                               },
                               filterMenuInit: function (e) {
                                   if (e.field === "counterMeasureStatusName") {
                                       sortCounterMeasureStatus(e, this);
                                   }
                                   else if (e.field == "openedDate" || e.field == "dueDate") {
                                       initDateFilter(e, this);
                                   }
                                   else if (e.field != "issue" && e.field != "action") {
                                       sortCounterMeasureFields(e, this);
                                   }
                               },
                               columns: [
                                          {
                                              field: "metricName",
                                              title: "Metric",
                                              filterable: { multi: true }
                                          },
                                          {
                                              field: "counterMeasurePriorityName",
                                              title: "Priority",
                                              filterable: { multi: true }
                                          },
                                          {
                                              field: "issue",
                                              title: "Issue",
                                              filterable: {
                                                  extra: false
                                              }
                                          },
                                          {
                                              field: "openedDate",
                                              title: "Opened"
                                          },
                                          {
                                              field: "action",
                                              title: "Actions",
                                              filterable: {
                                                  extra: false
                                              }
                                          },
                                          {
                                              field: "assignedUserName",
                                              title: "Who",
                                              filterable: { multi: true }
                                          },
                                          {
                                              field: "dueDate",
                                              title: "Due"
                                          },
                                          {
                                              field: "counterMeasureStatusId",
                                              title: "Status",
                                              filterable: {
                                                  field: "counterMeasureStatusName",
                                                  multi: true
                                              }
                                          },
                                          {
                                              field: "commentsCount",
                                              title: "Comments",
                                              headerTemplate: "<i class=\"fa fa-comments\"></i>",
                                              filterable: false,
                                              sortable: false
                                          }
                               ],
                               noRecords: {
                                   template: "No results to show"
                               },
                               rowTemplate: kendo.template($("#counterMeasureRowTemplate").html())
                           };
                       }

                       $scope.$watch('counterMeasureData', function () {
                           if ($scope.counterMeasureData !== undefined && $scope.counterMeasureData.length > 0) {
                               for (var i = 0; i < $scope.counterMeasureData.length; i++) {
                                   $scope.counterMeasureData[i].isEditFromTable = true;
                               }                               
                           }                           

                           refreshGrid();

                       });

                       var refreshGrid = function () {
                           setGridOptions();

                           self.counterMeasureData = new kendo.data.DataSource({
                               data: $scope.counterMeasureData,
                               schema: {
                                   model: {
                                       fields: {
                                           openedDate: {
                                               type: "date"
                                           },
                                           dueDate: {
                                               type: "date"
                                           },
                                           counterMeasureStatusId: {
                                               type: "number"
                                           }
                                       }
                                   }
                               }
                           });
                       }

                       $scope.editCounterMeasure = function (dataItem) {                           
                           var selectedCounterMeasure = $filter('filter')($scope.counterMeasureData, { id: dataItem.id }, true)[0];
                           var modalInstance = $uibModal.open({
                               templateUrl: 'ngViews/counter-measure/partials/counter-measure-popup.min.html',
                               controller: 'counterMeasureController',
                               controllerUrl: 'ngControllers/counter-measure/counter-measure-controller.min',
                               controllerAs: 'ctrl',
                               windowClass: 'counter-measure-popup',
                               backdrop: 'static',
                               keyboard: false,
                               resolve: {
                                   actualData: function () {
                                       $scope.scorecardData = {};
                                       $scope.scorecardData.scorecardId = $scope.scorecardId;
                                       $scope.scorecardData.isUserAuthorisedToEdit = $scope.isEditAuthorised;
                                       return $scope.scorecardData;
                                   },
                                   counterMeasure: function () {
                                       return selectedCounterMeasure;
                                   },
                                   metricDetails: null
                               }
                           }).result.then(function (updatedCounterMeasureRow) {
                               var index = $scope.counterMeasureData.indexOf(selectedCounterMeasure);
                               $scope.counterMeasureData[index] = updatedCounterMeasureRow;
                               $scope.counterMeasureData[index].isEditFromTable = true;

                               refreshGrid();
                           });
                       };
                   },
                   controllerAs: 'ctrl',
                   templateUrl: 'ngViews/kpIs/templates/counter-measure.tpl.min.html',
                   link: function (scope, elm, attr) {
                   }
               };
           }]);
});