app.factory('ResumenSemestralService', function ($http, $q) {
    return {
        getAseguradoras: function () {
            return $http({
                url: 'api/AseguradorasList',
                method: 'GET'
            });
        },

        getTiempoGestionPorAseguradora: function (periodo) {
            return $http({
                url: 'api/Tiempos_Gestion_Previas_Por_Aseguradora?periodo=' + periodo,
                method: 'GET'
            });
        },

        getRealizadasPorAseguradora: function (periodo) {
            return $http({
                url: 'api/Realizadas_Por_Aseguradora?periodo=' + periodo,
                method: 'GET'
            });
        },

        getFotosPorAseguradora: function (periodo) {
            return $http({
                url: 'api/Fotos_Por_Aseguradora?periodo=' + periodo,
                method: 'GET'
            });
        },

        getNoRealizadasPorAseguradora: function (periodo) {
            return $http({
                url: 'api/No_Realizadas_Previas_Por_Aseguradora?periodo=' + periodo,
                method: 'GET'
            });
        },

        getRealizadasSemestral: function (periodo) {
            return $http({
                url: 'api/resumen_semestral?periodo=' + periodo,
                method: 'GET'
            });
        },

        getRealizadasSemestralInspectores: function (periodo) {
            return $http({
                url: 'api/resumen_semestral_inspectores?periodo=' + periodo,
                method: 'GET'
            });
        },

        getRealizadasSemestralCentros: function (periodo) {
            return $http({
                url: 'api/resumen_semestral_centros?periodo=' + periodo,
                method: 'GET'
            });
        },
    }
});