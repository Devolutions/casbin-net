namespace casbinet.persist
{
    using System.Collections.Generic;

    using casbinet.Model;

    public interface IAdapter
    {
        void AddPolicy(string sec, string ptype, IList<string> rule);

        void LoadPolicy(Model model);

        void RemoveFilteredPolicy(string sec, string ptype, int fieldIndex, params string[] fieldValues);

        void RemovePolicy(string sec, string ptype, IList<string> rule);

        void SavePolicy(Model model);
    }
}