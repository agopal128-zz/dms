
'use strict';
require.config({
    waitSeconds: 0,
    urlArgs : 'ndms=v1.0',
    paths: {
        bootstrap: 'Scripts/bootstrap.min',       
        jquery: 'Scripts/jquery-1.10.2.min',
        angular: 'Scripts/angular.min',        
        angularRoute: 'Scripts/angular-route.min',
        uiBootstrap: 'Scripts/angular-ui/ui-bootstrap.min',
        uiBootstrapTpl: 'Scripts/angular-ui/ui-bootstrap-tpls.min',
        sanitize: 'Scripts/angular-sanitize.min',
        ngTagsInput: 'Scripts/ng-tags-input.min',
        uiGrid: 'Scripts/ui-grid.min',
        uiSelect: 'Scripts/select.min',
        angularAMD: 'Scripts/angularAMD.min',
        d3js: 'Scripts/d3/d3.min',
        highchart: 'Scripts/highchart/highcharts',
        'kendo.all.min': 'Scripts/kendo/kendo.all.min',

        appMenuConfig: 'ngApp/app-menu.config.min',
        app: 'ngApp/app.min',
        appConfig: 'ngApp/app.config',

        accordion: 'UIBehaviours/accordion.min',
        appMenu: 'UIBehaviours/appMenu.min',
        appBehaviour: 'UIBehaviours/appBehaviour.min',

        constants: 'Common/constants.min',
        validationHelper: 'Common/validationHelpers.min',

        /*register Directives - Start*/
        appMenuDirective: 'ngDirectives/app-menu-directive.min',
        calendar: 'ngDirectives/calendar-directive.min',
        counterMeasure: 'ngDirectives/counter-measure-directive.min',
        datePickerDirective: 'ngDirectives/datePickerDirective.min',
        enforceMaxTags: 'ngDirectives/enforce-max-tags-directive.min',
        graphDirective: 'ngDirectives/graph-directive.min',
        hierarchyChartDirective: 'ngDirectives/hierarchy-chart-directive.min',
        ionSlider: 'ngDirectives/slider.min',
        isDataLoaded: 'ngDirectives/is-data-loaded-directive.min',
        kpiLetter: 'ngDirectives/kpi-letter-directive.min',
        monthlyPlan: 'ngDirectives/monthly-plan-directive.min',
        sglClick: 'ngDirectives/sgl-click-directive.min',    
        toggleSwitch: 'ngDirectives/toggle-switch-directive.min',
        uiSelectWrap: 'ngDirectives/ui-select-wrap.min',
        validNumber: 'ngDirectives/valid-number-directive.min',
        validationDirective: 'ngDirectives/ValidationDirective.min',
        viewTarget: 'ngDirectives/view-target-directive.min',       
        workdayPattern: 'ngDirectives/workday-pattern-directive.min',
        fullYearCalendar: 'ngDirectives/full-year-calendar-directive.min',
        /*register Directives - End*/

        /*register Services - Start*/
        authService: 'ngServices/Common/authService.min',
        configService: 'ngServices/Common/configService.min',
        confirmationModalService: 'ngServices/Common/confirmationModalService.min',
        filterUrlService: 'ngServices/Common/filterUrlService.min',
        interceptor: 'ngServices/Common/ndmsServiceInterceptor.min',
        notificationModalService: 'ngServices/Common/notificationModalService.min',
        notificationService: 'ngServices/Common/notificationService.min',
        sessionStorageService: 'ngServices/Common/sessionStorageService.min',
        utilityService: 'ngServices/Common/utilityService.min',
        validationService: 'ngServices/Common/validationService.min',        
        actualsService: "ngServices/actuals-service.min",
        configurationService: "ngServices/configuration-service.min",
        holidayPatternService: "ngServices/holiday-pattern-service.min",
        kpiService: "ngServices/kpi-service.min",
        metricsService: "ngServices/metrics-service.min",
        monthNavigationService: "ngServices/Common/monthNavigationService.min",
        organizationalDataService: "ngServices/organizational-data-service.min",
        scorecardService: "ngServices/scorecard-service.min",
        targetService: "ngServices/target-service.min",
        /*register Services - End*/

        /*register Filters - Start*/
        gridDropdown:'ngFilters/grid-dropdown.min',
        metricType: 'ngFilters/metric-type.min',
        titleCase: 'ngFilters/title-case.min',
        /*register Filters - End*/
        
        /*register Controllers - Start*/
        actualsController: 'ngControllers/actuals/actuals-controller.min',
        confirmationModalController: 'ngControllers/Common/confirmationModalController.min',        
        counterMeasureController: 'ngControllers/counter-measure/counter-measure-controller.min',
        drillDownController: 'ngControllers/kpIs/drill-down-controller.min',
        kpiController: 'ngControllers/kpIs/kpi-controller.min',
        secondaryMetricKpiController: 'ngControllers/kpIs/secondary-metric-kpi-controller.min',
        AssignMetricsController: 'ngControllers/metrics/assign-metrics-controller.min',
        MetricsController: 'ngControllers/metrics/metrics-controller.min',
        ScorecardAdminController: 'ngControllers/scorecard/scorecard-admin-controller.min',
        scorecardDashboardController: 'ngControllers/scorecard/scorecard-dashboard-controller.min',
        ScorecardHierarchyController: 'ngControllers/scorecard/scorecard-hierarchy-controller.min',
        copyTargetController: 'ngControllers/target/copy-target-controller.min',
        dailyTargetController: 'ngControllers/target/daily-targets-tpl-controller.min',
        ManageTargetController: 'ngControllers/target/manage-target-controller.min',
        HolidayPatternController: 'ngControllers/admin/holiday-pattern-controller.min',
        HolidaySelectorController: 'ngControllers/admin/holiday-selector-controller.min',
        OrganizationalDataController: 'ngControllers/admin/organizational-data-controller.min',
        /*register Controllers - End*/

        /*register  Helper files - start*/
        rangeSlider: 'ngControllers/actuals/modules/ion.rangeSlider.min',
        dashboardHelper: 'ngControllers/scorecard/modules/dashboard-helper.min',
        hierarchyChartHelper: 'ngControllers/scorecard/modules/hierarchy-chart-helper.min',
        /*register  Helper files - End*/

    },

    // specifying library dependencies
    shim: {
        'bootstrap':{ deps:['jquery']},
        'angular': { deps: ['jquery'], exports: 'angular' },
        'angularRoute': { deps: ['angular'] },
        'uiBootstrap': { deps: ['angular'] },
        'uiBootstrapTpl': { deps: ['angular'] },
        'sanitize': { deps: ['angular'] },
        'ngTagsInput': { deps: ['angular'] },
        'angularAMD': { deps: ['angular'] },
        'uiGrid': { deps: ['angular'] },
        'rangeSlider': { deps: ['jquery','angular'] },
        'appBehaviour': { deps: ['jquery'] },
        'kendo.all.min':  {deps: ['angular'] },
    },

    // define application bootstrap
    deps: ['app']
});

