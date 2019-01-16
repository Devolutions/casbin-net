namespace casbinet.util.function
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    //using AbstractFunction = com.googlecode.aviator.runtime.function.AbstractFunction;
    //using FunctionUtils = com.googlecode.aviator.runtime.function.FunctionUtils;
    //using AviatorBoolean = com.googlecode.aviator.runtime.type.AviatorBoolean;
    //using AviatorObject = com.googlecode.aviator.runtime.type.AviatorObject;

    public class KeyMatch2Func : AbstractFunction
    {
        public virtual AviatorObject call(IDictionary<string, object> env, AviatorObject arg1, AviatorObject arg2)
        {
            string key1 = FunctionUtils.getStringValue(arg1, env);
            string key2 = FunctionUtils.getStringValue(arg2, env);

            return AviatorBoolean.valueOf(BuiltInFunctions.keyMatch2(key1, key2));
        }

        public virtual string Name
        {
            get
            {
                return "keyMatch2";
            }
        }
    }
}
