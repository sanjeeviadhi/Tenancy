using Tenancy_Shared.TenantProvider;

namespace Tenancy_Services.C_MiddleWare
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
        {
            var tenantId = context.Request.Headers["X-Tenant-ID"].FirstOrDefault();
            if (!string.IsNullOrEmpty(tenantId))
            {
                    tenantProvider.SetTenant(tenantId);
            }

            await _next(context);
        }
    }
}
