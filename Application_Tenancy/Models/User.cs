using Tenancy_Shared.Enums;

namespace Application_Tenancy.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? TenantId { get; set; }
        public UserRole Role { get; set; }
    }
}
