(function (module) {

    var surgicalPatientController = function ($scope, toastr, patientService) {

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
                patientService.updateSurgicalPatientStatus(data).then(function (data) {
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

        var onSurgicalPatients = function (data) {
            $scope.patients = data.data.data;
            $scope.pagingInfo.totalItems = data.data.count;
        }

        var onSurgicalPatientsError = function (err) {
            console.log(err);
            toastr.error('unable to load patients');
        }

        var init = function () {
            patientService.getSurgicalPatients($scope.pagingInfo).then(onSurgicalPatients, onSurgicalPatientsError);
        }

        init();

    }

    surgicalPatientController.$inject = ["$scope", "toastr", "patientService"];
    module.controller("surgicalPatientController", surgicalPatientController);


    /************* Add Or Update Customer Controller ****************/

    var addUpdateSurgicalPatientController = function ($scope, $state, $stateParams, toastr, patientService) {

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
            treatmentDateTime: new Date(),
        };

        $scope.isNewCustomer = true;

        $scope.isOpen = false;

        $scope.openCalendar = function (e) {
            e.preventDefault();
            e.stopPropagation();
            $scope.isOpen = true;
        };

        $scope.saveSurgicalPatient = function () {
            patientService.addSurgicalPatient($scope.model).then(function () {
                toastr.success("surgical patient added successfully");
                $state.go("surgical-patients");
            }, function (err) {
                console.log(err);
                toastr.error("unable to save surgical patient at this time");
            })
        }

        $scope.updateSurgicalPatient = function () {
            patientService.updateSurgicalPatient($scope.model).then(function () {
                toastr.success("surgical patient updated successfully");
                $state.go("surgical-patients");
            }, function (err) {
                console.log(err);
                toastr.error("unable to update surgical patient at this time");
            })
        }

        var onSurgicalPatient = function (data) {
            data = data.data;
            $scope.model.totalAmount = data.totalAmount;
            $scope.model.discount = data.discount;
            $scope.model.advance = data.advance;
            $scope.model.treatmentDateTime = data.treatmentDateTime;
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
            if ($state.current.name != "add-surgical-patient") {
                $scope.isNewCustomer = false;
                patientService.getSurgicalPatient(patientId).then(onSurgicalPatient, function (err) {
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

    addUpdateSurgicalPatientController.$inject = ["$scope", "$state", "$stateParams", "toastr", "patientService"];
    module.controller("addUpdateSurgicalPatientController", addUpdateSurgicalPatientController);

}(angular.module("HairPlus.controllers")))