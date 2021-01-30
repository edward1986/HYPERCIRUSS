/// <reference path="../common/common.js" />
(function ($) {
    var _options = {
    };

    var methods = {
        init: function (options) {
            var opts = $.extend(true, {}, _options, options);

            return this.each(function () {
                var obj = $(this);
                
                var url = obj.find('[data-url]').val();
                
                var pageSize = obj.find('.page-size');
                pageSize.change(function () {
                    var u = url + '?pageSize=' + $(this).val();

                    window.location = u;
                });

                var page = obj.find('.page');
                page.change(function () {
                    var u = url + '?pageNo=' + $(this).val() + '&pageSize=' + pageSize.val();

                    window.location = u;
                });
            });
        }
    };

    $.fn.listPager = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.listPager');
        }

    };

})(jQuery);

$(document).ready(function () {
    $('.list-pager').listPager();
});