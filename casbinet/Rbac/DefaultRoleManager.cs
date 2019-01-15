namespace casbinet.rbac
{
    using System.Collections.Generic;

    public class DefaultRoleManager : IRoleManager
    {
        private string user;

        private string firstRole;

        private string secondRole;

        private string[] domain;

        private Dictionary<string, >

        public DefaultRoleManager(int maxHierarchyLevel)
        {

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

        public string[] Domain
        {
            get => this.domain;

            set
            {
                this.domain = value;
            }
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public void AddLink(string roleOrUser, string role, params object[] domain)
        {
            throw new System.NotImplementedException();
        }

        public bool HasLink(string roleOrUser, string role, params object[] domain)
        {
            throw new System.NotImplementedException();
        }

        public List<string> GetRoles(string roleOrUser, params object[] domain)
        {
            throw new System.NotImplementedException();
        }

        public List<string> GetUsers(string role)
        {
            throw new System.NotImplementedException();
        }

        public void PrintRoles()
        {
            throw new System.NotImplementedException();
        }
    }
}
