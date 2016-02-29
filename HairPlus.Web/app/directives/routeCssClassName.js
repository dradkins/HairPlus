(function (module) {

    var routeCssClassnames = function ($rootScope) {
        return {
            restrict: 'A',
            scope: {},
            link: function (scope, elem, attr, ctrl) {

                $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
                    var fromClassnames = angular.isDefined(fromState.data) && angular.isDefined(fromState.data.cssClassnames) ? fromState.data.cssClassnames : null;
                    var toClassnames = angular.isDefined(toState.data) && angular.isDefined(toState.data.cssClassnames) ? toState.data.cssClassnames : null;

                    // don't do anything if they are the same
                    if (fromClassnames != toClassnames) {
                        if (fromClassnames) {
                            elem.removeClass(fromClassnames);
                        }

                        if (toClassnames) {
                            elem.addClass(toClassnames);
                        }
                    }
                });
            }
        }
    }

    routeCssClassnames.$inject = ["$rootScope"];
    module.directive("routeCssClassnames", routeCssClassnames)

    module.directive('fileUpload', function () {
        return {
            scope: true,        //create a new scope
            link: function (scope, el, attrs) {
                el.bind('change', function (event) {
                    var files = event.target.files;
                    //iterate files since 'multiple' may be specified on the element
                    for (var i = 0; i < files.length; i++) {
                        //emit event upward
                        scope.$emit("fileSelected", { file: files[i] });
                    }
                });
            }
        };
    });

}(angular.module('HairPlus.directives')));