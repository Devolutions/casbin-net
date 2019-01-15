namespace casbinet.rbac
{
    using System.Collections.Generic;

    public interface IRoleManager
    {
        string User { get; set; }

        string FirstRole { get; set; }

        string SecondRole { get; set; }

        string[] Domain { get; set; }

        void Clear();

        void AddLink(string roleOrUser, string role, params object[] domain);

        bool HasLink(string roleOrUser, string role, params object[] domain);

        List<string> GetRoles(string roleOrUser, params object[] domain);

        List<string> GetUsers(string role);

        void PrintRoles();
    }
}
