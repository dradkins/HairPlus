(function (module) {

    var invoiceController = function ($scope, $stateParams, toastr, invoiceService) {

        $scope.invoice;
        $scope.invoiceType;
        $scope.currentDate = new Date();

        var onInvoice = function (data) {
            $scope.invoice = data.data;
        }

        var onInvoiceError = function (err) {
            console.log(err);
            toastr.error('unable to load invoice');
        }

        var init = function () {
            var invoiceType = $stateParams.iType;
            var cID = $stateParams.cId;
            $scope.invoiceType = invoiceType;
            invoiceService.getInvoice({ invoiceType: invoiceType, customerId: cID }).then(onInvoice, onInvoiceError);
        }

        init();

    }

    invoiceController.$inject = ["$scope", "$stateParams", "toastr", "invoiceService"];
    module.controller("invoiceController", invoiceController);


}(angular.module("HairPlus.controllers")))