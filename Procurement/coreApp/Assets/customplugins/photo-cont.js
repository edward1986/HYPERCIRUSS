(function ($) {

    var methods = {
        init: function () {

            return $(this).each(function () {
                var obj = $(this);

                var img = obj.find('img');
                var fileInput = obj.find('input[type="file"]');
                var removeCB = obj.find('.remove-photo');

                fileInput.change(function () {
                    if (this.files[0]) {
                        var fileReader = new FileReader();
                        fileReader.onload = function (e) {
                            img.attr('src', e.target.result);
                        }
                        fileReader.readAsDataURL(this.files[0]);

                        obj.addClass('has-file');
                    } else {
                        obj.removeClass('has-file');
                    }
                });

                removeCB.click(function () {
                    if ($(this).prop('checked')) {
                        obj.addClass('remove-photo');
                    } else {
                        obj.removeClass('remove-photo');
                    }
                });
            });
        }
    };

    $.fn.photoCont = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.photoCont');
        }

    };

})(jQuery);
