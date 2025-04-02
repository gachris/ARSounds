$(".validate-form").initialize(function () {
    var form = $(this);
    $.validator.unobtrusive.parse(form);
    disableSubmit(form);
    var formInputs = form.find('[disable-submit-for]');
    formInputs.each(function () {
        $(this).keyup(function () {
            disableSubmit(form);
        });
    })
})

function disableSubmit(form) {
    var dataHasChanged = false;

    var inputs = form.find('[disable-submit-for]');

    if (inputs.length == 0) {
        return;
    }

    $(inputs).each(function () {
        var input = $(this);
        var inputId = '#' + input.attr("id");
        var oldValue = input.attr("disable-submit-for");
        var newValue = $(inputId).val();
        if (newValue != oldValue) {
            dataHasChanged = true;
        }
    });

    var button = form.find(':submit');

    if (dataHasChanged == false) {
        $(button).attr('disabled', true);
    } else {
        $(button).removeAttr('disabled');
    }
};

function notifyMessage(message, type) {
    $.notify(message, {
        placement: {
            from: "bottom",
            align: 'right'
        },
        animate: {
            enter: "animated fadeInUp",
            exit: "animated fadeOutDown"
        },
        type: type,
        allow_dismiss: true,
    });
    setTimeout(function () {
        $.notifyClose();
    }, 3000);
}

$(".checkbox-modern").initialize(function () {
    $('label[for="' + $(this).attr('id') + '"]').css('display', 'none');
    $(this).before('<input type="button" checkbox-id="' + $(this).attr('id') + '" class="checkbox-button" value="' + $('label[for="' + $(this).attr('id') + '"]').html() + '&nbsp;&nbsp; &#10008;">');
    $(this).css('display', 'none');

    $('input[checkbox-id="' + $(this).attr('id') + '"]').click(function () {
        $($('#' + $(this).attr('checkbox-id'))).prop('checked', !$($('#' + $(this).attr('checkbox-id'))).prop('checked'));
        if ($(this).hasClass('checked')) {
            $(this).removeClass('checked').children('input').val('');
            $(this).val($("<div />").html($('label[for="' + $($('#' + $(this).attr('checkbox-id'))).attr('id') + '"]').html() + '&nbsp;&nbsp; &#10008;').text());
        } else {
            $(this).addClass('checked').children('input').val($(this).data('val'));
            $(this).val($("<div />").html($('label[for="' + $($('#' + $(this).attr('checkbox-id'))).attr('id') + '"]').html() + '&nbsp;&nbsp; &#10004;').text());
        }
    });
});

$('.collapse').on('shown.bs.collapse', function (e) {
    var clicked = $(document).find("[data-target='#" + $(e.target).attr('id') + "']");
    clicked.attr('type', 'button');
}).on('show.bs.collapse', function (e) { });

$('.collapse').on('hidden.bs.collapse', function (e) {
    var clicked = $(document).find("[data-target='#" + $(e.target).attr('id') + "']");
    $('#' + $(e.target).attr('id')).empty();
    clicked.attr('type', 'submit');
}).on('hide.bs.collapse', function (e) { });

window.addEventListener("load", function () { document.querySelector('body').classList.add('loaded'); });
