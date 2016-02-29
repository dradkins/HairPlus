(function (module) {

    var reminderService = function ($http) {

        var getReminders = function (pagingInfo) {
            return $http.get("", { params: pagingInfo }, function (response) {
                return response.data;
            })
        }

        var addReminder = function (reminder) {
            return $http.post("", reminder, function (response) {
                return response.data;
            })
        }

        var deleteReminder = function (rId) {
            return $http.post("", { params: { id: rId } }, function (response) {
                return reponse.data;
            })
        }

        var updateReminder = function (reminder) {
            return $http.post("", reminder, function (response) {
                return response.data;
            })
        }

        return {
            getReminders: getReminders,
            addReminder: addReminder,
            deleteReminder: deleteReminder,
            updateReminder: updateReminder
        };

    }

    reminderService.$inject = ["$http"];
    module.factory("reminderService", reminderService);

}(angular.module("HairPlus.services")))