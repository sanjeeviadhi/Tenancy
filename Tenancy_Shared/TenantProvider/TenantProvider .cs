using Microsoft.Extensions.Configuration;
using Tenancy_Shared.Enums;

namespace Tenancy_Shared.TenantProvider
{
    public class TenantProvider : ITenantProvider
    {
        private string _tenantId;
        private UserRole _mode;
        private readonly IConfiguration _config;

        public TenantProvider(IConfiguration config)
        {
            _config = config;
        }

        public void SetTenant(string tenantId)
        {
            _tenantId = tenantId;
            var modeSetting = _config.GetValue<string>($"Tenants:{tenantId}:Mode");

            _mode = Enum.TryParse<UserRole>(modeSetting, ignoreCase: true, out var result)
                       ? result
                       : UserRole.Shared;
        }

        public string GetTenant() => _tenantId;
        public UserRole GetTenantMode() => _mode;

        public string GetConnectionString()
        {
            return _config[$"Tenants:{_tenantId}:ConnectionString"];
        }
    }
}
