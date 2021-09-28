using System;
using System.Globalization;

namespace NUNA.Helpers
{
    public static class NumberHelper
    {

        public static string ToRupiah(this decimal src)
        {
            var indonesianCultureInfo = new CultureInfo("id-ID");
            indonesianCultureInfo.NumberFormat.CurrencyNegativePattern = 1;
            return src.ToString("C0", indonesianCultureInfo).Replace("Rp", "Rp. ");
        }

        // 1.000.000,00
        internal static string IndoFormat(decimal num)
        {
            var numRound = Math.Round(num);
            var numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NumberDecimalSeparator = ",";
            numberFormatInfo.NumberGroupSeparator = ".";
            return numRound.ToString("N", numberFormatInfo);
        }

        // 1.000.000,12
        internal static string IndoFormatNoRound(decimal num)
        {
            var numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NumberDecimalSeparator = ",";
            numberFormatInfo.NumberGroupSeparator = ".";
            return num.ToString("N", numberFormatInfo);
        }

        // 1.000.000
        internal static string IndoFormatWithoutTail(decimal num)
        {
            var numRound = Math.Round(num);
            var numberFormatInfo = new NumberFormatInfo();
            numberFormatInfo.NumberGroupSeparator = ".";
            return numRound.ToString("N0", numberFormatInfo);
        }

        // Satu Juta
        internal static string Terbilang(this decimal y)
        {
            return TerbilangCore(y).TrimStart().TrimEnd();
        }

        // Satu Juta Koma Satu Dua
        internal static string TerbilangKoma(this decimal y)
        {
            if (y.ToString().Contains("."))
            {
                string s = y.ToString();
                string[] parts = s.Split('.');
                int i1 = int.Parse(parts[0]);
                string i2 = parts[1];
                var terbilang = Terbilang(Convert.ToDecimal(i1)).TrimEnd() + TerbilangKoma(i2);
                return terbilang.TrimStart().TrimEnd();
            }
            else
            {
                return TerbilangCore(y).TrimStart().TrimEnd();
            }
        }

        internal static string TerbilangKoma(string y)
        {
            char[] characters = y.ToCharArray();
            var result = " Koma ";
            foreach (var chr in characters)
            {
                if (chr == '0')
                {
                    result += "Nol ";
                }
                else if (chr == '1')
                {
                    result += "Satu ";
                }
                else if (chr == '2')
                {
                    result += "Dua ";
                }
                else if (chr == '3')
                {
                    result += "Tiga ";
                }
                else if (chr == '4')
                {
                    result += "Empat ";
                }
                else if (chr == '5')
                {
                    result += "Lima ";
                }
                else if (chr == '6')
                {
                    result += "Enam ";
                }
                else if (chr == '7')
                {
                    result += "Tujuh ";
                }
                else if (chr == '8')
                {
                    result += "Delapan ";
                }
                else if (chr == '9')
                {
                    result += "Sembilan ";
                }
            }
            return result;
        }

        internal static string Counting(this decimal? y)
        {
            return TerbilangCore(y);
        }

        private static string TerbilangCore(this decimal? y)
        {
            string[] bilangan = { "", "Satu", "Dua", "Tiga", "Empat", "Lima", "Enam", "Tujuh", "Delapan", "Sembilan", "Sepuluh", "Sebelas" };
            string temp = "";

            if (y == null)
            {
                return "-";
            }

            long x = Convert.ToInt64(y);

            if (x < 12)
            {
                temp = " " + bilangan[x];
            }
            else if (x < 20)
            {
                temp = Counting(x - 10).ToString() + " Belas";
            }
            else if (x < 100)
            {
                temp = Counting(x / 10) + " Puluh" + Counting(x % 10);
            }
            else if (x < 200)
            {
                temp = " Seratus" + Counting(x - 100);
            }
            else if (x < 1000)
            {
                temp = Counting(x / 100) + " Ratus" + Counting(x % 100);
            }
            else if (x < 2000)
            {
                temp = " Seribu" + Counting(x - 1000);
            }
            else if (x < 1000000)
            {
                temp = Counting(x / 1000) + " Ribu" + Counting(x % 1000);
            }
            else if (x < 1000000000)
            {
                temp = Counting(x / 1000000) + " Juta" + Counting(x % 1000000);
            }
            else if (x < 1000000000000)
            {
                temp = Counting(x / 1000000000) + " Miliar" + Counting(x % 1000000000);
            }
            else if (x < 1000000000000000)
            {
                temp = Counting(x / 1000000000000) + " Triliun" + Counting(x % 1000000000000);
            }

            return temp;
        }
    }
}
