'use strict';

app.controller('projectEditController', ['$scope', 'projectsService', '$routeParams', '$location', function ($scope, projectsService, $routeParams, $location) {

    $scope.project = [];
    $scope.message = '';

    loadProject($routeParams.projectId);

    function loadProject(projectId) {
        projectsService.getProject(projectId).then(function (results) {
            $scope.project = results.data;
        }, function (error) {
            $scope.message = "Failed to load due to: " + error.data;
        });
    }

    $scope.saveProject = function () {

        projectsService.updateProject($scope.project).then(function (results) {
            $location.path('/projects');
        }, function (response) {
            if (response.data.constructor === Array) {
                $scope.message = "Failed to save project due to: " + response.data.join(' ');
            }
            else {
                $scope.message = "Failed to save project due to: " + response.data;
            }
        });
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    }

}]);