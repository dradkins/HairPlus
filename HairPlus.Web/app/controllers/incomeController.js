(function (module) {

    var incomeController = function ($scope, toastr, incomeService) {

        $scope.incomes = [];

        $scope.pagingInfo = {
            page: 1,
            itemsPerPage: 50,
            sortBy: 'amount',
            reverse: false,
            search: '',
            totalItems: 0
        };

        $scope.deleteIncome = function (income) {
            if (confirm("Are you sure..?")) {
                incomeService.deleteIncome(income.incomeId).then(function (data) {
                    $scope.incomes.splice($scope.incomes.indexOf(income), 1);
                    toastr.success("Income deleted successfully")
                }, function (err) {
                    console.log(err);
                    toastr.error("unable to delete Income")
                })
            }
        }

        var onIncomes = function (data) {
            console.log(data.data);
            $scope.incomes = data.data.data;
            $scope.pagingInfo.totalItems = data.data.count;
        }

        var onIncomeError = function (err) {
            console.log(err);
            toastr.error('unable to load incomes');
        }

        var init = function () {
            incomeService.getIncomes($scope.pagingInfo).then(onIncomes, onIncomeError);
        }

        init();

    }

    incomeController.$inject = ["$scope", "toastr", "incomeService"];
    module.controller("incomeController", incomeController);

    /************* Add Or Update income Controller ****************/

    var addUpdateIncomeController = function ($scope, $state, $stateParams, toastr, incomeService) {

        $scope.model = {
            incomeId: 0,
            amount: 0,
            description: "",
            incomeDateTime: new Date()
        };
        $scope.isNewIncome = true;

        $scope.isOpen = false;

        $scope.openCalendar = function (e) {
            e.preventDefault();
            e.stopPropagation();
            $scope.isOpen = true;
        };

        $scope.saveIncome = function () {
            incomeService.addIncome($scope.model).then(function () {
                toastr.success("income added successfully");
                $state.go("incomes");
            }, function (err) {
                console.log(err);
                toastr.error("unable to save income at this time");
            })
        }

        $scope.updateIncome = function () {
            incomeService.updateIncome($scope.model).then(function () {
                toastr.success("income updated successfully");
                $state.go("incomes");
            }, function (err) {
                console.log(err);
                toastr.error("unable to update income at this time");
            })
        }

        var onIncome = function (data) {
            console.log(data);
            data = data.data;
            $scope.model.amount = data.amount;
            $scope.model.description = data.description;
            $scope.model.incomeDateTime = data.incomeDateTime;
        }

        var init = function () {
            if ($state.current.name != "add-income") {
                $scope.isNewIncome = false;
                var incomeId = $stateParams.iId;
                $scope.model.incomeId = incomeId;
                incomeService.getIncome(incomeId).then(onIncome, function (err) {
                    console.log(err);
                    toastr.error("unable to fetch expense data");
                })
            }
        }

        init();
    }

    addUpdateIncomeController.$inject = ["$scope", "$state", "$stateParams", "toastr", "incomeService"];
    module.controller("addUpdateIncomeController", addUpdateIncomeController);


}(angular.module("HairPlus.controllers")))