(function () {
    'use strict';

    angular
        .module('FileManager')
        .factory('DirectoryService', DirectoryService);

    DirectoryService.$inject = ['$resource', '$q', '$http'];

    function DirectoryService($resource, $q, $http) {
        var resource = $resource('/api/Directories/:action/:param', { action: '@action', param: '@param' }, {
            'update': { method: 'PUT' }
        });

        var _getDirectories = function () {
            $("#filemanager").hide();
            $("#preloader").show();
            var deferred = $q.defer();
            resource.query({ action: "get", param: ""},
				function (result) {
				    $("#preloader").hide();
				    $("#filemanager").show();
				    if (result == null) {
				        result = [];
				    };
				    deferred.resolve(result);
				},
				function (response) {
				    $("#preloader").hide();
				    $("#filemanager").show();
				    deferred.reject(response);
				});
            return deferred.promise;

        };

        var _getDirectory = function (path) {
            $("#filemanager").hide();
            $("#preloader").show();
            var deferred = $q.defer();
            resource.query({ action: 'getDirectory', param: path },
				function (result) {
				    $("#preloader").hide();
				    $("#filemanager").show();
				    if (result == null) {
				        result = [];
				    };

				    deferred.resolve(result);
				},
				function (response) {
				    $("#preloader").hide();
				    $("#filemanager").show();
				    deferred.reject(response);
				});
            return deferred.promise;
        };

        var _checkDir = function (dir) {
            $("#filemanager").hide();
            $("#preloader").show();
            var deferred = $q.defer();
            $http.post('/api/Directories/checkDir', dir)
                .then(function (result) {
                    $("#preloader").hide();
                    $("#filemanager").show();
                    deferred.resolve(result);
                },
                        function (response) {
                            $("#preloader").hide();
                            $("#filemanager").show();
                            deferred.reject(response);
                        });
            return deferred.promise;
        };
        var _openRoot = function (dir) {
            $("#filemanager").hide();
            $("#preloader").show();
            var deferred = $q.defer();
            $http.post('/api/Directories/openRoot', dir)
                .then(function (result) {
                    $("#preloader").hide();
                    $("#filemanager").show();
                    deferred.resolve(result);
                },
                        function (response) {
                            $("#preloader").hide();
                            $("#filemanager").show();
                            deferred.reject(response);
                        });
            return deferred.promise;
        };
        var _openParrent = function (dir) {
            $("#filemanager").hide();
            $("#preloader").show();
            var deferred = $q.defer();
            $http.post('/api/Directories/openParrent', dir)
                .then(function (result) {
                    $("#preloader").hide();
                    $("#filemanager").show();
                    deferred.resolve(result);
                },
                        function (response) {
                            $("#preloader").hide();
                            $("#filemanager").show();
                            deferred.reject(response);
                        });
            return deferred.promise;
        };

        return {
            getDirectories: _getDirectories,
            getDirectory: _getDirectory,
            checkDir: _checkDir,
            openRoot: _openRoot,
            openParrent: _openParrent,
        };

    }

})();