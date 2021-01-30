(function ($) {

    var methods = {
        init: function () {
            return $(this).each(function () {
                var obj = $(this);

                var startDate = obj.find('[name="StartDate"]');
                var endDate = obj.find('[name="EndDate"]');

                var _initType = obj.find('[name="_initType"]').val();
                var _startDate = obj.find('[name="_StartDate"]').val();
                var _EndDate = obj.find('[name="_EndDate"]').val();
                var _pstartDate = obj.find('[name="_pStartDate"]').val();
                var _pEndDate = obj.find('[name="_pEndDate"]').val();

                var prevTrigger = obj.find('.prev-trigger');
                prevTrigger.click(function (e) {
                    e.preventDefault();

                    if (isCurrent()) {
                        setPrev();
                    } else {
                        setCurrent();
                    }

                    prevTrigger.toggleClass('current');
                    setLabel();
                });

                var isCurrent = function () {
                    return prevTrigger.hasClass('current');
                };

                var setPrev = function () {
                    startDate.val(_pstartDate);
                    endDate.val(_pEndDate);
                };

                var setCurrent = function () {
                    startDate.val(_startDate);
                    endDate.val(_EndDate);
                };

                var setLabel = function () {
                    if (isCurrent()) {
                        prevTrigger.html('Previous ' + _initType);
                    } else {
                        prevTrigger.html('Current ' + _initType);
                    }
                };

                setLabel();
            });
        }
    };

    $.fn.period = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.period');
        }

    };

})(jQuery);