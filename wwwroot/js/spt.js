var year = new Date().getFullYear();
var select = document.getElementById("MasaPajak");
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