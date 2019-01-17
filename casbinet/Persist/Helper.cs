namespace casbinet.persist
{
    using System;
    using System.Linq;

    using casbinet.Model;

    public class Helper
    {
        public delegate void LoadPolicyLineHandler<in T, in U>(T t, U u);

        public static void LoadPolicyLine(string line, Model model)
        {
            if (string.IsNullOrEmpty(line))
            {
                return;
            }

            if (line.StartsWith('#'))
            {
                return;
            }

            string[] tokens = line.Split(", ");

            string key = tokens[0];
            string sec = key.Substring(0, 1);
            model.model[sec][key].Policy.Add(tokens.ToList());
        }
    }
}
