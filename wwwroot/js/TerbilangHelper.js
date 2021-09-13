function Terbilang(n) {
    k = ['', 'satu', 'dua', 'tiga', 'empat', 'lima', 'enam', 'tujuh', 'delapan', 'sembilan'];
    a = [1000000000000000, 1000000000000, 1000000000, 1000000, 1000, 100, 10, 1];
    s = ['kuadriliun', 'trilyun', 'milyar', 'juta', 'ribu', 'ratus', 'puluh', ''];
    var i = 0, x = '';
    while (n > 0) {
        b = a[i], c = Math.floor(n / b), n -= b * c;
        x += (c >= 10 ? Terbilang(c) + " " + s[i] + " " : ((c > 0 && c < 10) ? k[c] + " " + s[i] + " " : ""));
        i++;
    }
    return x.replace(new RegExp(/satu puluh (\w+)/gi), '$1 belas').replace(new RegExp(/satu (ribu|ratus|puluh|belas)/gi), 'se\$1');
}
