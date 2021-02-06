function selectPo(e) {
    e.modal.find("#searchponos").DataTable()
    var inputSearchPONo = e.modal.find("#SearchPONo");
    var savepotable = e.modal.find("#savepotable");
    var cancelpotable = e.modal.find("#closepotable");
    var isSelectButtonVisible = e.modal.find(".isSelectButtonVisible");
    var ponotext = e.modal.find("#ponotext");
    var searchponotable = e.modal.find("#searchponotable")
    searchponotable.css({ display: "none" });
    var searchponoTbody = e.modal.find('#searchponos tbody')
    //e.modal.find('#savepotable')[0].disabled = true
    var gettempposdata = '', getposdata = ""

    searchponoTbody.off("click").on('click', 'tr', function () {

        if ($(this).hasClass('selected')) {
            // $(this).removeClass('selected');
        }
        else {

            e.modal.find("#searchponos").DataTable().$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
        var attributes = e.modal.find("#searchponos").DataTable().$('tr.selected')
        if (attributes[0]) {
            gettempposdata = attributes[0].attributes["data-name"].value
            getposdata = gettempposdata

            /* attributes.dblclick(function () {

            });*/
            inputSearchPONo.val(getposdata)
            ponotext.text(getposdata)
            gettempposdata = ""
            isSelectButtonVisible.css({ display: "block" });
            searchponotable.css({ display: "none" });
        }
        /*
        if (gettempposdata) {
            e.modal.find('#savepotable')[0].disabled = false
        } else {
            e.modal.find('#savepotable')[0].disabled = true
        }
        */
    });

    savepotable.off('click').on('click', function (e) {
        getposdata = gettempposdata
        inputSearchPONo.val(getposdata)
        ponotext.text(getposdata)
        gettempposdata = ""
        isSelectButtonVisible.css({ display: "block" });
        searchponotable.css({ display: "none" });
    });
    cancelpotable.off('click').on('click', function (e) {
        gettempposdata = ""
        if (getposdata) {
            isSelectButtonVisible.css({ display: "block" });
            searchponotable.css({ display: "none" });
        } else {
            inputSearchPONo.val("")
            ponotext.text("")
            isSelectButtonVisible.css({ display: "block" });
            searchponotable.css({ display: "none" });
        }

    })
    e.modal.find('#browse').off('click').on('click', function (e) {

        $.ajax({
            type: "GET",
            url: "/SAM/PO/POs",
            datatype: JSON,
            success: function (response) {
                if (response.Success) {
                    gettempposdata = ""
                    $("#searchponos").DataTable().clear();
                    $("#searchponos").DataTable().destroy();
                    searchponoTbody.empty()
                    isSelectButtonVisible.css({ display: "none" });
                    searchponotable.css({ display: "block" });
                    var data = JSON.parse(response.model);
                    var i = 0;
                    while (i < data.length) {
                        var item = data[i];
                        var row = "<tr " + `class='${getposdata == item.Item.PONo ? "selected" : ""}'` + " data-name=" + item.Item.PONo + " name=" + item.Item.Id + ">" +
                            "<td>" + item.Item.PONo + "</td>" +
                            "<td>" + item.Item.PODate + "</td>" +
                            "<td>" + item.Item.PRNo + "</td>" +
                            "<td>" + item.Item.PRDate + "</td>" +
                            "<td>" + item.Item._SupplierName + "</td>" +
                            "<td>" + item.POModel.Status_Desc + "</td>" +
                            "<td>" + item.POModel.Amount + "</td>" + "</tr>"
                        searchponoTbody.append(row)
                        i++;
                    }
                    $("#searchponos").DataTable();
                }

            }
        }, "json")


    })
}