app.factory('SLAService', function ($http, $q) {
    return {
        //getUsers: function () {
        //    return simpleHttpGet($http, '/api/user', $q.reject);
        //},

        //login: function (email, pass) {
        //    return $http({
        //        url: '/api/login',
        //        method: 'POST',
        //        data: { email: email, pass: pass }
        //    });
        //},

        //listar: function () {
        //    return $http({
        //        url: 'api/User',
        //        method: 'GET'
        //    });
        //},

        getAseguradoras: function () {
            return $http({
                url: 'api/Aseguradoras',
                method: 'GET'
            });
        },

        getProvinciasFiltro: function () {
            return $http({
                url: 'api/get_provincias_sla',
                method: 'GET'
            });
        },

        existsSLA: function (aseguradora_id,periodo) {
            if (aseguradora_id != -1) {
                return $http({
                    url: 'api/check_sla_base_existence?aseguradora_id=' + aseguradora_id + '&mes_anio=' + periodo,
                    method: 'GET'
                });
            } else {
                return $http({
                    url: 'api/check_sla_base_existence?mes_anio=' + periodo,
                    method: 'GET'
                });
            }
        },

        getTiempoGestion: function (aseguradora, periodo, provincia) {
            if (aseguradora != -1) {
                return $http({
                    url: 'api/Tiempos_Gestion_Previas?aseguradora_id=' + aseguradora + '&periodo=' + periodo + '&provincia_id=' + provincia,
                    method: 'GET'
                });
            } else {
                return $http({
                    url: 'api/Tiempos_Gestion_Previas?periodo=' + periodo + '&provincia_id=' + provincia,
                    method: 'GET'
                });
            }
            
        },

        getProporcionRealizadas: function (aseguradora, mesActual, mesesHaciaAtras, provincia) {
            if (aseguradora != -1) {
                return $http({
                    url: 'api/ProporcionRealizadas?aseguradoraID=' + aseguradora + '&mesActual=' + mesActual + '&mesesHaciaAtras=' + mesesHaciaAtras + '&provincia_id=' + provincia,
                    method: 'GET'
                });
            } else {
                return $http({
                    url: 'api/ProporcionRealizadas?mesActual=' + mesActual + '&mesesHaciaAtras=' + mesesHaciaAtras + '&provincia_id=' + provincia,
                    method: 'GET'
                });
            }
        },

        getMotivosNoRealizadas: function (aseguradora, mesActual, provincia) {
            if (aseguradora != -1) {
                return $http({
                    url: 'api/MotivosNoRealizadas?aseguradoraID=' + aseguradora + '&mesActual=' + mesActual + '&provincia_id=' + provincia,
                    method: 'GET'
                });
            } else {
                return $http({
                    url: 'api/MotivosNoRealizadas?mesActual=' + mesActual + '&provincia_id=' + provincia,
                    method: 'GET'
                });
            }
        },

        getMotivosDiaPactado: function (aseguradora, mesActual, provincia_id) {
            if (aseguradora != -1) {
                return $http({
                    url: 'api/MotivosDiaPactado?aseguradora_id=' + aseguradora + '&periodo=' + mesActual + '&provincia_id=' + provincia_id,
                    method: 'GET'
                });
            } else {
                return $http({
                    url: 'api/MotivosDiaPactado?periodo=' + mesActual + '&provincia_id=' + provincia_id,
                    method: 'GET'
                });
            }
        },


        getCantidadFotos: function (aseguradora, periodo, provincia_id) {
            if (aseguradora != -1) {
                return $http({
                    url: 'api/CantidadFotos?aseguradora_id=' + aseguradora + '&periodo=' + periodo + '&provincia_id=' + provincia_id,
                    method: 'GET'
                });
            } else {
                return $http({
                    url: 'api/CantidadFotos?periodo=' + periodo + '&provincia_id=' + provincia_id,
                    method: 'GET'
                });
            }
        },

        getBaseSLA: function (mes_anio) {
            return $http({
                url: "api/generate_sla_base?mes_anio=" + mes_anio +"",
                cache: false,
                method: 'GET'
            });
        },

        getEstadoBaseSLA: function () {
            return $http({
                url: 'api/get_state_sla_base',
                cache: false,
                method: 'GET'
            });
        },
        
        cancelBaseSLA: function () {
            return $http({
                url: 'api/cancel_sla_base',
                cache: false,
                method: 'GET'
            });
        },

        getRealizadasCentro: function (aseguradora, periodo, provincia) {
            return $http({
                url: 'api/nro_insp_centro?aseguradora_id=' + aseguradora + '&periodo=' + periodo + '&provincia_id=' + provincia,
                cache: false,
                method: 'GET'
            });
        },

        getRealizadasDomicilio: function (aseguradora, periodo, provincia) {
            return $http({
                url: 'api/nro_insp_domicilio?aseguradora_id=' + aseguradora + '&periodo=' + periodo + '&provincia_id=' + provincia,
                cache: false,
                method: 'GET'
            });
        }
    };
});
