using System;
using System.Collections.Generic;
using System.Text;

namespace casbinet.Rbac
{
    public class GroupRoleManager : DefaultRoleManager
    {
        public GroupRoleManager(int maxHierarchyLevel) : base(maxHierarchyLevel)
        {
        }

        public override bool HasLink(string roleName1, string roleName2, string domain)
        {
            if (base.HasLink(roleName1, roleName2, domain))
            {
                return true;
            }

            try
            {
                List<string> roles = base.GetRoles(roleName1);
                List<string> groups = roles.Count > 0 ? base.GetRoles(roleName1) : new List<string>();
                foreach (string group in groups)
                {
                    if (this.HasLink(group, roleName2, domain))
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return false;
        }
    }
}
