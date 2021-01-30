(function ($) {

    var methods = {
        init: function () {

            return $(this).each(function () {
                var obj = $(this);
                var disp = obj.find('.customplugin-disp-ui');
                var sval = obj.find('.selected-value');
                var d = obj.find('.modal');

                var parentId = obj.attr('data-parent-id');
                var parentIdName = obj.attr('data-parent-id-name');
                var importUrl = obj.attr('data-import-url');

                var trigger = obj.attr('data-trigger');
                if (trigger == null) {

                    disp.click(function (e) {
                        e.stopPropagation();

                        d.modal();
                    });

                } else {
                    $(trigger).click(function (e) {
                        e.stopPropagation();

                        d.modal();
                    });
                }

                d.find('.select-all').click(function (e) {
                    e.stopPropagation();

                    d.find('.select-item').prop('checked', $(this).prop('checked'));
                });

                d.find('.cmd-cancel').click(function (e) {
                    e.stopPropagation();

                    d.modal('hide');
                });

                d.find('.cmd-select').click(function (e) {
                    e.stopPropagation();

                    if (d.find('.select-item:checked').length == 0) {
                        alert('No item selected');
                        return;
                    }

                    var sel = [];
                    d.find('.select-item:checked').each(function (i, v) {
                        sel.push($(v).attr('data-id'));
                    });

                    var data = {
                        parentId: parentId,
                        selection: sel.join(','),
                        startDate: obj.attr('data-startdate'),
                        endDate: obj.attr('data-enddate')
                    };
                    data[parentIdName] = parentId;


                    $.get(importUrl, data, function (res) {
                        if (res.IsSuccessful) {
                            window.location = '/Utilities/setMessage?url=' + window.location.href + '&message=' + res.Remarks + '&isError=false';
                        } else {
                            $('.global-error').html(res.Err);
                        }
                    }).done(function () {
                        d.modal('hide');
                    });
                    
                });
            });
        }
    };

    $.fn.batchSelect = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.batchSelect');
        }

    };

})(jQuery);