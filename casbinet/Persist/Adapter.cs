namespace casbinet.persist
{
    using System;
    using System.Collections.Generic;
    using System.Threading; 

    using casbinet.Model;

    public interface Adapter
    {

        void LoadPolicy(Model model);
        void SavePolicy(Model model);
        void AddPolicy(string sec, string ptype, IList<string> rule);
        void RemovePolicy(string sec, string ptype, IList<string> rule);
        void RemoveFilteredPolicy(string sec, string ptype, int fieldIndex, params string[] fieldValues);

    }
}
