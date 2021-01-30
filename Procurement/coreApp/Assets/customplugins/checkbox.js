(function ($) {

    var methods = {
        init: function () {
            return $(this).each(function () {
                var obj = $(this);

                obj.click(function () {

                    var value = obj.prop('checked');
                    obj.checkbox('set', value);

                })
            });
        },
        set: function (value) {
            return $(this).each(function () {
                var obj = $(this);
                var customValues = obj.attr('custom-values') != undefined;

                var _checked = value ? 'checked' : '';

                obj.prop('checked', value);

                if (!customValues) {
                    var _value = value ? 'true' : 'false';
                    
                    obj.attr('value', _value);
                    obj.siblings('input[type="hidden"]').val(_value);
                }
            });
        }
    };

    $.fn.checkbox = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.checkbox');
        }

    };

})(jQuery);