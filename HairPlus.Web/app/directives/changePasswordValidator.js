(function (module) {

    var changePasswordValidator = function () {

        return {
            link: function (scope, element) {
                jQuery(element).validate({
                    errorClass: 'validate-has-error',
                    errorElement: 'span',
                    errorPlacement: function (error, element) {
                        error.insertAfter(element);
                    },
                    highlight: function (e) {
                        jQuery(e).closest('.form-group').addClass('validate-has-error');
                    },
                    success: function (e) {
                        jQuery(e).closest('.form-group').removeClass('validate-has-error');
                    },
                    rules: {
                        'old-password': {
                            required: true,
                            minlength: 6
                        },
                        'new-password': {
                            required: true,
                            minlength: 6
                        },
                        'confirm-password': {
                            required: true,
                            equalTo: '#new-password'
                        }
                    },
                    messages: {
                        'old-password': {
                            required: 'Please enter old password',
                            minlength: 'Your password must be at least 6 characters long'
                        },
                        'new-password': {
                            required: 'Please provide a password',
                            minlength: 'Your password must be at least 6 characters long'
                        },
                        'confirm-password': {
                            required: 'Please provide a password',
                            minlength: 'Your password must be at least 6 characters long',
                            equalTo: 'Please enter the same password as above'
                        }
                    }
                });
            }
        }

    };

    module.directive("changePasswordValidator", changePasswordValidator);

}(angular.module("HairPlus.directives")));
