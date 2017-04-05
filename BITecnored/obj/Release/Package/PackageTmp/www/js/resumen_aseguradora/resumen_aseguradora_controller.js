app.controller('ResumenAseguradoraController', function ($scope, $compile, ResumenAseguradoraService) {
    var myConfig = initChart();
    zingchart.render({
        id: 'myChart',
        data: myConfig
    });

    function initChart() {
        return {
            type: "area",
            stacked: true,
            title: {
                text: "Inspecciones por aseguradora",
                fontColor: "#424242",
                adjustLayout: true,
                marginTop: 15
            },
            subtitle: {
                text: "Sancor Seguros",
                fontColor: "#616161",
                adjustLayout: true,
                marginTop: 45
            },
            plot: {
                aspect: "spline",
                alphaArea: 0.6
            },
            plotarea: {
                margin: "dynamic"
            },
            tooltip: { visible: false },
            scaleY: {
                short: true,
                shortUnit: 'k',
                lineColor: "#AAA5A5",
                tick: {
                    lineColor: "#AAA5A5"
                },
                item: {
                    fontColor: "#616161",
                    paddingRight: 5
                },
                guide: {
                    lineStyle: "dotted",
                    lineColor: "#AAA5A5"
                },
                label: {
                    text: "Inspecciones",
                    fontColor: "#616161"
                }
            },
            scaleX: {
                lineColor: "#AAA5A5",
                labels: ["Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"],
                tick: {
                    lineColor: "#AAA5A5"
                },
                item: {
                    fontColor: "#616161",
                    paddingTop: 5
                },
                label: {
                    text: "2016",
                    fontColor: "#616161"
                }
            },
            crosshairX: {
                lineColor: "#AAA5A5",
                plotLabel: {
                    backgroundColor: "#EBEBEC",
                    borderColor: "#AAA5A5",
                    borderWidth: 2,
                    borderRadius: 2,
                    thousandsSeparator: ',',
                    fontColor: '#616161'
                },
                scaleLabel: {
                    backgroundColor: "#EBEBEC",
                    borderColor: "#AAA5A5",
                    fontColor: "#424242"
                }
            },
            series: [
                {
                    values: [3435, 3035, 2735, 2635, 2435, 2375, 2035, 1997, 2003, 1775, 1805, 1807],
                    text: "Inspecciones",
                    backgroundColor: "#2196F3",
                    lineColor: "#2196F3",
                    marker: {
                        backgroundColor: "#1565C0",
                        borderColor: "#1565C0"
                    }
                }
            ]
        };
    }
});