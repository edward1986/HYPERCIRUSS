$(document).ready(function () {
    $('.reset').click(function (e) {
        e.preventDefault();

        $('.unselect-all').click();

        var permissions = $('[name="Permissions"]').val();
        if (permissions.indexOf('Info') >= 0) $('[name="Permission"][value="Info"]').prop('checked', true).parent().addClass('checked');
        if (permissions.indexOf('Children') >= 0) $('[name="Permission"][value="Children"]').prop('checked', true).parent().addClass('checked');
        if (permissions.indexOf('Education') >= 0) $('[name="Permission"][value="Education"]').prop('checked', true).parent().addClass('checked');
        if (permissions.indexOf('CivilService') >= 0) $('[name="Permission"][value="CivilService"]').prop('checked', true).parent().addClass('checked');
        if (permissions.indexOf('WorkExps') >= 0) $('[name="Permission"][value="WorkExps"]').prop('checked', true).parent().addClass('checked');
        if (permissions.indexOf('Trainings') >= 0) $('[name="Permission"][value="Trainings"]').prop('checked', true).parent().addClass('checked');
    });
});