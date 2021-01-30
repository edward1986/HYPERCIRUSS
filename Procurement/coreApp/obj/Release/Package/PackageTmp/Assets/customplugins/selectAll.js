(function ($) {

    var methods = {
        init: function () {
            return $(this).each(function () {
                var obj = $(this);
                var opts = {
                    parentInverse: obj.hasClass('select-all-parent-inverse')
                };

                var cbs = obj.find('[type="checkbox"]');

                var updateChildren = function (cb) {
                    var value = cb.prop('checked');
                    var children = cb.closest('li').find('li [type="checkbox"]');

                    children.prop('checked', value);
                    $.uniform.update(children);
                };

                var updateParents = function (cb) {
                    var siblings = cb.closest('ul').children('li').children('.checkbox');
                    var hasSelection = siblings.find('[type="checkbox"]:checked').length > 0;
                    var hasUnselected = siblings.find('[type="checkbox"]:checked').length < siblings.find('[type="checkbox"]').length;

                    cb.closest('li').parents('li').each(function (i, li) {
                        var cb = $(li).children('.checkbox').find('[type="checkbox"]');

                        if (cb.length > 0) {
                            if (opts.parentInverse) {
                                cb.prop('checked', !hasUnselected);
                            } else {
                                cb.prop('checked', hasSelection);
                            }
                            
                            $.uniform.update(cb);

                            updateParents(cb);
                        }                        
                    });
                };

                cbs.click(function () {
                    var cb = $(this);                    
                    
                    updateChildren(cb);
                    updateParents(cb);
                })
            });
        }
    };

    $.fn.selectAll = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.selectAll');
        }

    };

})(jQuery);