(function (module) {

    var sideBarCollapse = function ($rootScope) {
        return {
            link: function (scope, elem, attr, ctrl) {

                elem.on('click', function (e) {
                    e.preventDefault();
                    var pageContainer = jQuery(".page-container");
                    var sideBarClass = "sidebar-collapsed";
                    if (pageContainer.hasClass(sideBarClass)) {
                        pageContainer.removeClass(sideBarClass);
                    }
                    else {
                        pageContainer.addClass(sideBarClass);
                    }
                })
            }
        }
    }

    module.directive("sideBarCollapse", sideBarCollapse)

}(angular.module('HairPlus.directives')));