"use strict";
define(['angularAMD'], function (angularAMD) {

    angularAMD.directive('metricGraph', function () {
        return {
            restrict: 'E',
            template: '<div></div>',
            scope: {
                chartData: "=value",
                chartObj: "=?",
                page: "=",
            },
            transclude: true,
            replace: true,
            link: function ($scope, $element, $attrs) {

                // Update when charts data changes
                $scope.$watch('chartData', function (value) {
                    if (!value) {
                        return;
                    }
                    if ($scope.chartData.series) {
                        if ($scope.chartData.xAxis && $scope.chartData.xAxis.trackingMethode) {
                            var tooltipText = ($scope.chartData.xAxis.trackingMethode == 'daily') ? 'Day: ' : 'Month: ';
                        }
                        var monthList = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
                        var graphData = {
                            chart: {
                                type: 'area',
                                marginLeft: 65,
                                marginRight: 1,
                                marginBottom: 40,
                                plotBackgroundColor: '#FFF',
                                backgroundColor: 'transparent',
                                plotBorderWidth: 1,
                                plotBorderColor: '#CCC',
                                animation: false,
                                renderTo: $element[0]

                            },
                            legend: {
                                enabled: true,
                                align: 'right',
                                verticalAlign: 'top',
                                layout: 'horizontal',
                                backgroundColor: '#FFF',
                                borderColor: '#FFF',
                                borderWidth: 1,
                                symbolHeight: 20,
                                symbolWidth:20
                            },
                            title: {
                                text: ''
                            },
                            credits: {
                                enabled: false
                            },
                            xAxis: {
                                min: 0,

                                categories: [],
                                title: {
                                    margin: 0,
                                },
                                tickInterval: 1,
                                tickmarkPlacement: 'on',
                                gridLineWidth: 1,
                                gridLineColor: '#bbb',
                                alternateGridColor: '#FFF',
                                gridZIndex: 1,
                                startOnTick: true,
                                endOnTick: false,
                                minPadding: 0,
                                maxPadding: 0,
                                lineWidth: 1
                            },
                            yAxis: {
                                // min: 0,
                                title: {
                                    margin: 10,
                                    text: 'Target'
                                },
                                labels: {
                                    formatter: function () {
                                        return this.value;
                                    }
                                },
                                gridLineWidth: 1,
                                gridLineColor: '#ddd',
                                lineWidth: 1,
                                tickInterval: 10
                            },
                            plotOptions: {
                                series: {
                                    states: {
                                        hover: ''
                                    },
                                },
                                area: {
                                    //pointStart: 5,
                                    marker: {
                                        enabled: true,
                                        symbol: 'circle',
                                        radius: 4,
                                        lineWidth: 2,
                                        lineColor: '#FFF',
                                        states: {
                                            hover: {
                                                enabled: true
                                            }
                                        }
                                    }
                                }
                            },
                            series: [
                               {
                                   animation: false,
                                   fillOpacity: 0.4,
                                   name: 'Actual',
                                   color: "#5F8605",
                                   data: [],
                                   connectNulls: true
                               },
                                {

                                    animation: false,
                                    fillOpacity: 0.4,
                                    name: 'Stretch Goal',
                                    color: "#61DDD3",
                                    data: [],
                                    connectNulls: true
                                },
                                {
                                    animation: false,
                                    fillOpacity: 0.4,
                                    name: 'Goal',
                                    color: "#31ABEA",
                                    data: [],
                                    connectNulls: true
                                }
                            ]
                        };

                        graphData.series[0].data = $scope.chartData.series[0].data;
                        graphData.series[2].data = $scope.chartData.series[2].data;
                        graphData.series[1].data = $scope.chartData.series[1].data;
                        graphData.yAxis.min = $scope.chartData.yAxis.min;
                        graphData.yAxis.max = $scope.chartData.yAxis.max;
                        graphData.xAxis.categories = $scope.chartData.xAxis.categories;
                        graphData.yAxis.title.align = 'high';
                        graphData.yAxis.title.offset = -15;
                        graphData.yAxis.title.rotation = 0;
                        graphData.yAxis.title.y = -10;
                        graphData.xAxis.min = graphData.xAxis.min;
                        graphData.xAxis.max = $scope.chartData.xAxis.max;
                        graphData.xAxis.labels = {
                            formatter: function () {
                                return ($scope.chartData.xAxis && $scope.chartData.xAxis.trackingMethode && $scope.chartData.xAxis.trackingMethode == 'daily') ? this.value : monthList[this.value - 1];
                            }
                        };
                        graphData.tooltip = {
                            formatter: function () {
                                if ($scope.chartData.xAxis && $scope.chartData.xAxis.trackingMethode && $scope.chartData.xAxis.trackingMethode == 'daily') {
                                    return tooltipText + this.x + '<br>' + this.series.name + ': ' + this.y;
                                }
                                else {
                                    return tooltipText + monthList[this.x - 1] + '<br>' + this.series.name + ': ' + this.y;
                                }
                            }
                        };
                        if ($scope.chartData.xAxis && $scope.chartData.xAxis.trackingMethode) {
                            graphData.xAxis.title.text = ($scope.chartData.xAxis.trackingMethode == 'daily') ? 'Days' : 'Months';
                        }

                        if ($scope.page == 'scorecard') {
                            graphData.tooltip.enabled = false;
                            graphData.tooltip.formatter = '';
                            graphData.plotOptions.area.marker.states.hover.enabled = false;
                            graphData.legend.enabled = false;
                            graphData.chart.marginLeft = 45;
                            graphData.chart.marginRight = 10;
                            graphData.chart.marginBottom = 40;
                            graphData.chart.marginTop = 20;
                            graphData.plotOptions.area.marker.enabled = false;
                            graphData.plotOptions.series.states.hover = false;
                            graphData.xAxis.labels = {
                                formatter: function () {
                                    return this.value;
                                }
                            };
                            graphData.yAxis.labels = {
                                formatter: function () {
                                    if (this.value >= graphData.yAxis.max || this.value <= graphData.yAxis.min) {
                                        return this.value;
                                    }
                                    else {
                                        return null;
                                    }
                                }
                            };
                        }
                        $scope.chartObj = new Highcharts.Chart(graphData);
                    }
                }, true);
            }
        };
    });
});
