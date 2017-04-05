app.controller('FichadasController', function ($scope, $timeout, FichadasService) {
    $scope.clocks = [];

    $scope.getClocks = function () {
        FichadasService.get_clocks_fichadores().then(function (res) {
            var dates = res.data;
            $scope.clocks = dates;
            for (i = 0; i < dates.length; i++) {
                var date = new Date($scope.parse_net_date(dates[i].value2));
                $scope.setDate($scope.clocks[i], date);
            }
        });
    };

    $scope.parse_net_date = function (date) {
        var a = date.replace(/\//g, "-").split(" ");
        var b = a[0].split("-");
        var aux_str = (b[2] + '-' + b[1] + '-' + b[0] + 'T' + a[1] + '.000-03:00')
        return new Date(aux_str)+'';
    }

    $scope.tick = function () {
        for (i = 0; i < $scope.clocks.length; i++) {
            var date = new Date($scope.clocks[i].value2);
            date.setSeconds(date.getSeconds() + $scope.tickInterval / 1000);
            $scope.$apply($scope.setDate($scope.clocks[i], date));
        }
    }

    $scope.setDate = function (index, date) {
        index.value2 = $scope.formatDate(date);
    }

    $scope.formatDate = function(date) {
        var d = new Date(date),
        month = '' + $scope.addZ((d.getMonth() + 1)),
        day = '' + $scope.addZ(d.getDate()),
        year = d.getFullYear();
        hour = $scope.addZ(d.getHours());
        minutes = $scope.addZ(d.getMinutes());
        seconds = $scope.addZ(d.getSeconds());

        if (month.length < 2) month = '0' + month;
        if (day.length < 2) day = '0' + day;

        return [year, month, day].join('-') + ' ' + [hour, minutes, seconds].join(':');
    }

    $scope.addZ = function (n) { return n < 10 ? '0' + n : '' + n; }

    $scope.getClocks();
    $scope.tickInterval = 1000 //ms
    setInterval(function () {
        $scope.tick();
    }, $scope.tickInterval);
});