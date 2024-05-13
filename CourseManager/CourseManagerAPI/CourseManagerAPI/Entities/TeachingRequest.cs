using CourseManagerAPI.Constants;
using CourseManagerAPI.Models;

namespace CourseManagerAPI.Entities
{
    public class TeachingRequest : BaseEntity<int>
    {
        public string TeacherName { get; set; }
        public string TeacherId { get; set; }
        public UserEntity Teacher { get; set; }

        public int CourseId { get; set; }
        public virtual CourseEntity Course { get; set; }

        public string RequestStatus { get; set; } = StaticRequestStatus.PENDING; //pending, approved, rejected
    }
}
