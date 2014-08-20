'use strict';

// Declares how the application should be bootstrapped. See: http://docs.angularjs.org/guide/module
angular.module('app', ['ui.router', 'app.filters', 'app.services', 'app.directives', 'app.controllers'])

    // Gets executed during the provider registrations and configuration phase. Only providers and constants can be
    // injected here. This is to prevent accidental instantiation of services before they have been fully configured.
    .config(function ($stateProvider, $locationProvider, $urlRouterProvider, $httpProvider) {

        // UI States, URL Routing & Mapping. For more info see: https://github.com/angular-ui/ui-router
        // ------------------------------------------------------------------------------------------------------------

        // External states
        $stateProvider.state('externalLogin', {
            external: true,
            url: '/ExternalLogin'
        });

        $stateProvider
            .state('home', {
                url: '/',
                templateUrl: '/views/index',
                controller: 'HomeCtrl'
            })
            .state('about', {
                url: '/about',
                templateUrl: '/views/about',
                controller: 'AboutCtrl'
            })
            .state('login', {
                url: '/login',
                // layout: 'basic',
                templateUrl: '/views/login',
                controller: 'LoginCtrl'
            })            
            .state('otherwise', {
                url: '*path',
                templateUrl: '/views/404',
                controller: 'Error404Ctrl'
            });      

        // hashPrefix allow to handle OAuth access token
        $locationProvider.html5Mode(true).hashPrefix('!');

        // Add headers for authentication and handle errors
        $httpProvider.interceptors.push(function ($q, $location, $authHelper) {
            return {
                request: function (request) {
                    var headers = $authHelper.getHttpHeaders();
                    for (var header in headers) {
                        request.headers[header] = headers[header];
                    }

                    return request;
                },
                responseError: function (responseError) {
                    if (responseError.status === 401) {
                        $location.url("/login");
                    }

                    return $q.reject(responseError);
                }
            };
        });
    })

    // Gets executed after the injector is created and are used to kickstart the application. Only instances and constants
    // can be injected here. This is to prevent further system configuration during application run time.
    .run(function ($templateCache, $rootScope, $state, $stateParams, $window, $http, $account, $authHelper, $location) {

        // <ui-view> contains a pre-rendered template for the current view
        // caching it will prevent a round-trip to a server at the first page load
        var view = angular.element('#ui-view');
        $templateCache.put(view.data('tmpl-url'), view.html());

        // Allows to retrieve UI Router state information from inside templates
        $rootScope.$state = $state;
        $rootScope.$stateParams = $stateParams;

        $rootScope.userInfo = null;

        $rootScope.logout = function () {           
            $account.logout().then(function () {
                $rootScope.userInfo = null;
                $state.go("login");
            });
        };

        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {

            // Perform hash
            var hash = null;
            try {
                hash = JSON.parse("{ \"" + $location.hash().replace(/=/g, "\" : \"").replace(/&/g, "\" , \"") + "\" }");
                if (hash.access_token) {
                    $authHelper.updateAccessToken(hash.access_token, hash.expires_in);
                }
            } catch (e) {
                // No hash or hash has wrong format
            }

            // Go to external resource
            if (toState.external) {
                event.preventDefault();
                $window.location.reload();
            }

            // No access token or access token expire
            else if (!$authHelper.getAccessToken() && (toState.name !== "login")) {
                event.preventDefault();
                $rootScope.userInfo = null;
                $state.go("login");
            } 
        });

        $rootScope.$on('$stateChangeSuccess', function (event, toState) {

            // Retrive user info
            if (!$rootScope.userInfo && (toState.name !== "login")) {
                $account.getUserInfo().then(function (result) {
                    $rootScope.userInfo = result.data;
                });
            }

            // Sets the layout name, which can be used to display different layouts (header, footer etc.)
            // based on which page the user is located
            $rootScope.layout = toState.layout;
        });
    });