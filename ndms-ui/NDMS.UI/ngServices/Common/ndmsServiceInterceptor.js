'use strict';

var serviceInterceptor = ['$window', '$q', '$injector', function ($window, $q, $injector) {
    // Prepares the message based HTTP Verb
    var getMessage = function (verb) {
        switch (verb) {
            case "GET":
                return "Loading";
            case "POST":
                return "Saving";
            case "PUT":
                return "Updating";
            case "DELETE":
                return "Deleting";
            default:
                return "Loading";
        }
    };

    return {
        request: function (config) {
            var filterUrlSvc = $injector.get('filterUrlService');
            // Check whether the url requested needs authorization,
            // if so set the authentication header
            if (filterUrlSvc.needAuthorization(config.url)) {
                config.headers = config.headers || {};
                // If the user is authorized set the authentication header with
                // token
                var authService = $injector.get('authService');
                authService.setAuthHeader(config);
            }
            var notificationService = $injector.get('notificationService');
            if (config.url.indexOf('/token') != -1) {
                notificationService.notify("<span class='glyphicon glyphicon-refresh'/> " +
                "...Please wait...", { type: "info", autoClose: false });
            } else {
                notificationService.notify("<span class='glyphicon glyphicon-refresh'/> " +
                    getMessage(config.method) +
                "...Please wait...", { type: "info", autoClose: false });
            }
            //$('#ajaxSpinner').height($(document).height());
            //$('#ajaxSpinner').show();
            return config || $q.when(config);
        },

        requestError: function (rejection) {
            // $('#ajaxSpinner').hide();
            return $q.reject(rejection);
        },

        response: function (response) {
            //$('#ajaxSpinner').hide();
            var $http = $http || $injector.get('$http');
            if (response.data.hasError && !response.data.isBusinessValidation) {
                var notificationService = $injector.get('notificationModalService');
                notificationService.open({
                    header: "Error",
                    message: response.data.errorMessage
                });
                return $q.reject(response);
            }
            else if (response.data.hasError && response.data.isBusinessValidation) {
                response.data.errorMessage = response.data.errorMessage.replace(
                    new RegExp('newline', 'g'), '<br/>');
            }

            if ($http.pendingRequests.length < 1) {
                var notificationService = $injector.get('notificationService');
                notificationService.close();
            }

            return response || $q.when(response);
        },

        responseError: function (rejection) {
            var notificationService = $injector.get('notificationModalService');

            if (rejection.status === 401) {
                var configService = $injector.get('configService');
                var base = configService.getAppUrl();
                $window.location.href = base + '401.html';
                return $q.reject(rejection);
            }
            else if (rejection.status === -1) {
                notificationService.open({
                    header: "Error",
                    message: "Oops...something is not right! Kindly contact your system administrator."
                });
            }

            //notificationService.open({ header: "Error", message: rejection.data.errors.join("<br/>") });
            return $q.reject(rejection);
        }
    };

}];