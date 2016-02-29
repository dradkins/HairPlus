(function (app) {

    //for redirecting user to login page
    var loginRedirect = function ($q, $injector) {

        var lastPath = "dashboard"

        var responseError = function (response) {
            if (response.status == 401) {
                //lastPath = $location.path();
                $injector.get('$state').go('login');
                //$state.go('login');
            }
            return $q.reject(response);
        };

        var redirectPostLogin = function () {
            $injector.get('$state').go('login');
            //$state.go(lastPath);
        }

        return {
            responseError: responseError,
            redirectPostLogin: redirectPostLogin,
        };

    }

    loginRedirect.$inject = ["$q", "$injector"];
    app.factory("loginRedirect", loginRedirect);

    //registering interceptors
    app.config(function ($httpProvider) {
        $httpProvider.interceptors.push("loginRedirect");
    });

}(angular.module("HairPlus.interceptors")));