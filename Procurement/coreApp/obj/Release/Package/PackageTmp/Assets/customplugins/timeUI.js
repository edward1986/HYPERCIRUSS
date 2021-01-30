(function ($) {

    var methods = {
        init: function () {
            return $(this).each(function () {
                var obj = $(this);
                var fieldName = obj.attr('data-fieldname');
                var time = obj.find('[name="' + fieldName + '"]');

                var hour = obj.find('.hour');
                var minute = obj.find('.minute');
                var tt = obj.find('.tt');
                var prev = obj.find('.prev');
                var next = obj.find('.next');

                var t = new Date(time.val());
                var h = t.getHours();
                if (h < 12) {
                    hour.val(h == 0 ? 12 : h);
                    tt.val('am');
                } else {
                    hour.val(h == 12 ? 12 : h - 12);
                    tt.val('pm');
                }

                minute.val(t.getMinutes());

                var read = function () {
                    var d = new Date(time.val());
                    var yr = d.getFullYear();
                    var month = d.getMonth();
                    var dt = d.getDate();

                    var h = parseInt(hour.val());
                    if (tt.val() == 'am') {
                        if (h == 12) h = 0;
                    } else {
                        if (h != 12) h += 12;
                    }

                    var m = parseInt(minute.val());

                    d = new Date(yr, month, dt, h, m);

                    time.val($.formatDateTime('m/d/yy g:ii a', d));
                };

                obj.find('select').change(function () {
                    read();
                });

                //obj.find('[type="checkbox"]').click(function () {
                //    var me = $(this);
                //    me.val(me.prop('checked'));
                //});
            });
        }
    };

    $.fn.timeUI = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.timeUI');
        }

    };

})(jQuery);