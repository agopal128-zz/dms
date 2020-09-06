'use strict';
define(['angularAMD'], function (angularAMD) {

    angularAMD.service('authService', ['sessionStorageService', 'configService', '$window',
        '$http', '$q', '$httpParamSerializerJQLike',
        function (sessionStorageService, configService, $window,$http, $q, $httpParamSerializerJQLike) {

            var self = this,
            baseUrl = configService.getApiBaseUrl() + "Authorization/";
            // Check if the current session is authorized by checking whether
            // access token is present in session storage
            this.isAuthorized = function () {
                if (sessionStorageService.get(AppConfig.AuthTokenKey) === null ||
                sessionStorageService.get(AppConfig.AuthTokenKey) === undefined ||
                sessionStorageService.get(AppConfig.AuthTokenKey) === '') {
                    return false;
                }
                return true;
            };


            this.isAuthorizedAndRedirect = function () {
                if (sessionStorageService.get(AppConfig.AuthTokenKey) === null ||
                sessionStorageService.get(AppConfig.AuthTokenKey) === undefined ||
                sessionStorageService.get(AppConfig.AuthTokenKey) === '') {
                    var base = configService.getAppUrl();
                    $window.location.href = base + 'Login';
                }
                return true;
            };

            // Set the authentication header
            this.setAuthHeader = function (config) {
                var token = sessionStorageService.get(AppConfig.AuthTokenKey);
                if (token) {
                    config.headers['Authorization'] = 'Bearer ' + token;
                }
            };

            // Performs the login 
            this.login = function (username, password) {
                var loginData = {
                    'grant_type': 'password',
                    'username': username,
                    'password': password
                };
                var deferred = $q.defer();
                var tokenEndpoint = configService.getApiBaseUrl().replace('/api', '') + "token";
                $http({
                    url: tokenEndpoint,
                    method: 'POST',
                    data: $httpParamSerializerJQLike(loginData),
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    }
                }).success(function (data) {
                    if (data) {
                        var roles = [];
                        roles = data.role.split(",");                       
                        var userData = {
                            "user": data.user,
                            "fullName": data.fullname,
                            "roles": roles
                        };
                        sessionStorageService.set('userData', JSON.stringify(userData));
                        sessionStorageService.set(AppConfig.AuthTokenKey, data.access_token);
                        deferred.resolve(userData.roles);
                    }
                }).error(function (msg, code) {
                    deferred.reject(msg);
                });
                return deferred.promise;
            };

            // Performs logout by clearing the authentication token
            this.logout = function () {
                sessionStorageService.remove(AppConfig.AuthTokenKey);
                sessionStorageService.remove('userData');
            };

        }]);

});