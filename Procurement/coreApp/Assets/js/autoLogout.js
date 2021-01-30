var pauseOp = false;
var autoLogout_tmpTimer;
var dialogTimer, dialogTimer_sec;

window.load = initautoLogoutTimer;
window.onmousemove = resetautoLogoutTimer; // catches mouse movements
window.onmousedown = resetautoLogoutTimer; // catches mouse movements
window.onclick = resetautoLogoutTimer;     // catches mouse clicks
window.onscroll = resetautoLogoutTimer;    // catches scrolling
window.onkeypress = resetautoLogoutTimer;  //catches keyboard actions

function showautologoutDialog() {
    pauseOp = true;
    clearInterval(dialogTimer);
    dialogTimer_sec = 30;

    modalEmpty('Auto Logout', function (modal) {
        var text1 = $('<span />').text('Page has been idle for ' + autoLogoutInMins + ' minutes. Session will be logged out in ');
        var sec = $('<span />').addClass('tmp-sec');
        var text2 = $('<span />').text(' seconds');

        var confirmLogout = modal.find('.modal-footer .btn-ok');
        var stayLoggedIn = modal.find('.modal-footer .btn-cancel');

        confirmLogout.html('Confirm Logout');
        stayLoggedIn.html('Stay Logged In');

        modal.find('.modal-body')
            .append(text1)
            .append(sec)
            .append(text2);

        dialogTimer = setInterval(function () {
            dialogTimer_sec -= 1;
            if (dialogTimer_sec <= 0) {
                clearInterval(dialogTimer);
                confirmLogout.click();
            } else {
                sec.text(dialogTimer_sec);
            }
            
        }, 1000);
    }, function (ret, modal) {
        if (ret) {
            $('#logoutForm').submit();
        } else {
            pauseOp = false;
            resetautoLogoutTimer();
        }
    });
}

function resetautoLogoutTimer() {
    if (!pauseOp) {
        clearTimeout(autoLogout_tmpTimer);
        clearInterval(dialogTimer);
        initautoLogoutTimer();
    }
}

function initautoLogoutTimer() {
    autoLogout_tmpTimer = setTimeout(showautologoutDialog, (autoLogoutInMins * 60 * 1000));
}