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
        saveUrl: "/Products/SavePicture",
        autoUpload: true
    },
    success: onSuccess,
    remove: onRemove,
    validation: {
        allowedExtensions: [".jpeg", ".jpg", ".png"],
        maxFileSize: 30194304
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
// #endregion

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

$('#gridTable').on('row-reorder.dt', function (dragEvent, data, nodes) {
    for (var i = 0, ien = data.length; i < ien; i++) {
        var rowData = datatable.row(data[i].node).data();
        $.ajax({
            type: "GET",
            cache: false,
            contentType: "application/json; charset=utf-8",
            url: '/Products/UpdateRow',
            data: {fromPosition: data[i].oldData, toPosition: data[i].newData },
            dataType: "json"
        });
    }
});

function InitDataTable() {

    datatable = $('#gridTable').DataTable({
        //columnDefs: [{
        //    orderable: false,
        //    className: 'select-checkbox',
        //    targets: 0,
        //    checkboxes: {
        //        'selectRow': true
        //    }
        //}],
        //"pageLength": 10,
        //"scrollX": true,
        //"language": {
        //    "paginate": {
        //        "previous": "<<",
        //        "next": ">>"
        //    }
        //},
        rowReorder: true,
        columnDefs: [
            { orderable: true, className: 'reorder', targets: 0 },
            { orderable: false, targets: '_all' }
        ],
        //select: true,
        //paging: true,
        "order": [[0, "asc"]],
        //"info": false,
        //"bPaginate": false,
        "bLengthChange": false,
        //"bFilter": true,
        "bInfo": false
        //"bAutoWidth": false,
        //dom: 'Bfrtip'
    });
}

$("#btnSubmitProduct").click(function () {
    var pictureName = PictureUrl.split('\\');
    $.ajax({
        async: false,
        url: "/Products/InsertProduct",
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
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert(errorThrown);
        }
    });
});

function deleteProduct(id) {
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
                url: "/Products/DeleteProduct",
                data: "{ 'id': '" + id + "' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function(data) { return data; },
                success: function(data) {
                    if (data["success"] != undefined) {
                        location.reload(true);

                        Swal.fire(
                            'Deleted!',
                            'Your data has been deleted.',
                            'success'
                        );
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

$(document).ready(function () {
    InitDataTable();
});