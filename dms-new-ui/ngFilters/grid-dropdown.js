'use strict';
define(['angularAMD'], function (angularAMD) {

    angularAMD.filter('griddropdown', function () {
        return function (input, map, idField, valueField, initial) {
            if (typeof map !== "undefined" && typeof input !== "undefined") {
                for (var i = 0; i < map.length; i++) {
                    if (map[i][idField] == input.id) {
                        return map[i][valueField];
                    }
                }
            } else if (initial) {
                return initial;
            }
            return input;
        };
    });
});
