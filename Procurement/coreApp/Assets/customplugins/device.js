(function ($) {

    var methods = {
        init: function () {

            return $(this).each(function () {
                var obj = $(this);
                

            });
        },
        status: function () {

            return $(this).each(function () {
                var obj = $(this);
                var deviceId = obj.attr('data-device-id');

                var getStatus = function (callback) {

                    var loading = $.extend({}, SITE.Utility.ui.loading, {
                        obj: obj
                    });

                    loading.show();
                    SITE.Utility.dataAccess({
                        url: '/HR/Devices/getDeviceStatus',
                        data: {
                            deviceId: deviceId
                        },
                        success: function (res) {
                            if (res.IsSuccessful) {
                                callback.call(this, JSON.parse(res.Data), res.Err);
                            } else {
                                callback.call(this, false, res.Err);
                            }
                            loading.hide();
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            loading.hide();
                            callback.call(this, false, SITE.Utility.showError(textStatus, errorThrown));
                        }
                    });
                };
                                
                getStatus(function (data, err) {
                    if (data) {
                        obj.html('Connected');
                    } else {
                        obj.html('<span class="red-text">' + err + '</span>');
                    }
                });
            });
        }
    };

    $.fn.device = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.device');
        }

    };

})(jQuery);