(function (module) {

    var loginController = function ($scope, $state, toastr, currentUser, oauth) {

        $scope.username = "";
        $scope.password = "";
        $scope.user = currentUser.profile;

        $scope.login = function () {
            oauth.login($scope.username, $scope.password)
                 .then(onLoginSuccess, onLoginError);
        }

        var onLoginSuccess = function (data) {
            toastr.success("Welcom to HairPlus " + data);
            $state.go('dashboard');
        }

        var onLoginError = function (err) {
            toastr.error(err.data.error_description);
            $scope.password = "";
        }

    }

    loginController.$inject = ["$scope", "$state", "toastr", "currentUser", "oauth"];
    module.controller("loginController", loginController);


}(angular.module("HairPlus.controllers")))