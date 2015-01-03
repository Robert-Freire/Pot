
var app = angular.module('PotApp', ['ngRoute', 'LocalStorageModule', 'ui.bootstrap']);

app.config(function ($routeProvider) {

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "/app/views/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/app/views/login.html"
    });

    $routeProvider.when("/signup", {
        controller: "signupController",
        templateUrl: "/app/views/signup.html"
    });

    $routeProvider.when("/projects", {
        controller: "projectsListController",
        templateUrl: "/app/views/projects.html"
    });

    $routeProvider.when("/projects/:projectId", {
        controller: "projectEditController",
        templateUrl: "/app/views/project.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });

});

var serviceBase = 'http://localhost:28611/';
app.constant('ngAuthSettings', {
    apiServiceBaseUri: serviceBase,
    clientId: 'ngTestWebApi'
});

//app.config(function ($httpProvider) {
//    $httpProvider.interceptors.push('authInterceptorService');
//});

app.run(['authService', function (authService) {
       authService.fillAuthData();
}]);


