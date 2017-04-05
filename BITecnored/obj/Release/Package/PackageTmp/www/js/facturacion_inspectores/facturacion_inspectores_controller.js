app.controller('FacturacionInspectoresController', function ($scope, FacturacionInspectoresService) {
    //$(function () {
        //$('#tablaPeritos').DataTable({
        //    'paging': true,
        //    'lengthChange': false,
        //    'searching': true,
        //    'ordering': true,
        //    'info': true,
        //    'autoWidth': false
        //});
    //});

    // 0: inspectores | 1: facturacion
    $scope.selection = 0;

    $scope.select = function (value) {
        $scope.selection = value;
    }

    $scope.peritos = [
        {
            nombre: 'Juan',
            apellido: 'Amezcua',
            plus: 100,
            minimo: 1500,
            vinculo: 'Centro Tercerizado',
            iva: false,
            productividad: 100,
            limite_km: 100,
            lunes: true,
            martes: true,
            miercoles: false,
            jueves: false,
            viernes: false,
            sabado: false
        },
        {
            nombre: 'Julian',
            apellido: 'Weich',
            plus: 200,
            minimo: 1000,
            vinculo: 'Relacion',
            iva: true,
            productividad: 300,
            limite_km: 50,
            lunes: true,
            martes: false,
            miercoles: true,
            jueves: false,
            viernes: true,
            sabado: true
        }
    ];
});