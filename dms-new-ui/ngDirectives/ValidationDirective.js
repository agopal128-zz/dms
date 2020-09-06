"use strict";
define(['angularAMD'], function (angularAMD) {
  
    angularAMD.directive('validationInjector', function ($compile) {
        return {
            restrict: 'A',
            scope: {
                validationRules: '='
            },
            link: function (scope, elem, attrs) {
                var rules = scope.validationRules;
                for (var i = 0; i < rules.length; i++) {
                    attrs.$set(rules[i].type, rules[i].value);
                    attrs.$set('data-' + rules[i].type + '-message', rules[i].message);
                }
            }
        };
    });

});


