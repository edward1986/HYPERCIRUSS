var loadingIcon = '<div class="loader" title="loading..."><i class="icon-spinner"></i></div>';
var chosenObj
String.prototype.replaceAll = function (search, replacement) {
    var target = this;
    return target.replace(new RegExp(search, 'g'), replacement);
};

function initPlugins(cont) {
    setDatePickers();
    setTimePickers();
    setDateTimePickers();
    setDateRangePickers();
    setDateCollection();
    setPeriod();
    setCheckboxes();
    setSelectAll();
    setTables();
    setDetails();
    setOfficeDepartments();
    setEventSelection();
    setScroll();
    setTableSelectCheckbox();
    setPhotoCont();
    setEmployeeSearch();
    setTinyMCE();
    setMaskedInput();
    setLeaveEntry();
    setStateKeys();
    setInputLength();
    setSelect2();
    setChosen();
    setIntSpinner();
    
}

$(document).ready(function () {
    initPlugins();
});

function setIntSpinner(cont) {
    var obj;

    if (cont) {
        obj = cont.find('.int-spinner');
    } else {
        obj = $('.int-spinner');
    }

    if (obj.length > 0) {
        obj.intSpinner();
    }
}

function setStateKeys(cont) {
    var obj;

    if (cont) {
        obj = cont.find('[state-key]');
    } else {
        obj = $('[state-key]');
    }

    if (obj.length > 0) {
        obj.stateManager();
    }
}

function setLeaveEntry(cont, options) {
    var obj;

    if (cont) {
        obj = cont.find('.leave-entry');
    } else {
        obj = $('.leave-entry');
    }

    if (obj.length > 0) {
        obj.leaveEntry(options);
    }
}

function setMaskedInput(cont) {
    var obj;

    if (cont) {
        obj = cont.find('.masked-input');
    } else {
        obj = $('.masked-input');
    }

    if (obj.length > 0) {
        var maskTemplate = obj.attr('data-mask');
        obj.mask(maskTemplate);
    }
}

function setSelect2(cont) {
    var obj;

    if (cont) {
        obj = cont.find('.select2');
    } else {
        obj = $('.select2');
    }

    if (obj.length > 0) {
        obj.select2();
    }
}

function setChosen(cont) {

    if (cont) {
        chosenObj = cont.find('.chosen-select');
    } else {
        chosenObj = $('.chosen-select');
    }

    if (chosenObj.length > 0) {

        chosenObj.chosen({
            no_results_text: "No result found. Press TAB to add "
        })

    }
}


function setTinyMCE(cont) {
    var obj;

    if (cont) {
        obj = cont.find('textarea.tmce');
    } else {
        obj = $('textarea.tmce');
    }

    var opts = {
        height: 200
    };

    if (obj.attr('data-height')) {
        opts.height = parseFloat(obj.attr('data-height'));
    }
    
    if (obj.length > 0) {
        tinymce.init({
            selector: "textarea.tmce",
            toolbar: "bold italic | alignleft aligncenter alignright alignjustify | undo redo",
            menu: [],
            height: opts.height
        });
    }
}

function setEmployeeSearch(cont) {
    var obj;

    if (cont) {
        obj = cont.find('.search');
    } else {
        obj = $('.search');
    }

    obj.employeeSearch();
}

function setTableSelectCheckbox(cont) {
    var obj;

    if (cont) {
        obj = cont.find('.table-select-all');
    } else {
        obj = $('.table-select-checkbox');
    }

    obj.click(function () {
        var me = $(this);
        var v = me.prop('checked');
        var cbs = me.closest('table').find('tbody .table-select-item');
        cbs.prop('checked', v);
        if (v) {
            cbs.parent().addClass('checked');
        } else {
            cbs.parent().removeClass('checked');
        }
    });
}

function setPhotoCont(cont) {
    var obj;

    if (cont) {
        obj = cont.find('.photo-cont');
    } else {
        obj = $('.photo-cont');
    }

    obj.photoCont();
}

function setDatePickers(cont) {
    var obj;

    if (cont) {
        obj = cont.find('.datepicker');
    } else {
        obj = $('.datepicker');
    }

    obj.datepicker("destroy");
    obj.removeClass("hasDatepicker calendarclass");
    obj.unbind();

    obj.datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "-80:+20"
    });
}

function setTimePickers(cont) {
    var obj;

    if (cont) {
        obj = cont.find('.timepicker');
    } else {
        obj = $('.timepicker');
    }

    obj.timepicker({
        timeFormat: "h:mm TT"
    });
}

function setDateTimePickers(cont) {
    var obj;

    if (cont) {
        obj = cont.find('.datetimepicker');
    } else {
        obj = $('.datetimepicker');
    }

    obj.datetimepicker({
        timeFormat: "h:mm TT"
    });
}

function setDateCollection(cont) {
    var obj;
    var objDisp;

    if (cont) {
        obj = cont.find('.date-collection');
        objDisp = cont.find('.date-collection-display');
    } else {
        obj = $('.date-collection');
        objDisp = $('.date-collection-display');
    }

    obj.dateCollection();
    objDisp.dateCollection({
        readonly: true
    });
}

var ranges = {
    'Today': [moment(), moment()],
    'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
    'This Week': [moment().subtract(moment().day(), 'days'), moment().add(6 - moment().day(), 'days')],
    'Last 3 Months': [moment().subtract(2, 'months'), moment()],
    'Last 6 Months': [moment().subtract(5, 'months'), moment()],
    'This Month': [moment().startOf('month'), moment().endOf('month')],
    'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
    'This Year': [moment('1/1/' + moment().year()), moment('12/31/' + moment().year())],
    'Last Year': [moment('1/1/' + (moment().year() - 1)), moment('12/31/' + (moment().year() - 1))],
};

function setDateRangePickers(cont) {
    var obj;

    if (cont) {
        obj = cont.find('.daterangepicker');
    } else {
        obj = $('.daterangepicker');
    }

    obj.daterangepicker({
        ranges: ranges
    });
}

function setPeriod(cont) {
    var obj;

    if (cont) {
        obj = cont.find('.period-cont');
    } else {
        obj = $('.period-cont');
    }

    var callback = obj.attr('event-change');

    var start = moment(obj.attr('data-start-date'));
    var end = moment(obj.attr('data-end-date'));

    var minDate = obj.attr('data-min-date');
    var maxDate = obj.attr('data-max-date');

    var cb = function (start, end, isInit) {
        obj.attr('data-start-date', start.format('MM/DD/YYYY'));
        obj.attr('data-end-date', end.format('MM/DD/YYYY'));

        obj.find('span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));

        if (isInit != true) {
            window[callback](start.format('MM-DD-YYYY'), end.format('MM-DD-YYYY'), this);
        }
    };

    var opts = {
        showDropdowns: true,
        alwaysShowCalendars: true,
        startDate: start,
        endDate: end,
        ranges: ranges
    };

    if (minDate) opts.minDate = minDate;
    if (maxDate) opts.maxDate = maxDate;

    obj.daterangepicker(opts, cb);

    cb(start, end, true);
}

function setCheckboxes(cont) {
    var obj;

    if (cont) {
        obj = cont.find('[type="checkbox"]');
    } else {
        obj = $('[type="checkbox"]');
    }

    obj.uniform();
}

function setSelectAll(cont) {
    var obj;

    if (cont) {
        obj = cont.find('.select-all');
    } else {
        obj = $('.select-all');
    }

    obj.selectAll();
}

function setTables(cont, multiSelect, multiSelect_SelectEvent, multiSelect_DeselectEvent) {
    var obj;
    
    if (cont) {
        obj = cont.find('.table').not('.static-table').not('.skiptable');
    } else {
        obj = $('.table').not('.static-table').not('.skiptable');
    }

    var useEachInsteadOfEvery = obj.hasClass('use-each');
    var tableBindCallback = obj.attr('event-tablebind');
    var doneCallback = obj.attr('event-settables-done');

    var opts = {
        "select": multiSelect == true,
        "bStateSave": true,
        "fnStateSave": function (oSettings, oData) {
            sessionStorage.setItem('DataTables', JSON.stringify(oData));
        },
        "fnStateLoad": function (oSettings) {
            return JSON.parse(sessionStorage.getItem('DataTables'));
        },
        "lengthMenu": [[10, 25, 50, 100, -1], [10, 25, 50, 100, "All"]]
    };

    obj.find('tfoot tr:last-child').find('th').not('.nofilter').each(function () {
        var title = obj.find('thead th').eq($(this).index()).text().trim();
        $(this).html('<input type="text" class="col-filter" placeholder="Search ' + title + '" />');
    });

    obj.DataTable().destroy();

    $.fn.dataTable.moment('MM/DD/YYYY');
    $.fn.dataTable.moment('MM/DD/YYYY h:mm a');

    var table = obj.DataTable(opts);

    var bind = function () {
        obj.list({ nodes: table.rows().nodes() });

        if (tableBindCallback) {
            window[tableBindCallback](obj, table);
        }
    };

    //table.on('draw.dt', function () {
    //    bind();
    //});


    if (useEachInsteadOfEvery) {
        table.columns().each(function (v, i) {
            var column = this;

            var input = $(column.header()).closest('table').find('tfoot tr th').eq(i).find('input');
            input.on('keyup change', function () {
                column
                    .search(this.value)
                    .draw();
            });
        });
    } else {
        table.columns().every(function () {
            var column = this;

            var input = $(column.header()).closest('table').find('tfoot tr th').eq(column.index()).find('input');
            input.on('keyup change', function () {
                column
                    .search(this.value)
                    .draw();
            });
        });
    }
    
    
    if (multiSelect) {
        table
            .on('select', function (e, dt, type, indexes) {
                if (multiSelect_SelectEvent) {
                    multiSelect_SelectEvent(table, e, dt, type, indexes);
                }
            })
            .on('deselect', function (e, dt, type, indexes) {
                if (multiSelect_DeselectEvent) {
                    multiSelect_DeselectEvent(table, e, dt, type, indexes);
                }
            });
    }

    bind();

    if (doneCallback) {
        window[doneCallback](table);
    }
}

function setDetails(cont) {
    var obj;

    if (cont) {
        obj = cont.find('.details');
    } else {
        obj = $('.details');
    }

    obj.details();
}

function setOfficeDepartments(cont) {
    var obj;

    if (cont) {
        obj = cont.find('select[data-department]');
    } else {
        obj = $('select[data-department]');
    }

    var dept = $(obj.attr('data-department'));
    var deptValue = obj.attr('data-department-value');

    obj.unbind('change').bind('change', function () {
        var dd = $(this);
        dept.find('option').removeAttr('selected');
        dept.find('option').hide();
        dept.find('option[data-officeid="' + dd.val() + '"], option[data-officeid="-1"]').show();
        dept.val(-1);
    });

    obj.change();
    dept.val(deptValue);

}

function setEventSelection(cont) {
    var obj;

    if (cont) {
        obj = cont.find('.events-selection');
    } else {
        obj = $('.events-selection');
    }

    var events = $('#Events')

    var getValue = function () {
        var res = '';

        obj.find('.checkbox input').each(function (i, v) {
            var cb = $(v);
            if (cb.prop('checked')) {
                res += (res == '' ? '' : ',') + i.toString();
            }
        });

        events.val(res);
    };

    obj.find('.checkbox input').each(function (i, v) {
        var cb = $(v);
        if ($.inArray(i.toString(), events.val().split(',')) >= 0) {
            cb.prop('checked', true);
            cb.parent().addClass('checked');
        } else {
            cb.prop('checked', false);
            cb.parent().removeClass('checked');
        }

        cb.click(function () {
            getValue();
        });
    });
}

function setScroll(cont) {
    //var obj;

    //if (cont) {
    //    obj = cont.find('.scroll');
    //} else {
    //    obj = $('.scroll');
    //}

    //if (obj.length > 0) {
    //    if (obj.attr('data-scroll-reltowinheight') != undefined) {
    //        obj.height($(window).height() * 0.7);
    //    }
        
    //    obj.mCustomScrollbar({
    //        advanced: { autoScrollOnFocus: false },
    //        scrollInertia: 0
    //    });
    //}
}

function setEffectivityCont(cont) {
    var obj;

    if (cont) {
        obj = cont.find('.effectivity-field');
    } else {
        obj = $('.effectivity-field');
    }
        
    var select = obj.find('select');
    var hf = obj.find('.effectivity-field-hf');

    var lastValue = select.val();

    var initNewOption = function () {
        select.find('option[value="NEW"]').html('Set new date...');
        hf.val(lastValue);
    };

    
    select.change(function () {
        if (select.val() == 'NEW') { 

            modalEmpty('Set date', function (modal) {

                var tb = $('<div />');
                tb.addClass('datepicker');
                
                var body = modal.find('.modal-body');
                body.empty().append(tb);

                tb.datepicker({
                    changeMonth: true,
                    changeYear: true,
                    dateFormat: 'mm-dd-yy',
                    yearRange: "-40:+20"
                });

                var btnOk = modal.find('.modal-footer .btn-ok');
                btnOk.click(function (e) {
                    var ret = tb.val();
                    select.find('option[value="NEW"]').html(ret);
                    hf.val(ret);
                });
            })

        } else {
            lastValue = select.val();
            initNewOption();
        }
    });

    initNewOption();
}

function setInputLength(cont) {
    var obj;

    if (cont) {
        obj = cont.find('input[type="text"]').not('[maxlength]');
    } else {
        obj = $('input[type="text"]').not('[maxlength]');
    }

    obj.attr('maxlength', 50);
}