﻿(function () {
    'use strict';

    angular.module('FileManager')
        .config(['$routeProvider', function ($routeProvider) {
            $routeProvider.when('/', {
                templateUrl: '/App/Templates/Directory/Index.html',
                requiresLogin: false,
                controller: 'DirectoryController'
            })
                .when('/Directories/GetDirectory', {
                    templateUrl: '/App/Templates/Directory/Index.html',
                    requiresLogin: false,
                    controller: 'DirectoryController'
                })
          .when('/Account/Login', {
              templateUrl: '/App/Templates/Account/Login.html',
              controller: 'LoginController'
          })
          .when('/Account/Register', {
              templateUrl: '/App/Templates/Account/Register.html',
              controller: 'RegisterController'
          })
          .otherwise({
              templateUrl: '/App/Templates/Shared/_404.html'
          })
        }])
        .run(checkAuthentication);

        checkAuthentication.$inject = ['$rootScope', '$location', 'tokenHandler'];
        function checkAuthentication($rootScope, $location, tokenHandler) {
            $rootScope.$on('$routeChangeStart', function (event, next, current) {
                var requiresLogin = next.requiresLogin || false;
                if (requiresLogin) {

                    var loggedIn = tokenHandler.hasLoginToken();

                    if (!loggedIn) {
                        $location.path('/Account/Login');
                    }
                }
            });
        }
})();