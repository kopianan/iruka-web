let imageDataUrl = "";
let imageName = "";
let imageExtension = "";
let certificateDataUrl = "";
let certificateName = "";
let certificateExtension = "";

function readURL(input) {
    if (input.files && input.files[0]) {
        if (input.files[0].size > 30194304) {
            toastr.error('3mb is maximum accepted file size!');
            input.value = "";
        } else {
            let acceptableFileTypes = ['jpg', 'jpeg', 'png'];
            let reader = new FileReader();
            let extension = input.files[0].name.split('.').pop().toLowerCase(),
                isSuccess = acceptableFileTypes.indexOf(extension) > -1;

            if (extension == "jpg") {
                extension = "jpeg";
            }

            if (isSuccess) {
                reader.fileName = input.files[0].name;
                reader.onload = function (e) {
                    $('#imagePreview').css('background-image', 'url(' + e.target.result + ')');
                    $('#imagePreview').hide();
                    $('#imagePreview').fadeIn(650);

                    imageDataUrl = e.target.result;
                    imageName = e.target.fileName;
                    imageExtension = extension;

                    $('#Base64URL').val(imageDataUrl.replace("data:image/" + imageExtension + ";base64,", ""));
                    $('#PicturePath').val(imageName);
                }
                reader.readAsDataURL(input.files[0]);
            }
            else {
                toastr.error(`Can't read file name!`)
            }
        }
    }
}

$("#imageUpload").change(function () {
    readURL(this);
});

function readURLCertificate(input) {
    if (input.files && input.files[0]) {
        if (input.files[0].size > 30194304) {
            toastr.error('3mb is maximum accepted file size!');
            input.value = "";
        } else {
            let acceptableFileTypes = ['jpg', 'jpeg', 'png'];
            let reader = new FileReader();
            let extension = input.files[0].name.split('.').pop().toLowerCase(),
                isSuccess = acceptableFileTypes.indexOf(extension) > -1;

            if (extension == "jpg") {
                extension = "jpeg";
            }

            if (isSuccess) {
                reader.fileName = input.files[0].name;
                reader.onload = function (e) {
                    $('#certificatePreview').css('background-image', 'url(' + e.target.result + ')');
                    $('#certificatePreview').hide();
                    $('#certificatePreview').fadeIn(650);

                    certificateDataUrl = e.target.result;
                    certificateName = e.target.fileName;
                    certificateExtension = extension;

                    $('#Base64URLCertificate').val(certificateDataUrl.replace("data:image/" + certificateExtension + ";base64,", ""));
                    $('#CertificatePath').val(certificateName);
                    $('.certificate-container').removeClass('d-none');
                }
                reader.readAsDataURL(input.files[0]);
            }
            else {
                toastr.error(`Can't read file name!`)
            }
        }
    }
}

$("#certificateUpload").change(function () {
    readURLCertificate(this);
});

function initDataTable() {
    try {
        dataTable = $('#user-table').DataTable({
            "pageLength": 10,
            "scrollX": true,
            paging: true,
            "order": [],
            "info": false,
            "bPaginate": false,
            "bLengthChange": false,
            "bFilter": true,
            "bInfo": false,
            "bAutoWidth": false,
            dom: 'Bfrtip'
        });
    } catch (err) {
        toastr.error(`There has been an error initiating the DataTable! Error: ${err}`);
    }

}

function deleteUser(id, internalUser) {
    $.confirm({
        title: 'Delete user ?',
        content: `You won't be able to revert this!`,
        buttons: {
            yes: {
                text: 'Yes!',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax({
                        async: false,
                        url: "/Account/DeleteUser",
                        data: "{ 'id': '" + id + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            if (data["success"] != undefined) {
                                $.alert('Deleted successfully!');

                                if (internalUser) {
                                    window.location.replace("/Account/InternalUserRegister");
                                } else {
                                    window.location.replace("/Account/EndUserRegister");
                                }
                            } else {
                                $.alert({
                                    title: 'Encountered an error!',
                                    content: `Can't delete user, please contact support!`,
                                    text: 'Cancelled!',
                                    type: 'orange',
                                    typeAnimated: true
                                });
                            }
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            //alert(errorThrown);
                        }
                    });
                }
            },
            cancel: {
                text: 'Cancel!',
                btnClass: 'btn-green',
                keys: ['enter'],
                action: function () {
                    $.alert('Cancelled!');
                }
            }
        }
    });
}

function InitCitySearchAutoComplete() {
    let elem = $("#CoverageArea");
    let option = {};
    let targetDataSource = new kendo.data.DataSource
        ({
            transport: {
                read: function (options) {
                    var data = [];

                    $.ajax({
                        url: `/API/External/GetAllCitiesOfIndonesia`,
                        method: "GET",
                        dataType: "json",
                        contentType: "application/json",
                        async: false,
                        success: function (response) {
                            data = response;
                        },
                        error: function (response) {
                            let { responseJSON } = response;
                            ClinicGlobal.ShowNotification({
                                text: `There has been an error retrieving data from the server!
                                                                                                                                                                                                                Exception: ${responseJSON.ExceptionMessage}`,
                                type: 'error',
                                timeout: false
                            });
                        }
                    });

                    options.success(data);
                }
            }
        });

    option.select = function (e) {

    };
    option.dataSource = targetDataSource;
    option.placeholder = "Search city...";
    Global.InitKendoAutoComplete(elem, option);
}

$(document).ready(function () {
    initDataTable();
    InitCitySearchAutoComplete();
});