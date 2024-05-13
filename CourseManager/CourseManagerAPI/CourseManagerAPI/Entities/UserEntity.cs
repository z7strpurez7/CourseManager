using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace CourseManagerAPI.Entities
{
    public class UserEntity : IdentityUser
    {
      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<Enrollment>? Enrollments { get; set; }
        public virtual ICollection<TeachingRequest>? TeachingRequests { get; set; }

        [NotMapped]
        public IList<string> Roles { get; set; }
    }
}
