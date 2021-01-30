(function ($) {

    var setTab = function (obj, key) {
        obj.find('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            sessionStorage.setItem(key, $(e.target).closest('li').index());
        })
    };

    var updateTab = function (obj, key) {
        if (sessionStorage[key]) {
            var li = obj.find('li').eq(parseInt(sessionStorage[key]));
            var a = li.find('a');
            a.tab('show');
        }
    };

    var setEmployeeSearch = function (obj, key) {
        obj.find('.btn-search').click(function () {
            var params = {
                lastName: obj.find('.search-lastname').val(),
                firstName: obj.find('.search-firstname').val(),
                mfoId: -1,
                officeId: obj.find('.search-officeid').val(),
                departmentId: obj.find('.search-departmentid').val(),
                positionId: obj.find('.search-positionid').val(),
                groupId: obj.find('.search-groupid').val(),
                employmentType: obj.find('.search-employmenttype').val(),
                active: obj.find('.search-active').val()
            };

            var mfo = obj.find('.search-mfoid');
            if (mfo.length > 0) {
                params.mfoId = mfo.val();
            }

            sessionStorage.setItem(key, JSON.stringify(params));
        })
    };

    var updateEmployeeSearch = function (obj, key) {
        if (sessionStorage[key]) {
            var params = JSON.parse(sessionStorage[key]);

            obj.find('.search-lastname').val(params.lastName);
            obj.find('.search-firstname').val(params.firstName);

            var mfo = obj.find('.search-mfoid');
            if (mfo.length > 0) {
                mfo.val(params.mfoId);
            }

            obj.find('.search-officeid').val(params.officeId);
            obj.find('.search-officeid').change();

            obj.find('.search-departmentid').val(params.departmentId);
            obj.find('.search-positionid').val(params.positionId);
            obj.find('.search-groupid').val(params.groupId);
            obj.find('.search-employmenttype').val(params.employmentType);
            obj.find('.search-active').val(params.active);

            obj.find('.btn-search').click();
        }
    };

    var methods = {
        init: function () {
            return this.each(function () {
                var obj = $(this);
                var stateKey = obj.attr('state-key');
                var objectType = obj.attr('state-manager-for');
                var key = null;

                switch (objectType) {
                    case 'tabs':
                        key = stateKey + '_activetab';
                        setTab(obj, key);
                        updateTab(obj, key);
                        break;
                    case 'employee-search':
                        key = stateKey + '_params';
                        setEmployeeSearch(obj, key);
                        updateEmployeeSearch(obj, key);
                        break;
                }
            });
        }
    };

    $.fn.stateManager = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.stateManager');
        }

    };

})(jQuery);

$(document).ready(function () {
    $('[state-key]').stateManager();
});
