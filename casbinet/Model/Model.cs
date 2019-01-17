namespace casbinet.Model
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;

    using casbinet.Config;
    using casbinet.Util;

    public class Model : Policy
    {
        private static Dictionary<string, string> sectionNameMap;

        static Model()
        {
            sectionNameMap = new Dictionary<string, string>()
                {
                    {"r", "request_definition" },
                    {"p", "policy_definition" },
                    {"g", "role_definition" },
                    {"e", "policy_effect" },
                    {"m", "matchers" }
                };
        }

        public Model()
        {
            this.model = new Dictionary<string, Dictionary<string, Assertion>>();
        }

        private bool LoadAssertion(Model model, Config cfg, string sec, string key)
        {
            sectionNameMap.TryGetValue(sec, out string value);
            value = cfg.GetString(value + "::" + key);
            return model.AddDef(sec, key, value);
        }

        public bool AddDef(string sec, string key, string value)
        {
            if (value == string.Empty)
            {
                return false;
            }

            Assertion ast = new Assertion() { Key = key, Value = value };

            if(sec.Equals("r") || sec.Equals("p"))
            {
                foreach (var token in ast.Tokens.Select((s, i) => new {i, s}))
                {
                    ast.Tokens[token.i] = key + "_" + ast.Tokens[token.i];
                }
            }
            else
            {
                ast.Value = Util.RemoveComments(Util.EscapeAssertion(ast.Value));
            }

            if (!this.model.ContainsKey(sec))
            {
                this.model.Add(sec, new Dictionary<string, Assertion>());
            }

            if (this.model.TryGetValue(sec, out Dictionary<string, Assertion> assertionDictionary))
            {
                assertionDictionary.Add(key, ast);
            }

            return true;
        }

        private string GetKeySuffix(int i)
        {
            if (i == 1)
            {
                return "";
            }

            return i.ToString();
        }

        private void LoadSection(Model model, Config cfg, string sec)
        {
            int i = 0;
            bool assertionLoaded = true;
            while (assertionLoaded)
            {
                assertionLoaded = this.LoadAssertion(model, cfg, sec, sec + this.GetKeySuffix(++i));
            }
        }

        public void LoadModel(string path)
        {
            Config cfg = Config.NewConfig(path);
            this.LoadSectionForConfig(cfg);
        }

        public void LoadModelFromText(string text)
        {
            Config cfg = Config.NewConfigFromText(text);
            this.LoadSectionForConfig(cfg);
        }

        private void LoadSectionForConfig(Config cfg)
        {
            this.LoadSection(this, cfg, "r");
            this.LoadSection(this, cfg, "p");
            this.LoadSection(this, cfg, "e");
            this.LoadSection(this, cfg, "m");
            this.LoadSection(this, cfg, "g");
        }

        public void PrintModel()
        {
            Util.LogPrint("Model:");
            foreach (string key in this.model.Keys)
            {
                this.model.TryGetValue(key, out Dictionary<string, Assertion> assertionDictionary);
                foreach(string assertionKey in assertionDictionary.Keys)
                {
                    assertionDictionary.TryGetValue(assertionKey, out Assertion assertion);
                    Util.LogPrintf("%s.%s: %s", key, assertionKey, assertion.Value);
                }
            }
        }
    }
}
