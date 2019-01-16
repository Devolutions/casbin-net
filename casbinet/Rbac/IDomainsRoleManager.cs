namespace casbinet.Rbac
{

    public interface IDomainsRoleManager : IRoleManager
    {
        void AddLink(string roleOrUser, string role, params object[] domain);
    }
}
