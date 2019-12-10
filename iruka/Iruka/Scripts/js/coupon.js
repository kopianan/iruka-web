let txtDiscountValue = new Cleave("#DiscountValue", {
    numeral: true,
    numericOnly: true,
    numeralDecimalScale: 0,
    numeralDecimalMark: ',',
    delimiter: '.'
});

$(function () {
    InitDomBasedOnCouponType();
});

function InitDomBasedOnCouponType() {
    if ($('#CouponType').val() == 0) {
        $('#discount-section').removeClass('d-none');
    } else {
        $('#product-section').removeClass('d-none');
    }

    $('#CouponType').change(function () {
        if ($('#CouponType').val() == 0) {
            $('#discount-section').removeClass('d-none');
            $('#product-section').addClass('d-none');
        } else {
            $('#product-section').removeClass('d-none');
            $('#discount-section').addClass('d-none');
        }
    });
}

function confirmSubmit(form) {

    if ($('#CouponType').val() == 1) {
        if ($('#FreeProduct').val() == "") {
            toastr.error('Free product cannot be empty!');
        } else {
            $('#DiscountValue').val(0);
            form.submit();
        }
    } else {
        $('#DiscountValue').val(txtDiscountValue.getRawValue());
        form.submit();
    }
}

function deleteCoupon(id) {
    $.confirm({
        title: 'Delete coupon ?',
        content: `You won't be able to revert this!`,
        buttons: {
            yes: {
                text: 'Yes!',
                btnClass: 'btn-red',
                action: function () {
                    $.ajax({
                        async: false,
                        url: "/Coupons/Delete",
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
                                    content: `Can't delete coupon, please contact support!`,
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