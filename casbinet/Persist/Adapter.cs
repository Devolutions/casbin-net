namespace casbinet.persist
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    //import org.casbin.jcasbin.model.Model;

    public interface Adapter
    {

        void loadPolicy(Model model);
        void savePolicy(Model model);
        void addPolicy(String sec, String ptype, List<String> rule);
        void removePolicy(String sec, String ptype, List<String> rule);
        void removeFilteredPolicy(String sec, String ptype, int fieldIndex, params object[] fieldValues);

    }
}
