app.factory('FacturacionSiniestrosService', function ($http, $q) {
    return {
        generar_txt_gecom: function (data) {
            return $http({
                url: 'api/GecomFile',
                method: 'POST',
                data: data,
                dataType: 'json',
                contentType: 'application/json',
            });
        }
    }
});