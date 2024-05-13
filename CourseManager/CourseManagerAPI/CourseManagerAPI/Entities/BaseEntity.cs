using Microsoft.AspNetCore.Authentication;

namespace CourseManagerAPI.Models
{
    public class BaseEntity<TID>
    {
        public TID Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool isActive { get; set; } = true;
        public bool isDeleted { get; set; } = false;
    }
}
