var Global = function () {

    var global = function () {
        this.name = "Shuffling";
    }

    global.prototype.InitGrid = function ($elem, options) {

        let { dataSource, columns, height, editable, change, selectable, detailInit, remove, pageable, scrollable, toolbar, pdf, excel, groupable, pdfExport, filterable, sortable, dataBound, excelExport } = options;

        if ($elem != undefined) {

            $elem.kendoGrid({
                dataSource: dataSource != undefined ? dataSource : [],
                columns: columns != undefined ? columns : [],
                selectable: selectable != undefined ? selectable : "row",
                change: change != undefined ? change : function (e) { },
                detailInit: detailInit != undefined ? detailInit : false,
                sortable: sortable != undefined ? sortable : true,
                pageable: pageable != undefined ? pageable : {
                    pageSize: 20,
                    refresh: true,
                    previousNext: false
                },
                editable: editable != undefined ? editable : true,
                scrollable: scrollable != undefined ? scrollable : true,
                height: height != undefined ? height : 400,
                remove: remove != undefined ? remove : function () { return true },
                toolbar: toolbar != undefined ? toolbar : "",
                pdf: pdf != undefined ? pdf : {},
                excel: excel != undefined ? excel : {},
                excelExport: excelExport != undefined ? excelExport : function (e) { },
                groupable: groupable != undefined ? groupable : false,
                pdfExport: pdfExport != undefined ? pdfExport : function () { },
                filterable: filterable != undefined ? filterable : false,
                dataBound: dataBound != undefined ? dataBound : function () {
                    if (window.matchMedia("(max-width: 1366px)").matches) {
                        for (var i = 0; i < this.columns.length; i++) {
                            this.autoFitColumn(i);
                        }
                    }
                    else {
                        for (var i = 0; i < this.columns.length; i++) {
                            this.resizeColumn(this.columns[i], `${(100 / this.columns.length)}%`)
                        }
                    }
                },
                resizable: true
            });

            $elem.append(
                `<div class="d-none spinner-border position-absolute" style="top: 46%; left: 46%;" role="status">
                     <span class="sr-only">Loading...</span>
                 </div>`);

        }
        else {

            $elem.append("<p class='text-center'>There is no data right now!</p>");

        }

    }

    global.prototype.InitDatePicker = function ($elems, option) {

        option = option != undefined ? option : {};

        let { value } = option;


        if ($elems.length > 0) {

            $elems.kendoDatePicker({
                format: "dd-MM-yyyy",
                dateInput: true,
                value: value != undefined ? value : "day-month-year"
            });

        }

    }

    // instantiate the object
    var globalInstance = new global();
    // expose the object
    return globalInstance;
}();