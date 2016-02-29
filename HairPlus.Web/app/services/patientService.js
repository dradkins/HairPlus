(function (module) {

    var patientService = function ($http) {

        var getPatient = function (pId) {
            return $http.get("/api/patient/getPatient", { params: { id: pId } }, function (response) {
                return response.data;
            })
        }

        var addPatient = function (patient) {
            return $http.post("/api/patient/addPatient", patient, function (response) {
                return response.data;
            })
        }

        var deletePatient = function (rId) {
            return $http.delete("/api/patient/deletePatient", { params: { id: rId } }, function (response) {
                return reponse.data;
            })
        }


        var updatePatient = function (patient) {
            return $http.post("/api/patient/updatePatient", patient, function (response) {
                return response.data;
            })
        }

        var getPatientPhotos = function (pId) {
            return $http.get("/api/patient/getPatientPhotos", { params: { id: pId } }, function (response) {
                return response.data;
            })
        }

        var deletePicture = function (pId) {
            return $http.delete("/api/photos/deletePicture", { params: { id: pId } }, function (response) {
                return response.data;
            })
        }

        var setDefaultPicture = function (cId, pId) {
            return $http.post("/api/photos/setAsDefault", { CustomerId: cId, PicId: pId }, function (response) {
                return response.data;
            })
        }

        /************** Surgical Patients Section *****************/

        var addSurgicalPatient = function (patient) {
            return $http.post("/api/patient/addSurgicalPatient", patient, function (response) {
                return response.data;
            })
        }

        var updateSurgicalPatient = function (patient) {
            return $http.post("/api/patient/updateSurgicalPatient", patient, function (response) {
                return response.data;
            })
        }

        var getSurgicalPatient = function (pId) {
            return $http.get("/api/patient/getSurgicalPatient", { params: { id: pId } }, function (response) {
                return response.data;
            })
        }

        var getSurgicalPatients = function (pagingInfo) {
            return $http.get("/api/patient/getAllSurgicalPatients", { params: pagingInfo }, function (response) {
                return response.data;
            })
        }

        var updateSurgicalPatientStatus = function (data) {
            return $http.post("/api/patient/updateSurgicalPatientStatus", data, function (response) {
                return response.data;
            })
        }

        /*********** Non Surgical Patients Section *****************/

        var addNonSurgicalPatient = function (patient) {
            return $http.post("/api/patient/addNonSurgicalPatient", patient, function (response) {
                return response.data;
            })
        }

        var updateNonSurgicalPatient = function (patient) {
            return $http.post("/api/patient/updateNonSurgicalPatient", patient, function (response) {
                return response.data;
            })
        }

        var getNonSurgicalPatient = function (pId) {
            return $http.get("/api/patient/getSurgicalPatient", { params: { id: pId } }, function (response) {
                return response.data;
            })
        }

        var getNonSurgicalPatients = function (pagingInfo) {
            return $http.get("/api/patient/getAllNonSurgicalPatients", { params: pagingInfo }, function (response) {
                return response.data;
            })
        }

        var updateNonSurgicalPatientStatus = function (data) {
            return $http.post("/api/patient/updateNonSurgicalPatientStatus", data, function (response) {
                return response.data;
            })
        }



        return {
            //getPatients: getPatients,
            getPatient: getPatient,
            addPatient: addPatient,
            deletePatient: deletePatient,
            updatePatient: updatePatient,
            getPatientPhotos: getPatientPhotos,
            setDefaultPicture: setDefaultPicture,
            deletePicture: deletePicture,


            addSurgicalPatient: addSurgicalPatient,
            updateSurgicalPatient: updateSurgicalPatient,
            getSurgicalPatient: getSurgicalPatient,
            getSurgicalPatients: getSurgicalPatients,
            updateSurgicalPatientStatus: updateSurgicalPatientStatus,

            addNonSurgicalPatient: addNonSurgicalPatient,
            getNonSurgicalPatient: getNonSurgicalPatient,
            updateNonSurgicalPatient: updateNonSurgicalPatient,
            updateNonSurgicalPatientStatus: updateNonSurgicalPatientStatus,
            getNonSurgicalPatients: getNonSurgicalPatients,
        };

    }

    patientService.$inject = ["$http"];
    module.factory("patientService", patientService);

}(angular.module("HairPlus.services")))