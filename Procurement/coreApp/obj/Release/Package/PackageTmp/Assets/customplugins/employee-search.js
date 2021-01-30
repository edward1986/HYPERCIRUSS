(function ($) {
    var methods = {
        init: function () {

            return $(this).each(function () {
                var obj = $(this);

                var dataUrl = obj.attr('data-url');
                var excludeNoCareer = obj.attr('data-exclude-nocareer') == 'True';
                var excludeNoOffice = obj.attr('data-exclude-nooffice') == 'True';
                var enableSearch = obj.attr('data-enable-search') == 'True';

                var minimalView = obj.hasClass('minimal-view');
                var multiSelect = obj.hasClass('multi-select');
                var searchEmployeesSelectedItems = [];

                var searchBtn = obj.find('.btn-search');

                obj.keyup(function (e) {
                    if (e.which == 13) {
                        searchBtn.click();
                    }
                });

                searchBtn.click(function (e) {
                    e.preventDefault();

                    if (!enableSearch) return;

                    var search = $('.search');
                    var result = search.find('.result-section');
                    var selectedItems = search.find('.selectedItems');

                    result.addClass('loading-mask');

                    var param = {
                        lastName: search.find('.search-lastname').val(),
                        firstName: search.find('.search-firstname').val(),
                        mfoId: -1,
                        campusId: search.find('.search-campusid').val(),
                        officeId: search.find('.search-officeid').val().split(",")[1],
                        departmentId: search.find('.search-departmentid').val().split(",")[1],
                        positionId: search.find('.search-positionid').val(),
                        groupId: search.find('.search-groupid').val(),
                        employmentType: search.find('.search-employmenttype').val(),
                        active: search.find('.search-active').val(),
                        excludeNoCareer: excludeNoCareer,
                        excludeNoOffice: excludeNoOffice,
                        altSource: search.find('.altSource').val()
                    };

                    var mfo = search.find('.search-mfoid');
                    if (mfo.length > 0) {
                        param.mfoId = mfo.val();
                    }

                    var itemSelected = function (isSelect, table, e, dt, type, indexes) {
                        var rowsData = table.rows(indexes).data().toArray();

                        if (isSelect) {
                            rowsData.forEach(function (v) {
                                var i = $.inArray(v[0], searchEmployeesSelectedItems);
                                if (i == -1) {
                                    searchEmployeesSelectedItems.push(v[0]);
                                }
                            });
                        } else {
                            rowsData.forEach(function (v) {
                                var i = $.inArray(v[0], searchEmployeesSelectedItems);
                                while (i >= 0) {
                                    if (searchEmployeesSelectedItems.length == 1) {
                                        searchEmployeesSelectedItems = [];
                                    } else {
                                        searchEmployeesSelectedItems = searchEmployeesSelectedItems.splice(i, 1);
                                    }
                                    i = $.inArray(v[0], searchEmployeesSelectedItems);
                                }
                            });
                        }

                        selectedItems.val(searchEmployeesSelectedItems.join(','));
                    }

                    result.load('/User/EmployeeSearch/GetList', param, function () {
                        result.find('.table').attr('data-url', dataUrl);
                        result.find('.table').attr('data-multiselect', (multiSelect ? 'true' : 'false'));

                        if (multiSelect) {
                            setTables(result, true, function (table, e, dt, type, indexes) {
                                itemSelected(true, table, e, dt, type, indexes);
                            }, function (table, e, dt, type, indexes) {
                                itemSelected(false, table, e, dt, type, indexes);
                            });
                        } else {
                            setTables(result);
                        }

                        result.removeClass('loading-mask');
                    })
                });
            });
        }
    };

    $.fn.employeeSearch = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.employeeSearch');
        }

    };

})(jQuery);
