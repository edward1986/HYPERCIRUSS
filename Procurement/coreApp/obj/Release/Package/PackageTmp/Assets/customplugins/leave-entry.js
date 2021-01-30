(function ($) {

    var update = function (obj, clearHalfDays, updateCallback) {
        if (obj.length == 0) return;

        var periodCont = obj.find('.period-cont');
        var startDate = obj.find('[name="StartDate"]');
        var endDate = obj.find('[name="EndDate"]');
        var startDateHalfDay = obj.find('.startdate-halfday');
        var endDateHalfDay = obj.find('.enddate-halfday');

        startDate.val(periodCont.attr('data-start-date'));
        endDate.val(periodCont.attr('data-end-date'));

        var oneDay = startDate.val() == endDate.val();

        if (oneDay) {
            startDateHalfDay.find('label')[0].lastChild.nodeValue = ' Half-Day';
            endDateHalfDay.hide();
        } else {
            startDateHalfDay.find('label')[0].lastChild.nodeValue = ' Start Date is Half-Day';
            endDateHalfDay.show();
        }

        var cbstart = startDateHalfDay.find('[type="checkbox"]');
        var cbend = endDateHalfDay.find('[type="checkbox"]');

        if (clearHalfDays) {
            cbstart.prop('checked', false);
            cbend.prop('checked', false);
        }

        $.uniform.update(cbstart);
        $.uniform.update(cbend);

        obj.find('[type="checkbox"]').unbind('click').click(function () {
            if (updateCallback) updateCallback.call(this);
            $.uniform.update($(this));
        });
    };

    var defaultOptions = {
        src: null,
        updateCallback: null
    };

    var methods = {
        init: function (options) {
            var opts = $.extend({}, defaultOptions, options);

            return $(this).each(function () {
                var obj = $(this);

                update(obj, false, opts.updateCallback);
            });
        },
        update: function (options) {
            var opts = $.extend({}, defaultOptions, options);

            var obj = $(opts.src.element).closest('.leave-entry');
            update(obj, true, opts.updateCallback);
        }
    };

    $.fn.leaveEntry = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.leaveEntry');
        }

    };

})(jQuery);
