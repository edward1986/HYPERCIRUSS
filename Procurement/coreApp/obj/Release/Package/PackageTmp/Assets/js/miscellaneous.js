var SITE = {
    //siteUrl: url
    //languages: arrLanguages
};

//var getUrlParameter = function (selector, pname) {
//    var ret = '';
//    var src = $(selector).attr('src');
//    var qs = src.split('?');
//    var params = qs[1].split('&');
//    $.each(params, function (i, v) {
//        var item = v.split('=');
//        if (item[0] == pname) {
//            ret = item[1];
//        }
//    });
//    return ret;
//}

//SITE.siteUrl = decodeURIComponent(getUrlParameter('#script-framework', 'siteUrl'));

SITE.Utility = {
    isMobile: function () {
        var n = navigator.userAgent.toLowerCase();

        return n.indexOf('android') >= 0 ||
               n.indexOf('webos') >= 0 ||
               n.indexOf('iphone') >= 0 ||
               n.indexOf('ipad') >= 0 ||
               n.indexOf('ipod') >= 0 ||
               n.indexOf('blackberry') >= 0;
        //return (/android|webos|iphone|ipad|ipod|blackberry/i.test(navigator.userAgent.toLowerCase()));
    },

    isNumber: function (val) {
        return !isNaN(parseFloat(val)) && isFinite(val);
    },

    isBoolean: function (val) {
        return val.toLowerCase().trim() == 'true' || val.toLowerCase().trim() == 'false';
    },

    isDate: function (val) {
        var d = new Date(val);
        return !isNaN(d.valueOf());
    },

    isEmail: function (email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    },

    formatNumber: function (val, format) {
        return numeral(val).format(format);
    },

    formatDate: function (val, format) {
        if (val.indexOf('/Date') == 0) val = SITE.Utility.convertJSONDate(val);
        return $.format.date(val, format);
    },

    dateNow: function (format) {
        if (format == null) return this.formatDate(new Date(), 'MM/dd/yyyy');
        else return this.formatDate(new Date(), format);
    },

    guid: function () {
        function _p8(s) {
            var p = (Math.random().toString(16) + "000000000").substr(2, 8);
            return s ? "-" + p.substr(0, 4) + "-" + p.substr(4, 4) : p;
        }
        return _p8() + _p8(true) + _p8(true) + _p8();
    },

    markElements: function (obj, level) {
        var me = this;
        if (!obj.hasClass(level)) obj.addClass(level);
        obj.children().each(function (i, v) {
            if (!$(v).hasClass(level)) $(v).addClass(level);
            me.markElements($(v), level);
        });
    },

    normalizeDate: function (d) {
        var v = new Date(d);
        v.setHours(v.getHours() - v.getTimezoneOffset() / 60);
        return v;
    },

    convertJSONDate: function (val) {
        return new Date(parseInt(val.substr(6)));
    },

    convertDBDate: function (val) {
        var d = this.dateFromISO(val);
        //d.setHours(d.getHours() - d.getTimezoneOffset() / 60);
        return d;
    },

    convertTDate: function (val) {
        var v = val.split('T');
        var d = v[0].split('-'), t = v[1].split(':');
        return new Date(d[0], parseInt(d[1]) - 1, d[2], t[0], t[1], t[2].split('.')[0]);
    },

    dateFromISO: function (s) {
        var me = this;
        var D, M = [], hm, min = 0, d2,
        Rx = /([\d:]+)(\.\d+)?(Z|(([+\-])(\d\d):(\d\d))?)?$/;
        D = s.substring(0, 10).split('-');
        if (s.length > 11) {
            M = s.substring(11).match(Rx) || [];
            if (M[1]) D = D.concat(M[1].split(':'));
            if (M[2]) D.push(Math.round(M[2] * 1000));// msec
        }
        for (var i = 0, L = D.length; i < L; i++) {
            D[i] = parseInt(D[i], 10);
        }
        D[1] -= 1;
        while (D.length < 6) D.push(0);
        if (M[4]) {
            min = parseInt(M[6]) * 60 + parseInt(M[7], 10);// timezone not UTC
            if (M[5] == '+') min *= -1;
        }
        try {
            d2 = me.dateFromUTCArray(D);
            if (min) d2.setMinutes(d2.getMinutes() + min);
        }
        catch (er) {
            // bad input
        }
        return d2;
    },
    dateFromUTCArray: function (A) {
        var D = new Date;
        while (A.length < 7) A.push(0);
        var T = A.splice(3, A.length);
        D.setFullYear.apply(D, A);
        D.setHours.apply(D, T);
        return D;
    },

    getParameterByName: function (name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
            results = regex.exec(location.search);
        return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    },

    toTitleCase: function (str) {
        str = str.toLowerCase();
        return str.replace(/\s.|^./g, function (txt) {
            return txt.toUpperCase();
        });
    },

    ulLevel: function (ul, level) {
        var me = this;
        var lvl = 'level-' + level;
        ul.addClass(lvl);
        ul.children('li').each(function (i, v) {
            var li = $(v);
            li.addClass(lvl);
            li.children('ul').each(function (ii, vv) {
                var _ul = $(vv);
                me.ulLevel(_ul, level + 1);
            });
        });
    },

    getRecFromData: function (data, fieldName, fieldValue) {
        var ret = null;
        $(data).each(function (i, v) {
            if (v[fieldName] == fieldValue) ret = v;
        });
        return ret;
    },

    ui: {
        ajaxForm: {
            onSubmit: null,
            onSuccess: null,
            onError: null,
            init: function (form) {
                var me = this;
                var options = $.extend({}, SITE.jqForm.options, {
                    beforeSubmit: function () {
                        if (me.onSubmit != null) me.onSubmit.call();
                    },
                    success: function (responseText, statusText, xhr, $form) {
                        if (statusText == 'success') {
                            if (me.onSuccess != null) me.onSuccess.call(this, responseText);
                        } else {
                            if (me.onError != null) me.onError.call(this, responseText);
                        }
                    }
                });

                form.ajaxForm(options);

                form.find('.remove-image').unbind('click').bind('click', function () {
                    if (this.checked) $(this).siblings('.remove-image-val').val('true');
                    else $(this).siblings('.remove-image-val').val('false');
                });
            },

            clearImageInput: function (form) {
                form.find('.imagefile').val('');
                form.find('.remove-image').removeAttr('checked');
                form.find('.remove-image-val').val('false');
            }
        },

        dropDown: {
            set: function (select_obj, data, selectedValue, includeBlankItem, setFields, strlength, blankItemValue) {

                select_obj.empty();
                if (includeBlankItem) select_obj.append($('<option />').attr('value', blankItemValue == null ? '-1' : blankItemValue));

                $.each(data, function (i, v) {
                    var option = $('<option />');
                    var d = setFields.call(this, i, v);

                    var txt = strlength == null ? d.Description : SITE.Utility.trimString(d.Description, strlength);
                    option.attr('value', d.Value).html(txt);
                    if (strlength != null && txt.substr(-3) == '...') option.attr('title', d.Description);

                    select_obj.append(option);
                });

                select_obj.val(selectedValue);
            },

            makeRoles: function (roleId, strlength) {
                var sel = $('<select />').attr('fieldName', 'RoleId').attr('name', 'roleid');

                SITE.Utility.ui.dropDown.set(sel, roles, roleId, true, function (i, v) {
                    return {
                        Value: v.RoleId,
                        Description: v.RoleName
                    }
                }, strlength);
                return sel;
            }
        },

        loading: {
            obj: null,
            label: 'loading...',
            delay: null,
            pos: null,
            show: function () {
                if (!this.obj.isMasked()) {
                    this.obj.mask(this.label != null ? this.label : 'loading...', this.delay, this.pos);
                }
            },
            hide: function () {
                this.obj.unmask();
            }
        }

    },

    checkFileName: function (name) {
        var languages = '';
        $(SITE.languages).each(function (i, v) {
            if (v != 'en') languages += (languages == '' ? '' : '|') + '_' + v + '$';
        });

        name = name.replace(new RegExp(languages), '');

        var err = 'Filename can only contain characters like A-Z, a-z, 0-9, -, _; No white-spaces, and must not start with - or _. It must not exceed ' + fileTextLimit + ' characters';

        //var reg = /[^a-zA-Z\d-_]|^-|^_|^\d/;
        var reg = /[^a-zA-Z\d-_]|^-|^_/;   //no special characters and white spaces; - is allowed; should not start with - or a number
        var ok = !reg.test(name) && name.length <= fileTextLimit;

        return {
            isValid: ok,
            errMsg: err
        }
    },

    checkFundName: function (name) {
        var err = 'Fund name can only contain characters like A-Z, a-z, 0-9, -, _; Must not start with a number, - or _. It must not exceed ' + fileTextLimit + ' characters';

        var reg = /[^a-zA-Z \d-_]|^-|^_|^\d/;   //no special characters and white spaces; - is allowed; should not start with - or a number
        var ok = !reg.test(name) && name.length <= fileTextLimit;

        return {
            isValid: ok,
            errMsg: err
        }
    },

    dataAccess: function (options, doneCallback) {
        var opts = {
            dataType: "json",
            responseType: "json"
        };

        $.ajax($.extend({}, opts, options)).done(function () {
            if (doneCallback != null) doneCallback.call();
        });
    },

    trimString: function (str, length) {
        var s = str;

        if (str.length + 3 > length) {
            s = str.substring(0, length - 3) + '...';
        }

        return s;
    },

    htmlEncode: function (value) {
        if (value) {
            return $('<div />').text(value).html();
        } else {
            return '';
        }
    },

    htmlDecode: function (value) {
        if (value) {
            return $('<div />').html(value).text();
        } else {
            return '';
        }
    },

    setCookie: function (c_name, value, exdays) {
        var exdate = new Date();
        exdate.setDate(exdate.getDate() + exdays);
        var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
        document.cookie = c_name + "=" + c_value;
    },

    getCookie: function (c_name) {
        var c_value = document.cookie;
        var c_start = c_value.indexOf(" " + c_name + "=");
        if (c_start == -1) {
            c_start = c_value.indexOf(c_name + "=");
        }
        if (c_start == -1) {
            c_value = null;
        }
        else {
            c_start = c_value.indexOf("=", c_start) + 1;
            var c_end = c_value.indexOf(";", c_start);
            if (c_end == -1) {
                c_end = c_value.length;
            }
            c_value = unescape(c_value.substring(c_start, c_end));
        }
        return c_value;
    },

    setPageVar: function (c_name, value, cont) {
        var n = 'pvar-' + c_name;
        var obj = $(document).find('#' + n);
        if (obj.length == 0) {
            obj = $('<input />').attr('type', 'hidden').attr('id', n);
            obj.appendTo(cont);
        }
        obj.val(escape(value));
    },

    getPageVar: function (c_name) {
        var ret = '';
        var n = 'pvar-' + c_name;
        var obj = $(document).find('#' + n);
        if (obj.length > 0) {
            ret = unescape(obj.val());
        }
        return ret;
    },

    showError: function (textStatus, errorThrown) {
        var ret = textStatus;
        if (typeof errorThrown == 'string') ret += (ret == '' ? '' : '; ') + errorThrown;
        else if (typeof errorThrown.message == 'string') ret += (ret == '' ? '' : '; ') + errorThrown.message;
        return ret;
    },

    getCStyle: function (style) {
        var ret = 0;
        if (SITE.Utility.isNumber(style)) ret = style;
        else if (typeof (style) == 'string') {
            if (style == 'medium' || style == 'thin') ret = 1;
            else if (style == 'thick') ret = 2;
            else ret = parseInt(style);
        }
        return ret;
    },

    colorConvert: {
        rgbFromString: function (rgb) {
            var ret = null;
            rgb = rgb.replace('rgb(', '').replace(')', '');
            ret = rgb.split(',');
            ret[0] = parseInt(ret[0]);
            ret[1] = parseInt(ret[1]);
            ret[2] = parseInt(ret[2]);
            return ret;
        },
        rgbStringToHex: function (rgb) {
            var tmp = SITE.Utility.colorConvert.rgbFromString(rgb);
            return SITE.Utility.colorConvert.rgbToHex(tmp[0], tmp[1], tmp[2]);
        },
        rgbToHex: function (r, g, b) {
            var componentToHex = function (c) {
                var hex = c.toString(16);
                return hex.length == 1 ? "0" + hex : hex;
            };
            return "#" + componentToHex(r) + componentToHex(g) + componentToHex(b);
        },
        hexToRgb: function (hex) {
            var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
            return result ? {
                r: parseInt(result[1], 16),
                g: parseInt(result[2], 16),
                b: parseInt(result[3], 16)
            } : null;
        }
    },

    loadStyleSheet: function (path) {
        var head = document.getElementsByTagName('head')[0],
        link = document.createElement('link');
        link.setAttribute('href', path);
        link.setAttribute('rel', 'stylesheet');
        link.setAttribute('type', 'text/css');

        var sheet, cssRules;
        if ('sheet' in link) {
            sheet = 'sheet'; cssRules = 'cssRules';
        }
        else {
            sheet = 'styleSheet'; cssRules = 'rules';
        }

        head.appendChild(link);
        return link;
    },

    loadScript: function (path) {
        var head = document.getElementsByTagName('head')[0],
        script = document.createElement('script');
        script.setAttribute('type', 'text/javascript');
        script.setAttribute('src', path);

        head.appendChild(script);
        return script;
    }

};

SITE.COMMON = {
    addContent: function (isBlock, callback) {
        var ffv_Content = $.extend({}, SITE.OBJECTS.ffView, {
            options: {
                type: 2,
                readOnly: true,
                includeFiles: true,
                selectedValue: '/',
                showReserved: false
            },
            dialogTitle: 'Add Content',
            selectBtnClicked: null,
            selectedPath: ''
        });
        ffv_Content.init();

        ffv_Content.show('/', function (v, li) {
            if (li.hasClass('file')) {
                var txt, objStr;
                if (isBlock) {
                    txt = v.Name;
                    objStr = '<div class="eu2-obj-cont block block-content" bname="block-content" path="~' + basePaths.Content + v.Path + '" draggable="true" ondragstart="drag(event)" >' + txt + '</div>';
                } else {
                    txt = '*** \'' + v.Name + '\' content goes here ***';
                    objStr = '<img class="eu2-obj-cont" src="' + SITE.siteUrl + '/TextImage.ashx?text=' + txt + '" path="~' + basePaths.Content + v.Path + '" style="display: block; border: 1px dashed #f00; width: 100%; max-height: 50px" title="' + txt + '">';
                }
                ffv_Content.dialog.close();
                if (callback != null) callback.call(this, objStr);
            }
        });
    }
};

var content_html_start = '<!--CONTENT_HTML_START-->';
var content_html_end = '<!--CONTENT_HTML_END-->';

function loadEarningsUI() {
    var ui = $('.earnings-ui');
    var data = ui.attr('data-earnings') == '' ? [] : JSON.parse(ui.attr('data-earnings'));

    var addBtn = ui.find('.btn-addnew');
    var fieldSet = ui.find('fieldset');

    var loadData = function () {
        if (data != null) {
            data.forEach(function (d) {
                addRow(d);
            })
        }

        setUI();
    };

    var setUI = function () {
        var formRows = ui.find('.form-row').not('.earning-template');
        if (formRows.length == 0) {
            ui.addClass('no-earning');
        } else {
            ui.removeClass('no-earning');
        }
    };

    var setIds = function () {
        var formRows = ui.find('.form-row').not('.earning-template');

        formRows.each(function (i, v) {
            var row = $(v);
            var itemid = row.find('.item-id');
            var disp = row.find('.item-id-disp');
            var desc = row.find('.item-desc');
            var amount = row.find('.item-amount');

            var id = i + 1;

            itemid.val(id);
            disp.html(id);

            desc.attr('name', 'Earning-Description_' + id);
            amount.attr('name', 'Earning-Amount_' + id);
        });

        setUI();
    };

    var addRow = function (data) {
        var newRow = ui.find('.earning-template').clone().removeClass('earning-template hidden');
        var removeBtn = newRow.find('.btn-remove');

        if (data != null) {
            newRow.find('.item-desc').val(data.Description);
            newRow.find('.item-amount').val(data.Amount);
        }


        removeBtn.click(function (e) {
            e.preventDefault();

            var row = $(this).closest('.form-row');
            row.remove();
            setIds();
        })

        fieldSet.append(newRow);
        setIds();
    };

    addBtn.click(function (e) {
        e.preventDefault();

        addRow();
    })

    loadData();
}

function loadDeductionsUI() {
    var ui = $('.deductions-ui');
    var data = ui.attr('data-deductions') == '' ? [] : JSON.parse(ui.attr('data-deductions'));

    var addBtn = ui.find('.btn-addnew');
    var fieldSet = ui.find('fieldset');

    var loadData = function () {
        if (data != null) {
            data.forEach(function (d) {
                addRow(d);
            })
        }

        setUI();
    };

    var setUI = function () {
        var formRows = ui.find('.form-row').not('.deduction-template');
        if (formRows.length == 0) {
            ui.addClass('no-deduction');
        } else {
            ui.removeClass('no-deduction');
        }
    };

    var setIds = function () {
        var formRows = ui.find('.form-row').not('.deduction-template');

        formRows.each(function (i, v) {
            var row = $(v);
            var itemid = row.find('.item-id');
            var disp = row.find('.item-id-disp');
            var desc = row.find('.item-desc');
            var amount = row.find('.item-amount');

            var id = i + 1;

            itemid.val(id);
            disp.html(id);

            desc.attr('name', 'Deduction-Description_' + id);
            amount.attr('name', 'Deduction-Amount_' + id);
        });

        setUI();
    };

    var addRow = function (data) {
        var newRow = ui.find('.deduction-template').clone().removeClass('deduction-template hidden');
        var removeBtn = newRow.find('.btn-remove');

        if (data != null) {
            newRow.find('.item-desc').val(data.Description);
            newRow.find('.item-amount').val(data.Amount);
        }


        removeBtn.click(function (e) {
            e.preventDefault();

            var row = $(this).closest('.form-row');
            row.remove();
            setIds();
        })

        fieldSet.append(newRow);
        setIds();
    };

    addBtn.click(function (e) {
        e.preventDefault();

        addRow();
    })

    loadData();
}


function preSubmitDeductionsUI(e) {
    var err = [];

    e.modal.find('.deductions-ui .form-row').not('.deduction-template').each(function (i, v) {
        var row = $(v);
        var itemid = row.find('.item-id');
        var desc = row.find('.item-desc');
        var amount = row.find('.item-amount');

        var id = itemid.val();
        var _err = '';

        if (desc.val() == '') {
            _err = 'Description required';
        }

        if (amount.val() == '') {
            _err += (_err == '' ? '' : ' | ') + 'Amount required';
        } else if (!SITE.Utility.isNumber(amount.val())) {
            _err += (_err == '' ? '' : ' | ') + 'Invalid Amount entry';
        }

        if (_err != '') {
            err.push('Item #' + id + ' - ' + _err);
        }
    })

    if (err.length > 0) {
        modalMessage(err, 'Save deductions', true);
        return false;
    }
}