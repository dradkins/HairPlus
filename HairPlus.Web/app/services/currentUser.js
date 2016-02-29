(function (module) {

    var currentUser = function (formEncode, localStorageService) {

        var USERKEY = "JDStoreUser";

        var setProfile = function (username, token) {
            profile.username = username,
            profile.token = token
            localStorageService.set(USERKEY, angular.toJson(profile));
        };

        var initialize = function () {

            var user = {
                username: "",
                token: "",
                isLoggedIn: {
                    get function() {
                        console.log(this.token);
                        return this.token;
                    }
                }
            }
            var localUser = localStorageService.get(USERKEY);
            if (localUser) {
                var currentUser = angular.fromJson(localUser);
                user.username = currentUser.username;
                user.token = currentUser.token;
            }
            return user;
        }

        var profile = initialize();

        var logout = function (email) {
            localStorageService.remove(USERKEY);
            setProfile(null, null, null);
        }

        return {
            profile: profile,
            setProfile: setProfile,
            logout: logout
        };

    }

    currentUser.$inject = ["formEncode", "localStorageService"];
    module.factory("currentUser", currentUser);

}(angular.module("HairPlus.services")))