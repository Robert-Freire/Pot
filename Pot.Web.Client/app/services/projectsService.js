'use strict';
app.factory('projectsService', ['$http', 'ngAuthSettings', function ($http, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;

    var projectsServiceFactory = {};

    var _getProjects = function () {

        return $http.get(serviceBase + 'api/projects').then(function (results) {
            return results;
        });
    };

    var _getProject = function (idProject) {

        return $http.get(serviceBase + 'api/projects/' + idProject).then(function (results) {
            return results;
        });
    };

    var _updateProject = function (project) {

        return $http.put(serviceBase + 'api/projects/' + project.idProject, project).then(function (results) {
            return results;
        });
    };

    var _insertProject = function (project) {

        return $http.post(serviceBase + 'api/projects', project).then(function (results) {
            return results;
        });
    };

    var _deleteProject = function (projectId) {

        return $http.delete(serviceBase + 'api/projects/' + projectId).then(function (results) {
            return results;
        });
    };

    projectsServiceFactory.getProjects = _getProjects;
    projectsServiceFactory.getProject = _getProject;
    projectsServiceFactory.insertProject = _insertProject;
    projectsServiceFactory.updateProject = _updateProject;
    projectsServiceFactory.deleteProject = _deleteProject;

    return projectsServiceFactory;

}]);