var AuthorizationApp = angular.module("AuthorizationApp", []);

AuthorizationApp.controller("Authorization", function ($scope, $http) {
    $scope.IsError = false;
    $scope.ErrorMessage = "";
    $scope.Login = "";
    $scope.Password = "";

    $scope.btnLogin = function () {
        if ($scope.Login === "" || $scope.Password === "") {
            $scope.IsError = true;
            $scope.ErrorMessage = "Заполните данные";
            return;
        }

        $http({ method: 'GET', url: 'Authorization/Login', params: { 'login': $scope.Login, 'userPassword': $scope.Password } }).
            success(function (data) {
                if (data.IsError === true) {
                    $scope.IsError = true;
                    $scope.ErrorMessage = data.ErrorMessage;
                }
            });

    }
});


