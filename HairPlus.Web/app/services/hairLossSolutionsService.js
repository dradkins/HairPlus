(function (module) {

    var hairLossSolutionService = function ($http) {

        var getSolutions = function () {
            return $http.get("/api/hairLossSolution/getAll", function (response) {
                return response.data;
            })
        }

        return {
            getSolutions: getSolutions,
        };

    }

    hairLossSolutionService.$inject = ["$http"];
    module.factory("hairLossSolutionService", hairLossSolutionService);

}(angular.module("HairPlus.services")))