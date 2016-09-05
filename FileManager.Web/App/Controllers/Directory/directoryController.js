(function () {
    'use strict';

    angular
        .module('FileManager')
        .controller('DirectoryController', DirectoryController);

    DirectoryController.$inject = ['$scope', '$q', 'DirectoryService', 'errorHandler', '$modal'];

    function DirectoryController($scope, $q, DirectoryService, errorHandler, $modal) {
        (function startup() {
            var directory = DirectoryService.getDirectory();
            
            $q.all([directory]).then(function (data) {
                if (data != null) {
                    $scope.directory = []; $scope.drives = []; $scope.directory = data[0];
                    var currentDrivesList = $scope.drives.concat($scope.directory[0].drives);
                    $scope.drives = currentDrivesList;
                }
            }, function (reason) {
                errorHandler.logServiceError('DirectoryController', reason);
            }, function (update) {
                errorHandler.logServiceNotify('DirectoryController', update);
            });
        })();

        $scope.gotoDir = function (path) { DirectoryService.getDirectory(path); };
        $scope.directory = [];
        $scope.drives = [];

        $scope.Commands = {
            checkDir: function (dir) {
                DirectoryService.checkDir(dir).then(
                    function (result) {
                        if (result.data != null) {
                            $scope.directory = []; $scope.drives = [];
                            $scope.directory.push(result.data[0]);
                            $scope.drives = $scope.directory[0].drives;
                        }
                    },
                    function (response) {
                      //  $scope.directory = response;
                        console.log(response);
                    });
            },
            openRoot: function (dir) {
                DirectoryService.openRoot(dir).then(
                    function (result) {
                        if (result.data != null) {
                            $scope.directory = []; $scope.drives = [];
                            $scope.directory.push(result.data[0]);
                            $scope.drives = $scope.directory[0].drives;
                        }
                    },
                    function (response) {
                        //  $scope.directory = response;
                        console.log(response);
                    });
            },
            openParrent: function (dir) {
                DirectoryService.openParrent(dir).then(
                    function (result) {
                        if (result.data != null) {
                            $scope.directory = []; $scope.drives = [];
                            $scope.directory.push(result.data[0]);
                            $scope.drives = $scope.directory[0].drives;
                        }
                    },
                    function (response) {
                        //  $scope.directory = response;
                        console.log(response);
                    });
            }
        };

        $scope.Queries = {
            getDirectories: function () {
                DirectoryService.getDirectories();
            },
            getDirectory: function (path) {
                DirectoryService.getDirectory(path);
            }
        };

        $scope.Actions = {
            openDir: function (dir) {
                $scope.Commands.checkDir(dir);
              //  $scope.directory = DirectoryService.checkDir(dir);
            },
            openRoot: function (dir) {
                $scope.Commands.openRoot(dir);
                //  $scope.directory = DirectoryService.checkDir(dir);
            },
            openParrent: function (dir) {
                $scope.Commands.openParrent(dir);
                //  $scope.directory = DirectoryService.checkDir(dir);
            }
        },
        $scope.Modals = {
        }
    };
})
();