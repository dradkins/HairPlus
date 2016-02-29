(function (module) {

    var expenseService = function ($http) {

        var getExpenses = function (pagingInfo) {
            return $http.get("/api/expense/getAll", { params: pagingInfo }, function (response) {
                return response.data;
            })
        }

        var getExpense = function (eId) {
            return $http.get("/api/expense/getExpense", { params: { id: eId } }, function (response) {
                return response.data;
            })
        }

        var addExpense = function (expense) {
            return $http.post("/api/expense/addExpense", expense, function (response) {
                return response.data;
            })
        }

        var deleteExpense = function (eId) {
            return $http.delete("/api/expense/deleteExpense", { params: { id: eId } }, function (response) {
                return reponse.data;
            })
        }

        var updateExpense = function (expense) {
            return $http.post("/api/expense/updateExpense", expense, function (response) {
                return response.data;
            })
        }

        return {
            getExpenses: getExpenses,
            addExpense: addExpense,
            deleteExpense: deleteExpense,
            updateExpense: updateExpense,
            getExpense: getExpense
        };

    }

    expenseService.$inject = ["$http"];
    module.factory("expenseService", expenseService);

}(angular.module("HairPlus.services")))