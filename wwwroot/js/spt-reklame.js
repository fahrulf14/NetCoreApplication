if ($('#Bulan').length) {
    var year = new Date().getFullYear();
    var select = document.getElementById("Bulan");
    select.onchange = function () {
        var bulan = select.options[select.selectedIndex].value;
        if (bulan == "1") {
            document.getElementById("MasaPajak1").value = "01-01-" + year;
            document.getElementById("MasaPajak2").value = "31-01-" + year;
        }
        else if (bulan == "2") {
            if ((year % 100 === 0) ? (year % 400 === 0) : (year % 4 === 0)) {
                document.getElementById("MasaPajak1").value = "01-02-" + year;
                document.getElementById("MasaPajak2").value = "29-02-" + year;
            } else {
                document.getElementById("MasaPajak1").value = "01-02-" + year;
                document.getElementById("MasaPajak2").value = "28-02-" + year;
            }
        }
        else if (bulan == "3") {
            document.getElementById("MasaPajak1").value = "01-03-" + year;
            document.getElementById("MasaPajak2").value = "31-03-" + year;
        }
        else if (bulan == "4") {
            document.getElementById("MasaPajak1").value = "01-04-" + year;
            document.getElementById("MasaPajak2").value = "30-04-" + year;
        }
        else if (bulan == "5") {
            document.getElementById("MasaPajak1").value = "01-05-" + year;
            document.getElementById("MasaPajak2").value = "31-05-" + year;
        }
        else if (bulan == "6") {
            document.getElementById("MasaPajak1").value = "01-06-" + year;
            document.getElementById("MasaPajak2").value = "30-06-" + year;
        }
        else if (bulan == "7") {
            document.getElementById("MasaPajak1").value = "01-07-" + year;
            document.getElementById("MasaPajak2").value = "31-07-" + year;
        }
        else if (bulan == "8") {
            document.getElementById("MasaPajak1").value = "01-08-" + year;
            document.getElementById("MasaPajak2").value = "31-08-" + year;
        }
        else if (bulan == "9") {
            document.getElementById("MasaPajak1").value = "01-09-" + year;
            document.getElementById("MasaPajak2").value = "30-09-" + year;
        }
        else if (bulan == "10") {
            document.getElementById("MasaPajak1").value = "01-10-" + year;
            document.getElementById("MasaPajak2").value = "31-10-" + year;
        }
        else if (bulan == "11") {
            document.getElementById("MasaPajak1").value = "01-11-" + year;
            document.getElementById("MasaPajak2").value = "30-11-" + year;
        }
        else if (bulan == "12") {
            document.getElementById("MasaPajak1").value = "01-12-" + year;
            document.getElementById("MasaPajak2").value = "31-12-" + year;
        }
    }
}
else {
    var select = document.getElementById("Tahun");
    select.onchange = function () {
        var tahun = select.options[select.selectedIndex].value;
        document.getElementById("MasaPajak1").value = "01-01-" + tahun;
        document.getElementById("MasaPajak2").value = "31-12-" + tahun;
    }

    var masa = document.getElementById("MasaPajak1");
    masa.onchange = function () {
        var dt = masa.value;
        var day = dt.substring(0, 2);
        var month = dt.substring(3, 5);
        var year = dt.substring(6, 8);

        dt = new Date(month + "/" + day + "/" + year);

        dt.setDate(dt.getDate() + 365);
        var tahun = dt.getFullYear();
        var bulan = dt.getMonth() + 1;
        var tanggal = dt.getDate();

        document.getElementById("MasaPajak2").value = ('0' + tanggal).slice(-2) + "-" + ('0' + bulan).slice(-2) + "-" + tahun;
    }
}

var Inputmask = {
    init: function () {
        $(".ukuran").inputmask("decimal", {
            placeholder: "",
            integerDigits: 3,
            digits: 2,
            digitsOptional: false,
            radixPoint: ",",
            groupSeparator: ",",
            autoGroup: true,
            allowPlus: false,
            allowMinus: false
        })
    }
};
jQuery(document).ready(function () {
    Inputmask.init()
});


function Reklame(jnscoa) {
    if (jnscoa === "4110401" || jnscoa === "4110402") {
        UkuranMeter();
        WaktuHari();
        JumlahBuah();
        DisableDetik();
        Lokasi();
    }
    if (jnscoa === "4110411") {
        UkuranMeter();
        WaktuHari();
        JumlahTayangHari();
        Lokasi();
    }
    if (jnscoa === "4110403") {
        JumlahLembar();
        UkuranCenti();
        DisableDetik();
        Letak();
    }
    if (jnscoa === "4110404") {
        JumlahLembar();
        DisableUkuran();
        DisableDetik();
        Letak();
    }
    if (jnscoa === "4110405") {
        UkuranMeter();
        Letak();
        WaktuHari();
        DisableDetik();
        JumlahBuah();
    }
    if (jnscoa === "4110406") {
        DisableUkuran();
        Letak();
        DisableDetik();
        WaktuBulan();
    }
    if (jnscoa === "4110407") {
        DisableUkuran();
        Letak();
        DisableDetik();
        WaktuBulan();
    }
    if (jnscoa === "4110408" || jnscoa === "4110409") {
        JumlahTayang();
        DisableUkuran();
        Letak();
    }
    if (jnscoa === "4110410") {
        DisableUkuran();
        Letak();
        DisableDetik();
        JumlahKali();
    }
}

function WaktuHari() {
    document.getElementById("addon-hari").innerHTML = "Hari";
}

function WaktuBulan() {
    document.getElementById("addon-hari").innerHTML = "Bulan";
    document.getElementById("addon-kali").innerHTML = "Buah";
}

function JumlahLembar() {
    document.getElementById("JumlahHari").disabled = true;
    document.getElementById("JumlahDetik").disabled = true;
    document.getElementById("addon-kali").innerHTML = "Lembar";
}

function JumlahTayang() {
    document.getElementById("JumlahHari").disabled = true;
    document.getElementById("addon-kali").innerHTML = "Kali";
}

function JumlahTayangHari() {
    document.getElementById("addon-kali").innerHTML = "Kali/Hari";
}

function JumlahBuah() {
    document.getElementById("addon-kali").innerHTML = "Buah";
}

function JumlahKali() {
    document.getElementById("addon-kali").innerHTML = "Kali";
    document.getElementById("JumlahHari").disabled = true;
    document.getElementById("JumlahDetik").disabled = true;
}

function Letak() {
    document.getElementById("Letak").disabled = true;
}

function Lokasi() {
    document.getElementById("KelasJalan").disabled = false;
}

function DisableUkuran() {
    var op = document.getElementsByClassName("opt");
    for (var j = 0; j < op.length; j++) {
        op[j].disabled = true;
    }
}

function DisableDetik() {
    document.getElementById("JumlahDetik").disabled = true;
}


function UkuranMeter() {
    var m = document.getElementsByClassName("uk");
    for (var i = 0; i < m.length; i++) {
        m[i].innerHTML = "M";
    }
}

function UkuranCenti() {
    var cm = document.getElementsByClassName("uk");
    for (var i = 0; i < cm.length; i++) {
        cm[i].innerHTML = "CM";
    }
    var op = document.getElementsByClassName("opt");
    for (var j = 2; j < op.length; j++) {
        op[j].disabled = true;
    }
}

function ClearForm() {
    var clr = document.getElementsByClassName("clr");
    for (var i = 0; i < clr.length; i++) {
        clr[i].disabled = false;
    }
    document.getElementById("KelasJalan").disabled = true;
}