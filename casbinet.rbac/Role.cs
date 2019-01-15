namespace casbinet.rbac
{
    using System.Collections.Generic;

    public class Role
    {
        private string name;

        private List<Role> roles = new List<Role>();

        public Role(string name)
        {
            this.name = name;
        }

        public void AddRole(Role role)
        {
            foreach (Role role in roles)
            {
                
            }
        }
    }
}
