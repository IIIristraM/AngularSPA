'use strict';

// Demonstrate how to register services
// In this case it is a simple value service.
angular
    .module('app.services', [])
    .factory('$authHelper', function ($timeout) {
        var service = {};

        var accessTokenStorageKey = "accessToken";
        var storage = localStorage;

        service.updateAccessToken = function (accessToken, expiresIn) {
            storage[accessTokenStorageKey] = JSON.stringify({
                token: accessToken,
                expiresIn: moment().add("s", expiresIn)
            });
        };

        service.getAccessToken = function () {
            if (storage[accessTokenStorageKey]) {
                var accessToken = JSON.parse(storage[accessTokenStorageKey]);

                if (moment().isAfter(accessToken.expiresIn)) {
                    return null;
                }

                return accessToken;
            }

            return null;
        };

        service.getHttpHeaders = function () {
            var headers = {};

            var accessToken = service.getAccessToken();
            if (accessToken) {
                headers["Authorization"] = "Bearer " + accessToken.token;
            }

            return headers;
        };

        return service;
    })
    .factory('$account', function ($http, $authHelper) {
        var service = {};

        service.getUserInfo = function () {
            return $http.get("/api/Account/UserInfo");
        };

        service.logout = function () {
            return $http.get("/api/Account/Logout").then(function () {
                // TODO скорее всего надо что-то на сервере делать а не на клиенте
                $authHelper.updateAccessToken("", 0);
            });
        };

        return service;
    });