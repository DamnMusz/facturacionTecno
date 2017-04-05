app.controller('ResumenSemestralCentrosController', function ($scope, $compile, ResumenSemestralService) {
    $scope.loading_semestral = false;
    $scope.loading_table_text = "Buscando per\u00edodo...";

    $scope.group_column_name = "Centro";
    $scope.propertyName = 'centro';
    $scope.reverse = false;

    $scope.resumen_semestral = [];
    $scope.semestre = [];
    $scope.lista_aseguradoras = [];

    $scope.filtro = {
        inicio_semestre: $scope.padding(((new Date()).getMonth()), 2) + "/" + ((new Date()).getFullYear())
    };

    $scope.sortBy = function (propertyName) {
        $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
        $scope.propertyName = propertyName;
    };
    
    function equals_aseguradora(alias_value) {
        return function (row) {
            return row.aseguradora_alias == alias_value;
        }
    }

    $scope.getGroupingValue = function (fila) {
        return fila.taller_triki;
    }

    $scope.aseguradora_by_alias = function(alias) {
        for (var i = 0; i < $scope.lista_aseguradoras.length; i++) {
            if ($scope.lista_aseguradoras[i].alias == alias)
                return $scope.lista_aseguradoras[i].nombre;
        }
        return alias;
    }

    $scope.configure_datepicker = function () {
        $(function () {
            var year = (new Date).getFullYear();
            var month = (new Date).getMonth();
            $('#fechasResumenSemestral').datepicker({
                viewMode: 'months',
                minViewMode: 'months',
                autoclose: true,
                startDate: new Date(2003, 0, 1),
                endDate: new Date(year, month - 1, 01)
            }
            );
        });
    }

    $scope.set_fechas_semestre = function () {
        $scope.semestre = [];
        var fecha = $scope.toDate($scope.filtro.inicio_semestre);
        for (var i = 5; i > 0; i--) {
            var fechaaux = $scope.addMonths(fecha, -i);
            $scope.semestre.push(fechaaux);
        }
        $scope.semestre.push($scope.filtro.inicio_semestre);
    }

    $scope.GetSemestral = function () {
        if ($scope.filtro.inicio_semestre != 'undefined' && $scope.filtro.inicio_semestre != '') {
            $scope.loading_semestral = true;
            $scope.set_fechas_semestre();
            ResumenSemestralService.getRealizadasSemestralCentros($scope.filtro.inicio_semestre).then(function (res) {
                $scope.resumen_semestral = res.data;
                $scope.resumen_semestral.map(remove_id);
                //for (var i = 0; i < $scope.resumen_semestral.length; i++) {
                //    if ($scope.getGroupingValue($scope.resumen_semestral[i]) == '' ||
                //        $scope.getGroupingValue($scope.resumen_semestral[i]) == undefined ||
                //        semestre_todos_cero($scope.resumen_semestral[i])) {
                //        $scope.resumen_semestral.splice(i, 1);
                //    }
                //}
                $scope.loading_semestral = false;
            });
        }
    }

    function semestre_todos_cero(fila) {
        return (
            fila.mes1 == 0 &&
            fila.mes2 == 0 &&
            fila.mes3 == 0 &&
            fila.mes4 == 0 &&
            fila.mes5 == 0 &&
            fila.mes6 == 0
            )
    }

    function completar_aseguradora(item, index) {
        item.aseguradora = $scope.aseguradora_by_alias(item.aseguradora_alias);
    }

    function remove_id(item, index) {
        delete item['id'];
    }

    $scope.toDate = function (date) {
        if (typeof date == typeof "") {
            var aux = date.split('/');
            fecha_res = new Date(aux[1], aux[0]);
            fecha_res.setMonth(fecha_res.getMonth() - 1);
        } else {
            fecha_res = date;
        }
        return fecha_res;
    }

    $scope.addMonths = function (date, n) {
        var fecha_inicio = $scope.toDate(date);

        var fecha_fin = new Date(new Date(fecha_inicio).setMonth(fecha_inicio.getMonth() + n));
        return ("0" + (fecha_fin.getMonth() + 1)).slice(-2) + '/' + fecha_fin.getFullYear();
    }

    $scope.a_csv = function () {
        var keys = []; 
        keys.push({ key: 'taller_triki', alias: 'Centro' });
        keys.push({ key: 'mes1', alias: $scope.semestre[0] });
        keys.push({ key: 'mes2', alias: $scope.semestre[1] });
        keys.push({ key: 'mes3', alias: $scope.semestre[2] });
        keys.push({ key: 'mes4', alias: $scope.semestre[3] });
        keys.push({ key: 'mes5', alias: $scope.semestre[4] });
        keys.push({ key: 'mes6', alias: $scope.semestre[5] });
        keys.push({ key: 'proporcion_trimestre_1', alias: '% 1er Trimestre' });
        keys.push({ key: 'proporcion_trimestre_2', alias: '% 2do Trimestre' });
        keys.push({ key: 'proporcion_semestre', alias: '% Semestre' });
            
        var csv_content = $scope.json_to_csv($scope.resumen_semestral, keys);
        $scope.download_as_file(csv_content,'resumen_semestral_centros.csv')
    }

    $scope.configure_datepicker();

    ResumenSemestralService.getAseguradoras().then(function (res) {
        $scope.lista_aseguradoras = res.data;
    });

    $scope.GetSemestral();
});