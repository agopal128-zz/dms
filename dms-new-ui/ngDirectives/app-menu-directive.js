"use strict";
define(['angularAMD'], function (angularAMD) {

    angularAMD.directive('appMenu', function () {
        return {
            scope: {
                menu: '='
            },
            replace:true,
            templateUrl: 'ngTemplates/Common/AppMenu.tpl.min.html',
            controller: function ($scope) {
                var self = this;
                self.showChilds = function (index) {
                    $scope.menu[index].active = !$scope.menu[index].active;
                    //self.collapseAnother(index);
                };
                self.collapseAnother = function (index) {
                    for (var i = 0; i < $scope.menu.length; i++) {
                         if(i!=index){
                             $scope.menu[i].active = false;
                         }
                     }
                 };
            },
            controllerAs:"ctrl",

            link: function (scope, elem, attrs) {                            

                //scope.$watch('menu', function () {
                //    var data = scope.chartData;
                //    if (data.children && data.children.length > 0)
                //        CircularPackChart(data).draw(elem.find('svg'));
                //});
            }
            
        };

    });
});


