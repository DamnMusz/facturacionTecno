app.config(['$routeProvider',
  function ($routeProvider) {
      $routeProvider
          .when('/', {
              templateUrl: 'www/home.partial.html',
              controller: ''
          })
          .when('/sla', {
              templateUrl: 'www/sla.partial.html',
              controller: 'SLAController'
          })
          .when('/facturacion_siniestros', {
              templateUrl: 'www/facturacion_siniestros.partial.html',
              controller: 'FacturacionSiniestrosController'
          })
          .when('/login', {
              controller: 'LoginController',
              templateUrl: 'www/authentication/views/login.html'
          })
          .when('/abm_peritos', {
              controller: 'ABMPeritosController',
              templateUrl: 'www/abm_peritos.partial.html'
          })
          .when('/facturacion_inspectores', {
              controller: 'FacturacionInspectoresController',
              templateUrl: 'www/facturacion_inspectores.partial.html'
          })
          .when('/facturacion_centros', {
              controller: 'FacturacionCentrosController',
              templateUrl: 'www/facturacion_centros.partial.html'
          })
          .when('/fichadas', {
              controller: 'FichadasController',
              templateUrl: 'www/fichadas.partial.html'
          })
          .when('/resumen_ejecutivo', {
              templateUrl: 'www/resumen_ejecutivo.partial.html',
              controller: 'ResumenEjecutivoController'
          })
          .when('/resumen_semestral', {
              templateUrl: 'www/resumen_semestral.partial.html',
              controller: 'ResumenSemestralController'
          })
          .when('/resumen_semestral_centros', {
              templateUrl: 'www/resumen_semestral.partial.html',
              controller: 'ResumenSemestralCentrosController'
          })
          .when('/resumen_semestral_inspectores', {
              templateUrl: 'www/resumen_semestral.partial.html',
              controller: 'ResumenSemestralInspectoresController'
          })
          .when('/resumen_aseguradora', {
              templateUrl: 'www/resumen_aseguradora.partial.html',
              controller: 'ResumenAseguradoraController'
          })
      .otherwise({ redirectTo: '/login' });
      //.otherwise({ redirectTo: '/' });
	
  }])

.run(['$rootScope', '$location', '$cookieStore', '$http',
    function ($rootScope, $location, $cookieStore, $http) {
        // keep user logged in after page refresh
        $rootScope.globals = $cookieStore.get('globals') || {};
        if ($rootScope.globals.currentUser) {
            $http.defaults.headers.common['Authorization'] = 'Basic ' + $rootScope.globals.currentUser.authdata; // jshint ignore:line
        }

        $rootScope.$on('$locationChangeStart', function (event, next, current) {
            // redirect to login page if not logged in
            if ($location.path() !== '/login' && !$rootScope.globals.currentUser) {
                $location.path('/#/login');
            }
        });
    }])

.controller('IndexController', function ($scope) {
    $scope.message = 'This is index';
});
