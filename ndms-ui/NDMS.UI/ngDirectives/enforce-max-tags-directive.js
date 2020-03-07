"use strict";
define(['angularAMD'], function (angularAMD) {

    angularAMD.directive('enforceMaxTags', function () {
        return {
            restrict:'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModelController) {
                var maxTags = attrs.maxTags ? parseInt(attrs.maxTags, '10') : null;
                ngModelController.$validators.checkLength = function (value) {
                    if (value && maxTags && value.length > maxTags) {
                        value.splice(value.length - 1, 1);
                    }
                    return value;
                };
            }

        };

    });
});