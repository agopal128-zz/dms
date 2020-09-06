'use strict';
define(['angularAMD', 'angularRoute', 'bootstrap', 'uiBootstrap', 'uiBootstrapTpl', 'interceptor',
        'configService', 'notificationService', 'notificationModalService', 'validationService',
        'validationDirective', 'sessionStorageService', 'authService', 'appConfig', 'appMenu',
        'appMenuDirective', 'appMenuConfig', 'appBehaviour', 'accordion', 'sglClick', 'ngTagsInput', 'toggleSwitch', 'scorecardDropdown',
        'enforceMaxTags', 'uiGrid', 'configurationService', 'confirmationModalService', 'utilityService','scorecardService',
        'gridDropdown', 'datePickerDirective', 'filterUrlService', 'isDataLoaded', 'kendo.all.min', 'monthNavigationService', 'highchart'],
    function (angularAMD) {
        var app = angular.module('novApp', ['ngRoute', 'ui.bootstrap', 'ui.bootstrap.tpls',
        'ngTagsInput', 'ui.grid', 'ui.grid.edit', 'ui.grid.validate', 'ui.grid.rowEdit', 'ui.grid.cellNav', 'kendo.directives']);

        app.controller('appController', ['sessionStorageService', '$window', '$filter', '$rootScope', '$scope','$location', 'notificationModalService',
            'notificationService', 'configurationService', 'authService', 'configService', 'confirmationModalService', 'utilityService', 'scorecardService', 'monthNavigationService',
            function (sessionStorageService, $window, $filter, $rootScope, $scope, $location, notificationModalService,
                notificationService, configurationService, authService, configService, confirmationModalService, utilityService, scorecardService, monthNavigationService) {
                var self = this,
                menuData = [],
                userResources = configService.getUIResources('User'),
                scorecardResources = configService.getUIResources('Scorecard'),
                baseUrl = configService.getAppUrl();
                self.isUserAuthorised = authService.isAuthorized();
                utilityService.loadExtentionMethods();

                //below line included to avoid unauthorization error occured due to
                //negotiate authentication in IE browser for intranet sites
                document.execCommand("ClearAuthenticationCache", false);

                var loadCurrentDateAndYearId = function () {
                    configurationService.getCurrentDateAndYearId().then(function (data) {
                        var currentDate = data.currentDate.slice(0, 10);
                        self.currentDate = $filter('date')(currentDate, 'dd MMM yyyy');
                        self.monthId = parseInt($filter('date')(currentDate, 'M'), 10);
                        self.yearId = data.yearId;
                        self.day = $filter('date')(currentDate, 'dd');
                    },
                    function (err) {
                        if (err) {
                            notificationService.notify(err.errors.join("<br/>"),
                            { autoClose: false, type: "danger" });
                        }
                    });
                };

               

                //loads the list of Available root nodes 
                var loadHierarchyDropdownList = function () {
                    scorecardService.getHierarchyDropdownList().then(function (data) {
                        self.dropDownOptions = scorecardService.hierarchyDropdownDto;
                        
                    }, function (err) {
                        notificationService.notify(err.errors.join("<br/>"),
                            { autoClose: false, type: "danger" });
                    });
                };

                // DOM ready function
                var init = function () {
                    configurationService.getAll()
                        .then(function (data) {
                            sessionStorageService.set('configData', JSON.stringify(data));
                            loadCurrentDateAndYearId();
                            configureMenuAndUI();
                            if (self.isUserAuthorised) {
                                loadHierarchyDropdownList();
                            }
                        },
                        function (err) {
                            if (err) {
                                notificationService.notify(err.errors.join("<br/>"),
                                 { autoClose: false, type: "danger" });
                            }
                        });
                }();

                // Configures menu and other UI labels
                var configureMenuAndUI = function () {
                    var userData = JSON.parse(sessionStorageService.get('userData'));

                    if (!(userData && userData.roles)) {
                        userData = {};
                        userData.roles = ['Anonymous'];
                    }

                    self.isIE = /*@cc_on!@*/false || !!document.documentMode;
                    self.isFullScreen = $window.screen.width == $window.innerWidth && ($window.screen.height - $window.innerHeight) <= 3;
                    self.menuData = new appMenu().get(userData.roles);
                    self.user = userData && userData.user ? userData.user : '';
                    self.fullName = userData && userData.fullName || userData.fullName === "" ?
                                    (userData.fullName.trim() !== '' ? userData.fullName : userData.user) : '';

                    $rootScope.title = 'Scorecard';
                    $rootScope.kpiOwners = [];
                    $rootScope.referrer = document.referrer;
                    $rootScope.isScorecardDropdownVisible = false;
                    $rootScope.isRecordableVisible = false;
                    $rootScope.showAllScorecards = false;

                };
               
                //sign in icon click
                self.login = function () {
                    $window.location.href = baseUrl + 'Login/';
                };

                //sign out icon click
                self.logout = function () {
                    var modalInstance = confirmationModalService.openConfirmationModal('md', userResources.LogoutConfirmation);
                    modalInstance.result.then(function () {
                        var id = sessionStorageService.get('scorecardId');
                        sessionStorageService.remove(AppConfig.AuthTokenKey);
                        sessionStorageService.remove('userData');
                        sessionStorageService.remove('scorecardId');
                        if (id) {
                            $window.location.href = baseUrl + 'Scorecard/' + id;
                        }
                        else {
                            $window.location.href = baseUrl + 'Login/';
                        }

                    });
                };
                // close button click
                self.close = function () {
                    sessionStorageService.remove(AppConfig.AuthTokenKey);
                    sessionStorageService.remove('userData');
                    sessionStorageService.remove('scorecardId');
                    $window.close();
                    $window.location.reload();
                };


                // back button click
                self.back = function () {
                    $window.history.back();
                };

                //to load previous month scorecard details
                self.loadPrevMonthData = function () {
                    if (!$rootScope.disableHeader) {
                        var isNavigate = monthNavigationService.setPreviousMonthDetails();
                        $rootScope.displayMonth = monthNavigationService.getSelectedMonthDetails();
                        if (isNavigate) $scope.$broadcast('monthChange');
                        if (!isNavigate) {
                            var modalInstance = notificationModalService.open({
                                header: "",
                                message: scorecardResources.MonthNavigationMinError
                            });
                        }
                    }
                }

                //to load next month scorecard details
                self.loadNextMonthData = function () {
                    if (!$rootScope.disableHeader) {
                        var isNavigate = monthNavigationService.setNextMonthDetails();
                        $rootScope.displayMonth = monthNavigationService.getSelectedMonthDetails();
                        if (isNavigate) $scope.$broadcast('monthChange');
                        if (!isNavigate) {
                            var modalInstance = notificationModalService.open({
                                header: "",
                                message: scorecardResources.MonthNavigationMaxError
                            });
                        }
                    }
                }
               
                self.toggleHierarchy = function (viewAllScorecards) {
                    $scope.$broadcast('hierarchyToggle', { showAllScorecards: viewAllScorecards });
                }

                self.changeSelectedScorcard = function (hierarchyOption) {
                    $scope.$broadcast('selectedHierarchyOption', hierarchyOption);
                }
               
                ////Watch to capture fullscreen change event
                angular.element($window).on('resize', function () {
                    self.isIE = /*@cc_on!@*/false || !!document.documentMode;
                    self.isFullScreen = $window.screen.width == $window.innerWidth && ($window.screen.height - $window.innerHeight) <= 3;
                });

            }]);

        app.config(['$routeProvider', '$locationProvider',
          function ($routeProvider, $locationProvider) {
              $routeProvider
                  .when('/Scorecard/:id?/:yearId?/:monthId?', angularAMD.route({
                      templateUrl: 'ngViews/dashboard/partials/scorecard-dashboard.min.html',
                      controller: 'scorecardDashboardController',
                      controllerAs: 'ctrl',
                      //controllerUrl: 'ngControllers/scorecard/scorecard-dashboard-controller',
                      menuKey: 'dashboard'
                  }))
                  .when('/Hierarchy/:default?', angularAMD.route({
                      templateUrl: 'ngViews/scorecard/partials/scorecard-hierarchy.min.html',
                      controller: 'ScorecardHierarchyController',
                      controllerAs: 'ctrl',
                      //controllerUrl: 'ngControllers/scorecard/scorecard-hierarchy-controller',
                      menuKey: 'hierarchy'
                  }))
                  .when('/ScorecardAdmin/:mode/:id/:parentId?', angularAMD.route({
                      templateUrl: 'ngViews/scorecard/partials/add-edit-scorecard.min.html',
                      controller: 'ScorecardAdminController',
                      controllerAs: 'ctrl',
                      //controllerUrl: 'ngControllers/scorecard/scorecard-admin-controller',
                      menuKey: 'scorecardadmin'
                  }))
                  .when('/Admin/AddMetrics', angularAMD.route({
                      templateUrl: 'ngViews/metrics/partials/add-edit-metrics.min.html',
                      controller: 'MetricsController',
                      controllerAs: 'ctrl',
                      //controllerUrl: 'ngControllers/metrics/metrics-controller',
                      menuKey: 'addmetrics'
                  }))
                  .when('/Admin/AssignMetrics', angularAMD.route({
                      templateUrl: 'ngViews/metrics/partials/assign-metrics.min.html',
                      controller: 'AssignMetricsController',
                      controllerAs: 'ctrl',
                      //controllerUrl: 'ngControllers/metrics/assign-metrics-controller',
                      menuKey: 'assignmetrics'
                  }))
                  .when('/Admin/HolidaySchedule', angularAMD.route({
                      templateUrl: 'ngViews/admin/partials/add-holiday-pattern.min.html',
                      controller: 'HolidayPatternController',
                      controllerAs: 'ctrl',
                      //controllerUrl: 'ngControllers/admin/holiday-pattern-controller',
                      menuKey: 'addholidaypattern'
                  }))
                  .when('/Admin/HolidaySchedule/:id', angularAMD.route({
                      templateUrl: 'ngViews/admin/partials/view-holiday-pattern.min.html',
                      controller: 'HolidaySelectorController',
                      controllerAs: 'ctrl',
                      //controllerUrl: 'ngControllers/admin/holiday-selector-controller',
                      menuKey: 'editholidays'
                  }))
                  .when('/Admin/ListMaintenance', angularAMD.route({
                      templateUrl: 'ngViews/admin/partials/add-organizational-data.min.html',
                      controller: 'OrganizationalDataController',
                      controllerAs: 'ctrl',
                      menuKey: 'manageorganization'
                  }))
                  .when('/Targets/Manage/:mode/:scorecardId/:parentId?', angularAMD.route({
                      templateUrl: 'ngViews/targets/partials/manage-target.min.html',
                      controller: 'ManageTargetController',
                      controllerAs: 'ctrl',
                      //controllerUrl: 'ngControllers/target/manage-target-controller',
                      menuKey: 'target'
                  }))
                  .when('/KPI/:kpiId/:scorecardId/:yearId/:monthId/:day', angularAMD.route({
                      templateUrl: 'ngViews/kpIs/partials/kpi.html',
                      controller: 'kpiController',
                      controllerAs: 'ctrl',
                      //controllerUrl: 'ngControllers/kpIs/kpi-controller',
                      menuKey: 'kpi'
                  }))
                  .when('/Drilldown/:scorecardId/:kpiId/:monthId/:yearId', angularAMD.route({
                      templateUrl: 'ngViews/kpIs/partials/drill-down.min.html',
                      controller: 'drillDownController',
                      controllerAs: 'ctrl',
                      //controllerUrl: 'ngControllers/kpIs/drill-down-controller',
                      menuKey: 'drilldown'
                  }))
                  .when('/NotFound', {
                      templateUrl: '404.html'
                  })
                  .otherwise({
                      redirectTo: '/NotFound'
                  });
              // use the HTML5 History API
              $locationProvider.html5Mode(true);

          }]);

        app.run(['$location', '$window', '$rootScope', '$injector', '$interval', 'sessionStorageService',
            'configService', '$document', 'notificationModalService', '$uibModalStack',
        function ($location, $window, $rootScope, $injector, $interval, sessionStorageService,
            configService, $document, notificationModalService, $uibModalStack) {

            var lastDigestRun = Date.now(),
            configInfo = JSON.parse(sessionStorageService.get('configData')),
            sessionTimeout = configInfo ? configInfo.sessionTimeout * 60 * 1000 : 30 * 60 * 1000,
            id = sessionStorageService.get('scorecardId'),
            token = sessionStorageService.get(AppConfig.AuthTokenKey),
            autoRefreshDuration = configInfo ? configInfo.autoRefreshDuration * 60 * 1000 : 5 * 60 * 1000,
            bodyElement = angular.element($document);

            angular.forEach(['keydown', 'keyup', 'click', 'DOMMouseScroll', 'mousemove',
            'mousewheel', 'mousedown', 'touchstart', 'touchmove', 'scroll', 'focus'],
                function (EventName) {
                    bodyElement.bind(EventName, function () {
                        lastDigestRun = Date.now();
                    });
                });

            var timeoutRedirection = function () {
                var base = configService.getAppUrl();
                if (id) {
                    $window.location.href = base + 'Scorecard/' + id;
                }
                else {
                    $window.location.href = base + 'Login/';
                }
            };

            var idleCheck = $interval(function () {
                var now = Date.now(),
                userResources = configService.getUIResources('User'),
                configInfo = JSON.parse(sessionStorageService.get('configData')),
                sessionTimeout = configInfo ? configInfo.sessionTimeout * 60 * 1000 : 30 * 60 * 1000;
                if (token && now - lastDigestRun >= sessionTimeout) {
                    $interval.cancel(idleCheck);
                    sessionStorageService.remove(AppConfig.AuthTokenKey);
                    sessionStorageService.remove('userData');
                    var modalInstance = notificationModalService.open({
                        header: "Timeout",
                        message: userResources.sessionTimeout
                    });
                    modalInstance.result.then(function () {
                        timeoutRedirection();
                    }, function () {
                        timeoutRedirection();
                    });
                }
            }, 30 * 1000);

            function autoRefresh() {
                if (!token) {
                    location.reload(true);
                    //var base = configService.getAppUrl();
                    //window.location.href = base + 'Scorecard/' + id;
                }
            }

            var autoRefresh = $interval(autoRefresh, autoRefreshDuration);



            $rootScope.$on("$routeChangeStart", function (event, next, current) {
                lastDigestRun = Date.now();
                // Check the menu keys(angular routes(, which does not need authentication
                if (!((next.menuKey === 'dashboard') || (next.menuKey === 'kpi'))) {
                    var authService = $injector.get('authService');
                    if (authService.isAuthorizedAndRedirect()) {
                        //remove notification if any
                        $('.notify').remove();
                    }
                }
                // modal dismiss on browser back button click
                var top = $uibModalStack.getTop();
                if (top) {
                    $uibModalStack.dismiss(top.key);
                    event.preventDefault();
                }
            });
            
            $rootScope.$on("$routeChangeSuccess", function (event, next, current) {

                if ((next.menuKey === 'dashboard') || (next.menuKey === 'kpi')) {
                    $rootScope.isScorecardDropdownVisible = true;
                    $rootScope.isRecordableVisible = true;
                    $rootScope.isMonthNavigationVisible = true;
                    $rootScope.isHierarchyDropdownVisible = false;
                }
                else {
                    $rootScope.isScorecardDropdownVisible = false;
                    $rootScope.isRecordableVisible = false;
                    //$rootScope.isMonthNavigationVisible = false;
                    $rootScope.isHierarchyDropdownVisible = (next.menuKey === 'hierarchy');
                    $rootScope.isMonthNavigationVisible = (next.menuKey === 'drilldown');
                }
                //remove notification if any
                $('.notify').remove();
                //AppMenu.select(next.menuKey);
            });

        }]);

        // Adding an interceptor
        app.factory('ndmsServiceInterceptor', serviceInterceptor);
        app.config(function ($httpProvider) {
            $httpProvider.interceptors.push('ndmsServiceInterceptor');
        });

        /*app.config(['$httpProvider', function ($httpProvider) {
            //$httpProvider.defaults.cache = false;
            //$httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
            //$httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
        }]);*/


        return angularAMD.bootstrap(app);
    });