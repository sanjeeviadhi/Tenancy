namespace Tenancy_Shared.ConnectionManager
{
    public interface IConnectionResolver
    {
        string GetCurrentConnectionString();
    }
}
