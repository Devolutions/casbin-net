namespace casbinet.Util
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;

    public static class BuiltInOperator
    {
        public static bool KeyMatch(string key1, string key2)
        {
            int i = key2.IndexOf('*');
            if (i == -1)
            {
                return key1 == key2;
            }

            if (key1.Length > i)
            {
                return key1.Substring(0, i) == key2.Substring(0, i);
            }

            return key1.Equals(key2.Substring(0, i));
        }

        public static (object, Exception) KeyMatchFunc(params object[] args)
        {
            string name1 = args[0].ToString();
            string name2 = args[1].ToString();

            return (KeyMatch(name1, name2), null);
        }

        public static bool KeyMatch2(string key1, string key2)
        {
            key2 = key2.Replace("/*", "/.*");

            string rgxString = @"(.*):[^/]+(.*)";
            Regex rgx = new Regex(rgxString);
            while (true)
            {
                if (!key2.Contains("/:"))
                {
                    break;
                }

                key2 = "^" + rgx.Replace(key2, "$1[^/]+$2") + "$";
            }

            return RegexMatch(key1, key2);
        }

        public static (object, Exception) KeyMatch2Func(params object[] args)
        {
            string name1 = args[0].ToString();
            string name2 = args[1].ToString();

            return (KeyMatch2(name1, name2), null);
        }

        public static bool KeyMatch3(string key1, string key2)
        {
            key2 = key2.Replace("/*", "/.*");

            string rgxString = @"(.*)\\{[^/]+\\}(.*)";
            Regex rgx = new Regex(rgxString);
            while (true)
            {
                if (!key2.Contains("/:"))
                {
                    break;
                }

                key2 = rgx.Replace(key2, "$1[^/]+$2");
            }

            return RegexMatch(key1, key2);
        }

        public static (object, Exception) KeyMatch3Func(params object[] args)
        {
            string name1 = args[0].ToString();
            string name2 = args[1].ToString();

            return (KeyMatch3(name1, name2), null);
        }

        public static bool RegexMatch(string key1, string key2)
        {
            return Regex.IsMatch(key2, key1);
        }

        public static (object, Exception) RegexMatchFunc(params object[] args)
        {
            string name1 = args[0].ToString();
            string name2 = args[1].ToString();

            return (RegexMatch(name1, name2), null);
        }

        public static bool IpMatch(string ip1, string ip2)
        {
            try
            {
                IPNetwork ipn1 = IPNetwork.Parse(ip1);
                IPNetwork ipn2 = IPNetwork.Parse(ip2);

                try
                {
                 
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
