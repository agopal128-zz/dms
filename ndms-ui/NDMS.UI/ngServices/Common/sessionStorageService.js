'use strict';
define(['angularAMD'], function (angularAMD) {

    angularAMD.service('sessionStorageService', function () {
        // Retrieves an item based on key
        this.get = function (key) {
            return sessionStorage.getItem(key);
        };

        // Add an item with key
        this.set = function (key, value) {
            sessionStorage.setItem(key, value);
        };

        // Remove an item with key
        this.remove = function (key) {
            sessionStorage.removeItem(key);
        };

    });

});