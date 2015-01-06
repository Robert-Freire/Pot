'use strict';

app.controller('projectController', ['$scope', 'projectsService', '$modalInstance', 'projectId', function ($scope, projectsService, $modalInstance, projectId) {

    $scope.project = {};
    $scope.message = '';

    init();

    function init() {

        if (projectId != null){
            loadProject(projectId);
        }
    };

    function loadProject(projectId) {
        projectsService.getProject(projectId).then(function (results) {
            $scope.project = results.data;
        }, function (error) {
            $scope.message = "Failed to load due to: " + error.data;
        });
    }

    $scope.saveProject = function () {
        if (projectId != null) {
            projectsService.updateProject($scope.project).then(function (results) {
                $modalInstance.close($scope.project);
            }, function (response) {
                if (response.data.constructor === Array) {
                    $scope.message = "Failed to save project due to: " + response.data.join(' ');
                }
                else {
                    $scope.message = "Failed to save project due to: " + response.data;
                }
            });
        }
        else {
            projectsService.insertProject($scope.project).then(function (results) {
                $modalInstance.close($scope.project);
            }, function (response) {
                if (response.data.constructor === Array) {
                    $scope.message = "Failed to save project due to: " + response.data.join(' ');
                }
                else {
                    if (response.data === Object) {
                        $scope.message = "Failed to save project due to: " + response.data.message;
                    }
                    else {
                        $scope.message = "Failed to save project due to: " + response.data;
                    }
                }
            });
        }
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    }

}]);