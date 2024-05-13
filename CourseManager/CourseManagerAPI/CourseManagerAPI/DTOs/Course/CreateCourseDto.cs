namespace CourseManagerAPI.DTOs.Course
{
    public class CreateCourseDto
    {
        public string CourseName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string Address { get; set; }
        public int MaxParticipants { get; set; }
    }
}
