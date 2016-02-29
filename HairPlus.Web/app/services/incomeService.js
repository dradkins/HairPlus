(function (module) {

    var incomeService = function ($http) {

        var getIncomes = function (pagingInfo) {
            return $http.get("/api/income/getAll", { params: pagingInfo }, function (response) {
                return response.data;
            })
        }

        var getIncome = function (iId) {
            return $http.get("/api/income/getIncome", { params: { id: iId } }, function (response) {
                return response.data;
            })
        }

        var addIncome = function (income) {
            return $http.post("/api/income/addIncome", income, function (response) {
                return response.data;
            })
        }

        var deleteIncome = function (iId) {
            return $http.delete("/api/income/deleteIncome", { params: { id: iId } }, function (response) {
                return reponse.data;
            })
        }

        var updateIncome = function (income) {
            return $http.post("/api/income/updateIncome", income, function (response) {
                return response.data;
            })
        }

        return {
            getIncomes: getIncomes,
            addIncome: addIncome,
            deleteIncome: deleteIncome,
            updateIncome: updateIncome,
            getIncome: getIncome
        };

    }

    incomeService.$inject = ["$http"];
    module.factory("incomeService", incomeService);

}(angular.module("HairPlus.services")))