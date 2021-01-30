(function ($) {

    var methods = {
        init: function (levels) {

            return this.each(function () {
                var obj = $(this);

                obj.empty();

                var i = 1;
                levels.forEach(function (opt) {
                    var contents = $($('#progress-ui-template').prop('content')).find('.progress-ui-cont-outer').clone();
                    contents.addClass('progress_' + i);
                    contents.find('.progress-text').html(opt.text);

                    obj.append(contents);                   
                    i++;
                });
            });
        },
        update: function (value, level) {
            return this.each(function () {
                var obj = $(this);

                var pb = obj.find('.progress_' + level + ' .progress-ui-bar');
                var ptxtValue = obj.find('.progress_' + level + ' .progress-value');

                pb.css('width', value);
                ptxtValue.html(value);

            });
        }
    };

    $.fn.progressUI = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.progressUI');
        }

    };

})(jQuery);