(function ($) {
    var _options = {
        multiSelect: false,
        modalViewWidth: 'normal',
        modalEditWidth: 'nornal',
        modalCreateWidth: 'normal',
        modalCustomWidth: 'normal',
        modalCustomTitle: '',
        modalCustomButton: '',
        modalNoCancel: false,
        preLoadCallback: null,
        loadCallback: null,
        preSubmitCallback: null,
        postSubmitCallback: null,
        listLoadedCallback: null,
        listItemSelectedCallback: null,
        nodes: null
    };



    var loading = function (cont, show) {
        var content = cont.find('.modal-content');
        if (show) {
            content.addClass('loading-mask');
        } else {
            content.removeClass('loading-mask');
        }
    };

    var activateView = function (modal, viewType, recordId, modalNoView, modalViewUrl, modalEditUrl, modalDeleteUrl, modalCreateUrl, modalCustomUrl, modalOptions, tr) {

        modal.removeClass('show-view show-edit show-create');
        var url = '';
        var view = modal.find('.modal-body');

        modal.removeClass('modal-wide modal-mid-wide modal-most-wide show-view show-edit show-create show-custom');


        switch (viewType) {
            case 'view':
                if (modalOptions.modalViewWidth == 'wide') {
                    modal.addClass('modal-wide');
                } else if (modalOptions.modalViewWidth == 'most-wide') {
                    modal.addClass('modal-most-wide');
                } else if (modalOptions.modalViewWidth == 'mid-wide') {
                    modal.addClass('modal-mid-wide');
                }

                modal.addClass('show-view');
                modal.find('.modal-title').html('Record Details');
                url = modalViewUrl.replace('{0}', recordId);
                break;
            case 'edit':
                if (modalOptions.modalEditWidth == 'wide') {
                    modal.addClass('modal-wide');
                } else if (modalOptions.modalEditWidth == 'most-wide') {
                    modal.addClass('modal-most-wide');
                } else if (modalOptions.modalEditWidth == 'mid-wide') {
                    modal.addClass('modal-mid-wide');
                }

                modal.addClass('show-edit');
                modal.find('.modal-title').html('Edit Record');
                url = modalEditUrl.replace('{0}', recordId);
                break;
            case 'create':
                if (modalOptions.modalCreateWidth == 'wide') {
                    modal.addClass('modal-wide');
                } else if (modalOptions.modalCreateWidth == 'most-wide') {
                    modal.addClass('modal-most-wide');
                } else if (modalOptions.modalCreateWidth == 'mid-wide') {
                    modal.addClass('modal-mid-wide');
                }

                modal.addClass('show-create');
                modal.find('.modal-title').html('Create Record');
                url = modalCreateUrl;
                break;
            case 'custom':
                if (modalOptions.modalCustomWidth == 'wide') {
                    modal.addClass('modal-wide');
                } else if (modalOptions.modalCustomWidth == 'most-wide') {
                    modal.addClass('modal-most-wide');
                } else if (modalOptions.modalCustomWidth == 'mid-wide') {
                    modal.addClass('modal-mid-wide');
                }

                modal.addClass('show-custom');
                modal.find('.modal-title').html(modalOptions.modalCustomTitle);
                modal.find('.btn-custom').html(modalOptions.modalCustomButton);
                url = modalCustomUrl;
                break;
        }

        modal.find('.modal-footer a').each(function () {
            var a = $(this);
            var link = a.attr('data-link').replace('{0}', recordId)

            if (a.attr('data-link-custom') == undefined) {
                a.attr('href', link);
            } else {
                a.attr('data-link-tmp', link);
            }
        });

        modal.find('.modal-footer > *').removeAttr('style');

        if (modalOptions.modalNoCancel) {
            modal.find('.btn-cancel').remove();
        }

        var e = {
            modal: modal,
            view: view,
            viewType: viewType,
            recordId: recordId,
            modalNoView: modalNoView,
            modalViewUrl: modalViewUrl,
            modalEditUrl: modalEditUrl,
            modalDeleteUrl: modalDeleteUrl,
            modalCreateUrl: modalCreateUrl,
            modalCustomUrl: modalCustomUrl,
            modalOptions: modalOptions,
            url: url,
            tr: tr
        };

        if (modalOptions.preLoadCallback) {
            e = $.extend({}, e, window[modalOptions.preLoadCallback](e));
        }

        setWideModal(modal);

        loading(modal, true);

        var loaded = 0;

        view.load(e.url, function () {

            if (loaded > 0) return;
            loaded++;

            setDatePickers(e.view);
            setTimePickers(e.view);
            setDateTimePickers(e.view);
            setDateRangePickers(e.view);
            setDateCollection(e.view);
            setPeriod(e.view);
            setCheckboxes(e.view);
            setSelectAll(e.view);
            setOfficeDepartments(e.view);
            setEventSelection(e.view);
            setEffectivityCont(e.view);
            setTableSelectCheckbox(e.view);
            setScroll(e.view);
            setPhotoCont(e.view);
            setEmployeeSearch(e.view);
            setTinyMCE(e.view);
            setMaskedInput(e.view);
            setLeaveEntry(e.view);
            setInputLength(e.view);
            setSelect2(e.view);
            setChosen(e.view);
            setIntSpinner(e.view);

            e.modal.find('.btn-edit').unbind('click').bind('click', function () {
                activateView(e.modal, 'edit', e.recordId, e.modalNoView, e.modalViewUrl, e.modalEditUrl, e.modalDeleteUrl, e.modalCreateUrl, e.modalCustomUrl, e.modalOptions, e.tr);
            });
            e.modal.find('.btn-delete').unbind('click').bind('click', function () {
                modalConfirm('Are you sure you want to delete this record?', function (ret) {
                    if (ret) {
                        var proceed = true;
                        if (e.modalOptions.preSubmitCallback) {
                            proceed = window[e.modalOptions.preSubmitCallback]('delete', e);
                        }

                        if (proceed) {
                            var proc = modalProcessing();

                            $.post(modalDeleteUrl, { id: e.recordId }, function (res) {
                                if (e.modalOptions.postSubmitCallback) {
                                    proc.modal('hide');
                                    e.modal.find('.modal-content').addClass('loading-mask');
                                    proceed = window[e.modalOptions.postSubmitCallback](res, e, 'delete');
                                }

                                if (proceed) {
                                    if (res.IsSuccessful) {
                                        sessionStorage.clientMessage = res.Remarks;
                                        window.location.reload(true);
                                    } else {
                                        proc.modal('hide');
                                        modalMessage(res.Err.split('\n'), 'Delete Record', true, null, null, function (modal) {
                                            e.modal.find('.modal-content').removeClass('loading-mask');
                                        });
                                    }
                                }
                            }, 'json');
                        }
                    }
                });
            });

            e.modal.find('.btn-cancel').unbind('click').bind('click', function () {
                activateView(e.modal, 'view', e.recordId, e.modalNoView, e.modalViewUrl, e.modalEditUrl, e.modalDeleteUrl, e.modalCreateUrl, e.modalCustomUrl, e.modalOptions, e.tr);
            });
            e.modal.find('.btn-update').unbind('click').bind('click', function () {
                var proceed = true;
                if (e.modalOptions.preSubmitCallback) {
                    proceed = window[e.modalOptions.preSubmitCallback]('update', e);
                }

                if (proceed) {
                    var proc = modalProcessing();
                    var form = e.modal.find('.modal-body form');

                    form.ajaxForm(function (res) {
                        if (e.modalOptions.postSubmitCallback) {
                            proc.modal('hide');
                            e.modal.find('.modal-content').addClass('loading-mask');
                            proceed = window[e.modalOptions.postSubmitCallback](res, e, 'update');
                        }

                        if (proceed) {
                            if (res.IsSuccessful) {
                                sessionStorage.clientMessage = res.Remarks;
                                window.location.reload(true);
                            } else {
                                proc.modal('hide');
                                modalMessage(res.Err.split('\n'), 'Update Record', true, null, null, function (modal) {
                                    e.modal.find('.modal-content').removeClass('loading-mask');
                                });
                            }
                        }
                    });

                    form.submit();
                }
            });

            e.modal.find('.btn-create').unbind('click').bind('click', function () {
                var proceed = true;
                if (e.modalOptions.preSubmitCallback) {
                    proceed = window[e.modalOptions.preSubmitCallback]('create', e);
                }

                if (proceed) {
                    var proc = modalProcessing();
                    var form = e.modal.find('.modal-body form');

                    form.ajaxForm(function (res) {
                        if (e.modalOptions.postSubmitCallback) {
                            proc.modal('hide');
                            e.modal.find('.modal-content').addClass('loading-mask');
                            proceed = window[e.modalOptions.postSubmitCallback](res, e, 'create');
                        }

                        if (proceed) {
                            if (res.IsSuccessful) {
                                sessionStorage.clientMessage = res.Remarks;
                                window.location.reload(true);
                            } else {
                                proc.modal('hide');
                                modalMessage(res.Err.split('\n'), 'Create Record', true, null, null, function (modal) {
                                    e.modal.find('.modal-content').removeClass('loading-mask');
                                });
                            }
                        }
                    });

                    form.submit();
                }
            });

            e.modal.find('.btn-custom').unbind('click').bind('click', function () {
                var proceed = true;
                if (e.modalOptions.preSubmitCallback) {
                    proceed = window[e.modalOptions.preSubmitCallback]('custom', e);
                }

                if (proceed) {
                    var proc = modalProcessing();
                    var form = e.modal.find('.modal-body form');

                    form.ajaxForm(function (res) {
                        if (e.modalOptions.postSubmitCallback) {
                            proc.modal('hide');
                            e.modal.find('.modal-content').addClass('loading-mask');
                            proceed = window[e.modalOptions.postSubmitCallback](res, e, 'custom');
                        }

                        if (proceed) {
                            if (res.IsSuccessful) {
                                sessionStorage.clientMessage = res.Remarks;
                                window.location.reload(true);
                            } else {
                                proc.modal('hide');
                                modalMessage(res.Err.split('\n'), modalOptions.modalCustomTitle, true, null, null, function (modal) {
                                    e.modal.find('.modal-content').removeClass('loading-mask');
                                });
                            }
                        }
                    });

                    form.submit();
                }
            });

            var form = e.modal.find('.modal-body form');
            var textarea = form.find('textarea');

            textarea.unbind('keypress').bind('keypress', function (e) {
                if (e.keyCode == 13) {
                    e.stopPropagation();
                }
            });

            textarea.unbind('keyup').keyup(function (e) {
                if (e.which == 13) {
                    e.stopPropagation();
                }
            });

            form.unbind('keypress').bind('keypress', function (e) {
                if (e.keyCode == 13) {
                    return false;
                }
            });

            form.unbind('keyup').keyup(function (ee) {
                if (ee.which == 13) {
                    if (e.modal.hasClass('show-edit')) {
                        var btn = e.modal.find('.modal-footer .btn-update');
                        if (btn.is(':visible')) {
                            btn.click();
                        }
                    } else if (e.modal.hasClass('show-create')) {
                        var btn = e.modal.find('.modal-footer .btn-create');
                        if (btn.is(':visible')) {
                            btn.click();
                        }
                    }
                }
            });

            if (e.modalOptions.loadCallback) {
                window[e.modalOptions.loadCallback](e);
            }

            loading(e.modal, false);
        });

    };

    var setWideModal = function (modal) {
        modal.children('.modal-dialog').removeAttr('style');

        if (modal.hasClass('modal-wide')) {
            modal.children('.modal-dialog').css({ width: $(window).width() - 30, height: $(window).height(), 'padding-top': 15, 'padding-bottom': 15 });
            //modal.find('.scroll').height($(window).height() - 105).mCustomScrollbar("update");
        } else if (modal.hasClass('modal-most-wide')) {
            modal.children('.modal-dialog').css({ width: $(window).width() * .75, height: $(window).height(), 'padding-top': 15, 'padding-bottom': 15 });
        } else if (modal.hasClass('modal-mid-wide')) {
            modal.children('.modal-dialog').css({ width: $(window).width() / 2, height: $(window).height(), 'padding-top': 15, 'padding-bottom': 15 });
        }
    };

    var getOptions = function (tbl, opts) {

        if (tbl.attr('modal-preload-callback')) {
            opts.preLoadCallback = tbl.attr('modal-preload-callback');
        }
        if (tbl.attr('modal-load-callback')) {
            opts.loadCallback = tbl.attr('modal-load-callback');
        }
        if (tbl.attr('modal-presubmit-callback')) {
            opts.preSubmitCallback = tbl.attr('modal-presubmit-callback');
        }
        if (tbl.attr('modal-postsubmit-callback')) {
            opts.postSubmitCallback = tbl.attr('modal-postsubmit-callback');
        }
        if (tbl.attr('modal-listloaded-callback')) {
            opts.listLoadedCallback = tbl.attr('modal-listloaded-callback');
        }
        if (tbl.attr('modal-no-cancel')) {
            opts.modalNoCancel = tbl.attr('modal-no-cancel') == 'true';
        }
        if (tbl.attr('modal-view-width')) {
            opts.modalViewWidth = tbl.attr('modal-view-width');
        }
        if (tbl.attr('modal-edit-width')) {
            opts.modalEditWidth = tbl.attr('modal-edit-width');
        }
        if (tbl.attr('modal-create-width')) {
            opts.modalCreateWidth = tbl.attr('modal-create-width');
        }
        if (tbl.attr('modal-list-item-selected')) {
            opts.listItemSelectedCallback = tbl.attr('modal-list-item-selected');
        }

        return opts;
    };

    var methods = {
        init: function (options) {
            var opts = $.extend(true, {}, _options, options);

            return this.each(function () {
                var tbl = $(this);

                var modal;
                var useModal = tbl.attr('modal-target');
                var modalId = useModal ? tbl.attr('modal-target') : '';

                var modalNoView = tbl.attr('modal-noview') == 'true';

                var modalViewUrl = tbl.attr('modal-view-url');
                var modalEditUrl = tbl.attr('modal-edit-url');
                var modalDeleteUrl = tbl.attr('modal-delete-url');
                var modalCreateUrl = tbl.attr('modal-create-url');

                var modalStart = tbl.attr('modal-start');

                if (tbl.attr('data-multiselect')) {
                    opts.multiSelect = tbl.attr('data-multiselect') == 'true';
                }

                opts = getOptions(tbl, opts);

                var spl = tbl.find('tr .spl');
                spl.click(function (e) {
                    e.preventDefault();
                    e.stopPropagation();

                    window.location = $(this).attr('href');
                });



                var bindTr = function (trs) {
                    trs.unbind('click').bind('click', function (e) {
                        var tr = $(this);

                        if (tr.attr('locked') != undefined || opts.multiSelect) return;

                        e.preventDefault();

                        var recordId = tr.attr('record-id');

                        if (useModal) {
                            modal = $(modalId);

                            if (modalNoView) {
                                modal.addClass('modal-noview');
                            }

                            activateView(modal, modalStart, recordId, modalNoView, modalViewUrl, modalEditUrl, modalDeleteUrl, modalCreateUrl, '', opts, tr);


                            modal
                                .on('show.bs.modal', function () {
                                    setWideModal(modal);
                                })
                                .modal({
                                    backdrop: 'static',
                                    show: true
                                });

                        } else {
                            var detailsUrl = tbl.attr('data-url');
                            var url = detailsUrl.replace('{0}', recordId).replace('{1}', tr.attr('template-id'));

                            if (tr.attr('newtab') == undefined) {
                                window.location = url;
                            } else {
                                window.open(url, '_blank');
                            }
                        }
                    });
                };

                if (opts.nodes == null) {
                    bindTr(tbl.find('tbody tr'));
                } else {
                    var nodeCount = opts.nodes.length;
                    for (var i = 0; i < nodeCount; i++) {
                        var tr = $(opts.nodes[i]);

                        if (tr.attr('data-tableid') == undefined) {
                            bindTr(tr);
                        } else {
                            if (tbl.attr('id') == tr.attr('data-tableid')) {
                                bindTr(tr);
                            }
                        }
                    }
                }

                var row_commands = tbl.find('.row-commands');
                row_commands.find('.row-cmd').click(function (e) {
                    e.stopPropagation();
                });

                if (opts.listLoadedCallback) {
                    window[opts.listLoadedCallback](opts.nodes);
                }
            });
        },
        modalCreate: function (btn, url, _opts) {
            var opts = $.extend(true, {}, _options, _opts);

            var tbl = $(btn).closest('.block').find('.dataTables_wrapper').find('table');
            modalCreateUrl = tbl.attr('modal-create-url');
            if (url != null) {
                modalCreateUrl = url;
            }

            var modalId = tbl.attr('modal-target');

            opts = getOptions(tbl, opts);

            modal = $(modalId);

            activateView(modal, 'create', null, true, '', '', '', modalCreateUrl, '', opts);

            modal
                .on('show.bs.modal', function () {
                    setWideModal(modal);
                })
                .modal({
                    backdrop: 'static',
                    show: true
                });
        },
        modalCustom: function (btn, url, _opts) {
            var opts = $.extend(true, {}, _options, _opts);

            var tbl = $(btn).closest('.block').find('.dataTables_wrapper').find('table');

            var modalId = tbl.attr('modal-target');

            opts = getOptions(tbl, opts);

            modal = $(modalId);

            activateView(modal, 'custom', null, true, '', '', '', '', url, opts);

            modal
                .on('show.bs.modal', function () {
                    setWideModal(modal);
                })
                .modal({
                    backdrop: 'static',
                    show: true,
                    keyboard: false
                });
        }
    };

    $.fn.list = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.list');
        }

    };

})(jQuery);
