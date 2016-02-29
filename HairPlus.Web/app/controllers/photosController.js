(function (module) {

    var photosController = function ($scope, $stateParams, $http, toastr, patientService) {

        $scope.model = {
            customerId: 0,
        };


        $scope.photos;
        $scope.files = [];

        $scope.$on("fileSelected", function (event, args) {
            $scope.$apply(function () {
                $scope.files.push(args.file);
            });
        });

        //the save method
        $scope.save = function () {
            $http({
                method: 'POST',
                url: '/api/photos/postStuff',
                headers: { 'Content-Type': undefined },
                transformRequest: function (data) {
                    var formData = new FormData();
                    formData.append('model', angular.toJson(data.model));
                    for (var i = 0; i < data.files.length; i++) {
                        formData.append('file' + i, data.files[i]);
                    }
                    return formData;
                },
                data: { model: $scope.model, files: $scope.files }
            }).then(onPhotoAdded, onPhotoError);
        };

        var onPhotoAdded = function (data) {
            window.location.reload();
            //$scope.$apply(function () {
            //    angular.forEach(data, function (i) {
            //        $scope.photos.push(i);
            //    })
            //});
        }

        $scope.setAsDefault = function (pic) {
            patientService.setDefaultPicture($scope.model.customerId, pic.id).then(function (data) {
                toastr.success("picture set to default successfully")
            }, function (err) {
                toastr.error("unable to set default picture")
            })
        }

        $scope.deletePicture = function (pic) {
            patientService.deletePicture(pic.id).then(function (data) {
                $scope.photos.splice($scope.photos.indexOf(pic), 1)
                toastr.success("picture deleted successfully")
            }, function (err) {
                toastr.error("unable to delete picture")
            })
        }

        var onPhotoError = function (err) {
            console.log(err);
            toastr.error("unable to add photos at this time")
        }

        var onCustomerPhotos = function (data) {
            $scope.photos = data.data;
        }

        var onCustomerPhotosError = function (err) {
            console.log(err);
            toastr.error("unable to load customer photos");
        }

        var init = function () {
            var customerId = $stateParams.cId;
            $scope.model.customerId = customerId;
            patientService.getPatientPhotos(customerId).then(onCustomerPhotos, onCustomerPhotosError);
        }

        init();
    };

    photosController.$inject = ["$scope", "$stateParams", "$http", "toastr", "patientService"];
    module.controller("photosController", photosController);

}(angular.module("HairPlus.controllers")))