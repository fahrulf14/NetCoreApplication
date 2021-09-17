$(function () {
    $.ajaxSetup({ cache: false });

    $("a[mini-modal]").on("click", function (e) {
        $.ajax({
            type: 'POST',
            url: '/Session/Modal',
            dataType: 'json'
        });

        $('#minModalContent').load(this.href, function () {
            $('#minModal').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');
            $('.kt-selectpicker').selectpicker();
            bindFormMini(this);
        });
        return false;
    });
});

function bindFormMini(dialog) {
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
                    throwMessage(result);
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
            url: '/Session/Modal',
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
                    throwMessage(result);
                }
            }
        });
        return false;
    });
}

$(function () {
    $.ajaxSetup({ cache: false });
    $("a[big-modal]").on("click", function (e) {
        $.ajax({
            type: 'POST',
            url: '/Session/Modal',
            dataType: 'json'
        });
        $('#bigModalContent').load(this.href, function () {
            $('#bigModal').modal({
                keyboard: true
            }, 'show');
            $('.kt-selectpicker').selectpicker();
            bindFormBig(this);
        });
        return false;
    });
});

function bindFormBig(dialog) {
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
                    throwMessage(result);
                }
            }
        });
        return false;
    });
}

function throwMessage(result) {
    switch (result.type) {
        case 'success':
            toastr.success(result.message);
            break;
        case 'info':
            toastr.info(result.message);
            break;
        case 'warning':
            toastr.warning(result.message);
            break;
        case 'error':
            toastr.error(result.message);
            break;
        default:
    }
}

$(document).on("ajaxComplete", function (e) {
    $(".check").change(function () {
        var id = event.srcElement.id;
        if ($('#' + id).is(":checked")) {
            document.getElementById(id + "_Status").innerHTML = "Active";
        }
        else {
            document.getElementById(id + "_Status").innerHTML = "Inactive";
        }
    });
});

$(document).on("ajaxComplete", function (e) {
    $(".check2").change(function () {
        var id = event.srcElement.id;
        if ($('#' + id).is(":checked")) {
            document.getElementById(id + "_Status").innerHTML = "Yes";
        }
        else {
            document.getElementById(id + "_Status").innerHTML = "No";
        }
    });
});