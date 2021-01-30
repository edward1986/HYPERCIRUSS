var _cbSelect = {
    cont: null,
    selectall: null,
    cbs: null,
    init: function (obj) {
        var me = this;
        me.cont = obj;

        me.selectall = obj.find('.selectall');
        me.cbs = obj.find('tbody .selectitem');

        me.selectall.click(function () {
            me.cbs.prop('checked', $(this).prop('checked'));
            $.uniform.update(me.cbs)
        });

        me.cbs.click(function () {
            me.updateParent();
        });
    },
    updateParent: function () {
        var me = this;

        var selected = me.cont.find('tbody [type="checkbox"]:checked').length;
        var cbcount = me.cont.find('tbody [type="checkbox"]').length;
        var unselected = cbcount - selected;

        if (selected == cbcount) {
            me.selectall.prop('checked', true);
        }

        if (unselected > 0) {
            me.selectall.prop('checked', false);
        }

        $.uniform.update(me.selectall);
    },
    getSelection: function (nameAttr, oTable) {
        var me = this;
        var ret = [];

       
        if (oTable) {
            var selector = '[type="checkbox"]';
            if (nameAttr) {
                selector = '[name="' + nameAttr + '"]'
            }

            var nodes = oTable.rows().nodes();

            for (var i = 0; i < nodes.length; i++) {
                var tr = $(nodes[i]);

                var cb = tr.find(selector);
                if (cb.prop('checked')) {
                    var id = cb.attr('data-item');
                    ret.push(id);
                }                
            };

        } else {
            var selector = 'tbody [type="checkbox"]:checked';
            if (nameAttr) {
                selector = 'tbody [name="' + nameAttr + '"]:checked'
            }

            me.cont.find(selector).each(function (i, v) {
                var cb = $(v);
                var id = cb.attr('data-item');
                ret.push(id);
            });
        }

        return ret;
    }
};


var mainlist = {
    mainlistCont: $('.mainlist-cont'),
    itemSelected: null,
    itemsLoaded: null,
    init: function () {
        var me = this;
        me.load();
    },
    load: function (isReload) {
        var me = this;
        var url = mainListUrl;

        me.mainlistCont.addClass('loading-mask');
        me.mainlistCont.load(url, null, function () {
            setStateKeys(me.mainlistCont);

            var obj = me.mainlistCont.find('table.maintable');
            var tableBindCallback = obj.attr('event-tablebind');
            var doneCallback = obj.attr('event-settables-done');


            var table = me.prepTable(obj, isReload, function () {
                me.bind(obj);

                if (tableBindCallback) {
                    window[tableBindCallback](obj);
                }
            });

            me.bind(obj);

            if (tableBindCallback) {
                window[tableBindCallback](obj);
            }

            if (me.itemsLoaded) {
                me.itemsLoaded();
            }

            if (doneCallback) {
                window[doneCallback](table);
            }
            
            me.mainlistCont.removeClass('loading-mask');
        })
    },
    bind: function (obj) {
        var me = this;

        obj.find('tr').click(function (e) {
            e.preventDefault();
            var tr = $(this);

            if (me.itemSelected) {
                var data = JSON.parse(tr.attr('data-item'));
                me.itemSelected(data);
            }

        });
                        
    },
    prepTable: function (obj, isReload, drawCallback) {
        var useEachInsteadOfEvery = obj.hasClass('use-each');

        var opts = {
            "bStateSave": !isReload,
            "fnStateSave": function (oSettings, oData) {
                sessionStorage.setItem('DataTables', JSON.stringify(oData));
            },
            "fnStateLoad": function (oSettings) {
                return JSON.parse(sessionStorage.getItem('DataTables'));
            },
            "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
        };

        $.fn.dataTable.moment('MM/DD/YYYY');
                
        obj.find('tfoot tr:last-child').find('th').not('.nofilter').each(function () {
            var title = obj.find('thead th').eq($(this).index()).text().trim();
            $(this).html('<input type="text" class="col-filter" placeholder="Search ' + title + '" />');
        });

        obj.DataTable().destroy();
        var table = obj.DataTable(opts);

        table.on('draw.dt', function () {
            if (drawCallback) drawCallback();
        });

        if (useEachInsteadOfEvery) {
            table.columns().each(function (v, i) {
                var column = this;

                var input = $(column.header()).closest('table').find('tfoot tr th').eq(i).find('input');
                input.on('keyup change', function () {
                    column
                        .search(this.value)
                        .draw();
                });
            });
        } else {
            table.columns().every(function () {
                var column = this;

                var input = $(column.header()).closest('table').find('tfoot tr th').eq(column.index()).find('input');
                input.on('keyup change', function () {
                    column
                        .search(this.value)
                        .draw();
                });
            });
        }

        return table;
    }
};

var itemlist = {
    itemlistCont: $('.itemlist-cont'),
    itemSelected: null,
    init: function () {
        var me = this;
        me.load();
    },
    load: function () {
        var me = this;
        var url = itemListUrl;

        me.itemlistCont.addClass('loading-mask');
        me.itemlistCont.load(url, null, function () {

            var obj = me.itemlistCont.find('table.table');
            mainlist.prepTable(obj, false, function () {
                me.bind(obj);
            });

            me.bind(obj);

            me.itemlistCont.removeClass('loading-mask');
        })
    },
    bind: function (obj) {
        var me = this;

        obj.find('tbody tr').unbind('click').bind('click', function (e) {
            e.preventDefault();
            var tr = $(this);

            var itemId = tr.attr('record-id');
            var itemName = tr.attr('data-item-name');
            var itemPrice = tr.attr('data-item-price');
            var itemUnit = tr.attr('data-item-unit');

            var data = {
                id: itemId,
                name: itemName,
                price: itemPrice,
                unit: itemUnit
            };

            if (me.itemSelected) {
                me.itemSelected(data);
            }

        });
    }
};

var dialog = {
    modal: $('#modalItemQty'),
    title: $('#modalItemQty .modal-title'),
    body: $('#modalItemQty .modal-body'),
    onEditMode: function () {
        return this.modal.hasClass('editmode');
    },
    init: function () {
        var me = this;
        me.modal.children('.modal-dialog').css({ width: $(window).width() * .75, height: $(window).height(), 'padding-top': 15, 'padding-bottom': 15 });
        me.bind();

        me.modal.removeClass('readonly');
        if (dialogIsReadOnly) {
            me.modal.addClass('readonly');
        }
    },
    bind: function () {
        var me = this;

        me.modal.find('div.qty input[type="text"]').keypress(function (e) {
            return $.isNumeric(e.key);
        });

        me.modal.find('.btn-cancel-qty').unbind().bind('click', function (e) {
            e.preventDefault();
            e.stopPropagation();

            if (me.onEditMode()) {
                me.modal.modal('hide');
            } else {
                me.showitems();
            }
        });

        me.modal.find('.btn-delete').unbind().bind('click', function (e) {
            e.preventDefault();
            e.stopPropagation();

            modalConfirm('Are you sure you want to delete this item?', function (ret) {
                if (ret) {

                    me.delete(function (res) {
                        if (res.IsSuccessful) {
                            showClientMessage(res.Remarks);
                            mainlist.load(true);
                            me.modal.modal('hide');
                        } else {
                            modalMessage(res.Err.split('\n'), 'Delete Item', true);
                        }
                    });

                }
            });
        });

        me.modal.find('.btn-save').unbind().bind('click', function (e) {
            e.preventDefault();
            e.stopPropagation();

            me.save(function (res) {
                if (res.IsSuccessful) {
                    showClientMessage(res.Remarks);
                    mainlist.load(true);
                    me.modal.modal('hide');
                } else {
                    modalMessage(res.Err.split('\n'), 'Save Item', true);
                }
            });
        });

        me.modal.find('.btn-saveandcontinue').unbind().bind('click', function (e) {
            e.preventDefault();
            e.stopPropagation();

            me.save(function (res) {
                if (res.IsSuccessful) {
                    showClientMessage(res.Remarks);
                    mainlist.load(true);
                    me.showitems();
                } else {
                    modalMessage(res.Err.split('\n'), 'Save Item', true);
                }
            });

        });

        me.body.find('.qty').unbind().keyup(function (e) {
            if (e.which == 13 && !dialogIsReadOnly) {
                if (me.onEditMode()) {
                    me.modal.find('.btn-save').click();
                } else {
                    me.modal.find('.btn-saveandcontinue').click();
                }
            }
        });
    },
    delete: function (callback) {
        var me = this;
        var data = me.get();

        $.post(deleteItemUrl, data, function (res) {
            callback(res);
        }, 'json');
    },
    save: function (callback) {
        var me = this;
        var data = me.get();

        $.post(saveItemUrl, data, function (res) {
            callback(res);
        }, 'json');
    },
    clear: function () {
        var me = this;
        var inputs = me.body.find('.qty td input[type="text"]');
        
        inputs.val('');

        if (dialogIsReadOnly) {
            inputs.attr('readonly', 'readonly');
        } else {
            inputs.removeAttr('readonly');
        }
    },
    fill: function (data) {
        var me = this;

        var m1 = data.M1 == null ? 0 : data.M1;
        var m2 = data.M2 == null ? 0 : data.M2;
        var m3 = data.M3 == null ? 0 : data.M3;
        var m4 = data.M4 == null ? 0 : data.M4;
        var m5 = data.M5 == null ? 0 : data.M5;
        var m6 = data.M6 == null ? 0 : data.M6;
        var m7 = data.M7 == null ? 0 : data.M7;
        var m8 = data.M8 == null ? 0 : data.M8;
        var m9 = data.M9 == null ? 0 : data.M9;
        var m10 = data.M10 == null ? 0 : data.M10;
        var m11 = data.M11 == null ? 0 : data.M11;
        var m12 = data.M12 == null ? 0 : data.M12;

        me.body.find('.qty td input.m1').val(m1 == 0 ? '' : m1);
        me.body.find('.qty td input.m2').val(m2 == 0 ? '' : m2);
        me.body.find('.qty td input.m3').val(m3 == 0 ? '' : m3);
        me.body.find('.qty td input.m4').val(m4 == 0 ? '' : m4);
        me.body.find('.qty td input.m5').val(m5 == 0 ? '' : m5);
        me.body.find('.qty td input.m6').val(m6 == 0 ? '' : m6);
        me.body.find('.qty td input.m7').val(m7 == 0 ? '' : m7);
        me.body.find('.qty td input.m8').val(m8 == 0 ? '' : m8);
        me.body.find('.qty td input.m9').val(m9 == 0 ? '' : m9);
        me.body.find('.qty td input.m10').val(m10 == 0 ? '' : m10);
        me.body.find('.qty td input.m11').val(m11 == 0 ? '' : m11);
        me.body.find('.qty td input.m12').val(m12 == 0 ? '' : m12);
    },
    get: function () {
        var me = this;
        var id = me.body.find('.qty input.itemid').val();
        var m1 = me.body.find('.qty td input.m1').val();
        var m2 = me.body.find('.qty td input.m2').val();
        var m3 = me.body.find('.qty td input.m3').val();
        var m4 = me.body.find('.qty td input.m4').val();
        var m5 = me.body.find('.qty td input.m5').val();
        var m6 = me.body.find('.qty td input.m6').val();
        var m7 = me.body.find('.qty td input.m7').val();
        var m8 = me.body.find('.qty td input.m8').val();
        var m9 = me.body.find('.qty td input.m9').val();
        var m10 = me.body.find('.qty td input.m10').val();
        var m11 = me.body.find('.qty td input.m11').val();
        var m12 = me.body.find('.qty td input.m12').val();

        var data = {
            ItemId: id,
            PPMPId: ppmpId
        };

        data.M1 = SITE.Utility.isNumber(m1) ? parseInt(m1) : 0;
        data.M2 = SITE.Utility.isNumber(m2) ? parseInt(m2) : 0;
        data.M3 = SITE.Utility.isNumber(m3) ? parseInt(m3) : 0;
        data.M4 = SITE.Utility.isNumber(m4) ? parseInt(m4) : 0;
        data.M5 = SITE.Utility.isNumber(m5) ? parseInt(m5) : 0;
        data.M6 = SITE.Utility.isNumber(m6) ? parseInt(m6) : 0;
        data.M7 = SITE.Utility.isNumber(m7) ? parseInt(m7) : 0;
        data.M8 = SITE.Utility.isNumber(m8) ? parseInt(m8) : 0;
        data.M9 = SITE.Utility.isNumber(m9) ? parseInt(m9) : 0;
        data.M10 = SITE.Utility.isNumber(m10) ? parseInt(m10) : 0;
        data.M11 = SITE.Utility.isNumber(m11) ? parseInt(m11) : 0;
        data.M12 = SITE.Utility.isNumber(m12) ? parseInt(m12) : 0;

        return data;
    },
    editItem: function (data) {
        var me = this;

        me.modal.addClass('editmode');

        var title = dialogIsReadOnly ? 'View Item' : 'Update Item';
        me.title.html(title);

        me.body.find('.qty .itemid').val(data.ItemId);
        me.body.find('.qty .itemname').html(data.Item.Name);
        me.body.find('.qty .itemprice').html(SITE.Utility.formatNumber(data.Item.Price, '#,##0.00') + ' / ' + data.Item.Unit.Unit);

        me.clear();
        if (data != null) me.fill(data);

        me.modal.addClass('setqty');

        me.modal
            .modal({
                backdrop: 'static',
                show: true
            });
    },
    setImportItem: function (importItemsBtn) {
        var me = this;

        var forAPP = false;
        var forAPR = false;
        var forPR = false;
        var forCPR = false;
        var forCompanyPR = false;
        var forRFQ = false;
        var forAOB = false;
        var forPO = false;

        var _forapp = $(importItemsBtn).attr('data-forapp');
        var _forapr = $(importItemsBtn).attr('data-forapr');
        var _forpr = $(importItemsBtn).attr('data-forpr');
        var _forcpr = $(importItemsBtn).attr('data-forcpr');
        var _forcompanypr = $(importItemsBtn).attr('data-forcompanypr');
        var _forrfq = $(importItemsBtn).attr('data-forrfq');
        var _foraob = $(importItemsBtn).attr('data-foraob');
        var _forpo = $(importItemsBtn).attr('data-forpo');

        if (_forapp != undefined) {
            forAPP = JSON.parse(_forapp);
        }

        if (_forapr != undefined) {
            forAPR = JSON.parse(_forapr);
        }

        if (_forpr != undefined) {
            forPR = JSON.parse(_forpr);
        }

        if (_forcpr != undefined) {
            forCPR = JSON.parse(_forcpr);
        }

        if (_forcompanypr != undefined) {
            forCompanyPR = JSON.parse(_forcompanypr);
        }

        if (_forrfq != undefined) {
            forRFQ = JSON.parse(_forrfq);
        }

        if (_foraob != undefined) {
            forAOB = JSON.parse(_foraob);
        }

        if (_forpo != undefined) {
            forPO = JSON.parse(_forpo);
        }

        var bindOk = function () {
            var btnOk = m.find('.modal-footer .btn-ok');

            btnOk.click(function () {
                var title = isCompany ? 'Add Items' : 'Import Items';

                var ids =
                    forAOB ? ui.find('table.selection-list [name="RFQ"]:checked').val() :
                        forRFQ ? ui.find('table.selection-list [name="CPR"]:checked').val() :
                            forPR ? ui.find('table.selection-list [name="PPMP"]:checked').val() :
                                forPO ? ui.find('table.selection-list [name="AOB"]:checked').val() :
                                    forCompanyPR ? ui.find('table.selection-list [name="APR"]:checked').val() : cbSelect.getSelection();

                var cids = '';

                if (forCPR) {
                    ids = cbSelect.getSelection('PR');
                    cids = cbSelect.getSelection('CompanyPR');
                }

                var category_ids = cbSelectCategories.getSelection();
                var indbm_ids = cbSelectInDBM.getSelection();
                var period_ids = cbSelectPeriod.getSelection();
                var fund_ids = cbSelectFund.getSelection();
                var err = [];

                if (forCPR) {
                    if (ids.length == 0 && cids.length == 0) { err.push('No item selected'); }
                } else {
                    if (ids.length == 0) { err.push('No item selected'); }
                }

                if (forAPP && indbm_ids.length == 0) { err.push('No In DBM flag selected'); }
                if (!forCompanyPR && category_ids.length == 0) { err.push('No category selected'); }
                if (!forCompanyPR && period_ids.length == 0) { err.push('No month selected'); }
                if (!forCompanyPR && forAPR && fund_ids.length == 0) { err.push('No fund selected'); }

                if (err.length > 0) {
                    modalMessage(err, title, true, 9999999999999);
                    return;
                }

                ui.closest('.modal-content').addClass('loading-mask');

                var data = {
                    ids: ids,
                    indbm_ids: indbm_ids,
                    category_ids: category_ids,
                    period_ids: period_ids
                };

                if (forAPR) {
                    data.fund_ids = fund_ids;
                } else if (forCompanyPR) {
                    data.id = ids;
                } else if (forCPR) {
                    data.cids = cids;
                    data.fund_ids = fund_ids;
                }

                $.post(importItemsUrl, data, function (res) {
                    if (res.IsSuccessful) {
                        window.location.reload();
                    } else {
                        modalMessage(res.Err.split('\n'), title, true, 9999999999999);
                        ui.closest('.modal-content').removeClass('loading-mask');
                    }
                }, 'json');

            });
        };
        
        var m = $('#modalImport');
        m.children('.modal-dialog').css({ width: $(window).width() - 30, height: $(window).height(), 'padding-top': 15, 'padding-bottom': 15 });

        var title =
            forPO ? 'Add/Remove AOBs' :
                forAOB ? 'Add/Remove RFQs' :
                    forPR ? 'Add/Remove Office PPMPs' :
                        forRFQ ? 'Add/Remove Consolidated PRs' : 
                            forCPR ? 'Add/Remove PRs' :
                                forAPR ? 'Add/Remove Agency APPs (For Consolidation)' :
                                    forCompanyPR ? 'Select Agency APR' :
                                        isCompany ? 'Add/Remove Office PPMPs (For Consolidation)' : 'Import Items From Department PPMPs';

        m.find('.modal-title').html(title);

        var ui = m.find('.ppmp-select');
        var cbSelect = $.extend({}, _cbSelect);
        var cbSelectCategories = $.extend({}, _cbSelect);
        var cbSelectInDBM = $.extend({}, _cbSelect);
        var cbSelectPeriod = $.extend({}, _cbSelect);
        var cbSelectFund = $.extend({}, _cbSelect);
        
        var cbs = ui.find('[type="checkbox"]');
        cbs.uniform();
        cbSelect.init(ui.find('table.selection-list'));
        cbSelectCategories.init(ui.find('table.category-list'));
        cbSelectInDBM.init(ui.find('table.indbm-list'));
        cbSelectPeriod.init(ui.find('table.period-list'));
        cbSelectFund.init(ui.find('table.fund-list'));

        bindOk();

    },
    importItem: function () {
        
        var m = $('#modalImport');

        m
            .unbind('show.bs.modal')
            .modal({
                backdrop: 'static',
                show: true
            });

    },
    addItem: function () {
        var me = this;

        me.modal.removeClass('editmode');
        me.modal.addClass('modal fade modal-view');

        me.title.html('Add Item(s)');
        me.showitems();
        
        me.modal
            .modal({
                backdrop: 'static',
                show: true
            });
    },
    clearItems: function () {
        var me = this;

        modalConfirm('Are you sure you want to clear all items?', function (ret) {
            if (ret) {

                $.post(clearItemsUrl, null, function (res) {
                    if (res.IsSuccessful) {
                        showClientMessage(res.Remarks);
                        mainlist.load(true);
                    } else {
                        modalMessage(res.Err.split('\n'), 'Clear Items', true);
                    }
                }, 'json');

            }
        });
    },
    submit: function () {
        var me = this;

        modalConfirm('Are you sure you want to submit this document?', function (ret) {
            if (ret) {
                var proc = modalProcessing();

                $.post(submitUrl, null, function (res) {
                    proc.modal('hide');

                    if (res.IsSuccessful) {
                        window.location.reload();
                    } else {
                        modalMessage(res.Err.split('\n'), 'Submit Document', true);
                    }
                    
                }, 'json');

            }
        });
    },
    print: function (msg) {
        var me = this;

        modalConfirm(msg, function (ret) {
            var dlWord = 'true';

            if (ret) {
                dlWord = 'false';
            }

            window.open(printUrl + '?dlWord=' + dlWord, '_blank');
        });
    },
    showitems: function () {
        var me = this;
        me.modal.removeClass('setqty');
    },
    showQty: function (data) {
        var me = this;

        me.body.find('.qty .itemid').val(data.id);
        me.body.find('.qty .itemname').html(data.name);
        me.body.find('.qty .itemprice').html(SITE.Utility.formatNumber(data.price, '#,##0.00') + ' / ' + data.unit);

        me.clear();
        if (data != null) me.fill(data);

        me.modal.addClass('setqty');
    }
};