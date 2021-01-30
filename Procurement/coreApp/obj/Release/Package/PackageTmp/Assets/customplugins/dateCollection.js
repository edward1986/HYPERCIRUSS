(function ($) {
    var _options = {
        readonly: false,
        displayMaxCharacters: 100
    };

    function dataItem() {
        this.text = '';
        this.isPeriod = false;
        this.dt1 = null;
        this.dt2 = null;
    };

    dataItem.prototype.match = function (forPeriod, dt1, dt2) {
        var ret = false;

        if (forPeriod) {
            if (this.isPeriod) {
                ret = +this.dt1 == +dt1 && +this.dt2 == +dt2;
            }
        } else {
            if (!this.isPeriod) {
                ret = +this.dt1 == +dt1;
            }
        }
        
        return ret;
    };

    var methods = {
        init: function (options) {
            var opts = $.extend(true, {}, _options, options);

            var readOnlyUI = function (obj) {
                var origValue = obj.text().trim().replace(/,/g, ', ');
                var truncatedValue = '';
                var truncated = false;
                var max = opts.displayMaxCharacters;

                if (origValue.length <= max) {
                    truncated = false;
                    truncatedValue = origValue;
                } else {
                    truncated = true;
                    truncatedValue = origValue.substring(0, max);
                }

                var origText = $('<span class="orig-text" />');
                var truncText = $('<span class="trunc-text" />');
                var ellipsisBtn = $('<a href="#" class="ellipsis-btn">...</a>');

                var setEllipsisText = function () {
                    var text = obj.hasClass('truncate') ? '[more]' : '[less]'
                    ellipsisBtn.html(text);
                };

                origText.text(origValue);
                truncText.text(truncatedValue + ' ...');

                ellipsisBtn.click(function (e) {
                    e.preventDefault();
                    obj.toggleClass('truncate');

                    setEllipsisText();
                })

                if (truncated) {
                    obj.addClass('truncated truncate');
                }                

                obj.empty();
                obj.append(origText);
                obj.append(truncText);
                obj.append(ellipsisBtn);

                setEllipsisText();
            }

            var UI = function (obj) {
                var dataSource = $(obj.attr('data-source'));

                var data = [];

                var getDataFromSource = function () {
                    data = [];
                    dataSource.val().split(',').forEach(function (v) {
                        if (v != '') {

                            var obj = new dataItem();
                            obj.text = v;

                            if (v.indexOf('-') >= 0) {
                                var dates = v.split('-');

                                obj.isPeriod = true;
                                obj.dt1 = new Date(dates[0]);
                                obj.dt2 = new Date(dates[1]);
                            } else {
                                obj.isPeriod = false;
                                obj.dt1 = new Date(v);
                            }

                            data.push(obj);

                        }
                    });
                };

                var itemExists = function (forPeriod, dt1, dt2) {
                    return data.some(function (v) {
                        return v.match(forPeriod, dt1, dt2);
                    });
                };

                var addDate = function (dt) {

                    if (!itemExists(false, dt)) {

                        var obj = new dataItem();
                        obj.text = moment(dt).format('M/D/YYYY');
                        obj.isPeriod = false;
                        obj.dt1 = dt;

                        data.push(obj);

                        showData();
                    }
                };

                var addPeriod = function (dt1, dt2) {

                    if (!itemExists(true, dt1, dt2)) {

                        var obj = new dataItem();
                        obj.text = moment(dt1).format('M/D/YYYY') + '-' + moment(dt2).format('M/D/YYYY');;
                        obj.isPeriod = true;
                        obj.dt1 = dt1;
                        obj.dt2 = dt2;

                        data.push(obj);

                        showData();
                    }
                };

                var removeItem = function (index) {
                    data.splice(index, 1);
                    showData();
                };

                var createItem = function (i, v) {
                    var item = $('<div class="value" />');
                    var removeBtn = $('<a href="#" class="remove-item" title="remove item">x</a>');

                    removeBtn.click(function (e) {
                        e.preventDefault();
                        removeItem(i);
                    })

                    item.append(v.text);
                    item.append(removeBtn);

                    return item;
                }

                var saveData = function (d) {
                    var value = '';

                    data.forEach(function (v) {
                        value += (value == '' ? '' : ',') + v.text;
                    });

                    dataSource.val(value);
                };

                var showData = function () {
                    var divValues = obj.find('.values');

                    data.sort(function (a, b) {
                        return +b.dt1 - +a.dt1;
                    });

                    saveData(data);

                    divValues.empty();
                    $(data).each(function (i, v) {
                        var item = createItem(i, v);
                        divValues.append(item);
                    });
                };

                var setui = function () {
                    var btnAddDate = $('<a href="#" class="btn-adddate">Add Date</a>');
                    var btnAddPeriod = $('<a href="#" class="btn-addperiod">Add Period</a>');
                    var btnClear = $('<a href="#" class="btn-clear">Clear</a>');
                    var btnMaximize = $('<a href="#" class="btn-maximize">[+]</a>');
                    var divCommandCont = $('<div class="command-cont" />');
                    var divValues = $('<div class="values form-control" />');

                    divCommandCont
                        .append(btnAddDate)
                        .append(' | ')
                        .append(btnAddPeriod)
                        .append(' | ')
                        .append(btnClear)
                        .append(' | ')
                        .append(btnMaximize);

                    obj.empty()
                        .append(divCommandCont)
                        .append(divValues);
                };

                var wireObjects = function () {
                    var btnAddDate = obj.find('.btn-adddate');
                    var btnAddPeriod = obj.find('.btn-addperiod');
                    var btnClear = obj.find('.btn-clear');
                    var btnMaximize = obj.find('.btn-maximize');
                    var divValues = obj.find('.values');

                    btnAddDate.daterangepicker({
                        singleDatePicker: true,
                        showDropdowns: true,
                        minYear: 1901,
                        maxYear: parseInt(moment().format('YYYY'), 10)
                    })
                        .on('hide.daterangepicker', function (ev, picker) {
                            addDate(picker.startDate);
                        });

                    btnAddPeriod.daterangepicker({
                        ranges: ranges,
                        alwaysShowCalendars: true,
                        opens: 'left',
                        showDropdowns: true,
                        minYear: 1901,
                        maxYear: parseInt(moment().format('YYYY'), 10)
                    }, function (start, end) {
                        addPeriod(start, end);
                    });

                    btnClear.click(function (e) {
                        e.preventDefault();

                        modalConfirm('Are you sure you want to clear this field?', function (ret, modal) {
                            if (ret) {
                                data = [];
                                showData();
                            }
                        });

                    });

                    btnMaximize.click(function (e) {
                        e.preventDefault();

                        divValues.toggleClass('maximized');

                        var maximized = divValues.hasClass('maximized');
                        var text = maximized ? '[-]' : '[+]';

                        $(this).html(text);
                    });
                };

                getDataFromSource();
                setui();
                wireObjects();

                showData();
            }

            return $(this).each(function () {
                var obj = $(this);

                if (opts.readonly) {
                    readOnlyUI(obj);
                } else {
                    UI(obj);
                }
            });
        }
    };

    $.fn.dateCollection = function (method) {

        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.dateCollection');
        }

    };

})(jQuery);