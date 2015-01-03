'use strict';

app.controller('projectNewController', ['$scope', 'projectsService', '$modalInstance', function ($scope, projectsService, $modalInstance) {

    $scope.project = {};
    $scope.message = '';

    $scope.saveProject = function () {

        projectsService.insertProject($scope.project).then(function (results) {
            $modalInstance.close();
        }, function (response) {
            if (response.data.constructor === Array) {
                $scope.message = "Failed to save project due to: " + response.data.join(' ');
            }
            else {
                if (response.data === Object) {
                    $scope.message = "Failed to save project due to: " + response.data.message;
                } else {
                    $scope.message = "Failed to save project due to: " + response.data;
                }
            }
        });
    };

    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    }

}]);