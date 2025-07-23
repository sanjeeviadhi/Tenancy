using Microsoft.Extensions.Configuration;
using Tenancy_Shared.TenantProvider;
using Tenancy_Shared.Enums;

namespace Tenancy_Shared.ConnectionManager
{
    public class ConnectionResolver : IConnectionResolver
    {
        private readonly IConfiguration _config;
        private readonly ITenantProvider _tenantProvider;

        public ConnectionResolver(IConfiguration config, ITenantProvider tenantProvider)
        {
            _config = config;
            _tenantProvider = tenantProvider;
        }

        public string GetCurrentConnectionString()
        {
            var tenantId = _tenantProvider.GetTenant();

            var tenantSection = _config.GetSection($"Tenants:{tenantId}");
            var mode = tenantSection.GetValue<string>("Mode");
            var key = tenantSection.GetValue<string>("ConnectionKey");

            if (string.IsNullOrWhiteSpace(mode) || string.IsNullOrWhiteSpace(key))
                throw new InvalidOperationException($"Missing mode or connection key for tenant '{tenantId}'");

            var connectionString = _config.GetConnectionString(key);

            return string.IsNullOrWhiteSpace(connectionString)
                ? throw new InvalidOperationException($"Connection string not found for key '{key}'")
                : connectionString;
        }

    }
}
