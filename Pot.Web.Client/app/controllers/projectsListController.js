'use strict';

app.controller('projectsListController', ['$scope', 'projectsService', '$modal', '$route', function ($scope, projectsService, $modal, $route) {

    $scope.projects = [];

    init();

    function init() {

        loadProjects();
    };

    function loadProjects() {

        projectsService.getProjects().then(function (results) {

            $scope.projects = results.data;

        }, function (error) {
            alert('Error loading' + error.data.message);
        });
    };

    function getIndexSelectedProject(projectId) {
        for (var i = 0; i < $scope.projects.length; i++) {
            if ($scope.projects[i].projectId == projectId) {
                return i;
            }
        }
        return null;
    }


    $scope.newProject = function () {
        var modalInstance = $modal.open({
            templateUrl: 'app/views/project.html',
            controller: 'projectController'
        });

        modalInstance.result.then(function (result) {
            refresh();
        });
    };

    $scope.editProject = function (projectId) {
        var modalInstance = $modal.open({
            templateUrl: 'app/views/project.html',
            controller: 'projectController',
            resolve: { projectId: function () { return projectId; } }
        });

        modalInstance.result.then(function (result) {
            var index = getIndexSelectedProject(projectId);
            $scope.projects[index].name = result.name;
        });
    };

    $scope.deleteProject = function (projectId) {
        projectsService.deleteProject(projectId).then(function () {
            var index = getIndexSelectedProject(projectId);
            $scope.projects.splice(index, 1);
        }, function (error) {
            alert('Error deleting' + error.data.message);
        });
    };

}]);


