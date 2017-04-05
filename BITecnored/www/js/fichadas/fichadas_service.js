app.factory('FichadasService', function ($http, $q) {
    return {
        get_clocks_fichadores: function () {
            return $http({
                url: 'api/Fichador/GetClocks',
                method: 'GET'
            });
        }
    }
});