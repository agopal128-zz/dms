'use strict';

define(['angularAMD', 'angularRoute', 'bootstrap', 'uiBootstrap', 'uiBootstrapTpl', 'configService', 'sessionStorageService',
'notificationService', 'authService', 'serviceInterceptor', 'filterUrlService', 'scorecardService',
'notificationModalService', 'appConfig'],
    function (angularAMD) {
        var app = angular.module('loginApp', ['ngRoute', 'ui.bootstrap', 'ui.bootstrap.tpls']);

        app.run(['$rootScope', '$injector', function ($rootScope, $injector) {
            $rootScope.$on("$routeChangeSuccess", function (event, next, current) {
                //remove notification if any
                $('.notify').remove();
            });
        }]);

        // Adding an interceptor
        app.factory('ndmsServiceInterceptor', serviceInterceptor);

        app.config(['$routeProvider', '$locationProvider', '$httpProvider',
        function ($routeProvider, $locationProvider, $httpProvider) {
            $httpProvider.interceptors.push('ndmsServiceInterceptor');
        }]);

        app.controller('LoginController', ['sessionStorageService', '$window', '$filter', 
            '$rootScope', 'notificationService', 'authService', 'configService', 'scorecardService',
            function (sessionStorageService, $window, $filter, $rootScope, notificationService,
                authService, configService, scorecardService) {
                var self = this,
                base = configService.getAppUrl();

                self.loginData = {
                    username: '',
                    password: ''
                };
                self.isIE = /*@cc_on!@*/false || !!document.documentMode;
                self.isFullScreen = $window.screen.width == $window.innerWidth && ($window.screen.height - $window.innerHeight) <= 3;

                // check whether a role is available in logged-in user roles
                var isUserInRole = function (role, userRoles) {
                    return userRoles.indexOf(role) > -1 ? true : false;
                };

                // navigation to scorecard page
                var navigateToScorecard = function (id) {
                    $window.location.href = base + "Scorecard/" + id;
                };
                
                // navigation to hierarchy page
                var navigateToHierarchy = function () {
                    $window.location.href = base + "Hierarchy/0";
                };
                
                // navigation to unauthorized page 
                var navigateToUnauthorized = function () {
                    $window.location.href = base + "401.html";
                };

                // Get the scorecard id of the user and target status(set or not)
                var getScorecardAndTargetStatus = function () {
                    scorecardService.getScorecardAndTargetStatus().then(
                        function (data) {
                            if (data && data.id) {
                                sessionStorageService.set('scorecardId', data.id);
                                if (data.isTargetAvailable) {
                                    navigateToScorecard(data.id);
                                }
                                // If targets are not set on scorecard, navigate to hierarchy page
                                else {
                                    navigateToHierarchy();
                                }
                            }
                        },
                        function (err) {
                            if (err) {
                                notificationService.notify(err.error_description,
                                    { autoClose: false, type: "danger" });
                            }
                        });
                };

                // login functionality
                self.login = function (isValid) {
                    if (isValid) {
                        sessionStorageService.remove(AppConfig.AuthTokenKey);
                        authService.login(self.loginData.username, self.loginData.password).then(
                        function (data) {
                            var isAdmin = isUserInRole('Admin', data),
                            isKPIOwner = isUserInRole('KPIOwner', data),
                            isTeamMember = isUserInRole('TeamMember', data);
                            if (isAdmin) {
                                if (isKPIOwner || isTeamMember) {
                                    getScorecardAndTargetStatus();
                                }
                                else {
                                    navigateToHierarchy();
                                }
                            }
                            else if (isKPIOwner) {
                                getScorecardAndTargetStatus();
                            }
                            else if (isTeamMember) {
                                getScorecardAndTargetStatus();
                            }
                            else {
                                navigateToUnauthorized();
                            }
                        },
                        function (err) {
                            if (err && err.error_description) {
                                notificationService.notify(err.error_description,
                                    { autoClose: false, type: "danger" });
                            }
                        });
                    }
                };

                self.close = function () {
                    sessionStorageService.remove(AppConfig.AuthTokenKey);
                    sessionStorageService.remove('userData');
                    sessionStorageService.remove('scorecardId');
                    $window.close();
                };

                angular.element($window).on('resize', function () {
                    self.isIE = /*@cc_on!@*/false || !!document.documentMode;
                    self.isFullScreen = $window.screen.width == $window.innerWidth && ($window.screen.height - $window.innerHeight) <= 3;
                });
            }]);

        return angularAMD.bootstrap(app);
    });


