namespace casbinet.Rbac
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Role
    {
        private string name;

        private Dictionary<string, Role> roles = new Dictionary<string, Role>();

        public Role(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get => this.name;

            set
            {
                this.name = value;
            }
        }

        public void AddRole(Role role)
        {
            if (this.roles.ContainsKey(role.name))
            {
                return;
            }

            this.roles.Add(role.name, role);
        }

        public void DeleteRole(Role role)
        {
            if (this.roles.ContainsKey(role.name))
            {
                this.roles.Remove(role.name);
            }
        }

        public bool HasRole(string roleName, int hierarchyLevel)
        {
            if (this.name == roleName)
            {
                return true;
            }

            if (hierarchyLevel <= 0)
            {
                return false;
            }

            foreach (Role role in this.roles.Values.ToArray())
            {
                if (role.HasRole(roleName, hierarchyLevel - 1))
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasDirectRole(string roleName)
        {
            foreach (Role role in this.roles.Values.ToArray())
            {
                if (role.name == roleName)
                {
                    return true;
                }
            }

            return false;
        }

        public List<string> GetRoles()
        {
            List<string> names = new List<string>();
            foreach (Role role in this.roles.Values.ToArray())
            {
                names.Add(role.name);
            }

            return names;
        }

        public override string ToString()
        {
            Role[] allRoles = this.roles.Values.ToArray();
            StringBuilder names = new StringBuilder(allRoles[0].name);
            for (int i = 0; i < allRoles.Length; i++)
            {
                if (i == 0)
                {
                    names.Append(allRoles[i].name);
                }
                else
                {
                    names.Append(", " + allRoles[i].name);
                }
            }

            return this.name + " < " + names;
        }
    }
}
