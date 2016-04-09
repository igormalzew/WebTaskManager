var data = [{ name: "Moroni", age: 50, datetime: 1450562394000 },
               { name: "Simon", age: 43, datetime: 1450562394000 },
               { name: "Jacob", age: 27, datetime: 1450648794000 },
               { name: "Nephi", age: 29, datetime: 1450648794000 },
               { name: "Christian", age: 34, datetime: 1450475994000 },
               { name: "Tiancum", age: 43, datetime: 1450475994000 },
               { name: "Jacob", age: 27, datetime: 1450475994000 },
                { name: "Simon", age: 43, datetime: 1450562394000 },
               { name: "Jacob", age: 27, datetime: 1450648794000 },
               { name: "Nephi", age: 29, datetime: 1450648794000 },
               { name: "Christian", age: 34, datetime: 1450475994000 },
               { name: "Tiancum", age: 43, datetime: 1450475994000 },
               { name: "Jacob", age: 27, datetime: 1450475994000 },
                { name: "Simon", age: 43, datetime: 1450562394000 },
               { name: "Jacob", age: 27, datetime: 1450648794000 },
               { name: "Nephi", age: 29, datetime: 1450648794000 },
               { name: "Christian", age: 34, datetime: 1450475994000 },
               { name: "Tiancum", age: 43, datetime: 1450475994000 },
               { name: "Jacob", age: 27, datetime: 1450475994000 }
];

var TaskApp = angular.module("TaskApp", ["ngTable"]);

TaskApp.controller("TaskController", function ($scope, NgTableParams) {
    this.tableParams = new NgTableParams({}, { dataset: data });
});


