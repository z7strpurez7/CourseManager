namespace CourseManagerAPI.DTOs.Course
{
    public class GetCourseTitlesDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public DateTime startDate { get; set; }
    }
}
