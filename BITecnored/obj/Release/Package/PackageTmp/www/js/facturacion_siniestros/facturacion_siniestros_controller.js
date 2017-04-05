app.controller('FacturacionSiniestrosController', function ($scope, FacturacionSiniestrosService) {
    $scope.tab = 0;
    $scope.mostrar = false;
    $scope.mostrar_hojas = false;
    $scope.cantidad_hojas = "1";

    $scope.para_facturar = []
    $scope.facturacion_manual = []

    $scope.Decimals2 = function(num) {
        return num.toFixed(2);
    }

    $scope.agregar_lista_facturacion = function (siniestros) {
        for (var i = 0; i < siniestros.length; i++) {
            if(siniestros[i].observacion == "")
                $scope.para_facturar.push(siniestros[i]);
            else
                $scope.facturacion_manual.push(siniestros[i]);
        }
        $scope.asignar_ids();
        $scope.mostrar = true;
    }

    $scope.mover_a_manual = function (siniestro) {
        for (var i = 0; i < $scope.para_facturar.length; i++) {
            if ($scope.para_facturar[i].nro_siniestro == siniestro.nro_siniestro) {
                $scope.para_facturar.splice(i, 1);
                break;
            }
        }
        $scope.facturacion_manual.push(siniestro);
    }

    $scope.mover_a_automatico = function (siniestro) {
        for (var i = 0; i < $scope.facturacion_manual.length; i++) {
            if ($scope.facturacion_manual[i].nro_siniestro == siniestro.nro_siniestro) {
                $scope.facturacion_manual.splice(i, 1);
                break;
            }
        }
        $scope.para_facturar.push(siniestro);
    }

    $scope.mover_todos_a_automatico = function () {
        for (var i = 0; i < $scope.facturacion_manual.length; i++) {
            $scope.para_facturar.push($scope.facturacion_manual[i]);
        }
        $scope.facturacion_manual = [];
    }

    $scope.ocultar = function () {
        $scope.mostrar = false;
        $scope.mostrar_hojas = false;
        $scope.cantidad_hojas = "1";
    }

    $scope.reset = function () {
        $scope.para_facturar = []
        $scope.facturacion_manual = []
        $scope.ocultar();
    }

    $scope.asignar_ids = function () {
        var last_id = 0;
        for (var i = 0; i < $scope.para_facturar.length; i++) {
            $scope.para_facturar[i].id = i;
            last_id = i;
        }
        for (var i = 0; i < $scope.facturacion_manual.length; i++) {
            $scope.facturacion_manual[i].id = last_id + i + 1;
        }
    }

    $scope.generar_txt_gecom = function () {
        FacturacionSiniestrosService.generar_txt_gecom(JSON.stringify($scope.para_facturar)).then(function (res) {
            var hiddenElement = document.createElement('a');

            document.body.appendChild(hiddenElement);
            hiddenElement.href = 'data:attachment/text,' + encodeURIComponent(res.data);
            hiddenElement.target = '_blank';
            hiddenElement.download = 'terceros.txt';
            hiddenElement.click();
        });
    }

    $scope.mostrar_select_hojas = function () {
        $scope.mostrar_hojas = true;
    }

    $scope.config_file_input = function () {
        $("#input-file").fileinput({
            dropZoneTitle: 'Arrastre o busque los archivos de facturaci&oacute;n &hellip;',
            uploadUrl: 'api/FacturacionSiniestros',
            allowedFileExtensions: ["xlsx"],
            uploadAsync: false
        }).on('filebatchuploadsuccess', function (event, data, previewId, index) {
            $scope.$apply($scope.agregar_lista_facturacion(data.response));
        }).on('filebatchuploaderror', function (event, data, previewId, index) {
            alert(data.jqXHR.responseText);
        }).on('filecleared', function (event) {
            $scope.$apply($scope.reset());
        }).on('filebatchselected', function (event, files) {
            $scope.$apply($scope.mostrar_select_hojas);
        });
    };
    $scope.config_file_input();
});
