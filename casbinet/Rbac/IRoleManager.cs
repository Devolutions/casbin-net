namespace casbinet.Rbac
{
    using System.Collections.Generic;

    public interface IRoleManager
    {
        string User { get; set; }

        string FirstRole { get; set; }

        string SecondRole { get; set; }

        string Domain { get; set; }

        void Clear();

        void AddLink(string role1, string role2, params string[] domain);

        void DeleteLink(string role1, string role2, params string[] domain);

        bool HasLink(string role1, string role2, params string[] domain);

        List<string> GetRoles(string roleName, params string[] domain);

        List<string> GetUsers(string roleName);

        void PrintRoles();
    }
}
