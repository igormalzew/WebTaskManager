var Task = {
    TaskId: -1,
    Name: '',
    Description: '',
    Priority: '',
    Date: '',
    Category: '',
    SpendTime: ''
};
var CategoryArray = [];

var taskArray = [{ taskName: "awd", setDate: "awd", spendTime: "awda", isPerformance: true },
                       { name: "a sdcsdcwd", date: "awd", spendTime: "awda", isPerformance: false }];

var TaskApp = angular.module("TaskApp", ["ngTable"]);

TaskApp.controller("TaskController", function ($scope, NgTableParams, $http) {

    var self = this;

    function getParams() {
        return new NgTableParams({}, {
            getData: function ($defer, params) {
                $http.get('GetTasks').success(function (data) {
                    $defer.resolve(data);
                });

            }
        });
    }

    self.tableParams = getParams();

    $scope.btnGetTasks = function () {
        alert();
        self.tableParams = getParams();
    }
});


