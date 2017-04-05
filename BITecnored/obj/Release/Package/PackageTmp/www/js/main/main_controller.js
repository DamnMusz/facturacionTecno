var navBar;

app.controller('MainController',
    function ($scope, $cookies, $route) {
        $scope.alert_text = '';
        $scope.navBar = {
        src: ""
    };
    navBar = $scope.navBar;
	$scope.currentUser = {};
	$scope.online = function () {
	    var v = $cookies.get('online');
	    return v == 'true';
	}
	$scope.userList = [];

	$scope.goOffline2 = function () {
	    $cookies.put('online', false);
	    $cookies.remove('nickname');
	    $cookies.remove('nombre');
	    $cookies.remove('apellido');
	}

	$scope.goOffline = function(){
		$cookies.put('online',false);
		$scope.updateName();
		$route.reload();
	}
	
	$scope.goOnline = function(username, nombre, apellido) {
	    $cookies.put('online', true);
	    $cookies.put('nickname', username);
	    $cookies.put('nombre', nombre);
	    $cookies.put('apellido', apellido);
	    $scope.updateName();
		$route.reload();
	}
	
	$scope.reload = function () {
	    location.reload();
	}

    $scope.updateName = function() {    
	    if (document.cookie) {
	        if ($scope.online()) {
	            $scope.currentUser = {
	                email: "cookie@prueba.com.ar",
	                avatar_url: "../img/user_img.png",
	                nickname: $cookies.get('nickname'),
	                nombre: $cookies.get('nombre'),
	                apellido: $cookies.get('apellido')
	            };
	        }
	    }
    }

    $scope.alert_error = function(text) {
        $scope.alert_text = text;
        $('#modalAlertDanger').modal('show');
    }

    $scope.alert_warning = function (text) {
        $scope.alert_text = text;
        $('#modalAlertWarning').modal('show');
    }

    $scope.alert_success = function (text) {
        $scope.alert_text = text;
        $('#modalAlertSuccess').modal('show');
    }

    $scope.updateName();

    $scope.show_menu = false;
    $scope.menu_fixed_open = false;

    $scope.toogle_sidebar = function (bool_value) {
        $scope.show_menu = bool_value;
    }

    $scope.toogle_fix_sidebar = function () {
        $scope.menu_fixed_open = !$scope.menu_fixed_open;
    }

    $scope.this_year = function () {
        return new Date().getFullYear();
    }

    $scope.imprimir = function () {
        var rightSide = document.getElementById("noMargin");
        var oldMargin = rightSide.style.marginLeft;
        rightSide.style.marginLeft = "0";
        window.print();
        rightSide.style.marginLeft = oldMargin;
    }

    $scope.padding = function (n, width, z) {
        z = z || '0';
        n = n + '';
        return n.length >= width ? n : new Array(width - n.length + 1).join(z) + n;
    }

    $scope.round = function (num) {
        return Math.abs(Math.round(num * 100) / 100);
    }

    $scope.round2 = function (num) {
        var value = Math.abs(Math.round(num * 10) / 10);
        return (value==100) ? value : value.toFixed(1);
    }

    $scope.porcentaje = function (porcion, total) {
        return $scope.round2((porcion / total) * 100);
    }

    $scope.json_to_csv = function (object, keys) {
        var objArray = angular.toJson(object);
        var array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;
        var str = '';
        
        var line = '';
        for (var index in keys) {
            if (line != '') line += ','
            line += keys[index]['alias'];
        }
        str += line + '\r\n';
        for (var i = 0; i < array.length; i++) {
            var line = '';
            for (var index in keys) {
                if (line != '') line += ','
                line += array[i][keys[index]['key']];
            }
            str += line + '\r\n';
        }
        return str;
    }

    $scope.download_as_file = function (data, filename) {
        var hiddenElement = document.createElement('a');

        document.body.appendChild(hiddenElement);
        hiddenElement.href = 'data:attachment/text,' + encodeURIComponent(data);
        hiddenElement.target = '_blank';
        hiddenElement.download = filename;
        hiddenElement.click();
    }
});
