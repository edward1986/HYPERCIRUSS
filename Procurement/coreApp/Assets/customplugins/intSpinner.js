(function ($) {

    var methods = {
        init: function () {
            return $(this).each(function () {
                var obj = $(this);
                var tmp = 0;

                var span = $('<span class="ui-spinner" />');
                var up = $('<a class="ui-spinner-button ui-spinner-up ui-corner-tr" tabindex="-1"><span class="ui-icon ui-icon-triangle-1-n"></span></a>');
                var dn = $('<a class="ui-spinner-button ui-spinner-down ui-corner-br" tabindex="-1"><span class="ui-icon ui-icon-triangle-1-s"></span></a>');

                obj.wrap(span);
                obj.parent().append(up);
                obj.parent().append(dn);

                obj.attr('autocomplete', 'off');

                var allowBlank = true;
                var allowNeg = true;
                var increment = 1;

                if (obj.attr('data-allow-blank') != undefined) allowBlank = obj.attr('data-allow-blank') == 'true';
                if (obj.attr('data-allow-negative') != undefined) allowNeg = obj.attr('data-allow-negative') == 'true';
                if (obj.attr('data-increment') != undefined) increment = parseInt(obj.attr('data-increment'));
                
                var isValid = function (value) {
                    var ret = SITE.Utility.isNumber(value);
                    if (ret) {                        
                        if (!allowNeg) {                            
                            ret = value >= 0;
                        }
                    }
                    return ret;
                };

                var getValue = function () {
                    var ret = 0;
                    if (isValid(obj.val())) {
                        ret = obj.val();
                    }
                    return parseInt(ret);
                };

                up.click(function (e) {
                    e.preventDefault();
                    var newValue = getValue() + increment;
                    if (isValid(newValue)) {
                        obj.val(newValue);
                    }
                });

                dn.click(function (e) {
                    e.preventDefault();
                    var newValue = getValue() - increment;
                    if (isValid(newValue)) {
                        obj.val(newValue);
                    }
                });

                obj.focus(function () {
                    tmp = getValue();
                });

                obj.change(function () {
                    var value = obj.val();
                    if (allowBlank && value == '') {
                        
                    } else {
                        if (!isValid(value)) {
                            obj.val(tmp);
                        }
                    }
                });
            });
        }
    };

    $.fn.intSpinner = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.intSpinner');
        }

    };

})(jQuery);