var RegistrationApp = angular.module("RegistrationApp", []);

RegistrationApp.controller("Registration", function ($scope, $http) {
    $scope.IsError = false;
    $scope.RegAnswerMsg = "";

    $scope.NameErMs = "";
    $scope.LoginErMs = "";
    $scope.PasswordErMs = "";
    $scope.PasswordCloneErMs = "";
    $scope.EmailErMs = "";
    $scope.BirthDayErMs = "";

    $scope.btbRegister = function () {
        $scope.IsError = false;
        $scope.RegAnswerMsg = "";

        $scope.NameErMs = "";
        $scope.LoginErMs = "";
        $scope.PasswordErMs = "";
        $scope.PasswordCloneErMs = "";
        $scope.EmailErMs = "";
        $scope.BirthDayErMs = "";

        if (typeof ($scope.Name) === "undefined" || $scope.Name === '') {
            $scope.NameErMs = "Заполните имя";
        }
        if (typeof ($scope.Login) === "undefined" || $scope.Login === '') {
            $scope.LoginErMs = "Заполните логин";
        } else {
            if(!/^[a-zA-Z][a-zA-Z0-9-_\.]{1,20}$/.test($scope.Login)) {
                $scope.LoginErMs = "Логин может состоять из латинских букв и цифр. Должен начинаться с буквы.";
            }
        }
        if (typeof ($scope.Password) === "undefined" || $scope.Password === '') {
            $scope.PasswordErMs = "Заполните пароль";
        } else {

            if(!/(?=^.{8,20}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$/.test($scope.Password)) {
                $scope.PasswordErMs = "Пароль должен состоять из латинских букв в разных регистрах и цифр. Длина пароля от 8 до 20 символов.";
            }
        }
        if ($scope.Password !== $scope.PasswordClone) {
            $scope.PasswordCloneErMs = "Пароли не совпадают";
        }
        if (!/.+@.+\..+/.test($scope.Email)) {
            $scope.EmailErMs = "Некорректный Email";
        }

        if (typeof ($scope.BirthDay) !== "undefined") {
            if ($scope.BirthDay !== "" && !/([0-2]\d|3[01])\.(0\d|1[012])\.(\d{4})/.test($scope.BirthDay)) {
                $scope.BirthDayErMs = "Заполните корректно дату";
            }
        }

        if ($scope.NameErMs !== "" || $scope.LoginErMs !== "" || $scope.PasswordErMs !== "" || $scope.PasswordCloneErMs !== ""
            || $scope.EmailErMs !== "" || $scope.BirthDayErMs !== "") {
            return;
        }


        $http({
            method: 'GET', url: 'AddNewUser', params: {
                'name': $scope.Name, 'login': $scope.Login, 'password': $scope.Password,
                'email': $scope.Email, 'birthDay': $scope.BirthDay
            }
        }).
            success(function (data) {
                $scope.IsError = data.IsError;
                $scope.RegAnswerMsg = data.ErrorMessage;
            });

    }

    $scope.IndexRedirect = function() {
        window.location.replace("/");
    }
});


