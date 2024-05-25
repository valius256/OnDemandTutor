using Microsoft.AspNetCore.Identity;

namespace OnDemandTutor.DataAccess.Models
{
    public class Role : IdentityRole<Guid>
    {
        public Role()
        {
        }

        public Role(string roleName) : base(roleName)
        {
        }

        public string DisplayName { get; set; }
        public Guid? LocationId { get; set; }
    }
}
