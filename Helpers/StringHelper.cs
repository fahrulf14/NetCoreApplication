using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NUNA.Helpers
{
    public static class StringHelper
    {
        public static string GetURLWithoutHost(string path)
        {
            string finalpath = path;
            try
            {
                Regex RegexObj = new("[\\w\\W]*([\\/]Assets[\\w\\W\\s]*)");
                if (RegexObj.IsMatch(path))
                {
                    finalpath = RegexObj.Match(path).Groups[1].Value;
                }
            }
            catch (ArgumentException)
            {

            }
            return finalpath;
        }

        public static string MappedValueToHTML(string htmlContent, Object data)
        {
            if (string.IsNullOrEmpty(htmlContent))
            {
                return "";
            }
            else
            {
                string tmphtmlContent = htmlContent;
                Regex RegexObj = new(@"[\{][\{]([a-zA-Z0-9\.]*)[\}][\}]");
                Match MatchResults = RegexObj.Match(htmlContent);
                while (MatchResults.Success)
                {
                    for (int i = 1; i < MatchResults.Groups.Count; i++)
                    {
                        Group GroupObj = MatchResults.Groups[i];
                        if (GroupObj.Success)
                        {
                            try
                            {
                                var objValue = GetPropValue(data, GroupObj.Value);
                                var val = string.IsNullOrEmpty(objValue.ToString()) ? "(Belum ada data)" : GetPropValue(data, GroupObj.Value);
                                tmphtmlContent = tmphtmlContent.Replace("{{" + GroupObj.Value + "}}", val.ToString());
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                    MatchResults = MatchResults.NextMatch();
                }
                htmlContent = tmphtmlContent;
            }

            return htmlContent;
        }

        private static Object GetPropValue(this Object obj, String name)
        {
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }
    }
}
