(function ($) {

    var methods = {
        init: function () {

            return $(this).each(function () {
                var obj = $(this);
                var disp = obj.find('.customplugin-disp-ui');
                var sval = obj.find('.selected-value');
                var d = obj.find('.modal');

                disp.click(function (e) {
                    e.stopPropagation();

                    d.modal();
                });

                d.find('.department').click(function (e) {
                    e.stopPropagation();

                    sval.val($(this).attr('data-value'));
                    disp.text($(this).attr('data-text'));

                    d.modal('hide');
                });
            });
        }
    };

    $.fn.departmentSelect = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.departmentSelect');
        }

    };

})(jQuery);