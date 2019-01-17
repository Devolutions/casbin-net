using System;
using System.Collections.Generic;
using System.Text;

namespace casbinet.Model
{
    using casbinet.Util;

    public class FunctionMap
    {
        public delegate (object, Exception) KeyMatchFunction(params object[] args);

        public Dictionary<string, KeyMatchFunction> fm;

        public void AddFunction(string name, KeyMatchFunction function)
        {
            this.fm.Add(name, function);
        }

        public static FunctionMap LoadFunctionMap()
        {
            FunctionMap fm = new FunctionMap();

            fm.fm = new Dictionary<string, KeyMatchFunction>();
            fm.AddFunction("keyMatch", BuiltInOperator.KeyMatchFunc);
            fm.AddFunction("keymatch2", BuiltInOperator.KeyMatch2Func);
            fm.AddFunction("regexMatch", BuiltInOperator.RegexMatchFunc);
            fm.AddFunction("ipMatch", BuiltInOperator.IpMatchFunc);

            return fm;
        }
    }
}
