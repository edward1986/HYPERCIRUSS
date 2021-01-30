
(function ($) {
    var _options = {
        modalWidth: 'nornal',
        preLoadCallback: null,
        loadCallback: null,
        preSubmitCallback: null,
        postSubmitCallback: null
    };

    var activate = function (a, title, modalId, modalOptions) {

        var modal = $(modalId);
        modal.find('.modal-title').html(title);


        modal.removeClass('modal-wide modal-mid-wide modal-most-wide show-view show-edit show-create show-custom');

        if (modalOptions.modalWidth == 'wide') {
            modal.addClass('modal-wide');
        } else if (modalOptions.modalWidth == 'most-wide') {
            modal.addClass('modal-most-wide');
        } else if (modalOptions.modalWidth == 'mid-wide') {
            modal.addClass('modal-mid-wide');
        }
        
        var loading = function (show) {
            var content = modal.find('.modal-content');
            if (show) {
                content.addClass('loading-mask');
            } else {
                content.removeClass('loading-mask');
            }
        };

        var e = {
            modal: modal,
            modalOptions: modalOptions
        };

        if (modalOptions.preLoadCallback) {
            e = $.extend({}, e, window[modalOptions.preLoadCallback](e));
        }

        loading(true);

        var view = modal.find('.modal-body');
        var redirectUrl = a.attr('data-redirect-url');
        var url = a.attr('data-url');
        
        view.load(url, function () {
            //view.find('form').unbind('keypress').bind('keypress', function (e) {
            //    if (e.keyCode == 13) {
            //        return false;
            //    }
            //});
            setDatePickers(view);
            setDateRangePickers(view);
            setDateCollection(view);
            setPeriod(view);
            setCheckboxes(view);
            setSelectAll(view);
            setScroll(view);
            setPhotoCont(view);
            setTinyMCE(view);
            setMaskedInput(view);
            setLeaveEntry(view);
            setInputLength(view);
            setSelect2(view);
            setChosen(view);
            setIntSpinner(view);
            setIntSpinner(view)

            var form = view.find('form');
            var textarea = form.find('textarea');
            var submitBtn = modal.find('.btn-submit');
            
            submitBtn.unbind('click').bind('click', function () {
                var proceed = true;
                if (e.modalOptions.preSubmitCallback) {
                    proceed = window[e.modalOptions.preSubmitCallback]('update', e);
                }

                if (proceed) {

                    var form = e.modal.find('.modal-body form');

                    form.ajaxForm(function (res) {
                        if (e.modalOptions.postSubmitCallback) {
                            e.modal.find('.modal-content').addClass('loading-mask');
                            window[e.modalOptions.postSubmitCallback](res, e);
                        } else {
                            if (res.IsSuccessful) {
                                sessionStorage.clientMessage = res.Remarks;
                                if (redirectUrl == '{close-modal}') {
                                    loading(false);
                                    modal.modal('hide');
                                } else if (redirectUrl == '{page-reload}') {
                                    window.location.reload(true);
                                } else if (redirectUrl && redirectUrl != '') {
                                    window.location = redirectUrl.replace('{0}', res.Data);
                                } else {
                                    window.location.reload(true);
                                }
                            } else {
                                modalMessage(res.Err.split('\n'), 'Update Record', true);
                            }
                        }
                    });

                    form.submit();
                }
            });

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
                    submitBtn.click();
                }
            });


            if (e.modalOptions.loadCallback) {
                window[e.modalOptions.loadCallback](e);
            }

            loading(false);
        });

        modal
            .on('show.bs.modal', function () {
                setWideModal(modal);
            })
            .modal({
                backdrop: 'static',
                show: true
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

    var methods = {
        init: function (options) {

            return this.each(function () {
                var opts = $.extend({}, _options, options);

                var obj = $(this);

                var modalId = obj.attr('modal-target');
                var modalTitle = obj.attr('modal-title');

                var openBtn = obj.find('.btn-open');
                var createBtn = obj.find('.btn-create');
                var editBtn = obj.find('.btn-edit');
                var deleteBtn = obj.find('.btn-delete');

                if (obj.attr('modal-width')) {
                    opts.modalWidth = obj.attr('modal-width');
                }

                if (obj.attr('modal-preload-callback')) {
                    opts.preLoadCallback = obj.attr('modal-preload-callback');
                }

                if (obj.attr('modal-load-callback')) {
                    opts.loadCallback = obj.attr('modal-load-callback');
                }

                if (obj.attr('modal-presubmit-callback')) {
                    opts.preSubmitCallback = obj.attr('modal-presubmit-callback');
                }
                if (obj.attr('modal-postsubmit-callback')) {
                    opts.postSubmitCallback = obj.attr('modal-postsubmit-callback');
                }

                openBtn.unbind('click').bind('click', function (e) {
                    e.preventDefault();
                    var a = $(this);

                    activate(a, modalTitle, modalId, opts);
                });

                createBtn.unbind('click').bind('click', function (e) {
                    e.preventDefault();
                    var a = $(this);

                    activate(a, 'Create Record', modalId, opts);
                });

                editBtn.unbind('click').bind('click', function (e) {
                    e.preventDefault();
                    var a = $(this);

                    activate(a, 'Edit Record', modalId, opts);
                });

                deleteBtn.unbind('click').bind('click', function (e) {
                    e.preventDefault();
                    var a = $(this);
                    var url = a.attr('data-url');
                    var redirectUrl = a.attr('data-redirect-url');
                    var recordId = a.attr('data-record-id');

                    modalConfirm('Are you sure you want to delete this record?', function (ret) {
                        if (ret) {
                            $.post(url, { id: recordId }, function (res) {
                                if (res.IsSuccessful) {
                                    sessionStorage.clientMessage = res.Remarks;
                                    if (redirectUrl && redirectUrl != '') {
                                        window.location = redirectUrl;
                                    } else {
                                        window.location.reload(true);
                                    }
                                    
                                } else {
                                    modalMessage(res.Err.split('\n'), 'Save Record', true);
                                }
                            }, 'json');
                        }
                    });
                });
            });
        },
        modalCustom: function (btn, title, modalId, _opts) {
            var opts = $.extend(true, {}, _options, _opts);

            activate(btn, title, modalId, opts);
        }
    };

    $.fn.details = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.details');
        }

    };

})(jQuery);
