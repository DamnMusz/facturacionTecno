app.controller('ResumenEjecutivoController', function ($scope, $compile, ResumenEjecutivoService) {

    $scope.loading_table = false;
    $scope.loading_partial = false;
    $scope.loading_table_text = "Buscando per\u00edodo...";

    $scope.sumatoria_inspeccionadas = function (element) {
        var e = element.cant_insp_pub;
        return e.mismo_dia + e.en_24hs + e.en_48hs + e.en_72hs + e.mas_de_72hs;
    }

    $scope.prop_mismo_dia = function (element) {
        var res = $scope.round2(element.prom_insp_pub.mismo_dia * 100);
        if (res > 100)
            return 100;
        else
            return res;
    }

    $scope.prop_24hs = function (element) {
        var res = $scope.round2(
            (element.prom_insp_pub.mismo_dia + element.prom_insp_pub.en_24hs)
            * 100);
        if (res > 100)
            return 100;
        else
            return res;
    }

    $scope.prop_48hs = function (element) {
        var res = $scope.round2(
            (element.prom_insp_pub.mismo_dia + element.prom_insp_pub.en_24hs + element.prom_insp_pub.en_48hs)
            * 100);
        if (res > 100)
            return 100;
        else
            return res;
    }

    $scope.propertyName = 'aseguradora';
    $scope.reverse = false;

    $scope.sortBy = function (propertyName) {
        $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
        $scope.propertyName = propertyName;
    };

    $scope.filtro = {
        periodo: $scope.padding(((new Date()).getMonth()), 2) + "/" + ((new Date()).getFullYear())
    };

    $scope.toogle_resumen_ejecutivo_semestral = function () {
        $scope.resumen_ejecutivo_semestral = !$scope.resumen_ejecutivo_semestral;
    }

    $scope.GetTiemposGestionPorAseguradora = function () {
        if ($scope.filtro.periodo != 'undefined' && $scope.filtro.periodo != '') {
            $scope.resumen_por_aseguradora = [];
            if ($scope.is_loading())
                return;
            start_loading();
            ResumenEjecutivoService.getTiempoGestionPorAseguradora($scope.filtro.periodo).then(function (res) {
                ResumenEjecutivoService.getRealizadasPorAseguradora($scope.filtro.periodo).then(function (res_realizadas) {
                    var values = res.data;
                    var keys = Object.keys(values)
                    for (var i = 0; i < keys.length; i++) {
                        var key = keys[i];
                        var value = values[keys[i]];                        
                        $scope.resumen_por_aseguradora.push({
                            aseguradora: $scope.aseguradora_by_alias(key),
                            aseguradora_alias: key,
                            inspeccionadas: $scope.sumatoria_inspeccionadas(value),
                            mismo_dia: $scope.prop_mismo_dia(value),
                            en24hs: $scope.prop_24hs(value),
                            en48hs: $scope.prop_48hs(value),
                            solicitadas: 0,
                            porc_realizadas: 100,
                            realiz_2da: 0,
                            sin_efecto_2da: 0
                        });
                    }
                    var realiz_keys = Object.keys(res_realizadas.data);
                    for (var i = 0; i < realiz_keys.length; i++) {
                        for (var j = 0; j < $scope.resumen_por_aseguradora.length; j++) {
                            if ($scope.resumen_por_aseguradora[j].aseguradora_alias == realiz_keys[i]) {
                                $scope.resumen_por_aseguradora[j].solicitadas = res_realizadas.data[realiz_keys[i]].total_count;
                                $scope.resumen_por_aseguradora[j].porc_realizadas = $scope.porcentaje(res_realizadas.data[realiz_keys[i]].realiz_count, res_realizadas.data[realiz_keys[i]].total_count);
                            }
                        }
                    }
                    ResumenEjecutivoService.getFotosPorAseguradora($scope.filtro.periodo).then(function (res_fotos) {
                        res_fotos.data.map(completar_datos_fotos);
                        done_loading();
                    });
                    ResumenEjecutivoService.getNoRealizadasPorAseguradora($scope.filtro.periodo).then(function (res_insp) {
                        res_insp.data.map(completar_datos_no_realizadas);
                        done_loading();
                    });
                    done_loading();
                });
            });
        }
    }
    
    function start_loading() {
        $scope.loading_table = true;
        $scope.loading_partial = true;
    }

    $scope.is_loading = function() {
        if ($scope.loading_table == true ||
        $scope.loading_partial == true)
            return true
        else return false;
    }

    function done_loading() {
        if ($scope.loading_partial == false)
            $scope.loading_table = false;
        else
            $scope.loading_partial = false;
    }

    function equals_aseguradora(alias_value) {
        return function (row) {
            return row.aseguradora_alias == alias_value;
        }
    }

    function completar_datos_fotos(dato_foto) {
        var rows = $scope.resumen_por_aseguradora.filter(equals_aseguradora(dato_foto.aseguradora_alias));
        rows.map(set_datos_fotos(dato_foto));
    }

    function completar_datos_no_realizadas(res_insp) {
        var rows = $scope.resumen_por_aseguradora.filter(equals_aseguradora(res_insp.aseguradora_alias));
        rows.map(set_datos_no_realizadas(res_insp));
    }

    function set_datos_no_realizadas(res_insp) {
        return function (row) {
            row.realiz_2da = res_insp.realizadas;
            row.sin_efecto_2da = res_insp.sin_efecto;
        }
    }

    function set_datos_fotos(dato_foto) {
        return function (row) {
            row.prom_fotos = dato_foto.prom_fotos;
            row.mas_4_fotos = dato_foto.mas_de_4_fotos;
        }
    }
        
    $scope.aseguradora_by_alias = function(alias) {
        for (var i = 0; i < $scope.lista_aseguradoras.length; i++) {
            if ($scope.lista_aseguradoras[i].alias == alias)
                return $scope.lista_aseguradoras[i].nombre;
        }
        return alias;
    }

    $scope.a_csv = function () {
        var keys = [];
        keys.push({ key: 'aseguradora_alias', alias: 'Alias' });
        keys.push({ key: 'aseguradora', alias: 'Aseguradora' });
        keys.push({ key: 'inspeccionadas', alias: 'Inspeccionadas' });
        keys.push({ key: 'mismo_dia', alias: 'Mismo dia' });
        keys.push({ key: 'en24hs', alias: '1 dia' });
        keys.push({ key: 'en48hs', alias: '2 dias' });
        keys.push({ key: 'solicitadas', alias: 'Solicitadas Agda.' });
        keys.push({ key: 'porc_realizadas', alias: 'Solic. Realizadas' });
        keys.push({ key: 'prom_fotos', alias: 'Promedio Fotos' });
        keys.push({ key: 'mas_4_fotos', alias: 'Mas de 4 Fotos' });
        keys.push({ key: 'realiz_2da', alias: 'Realizadas 2da Instancia' });
        keys.push({ key: 'sin_efecto_2da', alias: 'Sin Efecto 2da Instancia' });

        var csv_content = $scope.json_to_csv($scope.resumen_por_aseguradora, keys);
        $scope.download_as_file(csv_content, 'resumen_ejecutivo.csv')
    }

    $scope.configure_datepicker = function () {
        $(function () {
            var year = (new Date).getFullYear();
            var month = (new Date).getMonth();
            $('#fechasResumen').datepicker({
                viewMode: 'months',
                minViewMode: 'months',
                autoclose: true,
                startDate: new Date(2003, 0, 1),
                endDate: new Date(year, month - 1, 01)
            }
            );
        });
    }

    $scope.configure_datepicker();
    
    ResumenEjecutivoService.getAseguradoras().then(function (res) {
        $scope.lista_aseguradoras = res.data;
        $scope.GetTiemposGestionPorAseguradora();
    });

    $scope.resumen_por_aseguradora = [];
    $scope.lista_aseguradoras = [];
});