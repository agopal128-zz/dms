"use strict";
define(['angularAMD'], function (angularAMD) {
    angularAMD.directive('uiSelectWrap', ['$document', 'uiGridEditConstants',
        function ($document, uiGridEditConstants) {
            return {
                link: function ($scope, $ele, $attr) {
                    $document.on('click', docClick);
                    function docClick(evt) {
                        if ($(evt.target).closest('.ui-select-container').size() === 0) {
                            $scope.$emit(uiGridEditConstants.events.END_CELL_EDIT);
                            $document.off('click', docClick);
                        }
                    }
                }
            };
        }]);
});