// PartialView Modal
$(document).on("ajaxComplete", function (e) {
    $(".rupiah").blur(function () {
        var id = '#' + event.srcElement.id;
        var Inputmask = {
            init: function () {
                $(id).inputmask("numeric", {
                    radixPoint: ",",
                    groupSeparator: ".",
                    digits: 3,
                    rightAlign: false,
                    placeholder: "",
                    autoGroup: true,
                    removeMaskOnSubmit: true,
                    autoUnmask: true
                })
            }
        };
        jQuery(document).ready(function () {
            Inputmask.init()
        });
    });

    $(".rupiah").focus(function () {
        this.select();
    });
});

// Main page
$(".rupiah").blur(function () {
    var id = '#' + event.srcElement.id;
    var Inputmask = {
        init: function () {
            $(id).inputmask("numeric", {
                radixPoint: ",",
                groupSeparator: ".",
                digits: 3,
                rightAlign: false,
                placeholder: "",
                autoGroup: true,
                removeMaskOnSubmit: true,
                autoUnmask: true
            })
        }
    };
    jQuery(document).ready(function () {
        Inputmask.init()
    });
});

$(".rupiah").focus(function () {
    this.select();
});