let historyGridOptions = {};
let couponGridOptions = {};
let discountTypeHolder = 0;
let discountHolder = 0;

let txtSubTotal = new Cleave("#SubTotal", {
    numeral: true,
    numericOnly: true,
    numeralDecimalScale: 0,
    numeralDecimalMark: ',',
    delimiter: '.'
});

let transactionTypesHolder = [];

$(document).ready(function () {
    $("#scan-result").focus();
    InitTransactionHistoryGrid();
    InitPurchaseableCouponsGrid();
});

$("#btn-input-scan").click(function () {
    let customerId = $('#scan-result').val();

    $.ajax({
        async: false,
        url: `/Transactions/GetCustomerData?userId=${customerId}`,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataFilter: function (data) { return data; },
        success: function (data) {
            if (data["success"] != undefined) {
                //toastr.success('Scanned successfully!')
                let response = data["success"];

                $('#CustomerId').val(response.Id);
                $('#CouponId').val(undefined);
                $('#Total').val(0);
                $('#profile-img').css('background-image', 'url(' + response.Picture + ')');
                $("#lbl-profile-name").text(response.Name);
                $("#lbl-profile-phone").text(response.PhoneNumber);
                $("#lbl-profile-address").text(response.Address);
                $("#lbl-profile-email").text(response.Email);
                $("#lbl-profile-desc").text(response.Description);
                $("#lbl-customer-points").text(response.Points);

                RefreshGridData($('#transaction-history-grid'), historyGridOptions, response.TransactionHistory);
                RefreshGridData($('#purchaseable-coupons-grid'), couponGridOptions, response.PurchaseableCoupons);

                $('fieldset').hide();
                $('fieldset').fadeIn(750);
            }

            if (data["error"] == "null") {
                toastr.error('Customer Data Not Found!')
            }

            if (data["error"] == "error") {
                toastr.error('Error found, please contact Administrator!')
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //alert(errorThrown);
        }
    });
});

function InitTransactionHistoryGrid() {
    historyGridOptions.pageable = false;
    historyGridOptions.scrollable = true;
    historyGridOptions.editable = false;
    historyGridOptions.dataBound = function () {
        for (var i = 0; i < this.columns.length; i++) {
            this.autoFitColumn(i);
        }
    };
    historyGridOptions.columns = [
        {
            field: "CreatedDate",
            title: "Date"
        }, {
            field: "TransactionType",
            title: "Transaction Type"
        }, {
            field: "SubTotal",
            title: "Shopping Total",
            format: "Rp. {0:N0}"
        }, {
            field: "CouponValue",
            title: "Coupon Usage"
        }, {
            field: "Total",
            format: "Rp. {0:N0}"
        }, {
            field: "EarnedPoint",
            title: "Earned Points"
        }, {
            field: "Notes",
            title: "Notes"
        }
    ];

    Global.InitGrid($('#transaction-history-grid'), historyGridOptions);
}

function InitPurchaseableCouponsGrid() {
    couponGridOptions.pageable = false;
    couponGridOptions.scrollable = true;
    couponGridOptions.editable = false;
    couponGridOptions.columns = [
        {
            field: "Id",
            hidden: true
        }, {
            field: "CouponType",
            hidden: true
        }, {
            field: "DiscountType",
            hidden: true
        }, {
            field: "DiscountValue",
            hidden: true
        }, {
            field: "CouponTypeValue",
            title: "Coupon Type",
            width: 140
        }, {
            field: "CouponValue",
            title: "Value"
        }, {
            field: "PointPrice",
            title: "Price in Point",
            width: 140
        }, {
            title: "Action",
            template: `<button type="button" class="btn btn-warning text-white font-weight-bold" onclick="UseCoupon($(this).parent().parent())">Use</button>`,
            width: 90
        }
    ];

    Global.InitGrid($('#purchaseable-coupons-grid'), couponGridOptions);
}

function RefreshGridData(elem, options, data) {
    let gridInstance = elem.data('kendoGrid');
    options.dataSource = data;
    gridInstance.setOptions(options);
    gridInstance.refresh();
}

function CalculateInvoiceTotal() {
    let discountTotal = 0;
    let invoiceTotal = 0;

    if (discountTypeHolder == 0) {
        discountTotal = discountHolder;
    } else {
        discountTotal = discountHolder / 100 * parseFloat(txtSubTotal.getRawValue());
    }

    invoiceTotal = parseFloat(txtSubTotal.getRawValue()) - parseFloat(discountTotal);

    $('#Total').val(invoiceTotal);
    $('#lbl-invoice-total').html(kendo.format("Rp. {0:N0}", invoiceTotal));
}

function UseCoupon(data) {
    $.confirm({
        title: 'Confirmation',
        content: `Use coupon ?`,
        buttons: {
            yes: {
                text: 'Yes!',
                btnClass: 'btn-red',
                action: function () {
                    let grid = $("#purchaseable-coupons-grid").data("kendoGrid");
                    var selectedCoupon = grid.dataItem(data);

                    if (selectedCoupon.CouponType == 0) {
                        $('#discount-coupon').removeClass('d-none');
                        $('#product-coupon').addClass('d-none');

                        discountTypeHolder = selectedCoupon.DiscountType;
                        discountHolder = selectedCoupon.DiscountValue;

                        $('#discount-coupon-value').text(selectedCoupon.CouponValue);
                        $('#CouponId').val(selectedCoupon.Id);
                    } else {
                        $('#discount-coupon').addClass('d-none');
                        $('#product-coupon').removeClass('d-none');

                        discountTypeHolder = 0;
                        discountHolder = 0;

                        $('#product-coupon-value').text(selectedCoupon.CouponValue);
                        $('#CouponId').val(selectedCoupon.Id);
                    }

                    CalculateInvoiceTotal();
                }
            },
            cancel: {
                text: 'Cancel!',
                btnClass: 'btn-green',
                keys: ['enter'],
                action: function () {

                }
            }
        }
    });
}

$('#SubTotal').keyup(function () {
    if ($('#SubTotal').val() == "") {
        txtSubTotal.setRawValue(0);
    }

    CalculateInvoiceTotal();
});

function confirmSubmit(form) {
    $.confirm({
        title: 'Confirmation',
        content: `Submit transaction data ?`,
        buttons: {
            yes: {
                text: 'Yes!',
                btnClass: 'btn-red',
                action: function () {
                    if ($('#CustomerId').val() == "") {
                        toastr.error('Please input customer data!');
                    } else {
                        if ($('#Total').val() <= 0) {
                            toastr.error('Please input transaction amount!');
                        } else {
                            if ($("input[name='transaction-types']:checked").length == 0) {
                                toastr.error('Please fill transaction type!');
                            } else {
                                $('#SubTotal').val(txtSubTotal.getRawValue());
                                $.each($("input[name='transaction-types']:checked"), function () {
                                    transactionTypesHolder.push($(this).val());
                                });
                                $('#TransactionType').val(transactionTypesHolder.join(', '));
                                form.submit();
                            }
                        }
                    }
                }
            },
            cancel: {
                text: 'Cancel!',
                btnClass: 'btn-green',
                keys: ['enter'],
                action: function () {

                }
            }
        }
    });
}