"use strict";
define(['angularAMD'], function (angularAMD) {
    angularAMD.directive('validNumber', function () {
        return {
            restrict: 'A',
            require: '?ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {

                //8:"BACKSPACE",9:"TAB",17:"CTRL",18:"ALT",37:"LEFT",
                //38:"UP",39:"RIGHT",40:"DOWN",45:"INSERT",46:"DELETE",
                //48:"0",49:"1",50:"2",51:"3",52:"4",53:"5",54:"6",55:"7",56:"8",57:"9", 67:"C",86:"V",
                //96:"0",97:"1",98:"2",99:"3",100:"4",101:"5",102:"6",103:"7",104:"8",105:"9",
                //109:"-",110:".",144:"NUMLOCK", 189:"-",190:".",

                var decimalKeyCodes = [8, 9, 17, 37, 38, 39, 40, 45, 46, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 67, 86, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 109, 110, 144, 189, 190];
                var wholeNumberKeyCodes = [8, 9, 17, 37, 38, 39, 40, 45, 46, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 67, 86, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105, 144];
                element.bind("keydown", function (event) {
                    switch (attrs.validNumber) {
                        case "decimal": {
                            if ($.inArray(event.which, decimalKeyCodes) == -1) {
                                scope.$apply(function () {
                                    event.preventDefault();
                                });
                                event.preventDefault();
                            }
                        }
                            break;
                        case "wholeNumber": {
                            if ($.inArray(event.which, wholeNumberKeyCodes) == -1) {
                                scope.$apply(function () {
                                    event.preventDefault();
                                });
                                event.preventDefault();
                            }
                        }
                            break;
                        default: {
                            if ($.inArray(event.which, decimalKeyCodes) == -1) {
                                scope.$apply(function () {
                                    event.preventDefault();
                                });
                                event.preventDefault();
                            }
                        }
                            break;
                    }

                   
                });
            }
        };
    });
});

