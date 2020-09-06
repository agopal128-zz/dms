"use strict";
define(['angularAMD'], function (angularAMD) {

    angularAMD.directive('progressValue',  function () {
        return {
            scope: {
                dataValue: '=progressValue'
            },
            restrict: 'A',
            link: function (scope, element, attrs) {
                scope.$watch('dataValue', function () {
                    if (element.closest('.letter-item').length > 0) {
                        element.circleProgress({
                            value: scope.dataValue,
                            size: 44,
                            startAngle: (-Math.PI * 1.5),
                            fill: {
                                image: 'Images/Common/circle-mini.png'
                            },
                            emptyFill: 'transparent',
                            thickness: 8,
                        });
                    }
                    else if (element.closest('.kpi-letter-wrapper').length > 0) {
                        element.circleProgress({
                            value: scope.dataValue,
                            size: 66,
                            startAngle: (-Math.PI * 1.5),
                            fill: {
                                image: 'Images/Common/circle-mini-b.png'
                            },
                            emptyFill: 'transparent',
                            thickness: 8,
                        });
                    }
                });

            }
        };

    });
});