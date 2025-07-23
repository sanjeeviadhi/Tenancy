using Tenancy_Shared.Enums;

namespace Tenancy_Shared.TenantProvider
{
    public interface ITenantProvider
    {
        void SetTenant(string tenantId);
        string GetTenant();
        UserRole GetTenantMode();
        string GetConnectionString();
    }
}