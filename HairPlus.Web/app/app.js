(function () {

    var app = angular.module("HairPlus", [
        "HairPlus.controllers",
        "HairPlus.services",
        "HairPlus.directives",
        "HairPlus.interceptors",
        "ui.bootstrap",
        "ui.router",
        "toastr",
        "angular-loading-bar",
        "LocalStorageModule",
        "ui.bootstrap.datetimepicker",
        "checklist-model",
        "ngFileUpload"
    ]);

    app.config(function ($stateProvider, $urlRouterProvider) {

        $stateProvider
            .state("login", { url: "/login", templateUrl: "/app/templates/login.html", controller: "loginController", data: { cssClassnames: "page-body login-page login-form-fall loaded login-form-fall-init" } })
            .state("me", {
                url: "/me", templateUrl: "/app/templates/menu.html", controller: "menuController", data: { cssClassnames: "page-body skin-cafe gray loaded" }})
                .state("dashboard", { url: "/dashboard", templateUrl: "/app/templates/dashboard.html", controller: "dashboardController", parent: "me" })

                .state("customers", { url: "/customers", templateUrl: "/app/templates/customers.html", controller: "customerController", parent: "me" })
                .state("customer-photos", { url: "/custome-photos/:cId", templateUrl: "/app/templates/customer-photos.html", controller: "photosController", parent: "me" })
                .state("add-customer", { url: "/add-customer", templateUrl: "/app/templates/customer-data-entry.html", controller: "addUpdateCustomerController", parent: "me" })
                .state("update-customer", { url: "/update-customer/:cId", templateUrl: "/app/templates/customer-data-entry.html", controller: "addUpdateCustomerController", parent: "me" })

                .state("add-surgical-patient", { url: "/add-surgical-patient/:pId", templateUrl: "/app/templates/surgical-patient-data-entry.html", controller: "addUpdateSurgicalPatientController", parent: "me" })
                .state("update-surgical-patient", { url: "/update-surgical-patient/:pId", templateUrl: "/app/templates/surgical-patient-data-entry.html", controller: "addUpdateSurgicalPatientController", parent: "me" })
                .state("surgical-patients", { url: "/surgical-patients", templateUrl: "/app/templates/surgical-patients.html", controller: "surgicalPatientController", parent: "me" })

                .state("non-surgical-patients", { url: "/non-surgical-patients", templateUrl: "/app/templates/non-surgical-patients.html", controller: "nonSurgicalPatientController", parent: "me" })
                .state("update-non-surgical-patient", { url: "/update-non-surgical-patient/:pId", templateUrl: "/app/templates/non-surgical-patient-data-entry.html", controller: "addUpdateNonSurgicalPatientController", parent: "me" })
                .state("add-non-surgical-patient", { url: "/add-non-surgical-patient/:pId", templateUrl: "/app/templates/non-surgical-patient-data-entry.html", controller: "addUpdateNonSurgicalPatientController", parent: "me" })

                .state("expenses", { url: "/expenses", templateUrl: "/app/templates/expenses.html", controller: "expenseController", parent: "me" })
                .state("add-expense", { url: "/add-expense", templateUrl: "/app/templates/expense-data-entry.html", controller: "addUpdateExpenseController", parent: "me" })
                .state("update-expense", { url: "/update-expense/:eId", templateUrl: "/app/templates/expense-data-entry.html", controller: "addUpdateExpenseController", parent: "me" })

                .state("incomes", { url: "/incomes", templateUrl: "/app/templates/incomes.html", controller: "incomeController", parent: "me" })
                .state("add-income", { url: "/add-income", templateUrl: "/app/templates/income-data-entry.html", controller: "addUpdateIncomeController", parent: "me" })
                .state("update-income", { url: "/update-income/:iId", templateUrl: "/app/templates/income-data-entry.html", controller: "addUpdateIncomeController", parent: "me" })

                .state("invoice", { url: "/invoice/:iType/:cId", templateUrl: "/app/templates/invoice.html", controller: "invoiceController", parent: "me" })
        $urlRouterProvider.otherwise("/login");
    });


}())