var AuthorizationApp = angular.module("AuthorizationApp", []);

AuthorizationApp.controller("Authorization", function ($scope) {
    $scope.IsError = false;
    $scope.ErrorMessage = "";
    $scope.Login = "";
    $scope.Password = "";

    $scope.btnLogin = function() {
        if ($scope.Login === "" || $scope.Password === "") {
            $scope.IsError = true;
            $scope.ErrorMessage = "Заполните данные";
        }
    }
});


