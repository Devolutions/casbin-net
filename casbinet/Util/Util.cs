namespace casbinet.Util
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Util
    {
        public static bool enableLog = true;

        //todo : Implement Log functions

        public static void LogPrint(string log)
        {
            if (!enableLog)
            {
                return;
            }

            Console.WriteLine(log);
        }

        public static void LogPrintf(string format, params string[] logs)
        {
            if (!enableLog)
            {
                return;
            }

            Console.WriteLine(logs);
        }

        public static string EscapeAssertion(string s)
        {
            if (s.StartsWith('r') || s.StartsWith('p'))
            {
                s.Replace(".", "_", true, CultureInfo.CurrentCulture);
            }

            string regex = @"(\|| |=|\)|\(|&|<|>|,|\+|-|!|\*|\/)(r|p)\.";
            MatchCollection collection = Regex.Matches(s, regex);
            return string.Concat(collection.Select(m => m.Value.Replace(".", "_")));
        }

        public static string RemoveComments(string text)
        {
            int pos = text.IndexOf('#');
            text = pos < 0 ? text : text.Substring(0, pos).Trim();
            return text;
        }

        public static bool ArrayEquals(List<string> a, List<string> b)
        {
            if (!(a != null && b != null) || a.Count != b.Count)
            {
                return false;
            }

            return Enumerable.SequenceEqual(a, b);
        }
    }
}
