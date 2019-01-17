namespace casbinet.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Net;

    using casbinet.Rbac;
    using casbinet.Util;

    public class Policy
    {
        public Dictionary<string, Dictionary<string, Assertion>> model;

        public void BuildRoleLinks(IRoleManager rm)
        {
            if (this.model.TryGetValue("g", out Dictionary<string, Assertion> assertionsDictionary))
            {
                foreach (Assertion ast in assertionsDictionary.Values)
                {
                    ast.BuildRoleLinks(rm);
                }
            }
        }

        public void PrintPolicy()
        {
            Util.LogPrint("Policy :");

            if (this.model.TryGetValue("p", out Dictionary<string, Assertion> pAssertionsDictionary))
            {
                this.Print(pAssertionsDictionary);
            }

            if (this.model.TryGetValue("g", out Dictionary<string, Assertion> gAssertionsDictionary))
            {
                this.Print(gAssertionsDictionary);
            }
        }

        private void Print(Dictionary<string, Assertion> assertionsDictionay)
        {
            foreach (string key in assertionsDictionay.Keys)
            {
                assertionsDictionay.TryGetValue(key, out Assertion ast);
                Util.LogPrint(key + ": " + ast.Value + ": " + ast.Policy);
            }
        }

        public void ClearPolicy()
        {
            if (this.model.TryGetValue("p", out Dictionary<string, Assertion> pAssertionsDictionary))
            {
                foreach (Assertion ast in pAssertionsDictionary.Values)
                {
                    ast.Policy.Clear();
                }
            }

            if (this.model.TryGetValue("g", out Dictionary<string, Assertion> gAssertionsDictionary))
            {
                foreach (Assertion ast in gAssertionsDictionary.Values)
                {
                    ast.Policy.Clear();
                }
            }
        }

        public List<List<string>> GetPolicy(string sec, string pType)
        {
            List<List<string>> policy = new List<List<string>>();

            if (this.model.TryGetValue(sec, out Dictionary<string, Assertion> secAssertionsDictionary))
            {
                secAssertionsDictionary.TryGetValue(pType, out Assertion ast);
                policy = ast.Policy;
            }

            return policy;
        }

        public List<List<string>> GetFilteredPolicy(string sec, string pType, int fieldIndex, params string[] fieldValues)
        {
            List<List<string>> res = this.GetPolicy(sec, pType);

            foreach (var rule in res.Select((list, i) => new {i, list}))
            {
                bool matched = true;
                foreach (string fieldValue in fieldValues)
                {
                    if (!fieldValue.Equals(string.Empty) && !rule.list[fieldIndex + rule.i].Equals(fieldValue))
                    {
                        matched = false;
                        break;
                    }
                }

                if (matched)
                {
                    res.Add(rule.list);
                }
            }

            return res;
        }

        public bool HasPolicy(string sec, string pType, List<string> rule)
        {
            foreach (List<string> r in this.GetPolicy(sec, pType))
            {
                if (Util.ArrayEquals(rule, r))
                {
                    return true;
                }
            }

            return false;
        }

        public bool AddPolicy(string sec, string pType, List<string> rule)
        {
            if (this.HasPolicy(sec, pType, rule))
            {
                return false;
            }

            this.GetPolicy(sec, pType).Add(rule);
            return true;
        }

        public bool RemovePolicy(string sec, string pType, List<string> rule)
        {
            List<List<string>> policy = this.GetPolicy(sec, pType);

            foreach (List<string> r in policy)
            {
                if (Util.ArrayEquals(rule, r))
                {
                    policy.Remove(r);
                    return true;
                }
            }

            return false;
        }

        public Assertion GetAssertion(string sec, string pType)
        {
            this.model.TryGetValue(sec, out Dictionary<string, Assertion> assertionsDictionary);
            assertionsDictionary.TryGetValue(pType, out Assertion assertion);
            return assertion;
        }

        public bool RemoveFilteredPolicy(string sec, string pType, int fieldIndex, params string[] fieldValues)
        {
            List<List<string>> tmp = new List<List<string>>();
            List<List<string>> policy = this.GetPolicy(sec, pType);
            bool res = false;

            foreach (List<string> rule in policy)
            {
                bool matched = true;
                foreach (var fieldValue in fieldValues.Select((s, i) => new {i, s}))
                {
                    if (!fieldValue.Equals(string.Empty) && !rule[fieldIndex + fieldValue.i].Equals(fieldValue))
                    {
                        matched = false;
                        break;
                    }
                }

                if (matched)
                {
                    res = true;
                }
                else
                {
                    tmp.Add(rule);
                }
            }

            this.GetAssertion(sec, pType).Policy = tmp;
            
            return res;
        }

        public List<string> GetValuesForFieldInPolicy(string sec, string pType, int fieldIndex)
        {
            List<string> values = new List<string>();

            foreach (List<string> rule in this.GetPolicy(sec, pType))
            {
                values.Add(rule[fieldIndex]);
            }

            return values;
        }
    }
}
