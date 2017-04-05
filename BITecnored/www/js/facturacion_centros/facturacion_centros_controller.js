app.filter("sanitize", ['$sce', function ($sce) {
    return function (htmlCode) {
        return $sce.trustAsHtml(htmlCode);
    }
}])

app.controller('FacturacionCentrosController', function ($scope, $compile, FacturacionCentrosService) {
    // 0: centros | 1: tarifas | 2: facturacion
    $scope.selection = 0;
    $scope.view_size = 10;
    $scope.actual_page = 1;
    $scope.cantidad_centros = 0;
    $scope.cantidad_paginas = 0;


    $scope.centros = [];


    $scope.paginas = [];
    $scope.loading_edit_modal = false;
    $scope.loading_table = true;
    $scope.loading_table_text = "";
    $scope.loading_edit_modal_text = '';
    $scope.txt_nueva_afinidad = "";
    $scope.solo_centros_activos = true;
    $scope.txt_ocultar_mostrar = "Mostrar";
    $scope.viendo_incompletos = false;
    $scope.animate = false;

    
    $scope.afinidades_tarifarias = [];
    $scope.provincias = [];
    $scope.localidades = [];

    $scope.centro_selected = {};
    $scope.centro_selected_aux = {};
    $scope.afinidad_tarifaria_eliminar = [];

    $scope.busqueda = {
        id: '',
        nombre: '',
        nombre_fantasia: '',
        cuit: '',
        razon_social: '',
        propio: '',
        activo: '',
        calle: '',
        numero: '',
        tipo_factura: '-',
        provincia: { id: '', value: '' },
        localidad: { id: '', value: '' },
        afinidad_tarifaria: { id: '', value: '' },
    }

    $scope.busqueda_centros_aplicada = [];
    $scope.hay_centros_incompletos = false;
    $scope.cantidad_centros_incompletos = 0;

    $scope.select = function (value) {
        $scope.animate = true;
        $scope.selection = value;
        if (value == 1 ) {
            if(!$scope.solo_centros_activos) {
                $scope.actual_page = 1;
                $scope.toogle_solo_centros_activos();
            }
            $scope.is_nueva_tarifa_selected = false;
            $scope.selected_centro_tarifa = {};
            $scope.tarifas = [];
        }

        $scope.reset_centros_view();
    }

    $scope.reset_centros_view = function () {
        $scope.busqueda = {
            id: '',
            nombre: '',
            nombre_fantasia: '',
            cuit: '',
            razon_social: '',
            propio: '',
            activo: '',
            calle: '',
            numero: '',
            tipo_factura: '-',
            provincia: { id: '', value: '' },
            localidad: { id: '', value: '' },
            afinidad_tarifaria: { id: '', value: '' },
        }
        $scope.busqueda_centros_aplicada = [];
        $scope.actual_page = 1;
        $scope.view_centros_completos();
        $scope.init();
    }

    $scope.toogle_solo_centros_activos = function () {
        if(!$scope.viendo_incompletos) {
            $scope.solo_centros_activos = !$scope.solo_centros_activos;
            if ($scope.solo_centros_activos)
                $scope.txt_ocultar_mostrar = "Mostrar";
            else
                $scope.txt_ocultar_mostrar = "Ocultar";
            $scope.loading_table = true;
            $scope.loading_table_text = "Actualizando centros...";
            $scope.getCantidadCentros();
            $scope.getCentros();
        }
    }

    $scope.buscar_cantidad_centros_incompletos = function () {
        FacturacionCentrosService.get_amount_centros_incompletos().then(function (res) {
            if(res.data.cantidad > 0)
                $scope.hay_centros_incompletos = true;
            $scope.cantidad_centros_incompletos = res.data.cantidad;
            if ($scope.viendo_incompletos)
                $scope.cantidad_centros = $scope.cantidad_centros_incompletos;
        });
    }

    $scope.view_centros_completos = function () {
        $scope.viendo_incompletos = false;
        $scope.getCantidadCentros();
        $scope.getCentros = $scope.getCentros_original;
        $scope.loading_table = true;
        $scope.getCentros();
    }

    $scope.view_centros_incompletos = function () {
        $scope.viendo_incompletos = true;
        $scope.cantidad_centros = $scope.cantidad_centros_incompletos;
        $scope.getCentros = $scope.get_centros_incompletos;
        $scope.getCentros();
    }

    $scope.toogle_view_centros_incompletos = function () {
        if ($scope.viendo_incompletos) {
            $scope.view_centros_completos();
        } else {
            $scope.view_centros_incompletos();
        }
        
    }

    $scope.get_centros_incompletos = function () {
        $scope.loading_table = true;
        FacturacionCentrosService.get_centros_incompletos($scope.actual_page, $scope.view_size).then(function (res) {
            $scope.centros = res.data;
            for (var i = 0; i < $scope.centros.length; i++) {
                $scope.centros[i].cuit = $scope.normalize_cuit($scope.centros[i].cuit);
            }
            $scope.loading_table = false;
            $scope.buscar_cantidad_centros_incompletos();
        });
    }

    $scope.copy_centro = function (src, dest) {
        dest.id = src.id;        
        dest.nombre_fantasia = src.nombre_fantasia;
        dest.nombre_centro = src.nombre_centro;
        dest.cuit = src.cuit;
        dest.razon_social = src.razon_social;
        dest.propio = src.propio;
        dest.direccion_legal_calle = src.direccion_legal_calle;
        dest.direccion_legal_numero = src.direccion_legal_numero;
        dest.provincia_legal = src.provincia_legal;
        dest.localidad_legal = src.localidad_legal;
        dest.activo = src.activo;
        dest.afinidad_tarifaria = src.afinidad_tarifaria;
        dest.tipo_factura = src.tipo_factura;
    }

    $scope.change_page = function (page_number) {
        $scope.actual_page = page_number;
        $scope.draw_pager();
        $scope.getCentros();
    }

    $scope.getCantidadCentros = function () {
        FacturacionCentrosService.get_amount_centros($scope.busqueda, $scope.solo_centros_activos).then(function (res) {
            $scope.cantidad_centros = res.data.cantidad;
            $scope.draw_pager();
        });
    }

    $scope.draw_pager = function () {
        $scope.cantidad_paginas = Math.ceil($scope.cantidad_centros / $scope.view_size);
        $scope.paginas = [];
        for (var i = Math.max($scope.actual_page - 2, 1) ; i <= $scope.cantidad_paginas && i < $scope.actual_page + 3; i++) {
            $scope.paginas.push(i);
        }
    }

    $scope.normalize_cuit = function (cuit) {
        if (typeof cuit != 'undefined' && cuit.length == 11) {
            var cuit_str = cuit.toString();
            var res = cuit_str.substring(0, 2) + "-" + substring(2, 10) + "-" + substring(10, 11);
            return res;
        }
        else
            return cuit.toString();
    }
    
    $scope.getCentros = function () {
        $scope.loading_table = true;
        FacturacionCentrosService.get_all_centros($scope.actual_page, $scope.view_size, $scope.solo_centros_activos).then(function (res) {
            $scope.centros = res.data;
            for (var i = 0; i < $scope.centros.length; i++) {
                $scope.centros[i].cuit = $scope.normalize_cuit($scope.centros[i].cuit);
            }
            $scope.loading_table = false;
        });
    }

    $scope.getCentros_original = $scope.getCentros;

    $scope.sort_id_value = function (a, b) { return (a.value > b.value) ? 1 : ((b.value > a.value) ? -1 : 0); }

    $scope.getProvincias = function () {
        $scope.loading_edit_modal = true;
        $scope.loading_edit_modal_text = 'Obteniendo provincias...';
        FacturacionCentrosService.get_all_provincias().then(function (res) {
            $scope.provincias = res.data;
            $scope.provincias.sort($scope.sort_id_value);
            $scope.loading_edit_modal = false;
        });
    }

    $scope.getLocalidades = function (provincia_legal) {
        $scope.loading_edit_modal = true;
        $scope.loading_edit_modal_text = 'Obteniendo localidades para ' + provincia_legal.value + '...';
        FacturacionCentrosService.get_localidades(provincia_legal.id).then(function (res) {
            $scope.localidades = res.data;
            $scope.localidades.sort($scope.sort_id_value);
            $scope.loading_edit_modal = false;
        });
    }

    $scope.getAfinidadesTarifarias = function () {
        $scope.loading_edit_modal = true;
        $scope.loading_edit_modal_text = 'Obteniendo afinidades tarifarias...';
        FacturacionCentrosService.get_afinidades_tarfiarias().then(function (res) {
            $scope.afinidades_tarifarias = res.data;
            $scope.afinidades_tarifarias.sort($scope.sort_id_value);
            $scope.loading_edit_modal = false;
        });
    }

    $scope.is_valid_centro = function (centro) {
        if (
            centro.cuit.length >= 11
            &&
            centro.razon_social.length > 0
            &&
            typeof centro.provincia_legal != 'undefined'
            &&
            centro.provincia_legal.value.length > 0
            &&
            typeof centro.localidad_legal != 'undefined'
            &&
            centro.localidad_legal.value.length > 0
            &&
            typeof centro.direccion_legal_calle != 'undefined'
            &&
            centro.direccion_legal_calle.length > 0
            &&
            typeof centro.direccion_legal_numero != 'undefined'
            &&
            centro.direccion_legal_numero.toString() > 0
            ) {
            return true;
        }
        else {
            return false;
        }
    }
    
    $scope.update_centro = function (centro) {
        if ($scope.is_valid_centro(centro) == true) {
            $scope.copy_centro(centro, $scope.centro_selected);

            $scope.loading_edit_modal = true;
            $scope.loading_edit_modal_text = 'Actualizando Centro...';
            FacturacionCentrosService.unlock().then(function(){});
            FacturacionCentrosService.update_centro($scope.centro_selected).then(function (res) {
                $scope.getCentros();
                $scope.loading_edit_modal = false;
                $('#modalEditRow').modal('hide');
            }, function errorCallback(res) {
                $scope.loading_edit_modal = false;
                $scope.alert_error('Error al actualizar centro.');
            });
        } else {
            $scope.alert_error('Falta(n) completar campo(s) obligatorio(s).');
        }
    }

    $scope.select_centro = function (centro) {
        $scope.centro_selected = centro;
        $scope.localidades = [];
        $scope.copy_centro(centro, $scope.centro_selected_aux);
        if ($scope.centro_selected_aux.activo == '')
            $scope.centro_selected_aux.activo = 'false';
        if ($scope.centro_selected_aux.propio == '')
            $scope.centro_selected_aux.propio = 'false';
        if ($scope.centro_selected_aux.direccion_legal_numero == -1)
            $scope.centro_selected_aux.direccion_legal_numero = '';
        if ($scope.centro_selected_aux.cuit == -1)
            $scope.centro_selected_aux.cuit = '';
        if (typeof $scope.centro_selected_aux.tipo_factura == 'undefined' || $scope.centro_selected_aux.tipo_factura == '')
            $scope.centro_selected_aux = '-';
        $scope.getAfinidadesTarifarias();
    }

    $scope.menu_afinidad = function () {
        $scope.txt_nueva_afinidad = '';
        $scope.afinidad_tarifaria_eliminar = [];
    }

    $scope.crear_afinidad = function () {
        var crear = true;
        if ($scope.txt_nueva_afinidad == '') {
            $scope.alert_error('No se puede crear afinidad: "' + $scope.txt_nueva_afinidad + '"');
            crear = false;
        }
        if ($scope.in_array_id_value($scope.txt_nueva_afinidad, $scope.afinidades_tarifarias) != false) {
            $scope.alert_error('Ya existe la afinidad: "' + $scope.txt_nueva_afinidad + '"');
            crear = false;
        }

        if (crear) {
            FacturacionCentrosService.post_afinidad_tarfiaria($scope.txt_nueva_afinidad).then(function (res) {
                $scope.getAfinidadesTarifarias();
            });
        }           
    }

    $scope.in_array_id_value = function (value, array) {
        for (var item in array) {
            if ($scope.afinidades_tarifarias[item].value == value) {
                return true;
            }
        }
        return false;
    }

    $scope.modal_eliminar_afinidad = function () {
        if ($scope.afinidad_tarifaria_eliminar.id === undefined) {
            $scope.alert_warning("Seleccione afinidad para eliminar");
        } else {
            $('#modalAlertEliminarAfinidad').modal('show');
        }
    }

    $scope.eliminar_afinidad = function () {
        if ($scope.afinidad_tarifaria_eliminar.id !== undefined) {
            FacturacionCentrosService.delete_afinidad_tarfiaria($scope.afinidad_tarifaria_eliminar.id).then(function (res) {
                $scope.getAfinidadesTarifarias();
                $scope.afinidad_tarifaria_eliminar = [];
            }, function errorCallback(res) {
                if (res.status == 412) {
                    $scope.alert_error('No se pudo eliminar: "' + $scope.afinidad_tarifaria_eliminar.value + '". Afinidad en uso vinculada a un centro.');
                }
                else
                    $scope.alert_error('No se pudo eliminar: "' + $scope.afinidad_tarifaria_eliminar.value + '". Error interno del sistema.');
            });
        }
    }

    $scope.toogle_factura = function (centro) {
        if (typeof centro.tipo_factura == 'undefined' || centro.tipo_factura == '' || centro.tipo_factura == '-')
        { centro.tipo_factura = 'A'; }
        else if (centro.tipo_factura == 'A')
        { centro.tipo_factura = 'B'; }
        else if (centro.tipo_factura == 'B')
        { centro.tipo_factura = 'C'; }
        else if (centro.tipo_factura == 'C')
        { centro.tipo_factura = '-'; }
    }

    $scope.select_afinidad = function (afinidad) {
        $scope.centro_selected_aux.afinidad_tarifaria = afinidad;
    }

    $scope.select_afinidad_eliminar = function (afinidad) {
        $scope.afinidad_tarifaria_eliminar = afinidad;
    }

    $scope.select_provincia = function (provincia) {
        $scope.centro_selected_aux.provincia_legal = provincia;
        $scope.getLocalidades($scope.centro_selected_aux.provincia_legal);
        $scope.centro_selected_aux.localidad_legal = '';
    }

    $scope.load_localidades = function () {
        if ($scope.localidades.length == 0) {
            $scope.getLocalidades($scope.centro_selected_aux.provincia_legal);
        }
    }

    $scope.select_localidad = function (localidad) {
        $scope.centro_selected_aux.localidad_legal = localidad;
    }

    $scope.select_provincia_buscar = function (provincia) {
        $scope.busqueda.provincia = provincia;
        $scope.getLocalidades(provincia);
        $scope.busqueda.localidad = '';
    }

    $scope.select_localidad_buscar = function (localidad) {
        $scope.busqueda.localidad = localidad;
    }

    $scope.select_afinidad_buscar = function (afinidad) {
        $scope.busqueda.afinidad_tarifaria = afinidad;
    }

    $scope.search_modal = function () {
        $scope.localidades = [];
        $('#modalSearch').modal('show');
        $scope.getAfinidadesTarifarias();
    }

    $scope.busqueda_centros = function () {
        $scope.loading_edit_modal = true;
        $scope.loading_edit_modal_text = "Obteniendo centros de acuerdo a los criterios de b\u00FAsqueda...";
        FacturacionCentrosService.get_filtered_centros($scope.actual_page, $scope.view_size, $scope.busqueda, $scope.solo_centros_activos).then(function (res) {
            $scope.centros = res.data;
            for (var i = 0; i < $scope.centros.length; i++) {
                $scope.centros[i].cuit = $scope.normalize_cuit($scope.centros[i].cuit);
            }
            $scope.loading_edit_modal = false;
            $scope.getCentros = $scope.buscarCentros;

            var keys = Object.keys($scope.busqueda);
            $scope.busqueda_centros_aplicada = [];
            for (var i = 0; i < keys.length; i++) {
                key = keys[i];
                if (typeof $scope.to_str_value($scope.busqueda[key]) != 'undefined' && $scope.to_str_value($scope.busqueda[key]) != null && $scope.to_str_value($scope.busqueda[key]).length != '' && (key != "tipo_factura" || $scope.busqueda[key] != "-")) {
                    $scope.busqueda_centros_aplicada.push(key);
                }
            }
            $scope.getCantidadCentros();
            $scope.loading_table = false;
            $('#modalSearch').modal('hide');
        });
        
    }

    $scope.buscarCentros = function () {
        FacturacionCentrosService.get_filtered_centros($scope.actual_page, $scope.view_size, $scope.busqueda, $scope.solo_centros_activos).then(function (res) {
            $scope.centros = res.data;
            for (var i = 0; i < $scope.centros.length; i++) {
                $scope.centros[i].cuit = $scope.normalize_cuit($scope.centros[i].cuit);
            }
            $scope.getCantidadCentros();
            $scope.loading_table = false;
        });
    }

    $scope.busqueda_centros_todos = function () {
        $scope.loading_edit_modal = true;
        $scope.loading_edit_modal_text = 'Obteniendo centros...';
        FacturacionCentrosService.get_all_centros($scope.actual_page, $scope.view_size, $scope.solo_centros_activos).then(function (res) {
            $scope.centros = res.data;
            for (var i = 0; i < $scope.centros.length; i++) {
                $scope.centros[i].cuit = $scope.normalize_cuit($scope.centros[i].cuit);
                $scope.loading_edit_modal = false;
                $scope.busqueda_centros_aplicada = [];
                $scope.busqueda = {
                    id: '',
                    nombre: '',
                    nombre_fantasia: '',
                    cuit: '',
                    razon_social: '',
                    propio: '',
                    activo: '',
                    calle: '',
                    numero: '',
                    tipo_factura: '-',
                    provincia: { id: '', value: '' },
                    localidad: { id: '', value: '' },
                    afinidad_tarifaria: { id: '', value: '' },
                }
                $scope.getCantidadCentros();
                $('#modalSearch').modal('hide');
            }
        });
        $scope.getCentros = $scope.getCentros_original;
    }

    $scope.to_str_value = function (data) {
        if (Object.prototype.toString.call(data) == '[object Object]') {
            return data.value;
        }
        return data;
    }

    $('#chk_propio').change(function () {
        if ($scope.busqueda.propio == '') {
            $scope.busqueda.propio = 'true';
            $("#chk_propio").prop("checked", true);
            $("#chk_propio").prop("indeterminate", false);
        } else if ($scope.busqueda.propio == 'true') {
            $scope.busqueda.propio = 'false';
            $("#chk_propio").prop("checked", false);
            $("#chk_propio").prop("indeterminate", false);

        } else {
            $scope.busqueda.propio = '';
            $("#chk_propio").prop("checked", false);
            $("#chk_propio").prop("indeterminate", true);
        }
    });

    $('#chk_activo').change(function () {
        if ($scope.busqueda.activo == '') {
            $scope.busqueda.activo = 'true';
            $("#chk_activo").prop("checked", true);
            $("#chk_activo").prop("indeterminate", false);
        } else if ($scope.busqueda.activo == 'true') {
            $scope.busqueda.activo = 'false';
            $("#chk_activo").prop("checked", false);
            $("#chk_activo").prop("indeterminate", false);

        } else {
            $scope.busqueda.activo = '';
            $("#chk_activo").prop("checked", false);
            $("#chk_activo").prop("indeterminate", true);
        }
    });

    $scope.init = function () {
        $("#chk_propio").prop("checked", false);
        $("#chk_propio").prop("indeterminate", true);
        $("#chk_activo").prop("checked", false);
        $("#chk_activo").prop("indeterminate", true);
        $('[data-toggle="tooltip"]').tooltip();
        $("[data-mask]").inputmask();
        $('select').material_select();
        $scope.getCantidadCentros();
        $scope.getCentros();
        $scope.getProvincias();
        $scope.buscar_cantidad_centros_incompletos();
    }

    $scope.init();

    // --------------------------------------------------------------------------------------------------

    $scope.buscar_centros_tarifas_incompletas = function () {
        $scope.loading_table = true;
        FacturacionCentrosService.get_centros_tarifas_incompletas($scope.actual_page, $scope.view_size).then(function (res) {
            $scope.centros = res.data;
            for (var i = 0; i < $scope.centros.length; i++) {
                $scope.centros[i].cuit = $scope.normalize_cuit($scope.centros[i].cuit);
            }
            $scope.loading_table = false;
        });
    }

    /* ----------------------------------------------- NUEVA PANTALLA -----------------------------------------------*/

    $scope.tarifas_centros = [
        {
            id: 1,
            nombre: 'ADROGUE',
            afinidad: { 
                id: 0,
                value: 'SORENSEN'
            },
            tarifas: [
                {
                    id: 0,
                    titulo: 'ESPECIAL SORENSEN 2015',
                    monto: 70,
                    descripcion: '',
                    color: '#006064',
                    periodo_desde: 201601,
                    periodo_hasta: 201604
                }
            ]
        },
        {
            id: 2,
            nombre: 'LANUS',
            afinidad: { 
                id: 1,
                value: 'MILLA'
            },
            tarifas: [
                {
                    id: 1,
                    titulo: 'ESPECIAL MILLA 2015',
                    monto: 60,
                    descripcion: '',
                    color: '#01579B',
                    periodo_desde: 201601,
                    periodo_hasta: 201606
                },
                {
                    id: 3,
                    titulo: 'ESPECIAL MILLA 2016',
                    monto: 120,
                    descripcion: '',
                    color: '#01579B',
                    periodo_desde: 201607,
                    periodo_hasta: 201612
                }
            ]
        },
        {
            id: 3,
            nombre: 'MORENO',
            afinidad: {
                id: 1,
                value: 'SORENSEN'
            },
            tarifas: [
                {
                    id: 0,
                    titulo: 'ESPECIAL SORENSEN 2015',
                    monto: 70,
                    descripcion: '',
                    color: '#006064',
                    periodo_desde: 201601,
                    periodo_hasta: 201604
                },
                {
                    id: 2,
                    titulo: 'ESPECIAL SORENSEN 2016',
                    monto: 90,
                    descripcion: '',
                    color: '#006064',
                    periodo_desde: 201605,
                    periodo_hasta: 201612
                }
            ]
        },
        {
            id: 4,
            nombre: 'BELLA VISTA',
            afinidad: {
                id: '',
                value: ''
            },
            tarifas: [
                {
                    id: 5,
                    titulo: 'TARIFA STANDARD 2015',
                    monto: 90,
                    descripcion: '',
                    color: '#01579B',
                    periodo_desde: 201504,
                    periodo_hasta: 201608
                },
                {
                    id: 6,
                    titulo: 'TARIFA STANDARD 2016',
                    monto: 115,
                    descripcion: '',
                    color: '#01579B',
                    periodo_desde: 201609,
                    periodo_hasta: 201612
                }
            ]
        },
    ];

    $scope.tarifas_vigentes = [];

    $scope.selected_tarifa_row = [];

    $scope.periodos_tarifario = [201601, 201602, 201603, 201604, 201605, 201606, 201607, 201608, 201609, 201610, 201611, 201612];

    $scope.available_colors = ['#D50000', '#C51162', '#AA00FF', '#311B92', '#304FFE', '#2962FF', '#01579B', '#006064', '#004D40', '#1B5E20', '#E65100', '#BF360C', '#3E2723', '#212121', '#263238'];
    $scope.selected_color = '';


    $scope.get_check_button_style = function () {
        return "height: 20px; width: 20px; padding-left:2px;padding-top:0; background-color:white; border-color:#9E9E9E;";
    }

    $scope.get_tarifario = function (centro) {
        var str = ''
            + '<td><button id="centro_' + centro.id + '" type="button" class="btn" onclick="select_row_centro(centro_' + centro.id + ')" style="' + $scope.get_check_button_style() + '" ng-click="select_centro(centro)"></button></td>'
            + '<td>' + centro.id + '</td>'
            + '<td>' + centro.nombre + '</td>'
            + '<td>' + centro.afinidad.value + '</td>';
        
        for (var i = 0; i < $scope.periodos_tarifario.length; i++) {
            for (var j = 0; j < centro.tarifas.length; j++) {
                if ($scope.is_periodo_incluido($scope.periodos_tarifario[i], centro.tarifas[j])) {
                    if ($scope.is_border_start($scope.periodos_tarifario[i], $scope.periodos_tarifario) || $scope.is_start_equal($scope.periodos_tarifario[i], centro.tarifas[j])) {
                        str += $scope.get_event_element(centro.tarifas[j]);
                        break;
                    }
                } else {
                    str += '<td></td>';
                }
            }
        }
        return str;
    }

    $scope.is_periodo_incluido = function(periodo, tarifa) {
        return ((periodo >= tarifa.periodo_desde) || (periodo <= tarifa.periodo_hasta));
    }

    $scope.is_border_start = function (periodo, periodos_tarifario) {
        return (periodo == periodos_tarifario[0]);
    }

    $scope.is_start_equal = function(periodo, tarifa) {
        return (periodo == tarifa.periodo_desde);
    }

    $scope.get_event_element = function (tarifa) {
        return '<td colspan="' + $scope.get_distancia_periodos(tarifa.periodo_desde, tarifa.periodo_hasta) + '"><a class="fc-event text-center" style="' + JSON.stringify($scope.get_style_from_color_2(tarifa.color)).replace(/\"/g, "").replace(/{/g, "").replace(/}/g, "").replace(/,/g, ";") + '"><div class="fc-content"><span class="fc-title">$' + tarifa.monto + '</span></div></a></td>'
    }

    $scope.get_distancia_periodos = function (desde, hasta) {
        return (Math.min(hasta, $scope.periodos_tarifario[$scope.periodos_tarifario.length-1]) - Math.max(desde, $scope.periodos_tarifario[0])) + 1;
    }

    

    $scope.get_style_from_color = function (obj) {
        obj = (obj == "") ? $scope.available_colors[$scope.available_colors.length - 1] : obj;
        return { 'border-color': '' + obj }, { 'color': '' + obj };
    }

    $scope.get_style_from_color_2 = function (obj) {
        obj = (obj == "") ? $scope.available_colors[$scope.available_colors.length - 1] : obj;
        return { 'border-color': '' + obj, 'background-color': '' + obj, 'color': (obj != '') ? '#FFFFFF' : '#000000', 'opacity': '0.9' };
    }

    $scope.select_color = function (color) {
        $scope.nueva_tarifa.color_vista = color;
        $scope.selected_color = color;
    }

    $scope.add_event_tarifa = function (titulo, color) {
        $scope.crear_tarifa(titulo, color)
    }

    $scope.nueva_tarifa = {}

    $scope.reset_nueva_tarifa = function () {
        $scope.nueva_tarifa = {
            id: '',
            nombre: '',
            tarifa_por_inspeccion: '',
            observacion: '',
            color: '',
            periodo_desde: '',
            periodo_hasta: ''
        }
        $scope.select_color($scope.available_colors[$scope.available_colors.length - 1]);
    }

    $scope.reset_nueva_tarifa();

    $scope.crear_tarifa = function () {
        $scope.nueva_tarifa.periodo_desde = $scope.deformat_date($scope.nueva_tarifa.periodo_desde);
        $scope.nueva_tarifa.periodo_hasta = $scope.deformat_date($scope.nueva_tarifa.periodo_hasta);

        console.log('crear_tarifa');
        console.log($scope.nueva_tarifa);

        // REEMPLAZAR POR ENVIO Y VUELTA
        FacturacionCentrosService.post_tarifa($scope.nueva_tarifa).then(function (res) {
            $scope.GetTarifasVigentes();
        });
        
        $scope.nueva_tarifa = {
            id: '',
            titulo: '',
            monto: '',
            descripcion: '',
            color: $scope.available_colors[$scope.available_colors.length-1],
            periodo_desde: '',
            periodo_hasta: ''
        }
    }

    $scope.GetTarifasVigentes = function () {
        FacturacionCentrosService.get_tarifas_vigentes().then(function (res) {
            $scope.tarifas_vigentes = res.data;
        });
    }

    $scope.aplicar_tarifa = function (tarifa) {
        $scope.reset_tarifas_rows();
    }

    $scope.reset_tarifas_rows = function () {
        for (var i = 0; i < $scope.selected_tarifa_row.length; i++) {
            $('#centro_' + $scope.selected_tarifa_row[i]).html("");
        }
        $scope.selected_tarifa_row = [];
    }

    $scope.select_tarifa_row = function (id) {
        $scope.selected_tarifa_row.push(id.substring(id.indexOf('_')+1));
    }

    $scope.format_date = function(date) {
        return date.toString().substring(4, 6) + "/" + date.toString().substring(0, 4);
    }

    $scope.deformat_date = function (date) {
        return date.toString().substring(3, 7) + date.toString().substring(0, 2);
    }

    $scope.GetTarifario = function () {
        FacturacionCentrosService.get_tarifario().then(function (res) {
            console.log('tarifario');
            console.log(res.data);
        });
    }

    $scope.GetTarifasVigentes();
    $scope.GetTarifario();
});