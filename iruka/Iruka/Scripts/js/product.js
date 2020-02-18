let pictureDataUrl = "";
let pictureName = "";
let pictureExtension = "";

function readPictureURL(input) {
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
                    $('#picture-preview').css('background-image', 'url(' + e.target.result + ')');
                    $('#picture-preview').hide();
                    $('#picture-preview').fadeIn(650);

                    pictureDataUrl = e.target.result;
                    pictureName = e.target.fileName;
                    pictureExtension = extension;

                    $('#Base64URL').val(pictureDataUrl.replace("data:image/" + pictureExtension + ";base64,", ""));
                    $('#PicturePath').val(pictureName);
                    $('.picture-container').removeClass('d-none');
                }
                reader.readAsDataURL(input.files[0]);
            }
            else {
                toastr.error(`Can't read file name!`)
            }
        }
    }
}

$("#picture-upload").change(function () {
    readPictureURL(this);
});

$('#ScheduleDate').datetimepicker({
    weekStart: 1,
    todayBtn: 1,
    autoclose: 1,
    todayHighlight: 1,
    startView: 2,
    minView: 2,
    forceParse: 0,
    startDate: new Date()
});

function InitOnGoingProductsDataTable() {

    onGoingDataTable = $('#ongoing-products-table').DataTable({
        "pageLength": 10,
        rowReorder: true,
        columnDefs: [
            { orderable: false, className: 'reorder', targets: 0 },
            { orderable: false, targets: '_all' }
        ],
        "order": [[0, "asc"]],
        "bLengthChange": false,
        "bInfo": false,
        "scrollX": true
    });
}

function InitPendingProductsDataTable() {
    pendingDataTable = $('#pending-products-table').DataTable({
        "pageLength": 10,
        "order": [],
        "bLengthChange": false,
        "bInfo": false,
        "scrollX": true
    });
}

function InitFinishedProductsDataTable() {
    finishedDataTable = $('#finished-products-table').DataTable({
        "pageLength": 10,
        "order": [],
        "bLengthChange": false,
        "bInfo": false,
        "scrollX": true
    });
}

$('#ongoing-products-table').on('row-reorder.dt', function (dragEvent, data, nodes) {

    let rowArray = [];

    $.each(data, function (i, v) {
        let rowObject = {
            OldData: v.oldData,
            NewData: v.newData,
        };

        rowArray.push(rowObject);
    });

    let obj = {
        UserId: userId,
        RowArray: JSON.stringify(rowArray)
    };

    $.ajax({
        url: "/API/Product/UpdateRow",
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify(obj),
        error: function () {
            toastr.error("There has been an error! Try refreshing the page.");
        },
        success: function () {
            toastr.success("Priority updated!");
        },
        async: false
    });

});

function deleteProduct(id) {
    $.confirm({
        title: 'Delete product ?',
        content: `You won't be able to revert this!`,
        buttons: {
            yes: {
                text: 'Yes!',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax({
                        async: false,
                        url: "/Products/DeleteProduct",
                        data: "{ 'id': '" + id + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            if (data["success"] != undefined) {
                                $.alert('Deleted successfully!');
                                location.reload(true);
                            } else {
                                $.alert({
                                    title: 'Encountered an error!',
                                    content: `Can't delete product, please contact support!`,
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

function startProduct(id) {
    $.confirm({
        title: 'Start Product Manually ?',
        buttons: {
            yes: {
                text: 'Yes!',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax({
                        async: false,
                        url: "/Products/StartProduct",
                        data: "{ 'id': '" + id + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            if (data["success"] != undefined) {
                                $.alert('Action completed successfully!');
                                location.reload(true);
                            } else {
                                $.alert({
                                    title: 'Encountered an error!',
                                    content: `Can't do action, please contact support!`,
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

function completeProduct(id) {
    $.confirm({
        title: 'Complete product ?',
        content: `You won't be able to revert this!`,
        buttons: {
            yes: {
                text: 'Yes!',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax({
                        async: false,
                        url: "/Products/CompleteProduct",
                        data: "{ 'id': '" + id + "' }",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            if (data["success"] != undefined) {
                                $.alert('Action completed successfully!');
                                location.reload(true);
                            } else {
                                $.alert({
                                    title: 'Encountered an error!',
                                    content: `Can't do action, please contact support!`,
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

$(document).ready(function () {
    InitOnGoingProductsDataTable();
    InitPendingProductsDataTable();
    InitFinishedProductsDataTable();
});