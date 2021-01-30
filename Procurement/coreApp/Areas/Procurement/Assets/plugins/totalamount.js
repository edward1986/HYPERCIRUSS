(function ($) {
    var _options = {
        doneCallback: null
    };


    var loader = '<div class="loader pull-right" title="loading..."><i class="icon-spinner"></i></div>';

    var methods = {
        init: function (options) {
            var opts = $.extend(true, {}, _options, options);
            
            return $(this).each(function () {
                var obj = $(this);
                
                obj.html(loader);

                var data = {
                    id: obj.closest('tr').attr('record-id'),
                    type: obj.attr('data-totalamount-type'),
                    inDBM: null
                }

                if (obj.attr('data-totalamount-indbm') != undefined) {
                    data.inDBM = obj.attr('data-totalamount-indbm') == 'true';
                }

                $.get('/Procurement/Utility/GetTotalAmount', data, function (res) {
                    var value = 0;

                    if (res.IsSuccessful) {
                        value = res.Data;
                        obj.html(SITE.Utility.formatNumber(value, '#,##0.00'));

                        opts.doneCallback(value);
                    } else {
                        opts.doneCallback(value, res.Err.split('\n'));
                    }                    
                }, 'json');

            });
        }
    };

    $.fn.totalAmount = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.totalAmount');
        }

    };

})(jQuery);