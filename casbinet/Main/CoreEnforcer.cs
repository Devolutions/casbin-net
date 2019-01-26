namespace casbinet.Main
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;

    using casbinet.Effect;
    using casbinet.Model;
    using casbinet.persist;
    using casbinet.Rbac;
    using casbinet.Util;

    public class CoreEnforcer
    {
        private IEffector eft;

        private bool enabled;

        private Model model;

        private IRoleManager rm;

        private Watcher watcher;

        public IAdapter Adapter { get; set; }

        public bool AutoBuildRoleLinks { get; set; }

        public bool AutoSave { get; set; }

        public FunctionMap Fm { get; set; }

        public Model Model
        {
            get => this.model;

            set
            {
                this.model = value;
                this.Fm = FunctionMap.LoadFunctionMap();
            }
        }

        public string ModelPath { get; set; }

        public Watcher Watcher
        {
            get => this.watcher;

            set
            {
                this.watcher = value;

                this.watcher.OnUpdate += this.Watcher_OnUpdate;
            }
        }

        public void Initialize()
        {
            this.rm = new DefaultRoleManager(10);
            this.eft = new DefaultEffector();
            this.enabled = true;
            this.AutoSave = true;
            this.AutoBuildRoleLinks = true;
        }

        public static Model NewModel()
        {
            return new Model();
        }

        public static Model NewModelFromText(string text)
        {
            Model m = new Model();
            m.LoadModelFromText(text);
            return m;
        }

        public static Model NewModelFromPath(string modelPath)
        {
            Model m = new Model();
            if (!string.IsNullOrEmpty(modelPath))
            {
                m.LoadModelFromText(modelPath);
            }

            return m;
        }

        public void LoadModel()
        {
            this.model = NewModel();
            this.model.LoadModel(this.ModelPath);
            this.model.PrintModel();
            this.Fm = FunctionMap.LoadFunctionMap();
        }

        public void ClearPolicy()
        {
            this.Model.ClearPolicy();
        }

        public void LoadPolicy()
        {
            this.Model.ClearPolicy();
            this.Adapter.LoadPolicy(this.Model);
            this.Model.PrintPolicy();

            if (this.AutoBuildRoleLinks)
            {
                this.BuildRoleLinks();
            }
        }

        public void LoadFilteredPolicy(object filter)
        {
            return;
        }

        public bool isFiltered()
        {
            return false;
        }

        public void SavePolicy()
        {
            if (this.isFiltered())
            {
                throw new Exception("Cannot save a filtered policy");
            }

            this.Adapter.SavePolicy(this.Model);
            if (this.Watcher != null)
            {
                this.Watcher.Update(null, null);
            }
        }

        public void EnableEnforce(bool enable)
        {
            this.enabled = enable;
        }

        public void EnableLog(bool enable)
        {
            Util.enableLog = enable;
        }

        public void EnableAutoSave(bool autoSave)
        {
            this.AutoSave = autoSave;
        }

        public void EnableAutoBuildRoleLinks(bool autoBuildRoleLinks)
        {
            this.AutoBuildRoleLinks = autoBuildRoleLinks;
        }

        public void BuildRoleLinks()
        {
            this.rm.Clear();
            this.Model.BuildRoleLinks(this.rm);
        }

        public bool Enforce(params object[] rVals)
        {
            if (!this.enabled)
            {
                return true;
            }

            Dictionary<string, FunctionMap.KeyMatchFunction> functions = new Dictionary<string, FunctionMap.KeyMatchFunction>();

            foreach (KeyValuePair<string, FunctionMap.KeyMatchFunction> entry in this.Fm.fm)
            {
                functions.Add(entry.Key, entry.Value);
            }

            if (this.Model.model.TryGetValue("g", out Dictionary<string, Assertion> values))
            {
                foreach (KeyValuePair<string, Assertion> value in values)
                {
                    functions.Add(value.Key, BuiltInOperator.GFunctionFactory(this.rm, value.Key));
                }
            }

            //todo : Added evaluation of function to compule with expString
            //string expString = this.model.model["m"]["m"].Value;

            Effect[] policyEffects;
            float[] matcherResults;
            int policyCount;

            if ((policyCount = this.model.model["p"]["p"].Policy.Count) != 0)
            {
                policyEffects = new Effect[policyCount];
                matcherResults = new float[policyCount];

                for (int i = 0; i < policyCount; i++)
                {
                    List<string> pVals = this.model.model["p"]["p"].Policy[i];
                    Dictionary<string, object> parameters = new Dictionary<string, object>();

                    foreach (var result in this.model.model["r"]["r"].Tokens.Select((s, index) => new{s, index}))
                    {
                        parameters.Add(result.s, rVals[result.index]);
                    }

                    foreach (var result in this.model.model["p"]["p"].Tokens.Select((s, index) => new {s, index}))
                    {
                        parameters.Add(result.s, pVals[result.index]);
                    }

                    var function = functions["m"];

                    //todo: switch IRoleManager and params in delegate
                    object functionResult = function.Invoke(null, parameters.ToArray()).Item1;

                    if (functionResult is bool boolResult)
                    {

                    }
                    else if (functionResult is float floatResult)
                    {
                        
                    }
                    else
                    {

                    }
                }
            }

        }

        private void Watcher_OnUpdate(string line, Model model)
        {
            this.LoadPolicy();
        }
    }
}