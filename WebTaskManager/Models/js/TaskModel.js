
var TaskApp = angular.module("TaskApp", ["ngTable"])
.directive("categoryDirective", function () {
    return function (scope, element, attrs) {
        element.attr('data-on-text', scope.categoryCopy.CategoryName);
        element.attr('id', scope.categoryCopy.CategoryTypeId);
        element.bootstrapSwitch();
    }
});;

TaskApp.controller("TaskController", function ($scope, NgTableParams, $http) {
    $scope.CategoryMain = true;
    $scope.PriorityMain = true;

    $scope.IsTaskEdit = false;
    $scope.IsTimeEdit = false;


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

    function reloadCategory() {
        $http.get('GetCategory').success(function (data) {
            $scope.CategoryArray = data;
            return data;
        }).then(function (data) {
            $scope.CategoryArrayCopy = JSON.parse(JSON.stringify(data.data));
        });
    }

    reloadCategory();

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



    // Изменение категорий
    $scope.CategoryChanges = function() {
        $('#categoryChangeModal').modal();
        // $("[name='my-checkbox']").bootstrapSwitch();
    };

    $scope.AddNewCategory = function () {
        if ($scope.NewCategory === undefined || $scope.NewCategory === '') {
            $('#newCategoryId').css("border", "1px solid red");
            return;
        }

        var cat = { CategoryTypeId: 0, CategoryName: $scope.NewCategory, IsNewCategory: true };
        $scope.CategoryArrayCopy.push(cat);

        $('#newCategoryId').css('border', '');
        $scope.NewCategory = '';
    }

    $scope.CategoryRefresh = function() {
        $scope.CategoryArrayCopy = JSON.parse(JSON.stringify($scope.CategoryArray));
    }

    $scope.SaveChangedCategory = function () {
        if (JSON.stringify($scope.CategoryArray) === JSON.stringify($scope.CategoryArrayCopy))
            return;

        // Формируем список ид на удаление
        $scope.removeCategoryList = [];
        var arr = $scope.CategoryArrayCopy;
        var checkedArr = $("[name='my-checkbox']");
        for (var i = 0; i < arr.length; i++) {
            if (checkedArr[i].checked === false && arr[i].IsNewCategory !== true)
                $scope.removeCategoryList.push(arr[i].CategoryTypeId);
        }

        // Формируем список категорий на добавление
        $scope.addCategoryList = [];
        for (var i = 0; i < arr.length; i++) {
            if (checkedArr[i].checked !== false && arr[i].IsNewCategory === true)
                $scope.addCategoryList.push(arr[i].CategoryName);
        }

        // Отправка на сервер
        $http({
            url: 'AddCategory',
            method: "GET",
            params: { category: JSON.stringify($scope.addCategoryList) }
        }).success(function () {
            $http({
                url: 'RemoveCategory', 
                method: "GET",
                params: { category: JSON.stringify($scope.removeCategoryList) }
            }).success(function () {
                reloadCategory();
            });
        });

    }

    // Изменение задачи
    $scope.TaskChanges = function () {
        $('[data-toggle="tooltip"]').tooltip();
        $scope.IsTaskEdit = true;
        // $("[name='my-checkbox']").bootstrapSwitch();
    };

    $scope.TimeEdit = function() {
        $scope.IsTimeEdit = true;
        $('.spinbox').css("border", "1px solid red");

        $scope.HoursBufer = $('#HoursTime').val();
        $scope.MinutesBufer = $('#MinutesTime').val();
        $('#HoursTime').val('');
        $('#MinutesTime').val('');
    };

    $scope.TimeEditComplete = function () {
        $scope.IsTimeEdit = false;
        $('.spinbox').css("border", "");

        var h = $('#HoursTime').val();
        var m = $('#MinutesTime').val();

        var sumMinute = ((parseInt($scope.HoursBufer) + parseInt(h !== '' ? h : 0)) * 60) + parseInt($scope.MinutesBufer) + parseInt(m !== '' ? m : 0);
        $('#HoursTime').val(Math.floor(sumMinute / 60));
        $('#MinutesTime').val(sumMinute % 60);
    };

    $scope.TimeEditRevert = function () {
        $scope.IsTimeEdit = false;
        $('.spinbox').css("border", "");
        $('#HoursTime').val($scope.HoursBufer);
        $('#MinutesTime').val($scope.MinutesBufer);
    };
});


