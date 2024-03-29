var KTBootstrapSelect = {
    init: function () {
        $(".kt-selectpicker").selectpicker()
    }
};
jQuery(document).ready(function () {
    KTBootstrapSelect.init()
});

var BootstrapDatepicker = {
    init: function () {
        $("#myModal").on("shown.bs.modal", function () {
            $(".datefield").datepicker({
                format: "dd-mm-yyyy",
                todayHighlight: !0,
                orientation: "bottom left",
                autoclose: true
            });
        }), $(".datefield").datepicker({
            format: "dd-mm-yyyy",
            todayHighlight: !0,
            orientation: "bottom left",
            autoclose: true
        }), $(".datereport").datepicker({
            format: "mm-dd-yyyy",
            todayHighlight: !0,
            orientation: "bottom left",
            autoclose: true
        }), $(".dateleft").datepicker({
            format: "mm-dd-yyyy",
            todayHighlight: !0,
            orientation: "bottom left",
            autoclose: true
        }), $(".dateright").datepicker({
            format: "mm-dd-yyyy",
            todayHighlight: !0,
            orientation: "bottom right",
            autoclose: true
        })
    }
};
jQuery(document).ready(function () {
    BootstrapDatepicker.init();
});

$(document).on("ajaxComplete", function () {
    BootstrapDatepicker.init();
});


function ShowLoading() {
    KTApp.blockPage({
        overlayColor: "#000000",
        type: "loader",
        state: "success",
        message: "Loading..."
    })
}


$(".check").change(function () {
    var id = event.srcElement.id;
    if ($('#' + id).is(":checked")) {
        document.getElementById(id + "_Status").innerHTML = "Active";
    }
    else {
        document.getElementById(id + "_Status").innerHTML = "Inactive";
    }
});

$(".check2").change(function () {
    var id = event.srcElement.id;
    if ($('#' + id).is(":checked")) {
        document.getElementById(id + "_Status").innerHTML = "Yes";
    }
    else {
        document.getElementById(id + "_Status").innerHTML = "No";
    }
});

if (document.getElementById("datenow") != null) {
    var date = new Date(Date.now());
    var bulan = new Array();
    bulan[0] = "January";
    bulan[1] = "February";
    bulan[2] = "Maret";
    bulan[3] = "April";
    bulan[4] = "May";
    bulan[5] = "Juny";
    bulan[6] = "July";
    bulan[7] = "August";
    bulan[8] = "September";
    bulan[9] = "October";
    bulan[10] = "November";
    bulan[11] = "Desember";

    var tanggal = date.getDate() + " " + bulan[date.getMonth()] + " " + date.getFullYear();
    document.getElementById("datenow").innerHTML = tanggal;
}

toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": true,
    "progressBar": false,
    "positionClass": "toast-bottom-right",
    "preventDuplicates": false,
    "onclick": null,
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "slideDown",
    "hideMethod": "slideUp"
};

function showLoading() {
    KTApp.blockPage({
        overlayColor: '#000000',
        type: 'v2',
        state: 'success',
        message: 'Please wait...'
    });
}

$(window).on('beforeunload', function () {
    showLoading();
});

$(document).ajaxStart(function () {
    showLoading();
});

$(document).ajaxStop(function () {
    KTApp.unblockPage();
});

$('.home-full-width').width($(window).width())

$(window).resize(function () {
    $('.home-full-width').width($(window).width())
});