
var TaskApp = angular.module("TaskApp", ["ngTable"])
.directive("categoryDirective", function () {
    return function (scope, element, attrs) {
        element.attr('data-on-text', scope.categoryCopy.CategoryName);
        element.attr('id', scope.categoryCopy.CategoryTypeId);
        element.bootstrapSwitch();
    }
})
.directive("tasktimeDirective", function() {
    return function (scope, element, attrs) {
        scope.user.timeFormat = Math.floor(scope.user.SpendTime / 60) + ':' + scope.user.SpendTime % 60;
        }
    });

TaskApp.controller("TaskController", function ($scope, NgTableParams, $http) {
    $scope.CategoryMain = false;
    $scope.PriorityMain = true;

    $scope.IsTaskEdit = false;
    $scope.IsTimeEdit = false;

    $scope.taskPriorityOption = '2';

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
    $scope.TaskChanges = function (taskData) {
        $scope.IsNewTask = taskData === undefined;
        $scope.IsTaskEdit = true;
        $scope.taskInput = $scope.mainTaskInput;
        $scope.mainTaskInput = '';

        $('#IsPerformanceTask').bootstrapSwitch('state', false, true);

        $('[data-toggle="tooltip"]').tooltip();
        $('#taskCategorySelect').multiselect({
            buttonWidth: '100%',
            dropRight: true,
            nonSelectedText: 'Не выбрано',
            nSelectedText: ' категорий',
            allSelectedText: 'Выбраны все категории'
        });
        if (taskData !== undefined && taskData !== null) {
            $scope.currentEditTaskId = taskData.TaskId;

            $scope.taskInput = taskData.TaskName;
            $scope.taskDescription = taskData.FullDescription;
            $scope.taskPriorityOption = taskData.PriorityId;
            $('#taskCategorySelect').multiselect('select', taskData.Category);
            $('#dateTimeTask').val(taskData.SetDate);

            $('#IsPerformanceTask').bootstrapSwitch('state', Boolean(taskData.IsPerformance), true);

            if (taskData.SpendTime !== null) {
                $('#HoursTime').val(Math.floor(taskData.SpendTime / 60));
                $('#MinutesTime').val(taskData.SpendTime % 60);
                $scope.hTask = Math.floor(taskData.SpendTime / 60);
                $scope.mTask = taskData.SpendTime % 60;
            }
        }
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

    $scope.TaskCancel = function() {
        $scope.IsTaskEdit = false;
        $scope.taskInput = '';
        $scope.mainTaskInput = '';
        $scope.taskDescription = '';
        $scope.taskPriorityOption = 2;
        $('#taskCategorySelect').val('');
        $('#taskCategorySelect').multiselect('rebuild');
        $('#dateTimeTask').val(moment().format('DD.MM.YYYY'));
        $('#HoursTime').val(0);
        $('#MinutesTime').val(0);

    };

    $scope.GetTaskData = function (){
        var h = $('#HoursTime').val();
        var m = $('#MinutesTime').val();

        var name = $scope.taskInput;
        var descriptyon = $scope.taskDescription;
        var priority = $scope.taskPriorityOption;
        var taskCategory = $('#taskCategorySelect').val();
        var taskDate = $('#dateTimeTask').val();
        var spendTime = (parseInt(h !== '' ? h : 0) * 60) + parseInt(m !== '' ? m : 0);

        var isPerformance = $('#IsPerformanceTask').bootstrapSwitch('state');

        var task = { name, descriptyon, priority, taskCategory, taskDate, spendTime, isPerformance };
        return task;
    }

    $scope.AddNewTask = function(isFastAdding) {
        var taskData = $scope.GetTaskData();

        if (isFastAdding) {
            if ($scope.mainTaskInput === '' || $scope.mainTaskInput === undefined) return;

            taskData.name = $scope.mainTaskInput;
            $scope.mainTaskInput = '';
        } else {
            if ($scope.taskInput === '' || $scope.taskInput === undefined) return;
        }


        $http({
            url: 'AddNewTask',
            method: "GET",
            params: { taskData: taskData }
        }).success(function () {
            $scope.btnGetTasks();
        });

        $scope.TaskCancel();
    }

    $scope.SaveTask = function() {
        var taskData = $scope.GetTaskData();
        taskData.taskId = $scope.currentEditTaskId;
        $http({
            url: 'SaveTask',
            method: "GET",
            params: { taskData: taskData }
        }).success(function() {
            $scope.btnGetTasks();
        });
    };

    $scope.RemoveTaskAsk = function ()
    {
        $('#removeTaskAskModal').modal();
    }

    $scope.RemoveTask = function () {
        $http({
            url: 'RemoveTask',
            method: "GET",
            params: { taskId: $scope.currentEditTaskId }
        }).success(function () {
            $scope.btnGetTasks();
        });
    }
});


