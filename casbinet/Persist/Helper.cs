namespace casbinet.persist
{
    using System;
    using System.Collections.Generic;
    using System.Threading; 

    using casbinet.Model;

    public class Helper
    {
        public interface loadPolicyLineHandler<T, U>
        {
            void accept(T t, U u);
        }

        public static void loadPolicyLine(String line, Model model)
        {
            if (line == String.Empty)
            {
                return;
            }

            if (line[0] == '#')
            {
                return;
            }

            String[] tokens = line.Split(", ");

            String key = tokens[0];
            String sec = key.Substring(0, 1);
            model.model[sec][key].policy.add(Arrays.asList(Arrays.copyOfRange(tokens, 1, tokens.Length)));
        }
    }
}
