using Application_Tenancy.Models;
using Dapper;
using System.Data;
using Tenancy_Shared.Enums;
using Tenancy_Shared.TenantProvider;
using Tenancy_Shared.ConnectionManager;
using Npgsql;

namespace Application_Tenancy.Service
{
    public class UserService
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly IConnectionResolver _connectionResolver;

        public UserService(ITenantProvider tenantProvider, IConnectionResolver connectionResolver)
        {
            _tenantProvider = tenantProvider;
            _connectionResolver = connectionResolver;
        }

        public IEnumerable<User> GetUsers()
        {
            var tenantId = _tenantProvider.GetTenant();
            if (string.IsNullOrWhiteSpace(tenantId))
                throw new InvalidOperationException("TenantId is missing from context.");

            var mode = _tenantProvider.GetTenantMode();
            using var connection = GetConnection(mode);

            var sql = mode == UserRole.Shared
                ? "SELECT * FROM Users WHERE TenantId = @TenantId"
                : "SELECT * FROM Users";

            return connection.Query<User>(sql, new { TenantId = tenantId });
        }

        public void AddUser(User user)
        {
            user.TenantId = _tenantProvider.GetTenant();
            if (string.IsNullOrWhiteSpace(user.TenantId))
                throw new InvalidOperationException("TenantId is missing from context.");

            var mode = _tenantProvider.GetTenantMode();
            using var connection = GetConnection(mode);

            var sql = "INSERT INTO Users (Name, TenantId, Role) VALUES (@Name, @TenantId, @Role)";
            connection.Execute(sql, user);
        }

        private IDbConnection GetConnection(UserRole mode)
        {
            var connectionString = _connectionResolver.GetCurrentConnectionString();
            return new NpgsqlConnection(connectionString);
        }
    }
}
