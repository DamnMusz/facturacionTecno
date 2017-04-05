app.controller('ABMPeritosController', function ($scope, ABMPeritosService) {
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

    $scope.peritos = [
        {
            nombre: 'Juan',
            apellido: 'Amezcua',
            dni: 33669455,
            fecha_nacimiento: new Date('1988-02-25'),
            edad: 23,
            provincia: 'Buenos Aires',
            localidad: 'Campana',
            direccion: 'Sarlanga 1234',
            cv_path: '',
            observaciones: 'Estas son observaciones a tener en cuenta.',
            esTRD: false,
            fueTRD: false,
            nro_telefono: 45675435,
            mail: 'dmuzalski@tecno-red.com.ar',
            genero: 'M',
            baja_definitiva: false
        },
        {
            nombre: 'Julian',
            apellido: 'Weich',
            dni: 33849652,
            fecha_nacimiento: new Date('1983-06-19'),
            edad: 25,
            provincia: 'Buenos Aires',
            localidad: 'Lanus',
            direccion: 'Calle Falsa 123',
            cv_path: '',
            observaciones: 'Acá van las observaciones',
            esTRD: false,
            fueTRD: true,
            nro_telefono: 48413225,
            mail: 'dmuzalski@tecno-red.com.ar',
            genero: 'M',
            baja_definitiva: false
        }
    ];
});
