namespace casbinet.Model
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using casbinet.Rbac;
    using casbinet.Util;

    public class Assertion
    {
        private const char UNDERSCORE_SYMBOL = '_';

        private string key;

        private string value;

        private string[] tokens;

        private List<List<string>> policy = new List<List<string>>();

        private IRoleManager roleManager;

        public Assertion()
        {
        }

        public string Key
        {
            get => this.key;

            set
            {
                this.key = value;
            }
        }

        public string Value
        {
            get => this.value;

            set
            {
                this.value = value;
                this.Tokens = value.Split(", ");
            }
        }

        public string[] Tokens
        {
            get => this.tokens;

            set
            {
                this.tokens = value;
            }
        }

        public List<List<string>> Policy
        {
            get => this.policy;

            set
            {
                this.policy = value;
            }
        }

        public IRoleManager RoleManager
        {
            get => this.roleManager;

            set
            {
                this.roleManager = value;
            }
        }

        public void BuildRoleLinks(IRoleManager rm)
        {
            this.RoleManager = rm;
            int count = this.Value.Count(x => x == UNDERSCORE_SYMBOL);

            if (count < 2)
            {
                throw new Exception("The number of \"_\" in role definition should be at least 2");
            }

            foreach (List<string> rule in this.Policy)
            {
                if (rule.Count < count)
                {
                    throw new Exception("Grouping policy elements do not meet role definition");
                }

                switch (count)
                {
                    case 2:
                        rm.AddLink(rule[0], rule[1]);
                        break;

                    case 3:
                        rm.AddLink(rule[0], rule[1], rule[2]);
                        break;

                    case 4:
                        rm.AddLink(rule[0], rule[1], rule[2], rule[3]);
                        break;
                }
            }

            Util.LogPrint("Role links for: " + this.key);
            rm.PrintRoles();
        }
    }
}
