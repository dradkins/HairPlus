(function (module) {

    var dashboardController = function ($scope, toastr, reminderService, patientService) {

        $scope.reminders = [];
        $scope.surgicalPatients = [];
        $scope.nonSurgicalPatients = [];

        var onReminders = function (data) {
            $scope.reminders = data;
        }

        var onRemindersError = function (err) {
            console.log(err);
            toastr.error('unable to load reminders');
        }

        var onUpcomingSurgicals = function (data) {
            $scope.surgicalPatients = data;
        }

        var onUpcomingSurgicalsError = function (err) {
            console.log(err);
            toastr.error("unable to load upcoming surgicals");
        }

        var onUpcomingNonSurgicals = function (data) {
            $scope.nonSurgicalPatients = data;
        }

        var onUpcomingNonSurgicalsError = function (err) {
            console.log(err);
            toastr.error("unable to load upcoming non surgicals");
        }

        var init = function () {
            reminderService.getReminders().then(onReminders, onRemindersError);
            patientService.getUpcomingNonSurgical(new Date(), new Date()).then(onUpcomingNonSurgicals, onUpcomingNonSurgicalsError);
            patientService.getUpcomingSurgical().then(onUpcomingSurgicals, onUpcomingSurgicalsError);
        }

        init();

    }

    dashboardController.$inject = ["$scope", "toastr", "reminderService", "patientService"];
    module.controller("dashboardController", dashboardController);


}(angular.module("HairPlus.controllers")))