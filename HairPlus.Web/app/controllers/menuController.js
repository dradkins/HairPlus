(function (module) {

    var menuController = function ($scope, $state, $uibModal, toastr, currentUser) {

        $scope.username = currentUser.profile.username;

        $scope.logout = function () {
            currentUser.logout();
            $state.go("login");
        }

        $scope.openChangeModal = function () {
            var modalInstance = $uibModal.open({
                animation: true,
                backdrop: true,
                templateUrl: 'changePasswordContent.html',
                controller: 'changePasswordController',
            });

            modalInstance.result.then(onPasswordChange, function () {
                console.info("change password cancel");
            });
        };

        var onPasswordChange = function (result) {
            if (result) {
                toastr.success("password change successfully");
            }
            else {
                toastr.error("unable to change password");
            }
        }
    }

    menuController.$inject = ["$scope", "$state", "$uibModal", "toastr", "currentUser"];
    module.controller("menuController", menuController);


    /********************** change password controller ********************/

    var changePasswordController = function ($scope, $uibModalInstance, userService) {

        $scope.changePasswordModal = {
            oldPassword: "",
            newPassword: "",
            confirmPassword: ""
        };

        $scope.changePassword = function (formId) {
            if (jQuery(formId).valid()) {
                userService.changePassword($scope.changePasswordModal).then(function (data) {
                    $uibModalInstance.close(true);
                }, function (err) {
                    console.log(err);
                    $uibModalInstance.close(false);
                });
            }
        }

        $scope.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        }

    }

    changePasswordController.$inject = ["$scope", "$uibModalInstance", "userService"];
    module.controller("changePasswordController", changePasswordController);

}(angular.module("HairPlus.controllers")))