namespace casbinet.Rbac
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    using casbinet.Util;

    public class DefaultRoleManager : IRoleManager
    {
        private string user;

        private string firstRole;

        private string secondRole;

        private string domain;

        private Dictionary<string, Role> allRoles;

        private int maxHierarchyLevel;

        public DefaultRoleManager(int maxHierarchyLevel)
        {
            this.maxHierarchyLevel = maxHierarchyLevel;
        }

        private bool HasRole(string roleName)
        {
            return this.allRoles.ContainsKey(roleName);
        }

        private Role CreateRole(string name)
        {
            if (this.HasRole(name))
            {
                this.allRoles.TryGetValue(name, out Role role);

                if (role != null)
                {
                    return role;
                }
            }

            Role newRole = new Role(name);
            this.allRoles.Add(name, newRole);
            return newRole;
        }

        public string User
        {
            get => this.user;

            set
            {
                this.user = value;
            }
        }

        public string FirstRole
        {
            get => this.firstRole;

            set
            {
                this.firstRole = value;
            }
        }

        public string SecondRole
        {
            get => this.secondRole;

            set
            {
                this.secondRole = value;
            }
        }

        public string Domain
        {
            get => this.domain;

            set
            {
                this.domain = value;
            }
        }

        public void Clear()
        {
            this.allRoles.Clear();
        }

        public void AddLink(string roleName1, string roleName2, params string[] domain)
        {
            roleName1 = domain + "::" + roleName1;
            roleName2 = domain + "::" + roleName2;

            Role role1 = this.CreateRole(roleName1);
            Role role2 = this.CreateRole(roleName2);
            role1.AddRole(role2);
        }

        public void DeleteLink(string roleName1, string roleName2, params string[] domain)
        {
            roleName1 = domain + "::" + roleName1;
            roleName2 = domain + "::" + roleName2;

            if (!this.HasRole(roleName1) || !this.HasRole(roleName2))
            {
                throw new Exception("error : roles or user does not exist");
            }

            Role role1 = this.CreateRole(roleName1);
            Role role2 = this.CreateRole(roleName2);
            role1.DeleteRole(role2);
        }

        public virtual bool HasLink(string roleName1, string roleName2, params string[] domain)
        {
            roleName1 = domain + "::" + roleName1;
            roleName2 = domain + "::" + roleName2;

            if (roleName1 == roleName2)
            {
                return true;
            }

            if (!this.HasRole(roleName1) || !this.HasRole(roleName2))
            {
                return false;
            }

            Role role1 = this.CreateRole(roleName1);
            return role1.HasRole(roleName2, this.maxHierarchyLevel);
        }

        public List<string> GetRoles(string roleName, params string[] domain)
        {
            roleName = domain + "::" + roleName;

            if (!this.HasRole(roleName))
            {
                throw new Exception("Error: name does not exists");
            }

            List<string> roles = this.CreateRole(roleName).GetRoles();
            roles.ForEach(
                delegate(string role)
                    {
                        role = role.Substring(domain.Length + 2, role.Length);
                    });

            return roles;
        }

        public List<string> GetUsers(string roleName)
        {
            if (!this.HasRole(roleName))
            {
                throw new Exception("Error: name does not exist");
            }

            List<string> names = new List<string>();
            foreach (Role role in this.allRoles.Values)
            {
                if (role.HasDirectRole(roleName))
                {
                    names.Add(role.Name);
                }
            }

            return names;
        }

        public virtual void PrintRoles()
        {
            foreach (Role role in this.allRoles.Values)
            {
                Util.LogPrint(role.ToString());
            }
        }
    }
}
