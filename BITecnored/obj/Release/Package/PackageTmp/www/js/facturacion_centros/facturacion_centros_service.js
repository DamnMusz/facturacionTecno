app.factory('FacturacionCentrosService', function ($http, $q) {
    return {
        get_all_centros: function (p, v_s, activos) {
            return $http({
                url: 'api/get_all_centros?page=' + p + '&view_size=' + v_s + '&activos=' + activos,
                method: 'GET'
            });
        },

        get_filtered_centros: function (p, v_s, data, activos) {
            return $http({
                url: 'api/buscar_centros?page=' + p + '&view_size=' + v_s + '&activos=' + activos,
                data: data,
                method: 'POST'
            });
        },

        get_amount_centros: function (data, activos) {
            return $http({
                url: 'api/get_amount_centros' + '?activos=' + activos,
                data: data,
                method: 'POST'
            });
        },

        get_centros_incompletos: function (p, v_s) {
            return $http({
                url: 'api/buscar_centros_incompletos?page=' + p + '&view_size=' + v_s,
                method: 'GET'
            });
        },

        get_centros_tarifas_incompletas: function (p, v_s) {
            return $http({
                url: 'api/buscar_centros_tarifas_incompletas?page=' + p + '&view_size=' + v_s,
                method: 'GET'
            });
        },

        get_amount_centros_tarifas_incompletas: function () {
            return $http({
                url: 'api/get_amount_centros_tarifas_incompletas',
                method: 'GET'
            });
        },
        
        get_amount_centros_incompletos: function () {
            return $http({
                url: 'api/get_amount_centros_incompletos',
                method: 'GET'
            });
        },

        get_all_provincias: function () {
            return $http({
                url: 'api/get_provincias',
                method: 'GET'
            });
        },

        get_localidades: function (provincia) {
            return $http({
                url: 'api/get_localidades?provincia='+provincia,
                method: 'GET'
            });
        },

        get_afinidades_tarfiarias: function () {
            return $http({
                url: 'api/afinidades_tarfiarias',
                method: 'GET'
            });
        },

        post_afinidad_tarfiaria: function (nombre) {
            return $http({
                url: 'api/afinidades_tarfiarias?nombre='+nombre,
                method: 'POST'
            });
        },

        delete_afinidad_tarfiaria: function (id) {
            return $http({
                url: 'api/afinidades_tarfiarias?id=' + id,
                method: 'DELETE'
            });
        },

        unlock: function() {
            return $http({
                url: 'api/centros_unlock',
                method: 'GET'
            });
        },

        update_centro: function (data) {
            data.cuit = "\"" + data.cuit + "\"";
            data.cuit = data.cuit.replace(/-/g, '');
            data.cuit = data.cuit.replace(/"/g, '');
            return $http({
                url: 'api/update_centro',
                method: 'POST',
                data: data,
                dataType: 'json',
                contentType: 'application/json',
            });
        },

        count_tarifas: function (id_centro) {
            return $http({
                url: 'api/Tarifas/Count?id_centro=' + id_centro,
                method: 'GET'
            });
        },

        get_tarifas: function (id_centro, p, v_s) {
            return $http({
                url: 'api/Tarifas?id_centro='+id_centro+'&page=' + p + '&view_size=' + v_s,
                method: 'GET'
            });
        },

        post_tarifa: function (tarifa) {
            return $http({
                url: 'api/Tarifas',
                data: tarifa,
                dataType: 'json',
                contentType: 'application/json',
                method: 'POST'
            });
        },

        put_tarifa: function (tarifa) {
            return $http({
                url: 'api/Tarifas/' + id_centro,
                data: tarifa,
                dataType: 'json',
                contentType: 'application/json',
                method: 'PUT'
            });
        },
    }
});