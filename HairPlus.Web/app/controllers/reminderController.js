(function (module) {

    var reminderController = function ($scope, toastr, reminderService) {

        $scope.reminders = [];

        $scope.pagingInfo = {
            page: 1,
            itemsPerPage: 50,
            sortBy: 'displayOrder',
            reverse: false,
            search: '',
            totalItems: 0
        };

        var onReminders = function (data) {
            $scope.reminders = data;
        }

        var onRemindersError = function (err) {
            console.log(err);
            toastr.error('unable to load reminders');
        }

        var init = function () {
            reminderService.getReminders(pagingInfo).then(onReminders, onRemindersError);
        }

        init();

    }

    reminderController.$inject = ["$scope", "toastr", "reminderService"];
    module.controller("reminderController", reminderController);


}(angular.module("HairPlus.controllers")))