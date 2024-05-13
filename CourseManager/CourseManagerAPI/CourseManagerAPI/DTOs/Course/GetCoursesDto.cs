using CourseManagerAPI.Entities;

namespace CourseManagerAPI.DTOs.Course
{
    public class GetCoursesDto
    {
        public string CourseName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string Address { get; set; }
        public int MaxParticipants { get; set; }
        public int? Enrollments { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public string TeacherName { get; set; }
    }
}
