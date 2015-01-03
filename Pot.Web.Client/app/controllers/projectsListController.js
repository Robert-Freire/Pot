'use strict';

app.controller('projectsListController', ['$scope', 'projectsService', '$modal', '$route', '$location', function ($scope, projectsService, $modal, $route, $location) {

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

    function refresh() {
        $route.reload();
    };

    $scope.newProject = function () {
        var modalInstance = $modal.open({
            templateUrl: 'app/views/project.html',
            controller: 'projectNewController'
        });

        modalInstance.result.then(function (result) {
            refresh();
        });
    };

    $scope.deleteProject = function (projectId) {
        projectsService.deleteProject(projectId).then(function () {
            refresh();
        }, function (error) {
            alert('Error deleting' + error.data.message);
        });
    };

}]);


