$(function () {
    $.ajaxSetup({ cache: false });

    $("a[mini-modal]").on("click", function (e) {
        $.ajax({
            type: 'POST',
            url: '/AjaxData/Modal',
            dataType: 'json'
        });

        $('#minModalContent').load(this.href, function () {
            $('#minModal').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');
            $('.kt-selectpicker').selectpicker();
            bindForm(this);
        });
        return false;
    });
});

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#minModal').modal('hide');
                    window.location.href = result.url;
                } else {
                    $('#minModalContent').html(result);
                    bindForm(dialog);
                }
            }
        });
        return false;
    });
}

$(function () {
    $.ajaxSetup({ cache: false });

    $("a[data-modal]").on("click", function (e) {
        // hide dropdown if any (this is used wehen invoking modal from link in bootstrap dropdown )
        //$(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');
        $.ajax({
            type: 'POST',
            url: '/AjaxData/Modal',
            dataType: 'json'
        });
        $('#myModalContent').load(this.href, function () {
            $('#myModal').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');
            $('.kt-selectpicker').selectpicker();
            bindForm(this);
        });
        return false;
    });
});

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    window.location.href = result.url;
                } else {
                    $('#myModalContent').html(result);
                    bindForm(dialog);
                }
            }
        });
        return false;
    });
}

$(function () {
    $.ajaxSetup({ cache: false });
    $("a[big-modal]").on("click", function (e) {
        $('#bigModalContent').load(this.href, function () {
            $('#bigModal').modal({
                keyboard: true
            }, 'show');
            $('.kt-selectpicker').selectpicker();
            bindForm2(this);
        });
        return false;
    });
});

function bindForm2(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#bigModal').modal('hide');
                    window.location.href = result.url;
                } else {
                    $('#bigModalContent').html(result);
                    bindForm(dialog);
                }
            }
        });
        return false;
    });
}

$(document).on("ajaxComplete", function (e) {
    $(".check").change(function () {
        var id = event.srcElement.id;
        if ($('#' + id).is(":checked")) {
            document.getElementById(id + "_Status").innerHTML = "Aktif";
        }
        else {
            document.getElementById(id + "_Status").innerHTML = "Non Aktif";
        }
    });
});

$(document).on("ajaxComplete", function (e) {
    $(".check2").change(function () {
        var id = event.srcElement.id;
        if ($('#' + id).is(":checked")) {
            document.getElementById(id + "_Status").innerHTML = "Ya";
        }
        else {
            document.getElementById(id + "_Status").innerHTML = "Tidak";
        }
    });
});