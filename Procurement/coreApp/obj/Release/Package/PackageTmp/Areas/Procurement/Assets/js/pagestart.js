

$(document).ready(function () {
    mainlist.itemsLoaded = function () {
        var btnAddItems = $('.btn-additems');
        btnAddItems.click(function (e) {
            dialog.addItem();
        });

        var btnImportItems = $('.btn-importitems');
        btnImportItems.click(function (e) {
            dialog.importItem();
        });
        dialog.setImportItem(btnImportItems);

        var btnClearItems = $('.btn-clearitems');
        btnClearItems.click(function (e) {
            dialog.clearItems();
        });

        var btnSubmit = $('.btn-submit');
        btnSubmit.click(function (e) {
            dialog.submit();
        });

        var btnPrint = $('.btn-print');
        btnPrint.click(function (e) {
            var btn = $(this);
            var msg = btn.attr('data-printmsg');
            dialog.print(msg);
        });
    };

    mainlist.itemSelected = function (data) {
        dialog.editItem(data);    
    };

    itemlist.itemSelected = function (data) {
        var matchedData = {};

        //get current data
        mainlist.mainlistCont.find('tr[data-item]').toArray().some(function (v) {
            var tr = $(v);
            var d = JSON.parse(tr.attr('data-item'));
            if (d.ItemId == data.id) {
                matchedData = d;
                return true;
            } else {
                return false;
            }
        });

        var d = $.extend({}, data, matchedData);

        dialog.showQty(d);
    };

    mainlist.init();
    itemlist.init();
    dialog.init();

    
});