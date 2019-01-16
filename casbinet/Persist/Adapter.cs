namespace casbinet.persist
{
    using System;
    using System.Collections.Generic;
    using System.Threading; 

    using Model = casbinet.model.Model;

    public interface Adapter
    {

        void loadPolicy(Model model);
        void savePolicy(Model model);
        void addPolicy(string sec, string ptype, IList<string> rule);
        void removePolicy(string sec, string ptype, IList<string> rule);
        void removeFilteredPolicy(string sec, string ptype, int fieldIndex, params string[] fieldValues);

    }
}
