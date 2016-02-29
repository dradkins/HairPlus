(function (module) {

    var customerController = function ($scope, toastr, userService) {

        $scope.customers = [];

        $scope.pagingInfo = {
            page: 1,
            itemsPerPage: 20,
            sortBy: 'name',
            reverse: false,
            search: '',
            totalItems: 0
        };

        var onCustomers = function (data) {
            $scope.customers = data.data;
            $scope.pagingInfo.totalItems = data.count;
        }

        var onCustomersError = function (err) {
            console.log(err);
            toastr.error('unable to load expenses');
        }

        var init = function () {
            userService.getSavedCustomers($scope.pagingInfo).then(onCustomers, onCustomersError);
        }

        init();

    }

    customerController.$inject = ["$scope", "toastr", "userService"];
    module.controller("customerController", customerController);


    /************* Add Or Update Customer Controller ****************/

    var addUpdateCustomerController = function ($scope, $state, $stateParams, toastr, hairLossSolutionService, patientService) {

        $scope.model = {
            customerId: 0,
            dateVisited: new Date(),
            name: "",
            gender: "1",
            age: 0,
            occupation: "",
            yearsExpHairLoss: 0,
            address: "",
            telephoneNo: "",
            mobileNo: "",
            emailAddress: "",
            sourceClinicInfo: "",
            hairLossStage: 1,
            solutionsWant: [],
            comment: "",
            estimatedPrice: 0,
            priority: "1"
        };

        $scope.hairLossSolutions = [];
        $scope.isNewCustomer = true;

        $scope.isOpen = false;

        $scope.openCalendar = function (e) {
            e.preventDefault();
            e.stopPropagation();
            $scope.isOpen = true;
        };

        $scope.saveCustomer = function () {
            patientService.addPatient($scope.model).then(function () {
                toastr.success("customer added successfully");
                $state.go("customers");
            }, function (err) {
                console.log(err);
                toastr.error("unable to save customer at this time");
            })
        }

        $scope.updateCustomer = function () {
            patientService.updatePatient($scope.model).then(function () {
                toastr.success("customer updated successfully");
                $state.go("customers");
            }, function (err) {
                console.log(err);
                toastr.error("unable to update customer at this time");
            })
        }

        //$scope.saveSurgicalCustomer = function () {
        //    patientService.addSurgicalPatient($scope.model).then(function () {
        //        toastr.success("customer added successfully");
        //        $state.go("customers");
        //    }, function (err) {
        //        console.log(err);
        //        toastr.error("unable to save customer at this time");
        //    })
        //}

        //$scope.saveNonSurgicalCustomer = function () {
        //    patientService.addNonSurgicalPatient($scope.model).then(function () {
        //        toastr.success("customer added successfully");
        //        $state.go("customers");
        //    }, function (err) {
        //        console.log(err);
        //        toastr.error("unable to save customer at this time");
        //    })
        //}

        var onCustomer = function (data) {
            data = data.data;
            $scope.model.dateVisited = data.dateVisited;
            $scope.model.name = data.name;
            $scope.model.gender = data.gender.toString();
            $scope.model.age = data.age;
            $scope.model.occupation = data.occupation;
            $scope.model.yearsExpHairLoss = data.yearsExpHairLoss;
            $scope.model.address = data.address;
            $scope.model.telephoneNo = data.telephoneNo;
            $scope.model.mobileNo = data.mobileNo;
            $scope.model.emailAddress = data.emailAddress;
            $scope.model.sourceClinicInfo = data.sourceClinicInfo;
            $scope.model.hairLossStage = data.hairLossStage;
            $scope.model.solutionsWant = data.solutionsWant;
            $scope.model.comment = data.comment;
            $scope.model.estimatedPrice = data.estimatedPrice;
            $scope.model.priority = data.priority.toString();
        }

        var init = function () {
            if ($state.current.name != "add-customer") {
                $scope.isNewCustomer = false;
                var customerId = $stateParams.cId;
                $scope.model.customerId = customerId;
                patientService.getPatient(customerId).then(onCustomer, function (err) {
                    console.log(err);
                    toastr.error("unable to fetch customer data");
                })
            }
            hairLossSolutionService.getSolutions().then(function (data) {
                $scope.hairLossSolutions = data.data;
            }, function (err) {
                console.log(err);
                toastr.error("unable to load hair loss solution at this time");
            })
        }

        init();
    }

    addUpdateCustomerController.$inject = ["$scope", "$state", "$stateParams", "toastr", "hairLossSolutionService", "patientService"];
    module.controller("addUpdateCustomerController", addUpdateCustomerController);

}(angular.module("HairPlus.controllers")))