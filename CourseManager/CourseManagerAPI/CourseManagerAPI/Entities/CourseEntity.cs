using CourseManagerAPI.Models;

namespace CourseManagerAPI.Entities
{
    public class CourseEntity : BaseEntity<int>
    {
    
        public string CourseName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public String Address { get; set; }
        public int MaxParticipants { get; set; }
        public UserEntity? Teacher { get; set; }
        public ICollection<Enrollment>? Enrollments { get; set; }
    }
}
