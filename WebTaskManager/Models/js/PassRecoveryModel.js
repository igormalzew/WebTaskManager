var PassRecoveryApp = angular.module("PassRecoveryApp", []);

PassRecoveryApp.controller("PassRecovery", function ($scope, $http) {
    // Заявка на восстановление
    $scope.recoveryEmailErMs = "";
    

    $scope.btnRecovery = function () {
        
        $scope.recoveryEmailErMs = "";

        if (!/.+@.+\..+/.test($scope.recoveryEmail)) {
            $scope.recoveryEmailErMs = "Некорректный Email";
            return;
        }

        $http({
            method: 'GET', url: 'PassRecoveryRequest', params: {
                'email': $scope.recoveryEmail
            }
        }).
            success(function (data) {
                $scope.recoveryEmailErMs = data.ErrorMessage;
                if (data.IsError === false) {
                    $('#emailRecoveryModal').modal();
                }
            });

    }

    $scope.recoveryEmailErMs = "";

    // Сохранение нового пароля
    $scope.btnSaveNewPass = function () {

        $scope.recoveryPasswordErMs = "";
        $scope.recoveryPasswordRepeatErMs = "";

        if (typeof ($scope.recoveryPassword) === "undefined" || $scope.recoveryPassword === '') {
            $scope.recoveryPasswordErMs = "Заполните пароль";
        } else {

            if (!/(?=^.{8,20}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$/.test($scope.recoveryPassword)) {
                $scope.recoveryPasswordErMs = "Пароль должен состоять из латинских букв в разных регистрах и цифр. Длина пароля от 8 до 20 символов.";
                return;
            }
        }
        if ($scope.recoveryPasswordRepeat !== $scope.recoveryPassword) {
            $scope.recoveryPasswordRepeatErMs = "Пароли не совпадают";
            return;
        }

        $http({
            method: 'GET',
            url: 'SaveRecoveryPass',
            params: {
                'hash': $('#PasswordHash').attr('name'),
                'pass': $scope.recoveryPassword
            }
        }).
            success(function (data) {
                if (data.IsError === false) {
                    $scope.modalMessage = 'Пароль успешно сохранен';
                    $('#newPassSavedModal').modal();
                } else {
                    $scope.modalMessage = 'Непредвиденная ошибка. Повторите еще раз';
                    $('#newPassSavedModal').modal();
                }
            });

    }

    $scope.IndexRedirect = function() {
        window.location.replace("/");
    }
});


