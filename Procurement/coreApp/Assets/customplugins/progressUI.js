(function ($) {
    var _options = {
        label: '',
        data: null,
        clientUrl: '',
        clientDataId: '',
        clientData: {
            id: null
        },
        doneUrl: ''
    };

    var methods = {
        init: function (options) {
            var opts = $.extend(true, {}, _options, options);
            var result = {
                successful: 0,
                failed: []
            };

            return this.each(function () {
                var obj = $(this);

                obj.find('.ui-title').html(opts.label);
                
                var progressBar = obj.find('.progressbar');
                var errorMessages = obj.find('.errors');

                var progressValue = 0, progressTotal = opts.data.length;

                var call_recalculate = function (id, callback) {

                    opts.clientData[opts.clientDataId] = id;

                    SITE.Utility.dataAccess({
                        url: opts.clientUrl,
                        data: opts.clientData,
                        success: function (res) {
                            if (!res.IsSuccessful) {
                                callback.call(this, res.Err);
                            }
                            else {
                                callback.call(this, '');
                            }
                        },
                        error: function (jqXHR, textStatus, errorThrown) {
                            callback.call(this, SITE.Utility.showError(textStatus, errorThrown));                            
                        }
                    });
                };

                var updateProgress = function (v) {
                    progressBar.css({ width: v + '%' });
                    progressBar.siblings('.progressbar-completed').html(SITE.Utility.formatNumber(v, '##0') + '%');
                }

                var showProgress = function () {
                    var v = (progressValue / progressTotal) * 100;
                    updateProgress(v);

                    return v;
                };

                var showErrorMessage = function (err) {
                    if (err != '') {
                        if (errorMessages.is(':empty')) {
                            errorMessages.html(err)
                        } else {
                            errorMessages.append('<br />' + err);
                        }
                    }
                };

                var start = function () {

                    for (var i = 0; i <= opts.data.length - 1; i++) {

                        call_recalculate(opts.data[i], function (err) {
                            progressValue += 1;

                            if (err != '') {
                                result.failed.push(err);
                            } else {
                                result.successful += 1;
                            }

                            showErrorMessage(err);
                            var total = showProgress();

                            if (total >= 100) {
                                window.location = opts.doneUrl + '&result=' + JSON.stringify(result);
                            }
                        });
                    };
                };

                showProgress();
                start();
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