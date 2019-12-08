$("#btnInputUserId").click(function () {
    //$.ajax({
    //    async: false,
    //    url: "/Transactions/GetUserInfo",
    //    data: "{ 'userId': '" + $("#scanId").val() + "'}",
    //    dataType: "json",
    //    type: "POST",
    //    contentType: "application/json; charset=utf-8",
    //    dataFilter: function (data) { return data; },
    //    success: function (data) {
    //        if (data["success"] != undefined) {
    //            location.reload();
    //            $('#reloadTable').load('/GuestInvitations/InputFilterRegulerVisitorSummary?id=' + temp, function () {
    //                datatable.destroy();
    //                InitDataTable();
    //            });
    //        }
    //    },
    //    error: function (XMLHttpRequest, textStatus, errorThrown) {
    //        //alert(errorThrown);
    //    }
    //});

    $("#nameProfile").text("Lukito Hadisaputra");
    $("#addressProfile").text("Holis Regency Blok M30");
    $("#emailProfile").text("lukito@iruka.com");
    $("#phoneProfile").text("+987654321");
    $("#descProfile").text("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.");
    var address = "http://iruka.diodeiva.dev/Media/ProfileTemp1.jpg";
    $('#imgProfile').css('background-image', 'url(' + address + ')');
    $('fieldset').hide();
    $('fieldset').fadeIn(650);
   
    $(".coupon-image").removeClass("d-none");

    $("#productPoint").text("195");
    $("#transTypePoint").text("Service");
    $("#descPoint").text("Transaction Description");
    $("#datePoint").text("24 November 2019");
});

function InitDataTable() {

    datatable = $('#historyTable').DataTable({
        "scrollX": true,
        "scrollY": true,
        "order": [[0, "asc"]],
        "info": false,
        "bPaginate": false,
        "bLengthChange": false,
        "bFilter": false,
        "bInfo": false,
        "bAutoWidth": false,
        dom: 'Bfrtip'
    });
}

function checkCoupon() {
    if ($("#chkProduct").is(":checked")) {
        $("#currentProductPoint").text("1");
        $(".coupon").removeClass("d-none");
    } else {
        $("#currentProductPoint").text("0");
        $(".coupon").addClass("d-none");

    }

    if ($("#chkService").is(":checked")) {
        $("#currentServicePoint").text("1");
    } else {
        $("#currentServicePoint").text("0");
    }
}

$(document).ready(function () {
    InitDataTable();
});