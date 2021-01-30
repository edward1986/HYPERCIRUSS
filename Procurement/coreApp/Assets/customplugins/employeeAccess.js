
(function ($) {
    var _options = {
    };

    var methods = {
        init: function (options) {
            var opts = $.extend(true, {}, _options, options);

            return this.each(function () {
                var obj = $(this);

                var setLI = function (li, value) {
                    var span = li.children('.checker').children('span');
                    var inputCB = li.children('.checker').find('input[type="checkbox"]');
                    var inputHidden = li.children('input[type="hidden"]');

                    span.removeClass('checked');
                    if (value) span.addClass('checked');

                    inputCB.removeAttr('checked');
                    if (value) inputCB.attr('checked', 'checked');

                    inputCB.attr('value', value ? 'true' : 'false');
                    inputHidden.attr('value', value ? 'true' : 'false');
                };
                
                obj.find('li').click(function (e) {
                    e.stopPropagation();

                    var li = $(this);
                    var checked = $(this).children('.checker').children('span').hasClass('checked');
                    
                    if (checked) {
                        li.parents('li').each(function () {
                            setLI($(this), checked)
                        });
                    }

                    if (!checked) {
                        li.find('li').each(function () {
                            setLI($(this), checked);
                        });
                    }
                });
            });
        }
    };

    $.fn.employeeAccess = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.employeeAccess');
        }

    };

})(jQuery);

$(document).ready(function () {
    $('.employee-access').employeeAccess();
});