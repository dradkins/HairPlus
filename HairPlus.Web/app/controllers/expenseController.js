(function (module) {

    var expenseController = function ($scope, toastr, expenseService) {

        $scope.expenses = [];

        $scope.pagingInfo = {
            page: 1,
            itemsPerPage: 50,
            sortBy: 'amount',
            reverse: false,
            search: '',
            totalItems: 0
        };

        $scope.deleteExpense = function (expense) {
            if (confirm("Are you sure..?")) {
                expenseService.deleteExpense(expense.expenseId).then(function (data) {
                    $scope.expenses.splice($scope.expenses.indexOf(expense), 1);
                    toastr.success("Expense deleted successfully")
                }, function (err) {
                    console.log(err);
                    toastr.error("unable to delete expense")
                })
            }
        }

        var onExpenses = function (data) {
            $scope.expenses = data.data.data;
            $scope.pagingInfo.totalItems = data.data.count;
        }

        var onExpensesError = function (err) {
            console.log(err);
            toastr.error('unable to load expenses');
        }

        var init = function () {
            expenseService.getExpenses($scope.pagingInfo).then(onExpenses, onExpensesError);
        }

        init();

    }

    expenseController.$inject = ["$scope", "toastr", "expenseService"];
    module.controller("expenseController", expenseController);

    /************* Add Or Update expense Controller ****************/

    var addUpdateExpenseController = function ($scope, $state, $stateParams, toastr, expenseService) {

        $scope.model = {
            expenseId: 0,
            amount: 0,
            description: "",
            expenseDateTime: new Date()
        };
        $scope.isNewExpense = true;

        $scope.isOpen = false;

        $scope.openCalendar = function (e) {
            e.preventDefault();
            e.stopPropagation();
            $scope.isOpen = true;
        };

        $scope.saveExpense = function () {
            expenseService.addExpense($scope.model).then(function () {
                toastr.success("expense added successfully");
                $state.go("expenses");
            }, function (err) {
                console.log(err);
                toastr.error("unable to save expense at this time");
            })
        }

        $scope.updateExpense = function () {
            expenseService.updateExpense($scope.model).then(function () {
                toastr.success("expense updated successfully");
                $state.go("expenses");
            }, function (err) {
                console.log(err);
                toastr.error("unable to update expense at this time");
            })
        }

        var onExpense = function (data) {
            data = data.data;
            $scope.model.amount = data.amount;
            $scope.model.description = data.description;
            $scope.model.expenseDateTime = data.expenseDateTime;
        }

        var init = function () {
            if ($state.current.name != "add-expense") {
                $scope.isNewExpense = false;
                var expenseId = $stateParams.eId;
                $scope.model.expenseId = expenseId;
                expenseService.getExpense(expenseId).then(onExpense, function (err) {
                    console.log(err);
                    toastr.error("unable to fetch expense data");
                })
            }
        }

        init();
    }

    addUpdateExpenseController.$inject = ["$scope", "$state", "$stateParams", "toastr", "expenseService"];
    module.controller("addUpdateExpenseController", addUpdateExpenseController);


}(angular.module("HairPlus.controllers")))