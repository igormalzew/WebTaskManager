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

var TaskApp = angular.module("TaskApp", ["ngTable"]);

TaskApp.controller("TaskController", function ($scope, NgTableParams, $http) {
    var taskArray = [{ name: "awd", date: "awd", spendTime: "awda", isPerformance: true },
                        { name: "a sdcsdcwd", date: "awd", spendTime: "awda", isPerformance: false }];

    this.tableParams = new NgTableParams({}, { dataset: taskArray });


    $scope.btnGetTasks = function () {
        $http({
            method: 'GET', url: 'GetTasks'
        }).
           success(function (data) {
               $scope.taskArray = data.Data;
           });
    }
});


