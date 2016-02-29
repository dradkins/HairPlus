(function (module) {

    var userService = function ($http) {

        var changePassword = function (modal) {
            return $http.post("/api/account/changePassword", modal)
                        .then(function (response) {
                            return response.data;
                        })
        }

        var getSavedCustomers = function (pagingInfo) {
            return $http.get("/api/customer/getAll", { params: pagingInfo })
                        .then(function (response) {
                            return response.data;
                        })
        }

        return {
            changePassword: changePassword,
            getSavedCustomers: getSavedCustomers
        };

    }

    userService.$inject = ["$http"];
    module.factory("userService", userService)

}(angular.module("HairPlus.services")))