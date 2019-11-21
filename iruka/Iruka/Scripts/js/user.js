var PictureUrl = "";
var CertificateUrl = "";
var userID = "";

// #region UPLOAD
$("#pictureUploader").kendoUpload({
    multiple: false,
    localization: {
        select: 'Choose Picture'
    },
    async: {
        saveUrl: "/Users/SavePicture",
        autoUpload: true
    },
    success: onSuccess,
    remove:onRemove,
    validation: {
        allowedExtensions: [".jpeg", ".jpg", ".png"]
    }
});
function onSuccess(e) {
    PictureUrl = e.response.success;
    $("#PicturePath").val(PictureUrl);
}
function onRemove(e) {
    var PictA = $("#pictureUploader").data().kendoUpload;
    var RemovingPictA = PictA.wrapper.find('.k-file');
    PictA._removeFileEntry(RemovingPictA);
}

$("#certificateUploader").kendoUpload({
    multiple: false,
    localization: {
        select: 'Choose Picture'
    },
    async: {
        saveUrl: "/Users/SavingCertificate",
        autoUpload: true
    },
    success: onSuccessCertificate,
    remove: onRemoveCertificate,
    validation: {
        allowedExtensions: [".jpeg", ".jpg", ".png"]
    }
});
function onSuccessCertificate(e) {
    CertificateUrl = e.response.success;
    $("#CertificatePath").val(CertificateUrl);
}
function onRemoveCertificate(e) {
    var PictA = $("#certificateUploader").data().kendoUpload;
    var RemovingPictA = PictA.wrapper.find('.k-file');
    PictA._removeFileEntry(RemovingPictA);
}

// #endregion

// #region REGISTER
$("#btnSubmitRegisterContentManager").click(function () {
    var pictureName = PictureUrl.split('\\');
    $.ajax({
        async: false,
        url: "/Users/UserRegistration",
        data: "{ 'name': '" + $("#Name").val() + "','email': '" + $("#Email").val() + "','password': '" +
            $("#Password").val() + "','phone': '" + $("#PhoneNumber").val() + "','address': '" +
            $("#Address").val() + "','picture': '" + pictureName[6] + "','desc': '" + $("#Description").val() + "','role': '" + "ContentManager" + "' }",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {
            if (data["success"] != undefined) {
                clearInput();
                Swal.fire('Success created user');
                reloadContentManagerTable();
            } else {
                Swal.fire('Failed to create user, please check your input again');
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert(errorThrown);
        }
    });
});
$("#btnSubmitRegisterFinance").click(function () {
    var pictureName = PictureUrl.split('\\');
    $.ajax({
        async: false,
        url: "/Users/UserRegistration",
        data: "{ 'name': '" + $("#Name").val() + "','email': '" + $("#Email").val() + "','password': '" +
            $("#Password").val() + "','phone': '" + $("#PhoneNumber").val() + "','address': '" +
            $("#Address").val() + "','picture': '" + pictureName[6] + "','desc': '" + $("#Description").val() + "','role': '" + "Finance" + "' }",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {
            if (data["success"] != undefined) {
                clearInput();
                Swal.fire('Success created user');
                reloadFinanceTable();
            } else {
                Swal.fire('Failed to create user, please check your input again');
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert(errorThrown);
        }
    });
});
$("#btnSubmitRegisterSuperAdmin").click(function () {
    var pictureName = PictureUrl.split('\\');
    $.ajax({
        async: false,
        url: "/Users/UserRegistration",
        data: "{ 'name': '" + $("#Name").val() + "','email': '" + $("#Email").val() + "','password': '" +
            $("#Password").val() + "','phone': '" + $("#PhoneNumber").val() + "','address': '" +
            $("#Address").val() + "','picture': '" + pictureName[6] + "','desc': '" + $("#Description").val() + "','role': '" + "SuperAdmin" + "' }",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {
            if (data["success"] != undefined) {
                clearInput();
                Swal.fire('Success created user');
                reloadSuperAdminTable();
            } else {
                Swal.fire('Failed to create user, please check your input again');
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert(errorThrown);
        }
    });
});
$("#btnSubmitRegisterAdmin").click(function() {
    var pictureName = PictureUrl.split('\\');
    $.ajax({
        async: false,
        url: "/Users/UserRegistration",
        data: "{ 'name': '" + $("#Name").val() + "','email': '" + $("#Email").val() + "','password': '" +
            $("#Password").val() + "','phone': '" + $("#PhoneNumber").val() + "','address': '" +
            $("#Address").val() + "','picture': '" + pictureName[6] + "','desc': '" + $("#Description").val() + "','role': '" + "Admin" + "' }",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function(data) { return data; },
        success: function(data) {
            if (data["success"] != undefined) {
                clearInput();
                Swal.fire('Success created user');
                reloadAdminTable();
            } else {
                Swal.fire('Failed to create user, please check your input again');
            }
        },
        error: function(XMLHttpRequest, textStatus, errorThrown) {
            //alert(errorThrown);
        }
    });
});

$("#btnSubmitRegisterGroomer").click(function () {
    var pictureName = PictureUrl.split('\\');
    var certificateName = CertificateUrl.split('\\');
    $.ajax({
        async: false,
        url: "/Users/UserRegistration",
        data: "{ 'name': '" + $("#Name").val() + "','email': '" + $("#Email").val() + "','password': '" +
            $("#Password").val() + "','phone': '" + $("#PhoneNumber").val() + "','address': '" +
            $("#Address").val() + "','picture': '" + pictureName[6] + "','certificate':'" + certificateName[6] +
            "','desc': '" + $("#Description").val() + "','role': '" + "Groomer" + "' }",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {
            if (data["success"] != undefined) {
                clearInput();

                var PictA = $("#certificateUploader").data().kendoUpload;
                var RemovingPictA = PictA.wrapper.find('.k-file');
                PictA._removeFileEntry(RemovingPictA);

                Swal.fire('Success created user');
                reloadGroomerTable();
            } else {
                Swal.fire('Failed to create user, please check your input again');
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert(errorThrown);
        }
    });
});

$("#btnSubmitRegisterCustomer").click(function () {
    var pictureName = PictureUrl.split('\\');
    $.ajax({
        async: false,
        url: "/Users/UserRegistration",
        data: "{ 'name': '" + $("#Name").val() + "','email': '" + $("#Email").val() + "','password': '" +
            $("#Password").val() + "','phone': '" + $("#PhoneNumber").val() + "','address': '" +
            $("#Address").val() + "','picture': '" + pictureName[6] + "','desc': '" + $("#Description").val() + "','role': '" + "Customer" + "' }",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {
            if (data["success"] != undefined) {
                clearInput();
                Swal.fire('Success created user');
                reloadCustomerTable();
            } else {
                Swal.fire('Failed to create user, please check your input again');
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert(errorThrown);
        }
    });
});

$("#btnSubmitRegisterOwner").click(function () {
    var pictureName = PictureUrl.split('\\');
    $.ajax({
        async: false,
        url: "/Users/UserRegistration",
        data: "{ 'name': '" + $("#Name").val() + "','email': '" + $("#Email").val() + "','password': '" +
            $("#Password").val() + "','phone': '" + $("#PhoneNumber").val() + "','address': '" +
            $("#Address").val() + "','picture': '" + pictureName[6] + "','desc': '" + $("#Description").val() + "','role': '" + "Owner" + "' }",
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {
            if (data["success"] != undefined) {
                clearInput();
                Swal.fire('Success created user');
                reloadOwnerTable();
            } else {
                Swal.fire('Failed to create user, please check your input again');
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert(errorThrown);
        }
    });
});
// #endregion

function InitDataTable() {

    datatable = $('#adminTable').DataTable({
        columnDefs: [{
            orderable: false,
            className: 'select-checkbox',
            targets: 0,
            checkboxes: {
                'selectRow': true
            }
        }],
        "pageLength": 10,
        "scrollX": true,
        "language": {
            "paginate": {
                "previous": "<<",
                "next": ">>"
            }
        },
        select: true,
        paging: true,
        "order": [[0, "asc"]],
        "info": false,
        "bPaginate": false,
        "bLengthChange": false,
        "bFilter": true,
        "bInfo": false,
        "bAutoWidth": false,
        dom: 'Bfrtip'
    });
}

// #region DELETE
function deleteContentManager(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                async: false,
                url: "/Users/DeleteUser",
                data: "{ 'id': '" + id + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    if (data["success"] != undefined) {
                        Swal.fire(
                            'Deleted!',
                            'Your data has been deleted.',
                            'success'
                        );
                        reloadContentManagerTable();
                    } else {
                        Swal.fire('Delete User error, please contact support');
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }
    });
}
function deleteFinance(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                async: false,
                url: "/Users/DeleteUser",
                data: "{ 'id': '" + id + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    if (data["success"] != undefined) {
                        Swal.fire(
                            'Deleted!',
                            'Your data has been deleted.',
                            'success'
                        );
                        reloadFinanceTable();
                    } else {
                        Swal.fire('Delete User error, please contact support');
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }
    });
}
function deleteSuperAdmin(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                async: false,
                url: "/Users/DeleteUser",
                data: "{ 'id': '" + id + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    if (data["success"] != undefined) {
                        Swal.fire(
                            'Deleted!',
                            'Your data has been deleted.',
                            'success'
                        );
                        reloadSuperAdminTable();
                    } else {
                        Swal.fire('Delete User error, please contact support');
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }
    });
}
function deleteOwner(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                async: false,
                url: "/Users/DeleteUser",
                data: "{ 'id': '" + id + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    if (data["success"] != undefined) {
                        Swal.fire(
                            'Deleted!',
                            'Your data has been deleted.',
                            'success'
                        );
                        reloadOwnerTable();
                    } else {
                        Swal.fire('Delete User error, please contact support');
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }
    });
}
function deleteAdmin(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                async: false,
                url: "/Users/DeleteUser",
                data: "{ 'id': '" + id + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function(data) {
                    if (data["success"]!=undefined) {
                        Swal.fire(
                            'Deleted!',
                            'Your data has been deleted.',
                            'success'
                        );
                        reloadAdminTable();
                    } else {
                        Swal.fire('Delete User error, please contact support');
                    }
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }
    });
}
function deleteCustomer(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                async: false,
                url: "/Users/DeleteUser",
                data: "{ 'id': '" + id + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    if (data["success"] != undefined) {
                        Swal.fire(
                            'Deleted!',
                            'Your data has been deleted.',
                            'success'
                        );
                        reloadCustomerTable();
                    } else {
                        Swal.fire('Delete User error, please contact support');
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }
    });
}
function deleteGroomer(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                async: false,
                url: "/Users/DeleteUser",
                data: "{ 'id': '" + id + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    if (data["success"] != undefined) {
                        Swal.fire(
                            'Deleted!',
                            'Your data has been deleted.',
                            'success'
                        );
                        reloadGroomerTable();
                    } else {
                        Swal.fire('Delete User error, please contact support');
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //alert(errorThrown);
                }
            });
        }
    });
}
// #endregion

// #region RELOAD
function reloadFinanceTable() {
    $("#reloadTable").load("/Users/ReloadRegisterFinanceTable", function () {
        datatable.destroy();
        InitDataTable();
    });
}
function reloadAdminTable() {
    $("#reloadTable").load("/Users/ReloadRegisterAdminTable", function() {
        datatable.destroy();
        InitDataTable();
    });
}

function reloadGroomerTable() {
    $("#reloadTable").load("/Users/ReloadRegisterGroomerTable", function () {
        datatable.destroy();
        InitDataTable();
    });
}

function reloadCustomerTable() {
    $("#reloadTable").load("/Users/ReloadRegisterCustomerTable", function () {
        datatable.destroy();
        InitDataTable();
    });
}

function reloadOwnerTable() {
    $("#reloadTable").load("/Users/ReloadRegisterOwnerTable", function () {
        datatable.destroy();
        InitDataTable();
    });
}

function reloadSuperAdminTable() {
    $("#reloadTable").load("/Users/ReloadRegisterSuperAdminTable", function () {
        datatable.destroy();
        InitDataTable();
    });
}

function reloadContentManagerTable() {
    $("#reloadTable").load("/Users/ReloadRegisterContentManagerTable", function () {
        datatable.destroy();
        InitDataTable();
    });
}
// #endregion

function clearInput() {
    $("#Name").val("");
    $("#Email").val("");
    $("#Password").val("");
    $("#PhoneNumber").val("");
    $("#Address").val("");
    $("#Description").val("");
    
    var PictA = $("#pictureUploader").data().kendoUpload;
    var RemovingPictA = PictA.wrapper.find('.k-file');
    PictA._removeFileEntry(RemovingPictA);
}
$(document).ready(function() {
    InitDataTable();
});