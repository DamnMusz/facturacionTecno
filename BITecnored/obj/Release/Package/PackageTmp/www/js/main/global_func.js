function simpleHttpGet($http, serviceUrl, rejectHandler) {
    return $http({
        url: url,
        method: 'GET'
    }).then(function (response) {
        return response.data;
    }, function (response) {
        // something went wrong
        return rejectHandler(response);
    });
}
