
function loanUtility(cont, principal, interest, nop, mp, nop_calc, mp_calc) {

    this.checkEntries = function (forMonthlyPayment) {
        return check(forMonthlyPayment);
    };

    this.getLoanValues = function () {
        return getValues();
    };

    var check = function (forMonthlyPayment) {
        var err = [];

        if (interest.val() == '') {
            err.push('\'Interest Rate\' required');
        } else if (!SITE.Utility.isNumber(interest.val())) {
            err.push('Invalid \'Interest Rate\' entry');
        }

        if (principal.val() == '') {
            err.push('\'Principal Amount\' required');
        } else if (!SITE.Utility.isNumber(principal.val())) {
            err.push('Invalid \'Principal Amount\' entry');
        } else if (parseFloat(principal.val()) <= 0) {
            err.push('Invalid \'Principal Amount\' entry');
        }

        if (forMonthlyPayment == null || forMonthlyPayment == true) {
            if (nop.val() == '') {
                err.push('\'No. of Payments\' required');
            } else if (!SITE.Utility.isNumber(nop.val())) {
                err.push('Invalid \'No. of Payments\' entry');
            }
        }
        if (forMonthlyPayment == null || forMonthlyPayment == false) {
            if (mp.val() == '') {
                err.push('\'Monthly Payment\' required');
            } else if (!SITE.Utility.isNumber(mp.val())) {
                err.push('Invalid \'Monthly Payment\' entry');
            }
        }

        return err;
    };

    var getValues = function () {
        return {
            principal: parseFloat(principal.val()),
            interest: parseFloat(interest.val()) / 100,
            nop: parseInt(nop.val()),
            mp: parseFloat(mp.val())
        };
    };

    nop_calc.click(function (e) {
        e.preventDefault();

        var err = check(false);

        if (err.length > 0) {
            modalMessage(err, 'Get No. of Payments', true);
            return;
        }

        var v = getValues();

        cont.addClass('loading-mask');

        var url = '/FinanceModule/LoanDefs/getNoOfPayments';

        var data = {
            principal: v.principal,
            interest: v.interest,
            monthlyPayment: v.mp
        };

        $.get(url, data, function (res) {
            if (res.IsSuccessful) {
                nop.val(res.Data);
            } else {
                modalMessage(res.Err.split('\n'), 'Get Monthly Payment', true);
            }

            cont.removeClass('loading-mask');
        }, 'json');
    });

    mp_calc.click(function (e) {
        e.preventDefault();

        var err = check(true);

        if (err.length > 0) {
            modalMessage(err, 'Get Monthly Payment', true);
            return;
        }

        var v = getValues();

        cont.addClass('loading-mask');

        var url = '/FinanceModule/LoanDefs/getMonthlyPayment';

        var data = {
            principal: v.principal,
            interest: v.interest,
            noOfPayments: v.nop
        };

        $.get(url, data, function (res) {
            if (res.IsSuccessful) {
                mp.val(res.Data);
            } else {
                modalMessage(res.Err.split('\n'), 'Get No. of Payments', true);
            }

            cont.removeClass('loading-mask');
        }, 'json');
    });
}