(function (module) {

    var nonSurgicalPatientController = function ($scope, toastr, patientService) {

        $scope.patients = [];

        $scope.pagingInfo = {
            page: 1,
            itemsPerPage: 20,
            sortBy: 'createdOn',
            reverse: true,
            search: '',
            totalItems: 0,
            patientStatus: 2
        };

        $scope.updateStatus = function (p, status) {
            if ($scope.pagingInfo.patientStatus == status) {
                toastr.success("status updated successfully");
                return false;
            }
            else {
                var data = {
                    patientId: p.patientId,
                    status: status
                };
                patientService.updateNonSurgicalPatientStatus(data).then(function (data) {
                    $scope.patients.splice($scope.patients.indexOf(p), 1);
                    toastr.success("status updated successfully");
                }, function (err) {
                    console.log(err);
                    toastr.error("unable to update status");
                })
            }
        }

        $scope.deletePatient = function (p) {
            if (confirm("Are you sure..?")) {
                patientService.deletePatient(p.patientId).then(function (data) {
                    $scope.patients.splice($scope.patients.indexOf(p), 1);
                    toastr.success('patient deleted successfully');
                }, function (err) {
                    console.log(err);
                    toastr.error('unable to delete patient');
                })
            }
        }


        $scope.changePatientStatus = function (status) {
            $scope.pagingInfo.patientStatus = status;
            init();
        }

        var onNonSurgicalPatients = function (data) {
            $scope.patients = data.data.data;
            $scope.pagingInfo.totalItems = data.data.count;
        }

        var onNonSurgicalPatientsError = function (err) {
            console.log(err);
            toastr.error('unable to load patients');
        }

        var init = function () {
            patientService.getNonSurgicalPatients($scope.pagingInfo).then(onNonSurgicalPatients, onNonSurgicalPatientsError);
        }

        init();

    }

    nonSurgicalPatientController.$inject = ["$scope", "toastr", "patientService"];
    module.controller("nonSurgicalPatientController", nonSurgicalPatientController);


    /************* Add Or Update Customer Controller ****************/

    var addUpdateNonSurgicalPatientController = function ($scope, $state, $stateParams, toastr, patientService) {

        $scope.data = {
            name: "",
            address: "",
            telephoneNo: "",
            mobileNo: "",
        }

        $scope.model = {
            patientId: 0,
            totalAmount: 0,
            discount: 0,
            advance: 0,
            maintainanceCharges: 0,
            treatmentDateTime: new Date(),

            style: "Free Style",

            humaFiber: 0,
            syntheticFiber: 0,
            grayFiber: 0,
            color: "1b",
            size: 0,

            length: "1",
            curly: "1",
            density: "1",

            colorInstructions: "",
            baseInstructions: ""
        };

        $scope.isNewCustomer = true;

        $scope.isOpen = false;

        $scope.openCalendar = function (e) {
            e.preventDefault();
            e.stopPropagation();
            $scope.isOpen = true;
        };

        $scope.saveNonSurgicalPatient = function () {
            patientService.addNonSurgicalPatient($scope.model).then(function () {
                toastr.success("surgical patient added successfully");
                $state.go("non-surgical-patients");
            }, function (err) {
                console.log(err);
                toastr.error("unable to save surgical patient at this time");
            })
        }

        $scope.updateNonSurgicalPatient = function () {
            patientService.updateNonSurgicalPatient($scope.model).then(function () {
                toastr.success("surgical patient updated successfully");
                $state.go("non-surgical-patients");
            }, function (err) {
                console.log(err);
                toastr.error("unable to update surgical patient at this time");
            })
        }

        var onNonSurgicalPatient = function (data) {
            data = data.data;
            $scope.model.totalAmount = data.totalAmount;
            $scope.model.discount = data.discount;
            $scope.model.advance = data.advance;
            $scope.model.treatmentDateTime = data.treatmentDateTime;

            $scope.model.style = data.style;

            $scope.model.humaFiber = data.humaFiber;
            $scope.model.syntheticFiber = data.syntheticFiber;
            $scope.model.grayFiber = data.grayFiber;
            $scope.model.color = data.color;
            $scope.model.size = data.size;

            $scope.model.length = data.length;
            $scope.model.curly = data.curly;
            $scope.model.density = data.density;

            $scope.model.colorInstructions = data.colorInstructions;
            $scope.model.baseInstructions = data.baseInstructions;
        }

        var onPatient = function (data) {
            data = data.data;
            $scope.data.name = data.name;
            $scope.data.address = data.address;
            $scope.data.telephoneNo = data.telephoneNo;
            $scope.data.mobileNo = data.mobileNo;
        }

        var init = function () {
            console.log($state.current.name);
            var patientId = $stateParams.pId;
            $scope.model.patientId = patientId;
            if ($state.current.name != "add-non-surgical-patient") {
                $scope.isNewCustomer = false;
                patientService.getSurgicalPatient(patientId).then(onNonSurgicalPatient, function (err) {
                    console.log(err);
                    toastr.error("unable to fetch patient data");
                })
            }
            patientService.getPatient(patientId).then(onPatient, function (err) {
                console.log(err);
                toastr.error("unable to fetch customer data");
            })
        }

        init();
    }

    addUpdateNonSurgicalPatientController.$inject = ["$scope", "$state", "$stateParams", "toastr", "patientService"];
    module.controller("addUpdateNonSurgicalPatientController", addUpdateNonSurgicalPatientController);

}(angular.module("HairPlus.controllers")))