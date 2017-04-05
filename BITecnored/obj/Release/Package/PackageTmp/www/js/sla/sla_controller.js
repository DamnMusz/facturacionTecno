app.directive('knob', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            var $el = $(element);
            $el.knob();
            scope.$watch(attrs.model, function (number) {
                $el.val(number).change();
            });
        }
    };
});

app.controller('SLAController', function ($scope, SLAService, $window) {
    $scope.loading_tiempos_previas = false;
    $scope.realizadas_centro = 0;
    $scope.realizadas_domicilio = 0;
    $scope.proporcion_realizadas = [];
    $scope.motivos_sin_efecto = "";
    $scope.cantidad_de_fotos =
        {
            promedio_total: 0,
            valores:
            [{
                texto: "",
                cantidad: 0,
                promedio: 0
            }]
        };
    $scope.motivos_dia_pactado = [{
        nombre: '',
        total: 0,
        realizadas: 0,
        en_gestion: 0,
        sin_efecto: 0,
    }];

    $scope.motivos_dia_pactado_suma_total = function () {
        var suma = 0;
        for (var i = 0; i < $scope.motivos_dia_pactado.length; i++) {
            suma += Number($scope.motivos_dia_pactado[i].total);
        }
        return suma;
    }

    $scope.motivos_dia_pactado_suma_realizadas = function () {
        var suma = 0;
        for (var i = 0; i < $scope.motivos_dia_pactado.length; i++) {
            suma += Number($scope.motivos_dia_pactado[i].realizadas);
        }
        return suma;
    }

    $scope.motivos_dia_pactado_suma_en_gestion = function () {
        var suma = 0;
        for (var i = 0; i < $scope.motivos_dia_pactado.length; i++) {
            suma += Number($scope.motivos_dia_pactado[i].en_gestion);
        }
        return suma;
    }

    $scope.motivos_dia_pactado_suma_sin_efecto = function () {
        var suma = 0;
        for (var i = 0; i < $scope.motivos_dia_pactado.length; i++) {
            suma += Number($scope.motivos_dia_pactado[i].sin_efecto);
        }
        return suma;
    }

    $scope.total_motivos = function () {
        var total = 0;
        for (var i = 0; i < $scope.motivos_sin_efecto.length; i++) {
            total += Number($scope.motivos_sin_efecto[i].cantidad);
        }
        return total;
    }

    $scope.fotos_total_ip = function () {
        var total = 0;
        for (var i = 0; i < $scope.cantidad_de_fotos.valores.length; i++) {
            total += Number($scope.cantidad_de_fotos.valores[i].cantidad);
        }
        return total;
    }

    $scope.lista_aseguradoras = [];
    $scope.lista_provincias = [];
    $scope.mostrar = false;
    $scope.res_ok = true;

    $scope.filtro = {
        fechaDesde: "",
        fechaHasta: "",
        mes: $scope.padding(((new Date()).getMonth()),2)+"/"+((new Date()).getFullYear()),
        aseguradora_id: "",
        aseguradora: "",
        provincia: ""
    };

    $scope.tiempo_gestion = { "cant_insp_pub": { "en_24hs": 0, "en_48hs": 0, "en_72hs": 0, "mas_de_72hs": 0, "mismo_dia": 0 }, "cant_sol_insp": { "en_24hs": 0, "en_48hs": 0, "en_72hs": 0, "mas_de_72hs": 0, "mismo_dia": 0 }, "prom_insp_pub": { "en_24hs": 0.0, "en_48hs": 0.0, "en_72hs": 0.0, "mas_de_72hs": 0.0, "mismo_dia": 0.0 }, "prom_sol_insp": { "en_24hs": 0, "en_48hs": 0, "en_72hs": 0, "mas_de_72hs": 0, "mismo_dia": 0 } };
    $scope.totalDeInspeccionAInforme = 0;
    $scope.totalDeSolicitudAInspeccion = 0;
    
    $scope.buscar = function () {
        SLAService.existsSLA($scope.filtro.aseguradora.id, $scope.filtro.mes).then(function (res) {
            if (res.data.exist == true)
                $scope.doBuscar();
            else
                $scope.alert_error('No existen datos para el per\u00edodo y aseguradora seleccionados (Utilice \'Actualizar Per\u00edodo\' para armar la base de consultas).')
        });
    }

    $scope.doBuscar = function () {
        $scope.mostrar = false;
        if ($scope.filtro.provincia == 'undefined' || $scope.filtro.provincia == "") {
            $scope.filtro.provincia = $scope.lista_provincias[0];
        }
        $scope.filtro.fechaDesde = $scope.mesBaseSLA;
        $scope.filtro.fechaHasta = $scope.mesBaseSLA;
        $scope.getTiempoGestion();
        $scope.getProporcionRealizadas(2);
        $scope.getMotivosNoRealizadas();
        $scope.getCantidadFotos();
        $scope.getMotivosDiaPactado();
        $scope.get_realizadas_centro();
        $scope.get_realizadas_domicilio();
    };

    $scope.cambiarAseguradora = function (aseguradora) {
        $scope.mostrar = false;
        $scope.filtro.aseguradora = aseguradora;
    }

    $scope.cambiarProvincia = function (aseguradora) {
        $scope.mostrar = false;
        $scope.filtro.provincia = aseguradora;
    }

    $scope.getAseguradoras = function () {
        SLAService.getAseguradoras().then(function (res) {
            $scope.lista_aseguradoras = $.parseJSON(res.data);
            $scope.lista_aseguradoras.unshift({
                codigo: "---",
                id:-1,
                nombre:"Todas"
            });
        });
    };

    $scope.getProvincias = function () {
        SLAService.getProvinciasFiltro().then(function (res) {
            $scope.lista_provincias = res.data;
            $scope.lista_provincias.unshift({
                id: -1,
                value: "Todas"
            });
        });
    };

    $scope.getTiempoGestion = function () {
        if ($scope.filtro.aseguradora != 'undefined' && $scope.filtro.aseguradora != "") {
            $scope.loading_tiempos_previas = true;
            SLAService.getTiempoGestion($scope.filtro.aseguradora.id, $scope.filtro.mes, $scope.filtro.provincia.id).then(function (res) {
                $scope.tiempo_gestion = res.data;
                $scope.mostrar = true;
                $scope.loading_tiempos_previas = false;
            });
        }
        else {
            $scope.alert_error('Seleccione una aseguradora.');
        }
    };

    $scope.getProporcionRealizadas = function (cant_meses) {
        SLAService.getProporcionRealizadas($scope.filtro.aseguradora.id, $scope.filtro.mes, cant_meses, $scope.filtro.provincia.id).then(function (res) {
            $scope.proporcion_realizadas = $.parseJSON(res.data);

            zingchart.exec('myChart0', 'setseriesvalues', {
                values : [
                    [$scope.proporcion_realizadas[0].realizadas],
                    [$scope.proporcion_realizadas[0].sin_efecto]
                ]
            });

            zingchart.exec('myChart1', 'setseriesvalues', {
                values: [
                    [$scope.proporcion_realizadas[1].realizadas],
                    [$scope.proporcion_realizadas[1].sin_efecto]
                ]
            });

            zingchart.exec('myChart2', 'setseriesvalues', {
                values: [
                    [$scope.proporcion_realizadas[2].realizadas],
                    [$scope.proporcion_realizadas[2].sin_efecto]
                ]
            });

            zingchart.exec('myChart0', 'reload');
            zingchart.exec('myChart1', 'reload');
            zingchart.exec('myChart2', 'reload');

            return true;
        });
    };

    function initChart() {
        return {
            globals: {
                shadow: false,
                fontFamily: "Verdana",
                fontWeight: "100"
            },
            type: "pie",
            backgroundColor: "#fff",

            legend: {
                layout: "x1",
                position: "0%",
                borderColor: "transparent",
                marker: {
                    borderRadius: 10,
                    borderColor: "transparent"
                }
            },
            tooltip: {
                text: "%v inspecciones"
            },
            plot: {
                refAngle: "-90",
                borderWidth: "3px",
                valueBox: {
                    placement: "in",
                    text: "%npv %",
                    fontSize: "15px",
                    textAlpha: 1,
                }
            },
            series: [{
                text: "Realizadas",
                values: [0],
                backgroundColor: "#66BB6A #66BB6A",
            }, {
                text: "No Realizadas",
                values: [0],
                backgroundColor: "#D32F2F #D32F2F"
            }]
        };
    }

    $scope.myJson0 = initChart();
    $scope.myJson1 = initChart();
    $scope.myJson2 = initChart();

    $scope.compare_motivos = function (a, b) {
        return b.cantidad - a.cantidad;
    };

    $scope.compare_dia_pactado = function (a, b) {
        return b.total - a.total;
    };

    $scope.getMotivosNoRealizadas = function () {
        SLAService.getMotivosNoRealizadas($scope.filtro.aseguradora.id, $scope.filtro.mes,$scope.filtro.provincia.id).then(function (res) {
            $scope.motivos_sin_efecto = $.parseJSON(res.data);
            $scope.motivos_sin_efecto.sort($scope.compare_motivos);
            return true;
        });
    };

    $scope.getMotivosDiaPactado = function () {
        SLAService.getMotivosDiaPactado($scope.filtro.aseguradora.id, $scope.filtro.mes, $scope.filtro.provincia.id).then(function (res) {
            $scope.motivos_dia_pactado = res.data;
            $scope.motivos_dia_pactado.sort($scope.compare_dia_pactado);
            return true;
        });
    };

    $scope.getCantidadFotos = function () {
        SLAService.getCantidadFotos($scope.filtro.aseguradora.id, $scope.filtro.mes, $scope.filtro.provincia.id).then(function (res) {
            $scope.cantidad_de_fotos = res.data;
            return true;
        });
    };

    $scope.suma = function (n1, n2) {
        $scope.acum = (parseFloat(n1) + parseFloat(n2)).toFixed(1);
        return $scope.acum;
    }

    $scope.configure_datepicker = function () {
        $(function () {
            var year = (new Date).getFullYear();
            var month = (new Date).getMonth();
            $('#fechasSLA').datepicker({
                viewMode: 'months',
                minViewMode: 'months',
                autoclose: true,
                startDate: new Date(2003, 0, 1),
                endDate: new Date(year, month - 1, 01)
            }
            );
        });
        $(function () {
            var year = (new Date).getFullYear();
            var month = (new Date).getMonth();
            $('#fechasBase').datepicker({
                viewMode: 'months',
                minViewMode: 'months',
                autoclose: true,
                startDate: new Date(2003, 0, 1),
                endDate: new Date(year, month - 1, 01)
            }
            );
        });
    }

    $scope.getAseguradoras();
    $scope.getProvincias();
    $scope.configure_datepicker();

    //---------------------------------------------------------------------------------

    $scope.mesBaseSLA = "";
    $scope.sla_generator_in_progress = false;
    $scope.sla_generator_in_progress_text = 'Generando bases para el per\u00edodo.';
    $scope.is_interval_running = false;
    $scope.refreshIntervalId = "";

    $scope.cancelBaseSLA = function () {
        $scope.sla_generator_in_progress_text = 'Cancelando...';
        SLAService.cancelBaseSLA().then(function (res) {});
    }

    $scope.getBaseSLA = function () {
        $scope.getSlaGeneratorState();
        SLAService.getBaseSLA($scope.mesBaseSLA).then(function (res) {
            if (res.status == 200) {
                $scope.alert_success("Proceso finalizado.");
            }
        }).catch(function (res) {
            if (res.status == 302) {
                $scope.alert_warning("Ya existe la base.");
            }
        });
        $scope.getSlaGeneratorState();
    }

    $scope.getSlaGeneratorState = function () {
        SLAService.getEstadoBaseSLA().then(function (res) {
            if (res.data.estado == "RUNNING") {
                $scope.sla_generator_in_progress_text = 'Generando bases para el per\u00edodo ' + res.data.periodo.substring(4, 6) + '/' + res.data.periodo.substring(0, 4) + ' desde: ' + res.data.tiempo_inicio;
                $scope.sla_generator_in_progress = true;
                if (!$scope.is_interval_running)
                    $scope.start_interval();
            }
            else {
                $scope.sla_generator_in_progress_text = '';
                $scope.sla_generator_in_progress = false;
                if ($scope.is_interval_running)
                    $scope.stop_interval();
            }
        });
    }

    $scope.start_interval = function () {
        $scope.is_interval_running = true;
        $scope.refreshSlaState();
    }

    $scope.stop_interval = function () {
        $scope.is_interval_running = false;
        clearInterval($scope.refreshIntervalId);
    }

    $scope.refreshSlaState = function () {
        $scope.refreshIntervalId = setInterval(function () {
            $scope.$apply($scope.getSlaGeneratorState());
        }, 2000);
    }

    $scope.onModalOpen = function () {
        $scope.getSlaGeneratorState();
    }

    $scope.acum;

    $scope.acumInspEntregaMismoDia = function () {
        return $scope.round((parseFloat($scope.tiempo_gestion.prom_insp_pub.mismo_dia * 100)));
    }

    $scope.acumInspEntregaEn24Hs = function () {
        return (parseFloat($scope.acumInspEntregaMismoDia())
        + $scope.round(parseFloat($scope.tiempo_gestion.prom_insp_pub.en_24hs * 100)));
    }

    $scope.acumInspEntregaEn48Hs = function () {
        return (parseFloat($scope.acumInspEntregaEn24Hs())
        + $scope.round(parseFloat($scope.tiempo_gestion.prom_insp_pub.en_48hs * 100)));
    }

    $scope.acumInspEntregaEn72Hs = function () {
        return (parseFloat($scope.acumInspEntregaEn48Hs())
        + $scope.round(parseFloat($scope.tiempo_gestion.prom_insp_pub.en_72hs * 100)));
    }

    $scope.acumInspEntregaMasDe72Hs = function () {
        return 100;
    }

    $scope.acumSolicInspMismoDia = function () {
        return $scope.round(parseFloat($scope.tiempo_gestion.prom_sol_insp.mismo_dia * 100));
    }

    $scope.acumSolicInspEn24Hs = function () {
        return $scope.round(parseFloat($scope.acumSolicInspMismoDia())
        + parseFloat($scope.tiempo_gestion.prom_sol_insp.en_24hs * 100));
    }

    $scope.acumSolicInspEn48Hs = function () {
        return $scope.round(parseFloat($scope.acumSolicInspEn24Hs())
        + parseFloat($scope.tiempo_gestion.prom_sol_insp.en_48hs * 100));
    }

    $scope.acumSolicInspEn72Hs = function () {
        return $scope.round(parseFloat($scope.acumSolicInspEn48Hs())
        + parseFloat($scope.tiempo_gestion.prom_sol_insp.en_72hs * 100));
    }

    $scope.acumSolicInspMasDe72Hs = function () {
        return 100;
    }

    $scope.total_de_inspeccion_a_informe = function () {
        var aux = $scope.tiempo_gestion.cant_insp_pub;
        return (
            aux.mismo_dia
          + aux.en_24hs
          + aux.en_48hs
          + aux.en_72hs
          + aux.mas_de_72hs);
    }

    $scope.total_de_solicitud_a_inspeccion = function () {
        var aux = $scope.tiempo_gestion.cant_sol_insp;
        return (
            aux.mismo_dia
          + aux.en_24hs
          + aux.en_48hs
          + aux.en_72hs
          + aux.mas_de_72hs);
    }

    $scope.get_realizadas_centro = function () {
        SLAService.getRealizadasCentro($scope.filtro.aseguradora.id, $scope.filtro.mes, $scope.filtro.provincia.id).then(function (res) {
            $scope.realizadas_centro = res.data;
        });
    }

    $scope.get_realizadas_domicilio = function () {
        SLAService.getRealizadasDomicilio($scope.filtro.aseguradora.id, $scope.filtro.mes, $scope.filtro.provincia.id).then(function (res) {
            $scope.realizadas_domicilio = res.data;
        });
    }
});
