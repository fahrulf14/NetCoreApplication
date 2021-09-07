$(function () {
    $("#IndProvinsiId").on("changed.bs.select", function () {
        $('#IndKecamatanId option').prop("selected", false).trigger('change');
        $('#IndKelurahanId option').prop("selected", false).trigger('change');
        Drop_Kota(this.value);
    });
    $("#IndKabKotaId").on("changed.bs.select", function () {
        $('#IndKelurahanId option').prop("selected", false).trigger('change');
        Drop_Kecamatan(this.value);
    });
    $("#IndKecamatanId").on("changed.bs.select", function () {
        Drop_Kelurahan(this.value);
    });
});

// LOAD DROPDOWN KOTA
function Drop_Kota(id) {
    $("#IndKabKotaId option").remove();
    var sel = $("#IndKabKotaId");
    $.ajax({
        type: 'POST',
        url: "/AjaxData/DropKota",
        dataType: 'json',
        data: { id },
        success: function (data) {
            sel.removeAttr("disabled")
            sel.append('<option hidden>Pilih Kab/Kota...</option>');
            $.each(data, function (i, item) {
                var option =
                    '<option value="' + item.id + '">' + item.uraian + '</option>';
                $("#IndKabKotaId").append(option);
            });
            $('#IndKabKotaId').selectpicker('refresh');
        },
        complete: function () {
            $.unblockUI();
        }
    });
    return false;
}

// LOAD DROPDOWN KECAMATAN
function Drop_Kecamatan(id) {
    ShowLoading();
    $("#IndKecamatanId option").remove();
    var sel = $("#IndKecamatanId");
    $.ajax({
        type: 'POST',
        url: "/AjaxData/DropKecamatan",
        dataType: 'json',
        data: { id },
        success: function (data) {
            sel.removeAttr("disabled")
            sel.append('<option hidden>Pilih Kecamatan...</option>');
            $.each(data, function (i, item) {
                var option =
                    '<option value="' + item.id + '">' + item.uraian + '</option>';
                sel.append(option);
            });
            $('#IndKecamatanId').selectpicker('refresh');
        },
        complete: function () {
            $.unblockUI();
        }
    });
    return false;
}

// LOAD DROPDOWN KELURAHAN
function Drop_Kelurahan(id) {
    ShowLoading();
    $("#IndKelurahanId option").remove();
    var sel = $("#IndKelurahanId");
    $.ajax({
        type: 'POST',
        url: "/AjaxData/DropKelurahan",
        dataType: 'json',
        data: { id },
        success: function (data) {
            sel.removeAttr("disabled")
            sel.append('<option hidden>Pilih Kelurahan...</option>');
            $.each(data, function (i, item) {
                var option =
                    '<option value="' + item.id + '">' + item.uraian + '</option>';
                sel.append(option);
            });
            $('#IndKelurahanId').selectpicker('refresh');
        },
        complete: function () {
            $.unblockUI();
        }
    });
    return false;
}