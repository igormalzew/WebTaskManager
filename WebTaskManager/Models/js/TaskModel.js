
var TaskApp = angular.module("TaskApp", ["ngTable"]);

TaskApp.controller("TaskController", function ($scope, NgTableParams, $http) {
    $scope.CategoryMain = true;
    $scope.PriorityMain = true;

    var self = this;

    function getParams(userfilter) {
        return new NgTableParams({}, {
            getData: function ($defer, params) {
                $http({
                    url: 'GetTasks', 
                    method: "GET",
                    params: { filter: userfilter }
                }).success(function (data) {
                    $defer.resolve(data);
                });

            }
        });
    }

    $http.get('GetCategory').success(function (data) {
        $scope.CategoryArray = data;
    });

    $scope.btnGetTasks = function (isInitTable) {
        if (isInitTable) {
            self.tableParams = getParams('');
            return;
        }

        var priorityFilter = [];
        for (var i = 0; i < $('.PriorityFilter').length; i++) {
            if ($('.PriorityFilter')[i].checked)
                priorityFilter.push($('.PriorityFilter')[i].id);
        }
        
        var categoryFilter = [];
        for (var i = 0; i < $('.CategoryFilter').length; i++) {
            if ($('.CategoryFilter')[i].checked)
                categoryFilter.push($('.CategoryFilter')[i].id);
        }

        var reportrange = $('#reportrange').prop("innerText");
        var startDate = reportrange.split(' - ')[0];
        var endDate = reportrange.split(' - ')[1];

        var isPerformanceFilter = $('#isPerformanceFilter').prop("checked");

        var filter = { priorityFilter, categoryFilter, startDate, endDate, isPerformanceFilter }

        self.tableParams = getParams(filter);
    }

    $scope.btnGetTasks(true);

    $scope.PriorityAllSelect = function () {
        $('.PriorityFilter').prop("checked", $scope.PriorityMain);
    }

    $scope.CategoryAllSelect = function () {
        $('.CategoryFilter').prop("checked", $scope.CategoryMain);
    }

});


