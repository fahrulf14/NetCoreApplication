$(function () {
    $("#IndKecamatanId").on("changed.bs.select", function () {
        Drop_Kelurahan(this.value);
    });
});

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
            sel.append('<option hidden value="">Pilih Kelurahan...</option>');
            $.each(data, function (i, item) {
                var option =
                    '<option value="' + item.id + '">' + item.uraian + '</option>';
                sel.append(option);
            });
        },
        complete: function () {
            $('select[name=IndKelurahanId]').val("");
            $('#IndKelurahanId').selectpicker('refresh');
            $.unblockUI();
        }
    });
    return false;
}