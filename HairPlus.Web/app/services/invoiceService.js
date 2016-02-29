(function (module) {

    var invoiceService = function ($http) {

        var getInvoice = function (iData) {
            return $http.get("/api/invoice/getInvoice", { params: iData }, function (response) {
                return response.data;
            })
        }

        return {
            getInvoice: getInvoice,
        };

    }

    invoiceService.$inject = ["$http"];
    module.factory("invoiceService", invoiceService);

}(angular.module("HairPlus.services")))