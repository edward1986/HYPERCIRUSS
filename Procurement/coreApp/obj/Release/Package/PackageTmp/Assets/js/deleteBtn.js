
$(document).ready(function () {
    $('.delete-btn').unbind('click').bind('click', function (e) {
        e.preventDefault();
        e.stopPropagation();

        var me = $(this);

        var formId = me.attr('data-delete-form-id');
        var formIdValue = me.attr('data-delete-form-id-value');
        var deleteForm = $(formId);

        deleteForm.find('[name="id"]').val(formIdValue);


        $("#dialog-confirm").removeClass('hide').dialog({
            resizable: false,
            width: '400',
            modal: true,
            title: 'Delete record',
            title_html: false,
            buttons: [
                {
                    html: "<i class='ace-icon fa fa-trash-o bigger-110'></i>&nbsp; Confirm delete",
                    "class": "btn btn-danger btn-sm",
                    click: function () {
                        deleteForm.submit();
                        $(this).dialog("close");
                    }
                }
                ,
                {
                    html: "<i class='ace-icon fa fa-times bigger-110'></i>&nbsp; Cancel",
                    "class": "btn btn-sm",
                    click: function () {
                        $(this).dialog("close");
                    }
                }
            ]
        });
    });
});