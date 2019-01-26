namespace casbinet.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using casbinet.Model;
    using casbinet.Rbac;
    using casbinet.Util.Function;

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

        public static (object, Exception) KeyMatchFunc(IRoleManager rm, params object[] args)
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

        public static (object, Exception) KeyMatch2Func(IRoleManager rm, params object[] args)
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

        public static (object, Exception) KeyMatch3Func(IRoleManager rm, params object[] args)
        {
            string name1 = args[0].ToString();
            string name2 = args[1].ToString();

            return (KeyMatch3(name1, name2), null);
        }

        public static bool RegexMatch(string key1, string key2)
        {
            return Regex.IsMatch(key2, key1);
        }

        public static (object, Exception) RegexMatchFunc(IRoleManager rm, params object[] args)
        {
            string name1 = args[0].ToString();
            string name2 = args[1].ToString();

            return (RegexMatch(name1, name2), null);
        }

        public static bool IpMatch(string ip1, string ip2)
        {
            string rgxString = @"^(((\d)|(\d\d)|((1)\d\d)|(2[0-4]\d)|(25[0-5])).){3}((\d)|(\d\d)|((1)\d\d)|(2[0-4]\d)|(25[0-5]))$";
            Regex rgx = new Regex(rgxString);
            if(!rgx.IsMatch(ip1))
            {
                throw new Exception("invalid argument: ip1 in IPMatch() function is not an IP address.");
            }

            IPNetwork ipNetwork2 = IPNetwork.Parse(ip2);
            if(!(ipNetwork2.AddressFamily == AddressFamily.InterNetwork || ipNetwork2.AddressFamily == AddressFamily.InterNetworkV6))
            {
                throw new Exception("invalid argument: ip2 in IPMatch() function is neither an IP address nor a CIDR.");
            }

            IPNetwork ipNetwork1 = IPNetwork.Parse(ip1);

            if (ipNetwork1.Equals(ipNetwork2))
            {
                return true;
            }

            return ipNetwork1.Netmask.Equals(IPAddress.Parse(ip2));
        }

        public static (object, Exception) IpMatchFunc(IRoleManager rm, params object[] args)
        {
            string name1 = args[0].ToString();
            string name2 = args[1].ToString();

            return (IpMatch(name1, name2), null);
        }

        public static FunctionMap.KeyMatchFunction GFunctionFactory(IRoleManager rm, params object[] args)
        {
            FunctionMap.KeyMatchFunction func = delegate(IRoleManager manager, object[] objects)
                {
                    string name1 = args[0].ToString();
                    string name2 = args[1].ToString();
                    string domain = null;

                    if (args.Length > 2)
                    {
                        domain = args[2].ToString();
                    }

                    if (rm == null)
                    {
                        return (name1.Equals(name2), null);
                    }

                    if (string.IsNullOrEmpty(domain))
                    {
                        return (rm.HasLink(name1, name2), null);
                    }

                    return (rm.HasLink(name1, name2, domain), null);
                };

            return func;
        }
    }
}
