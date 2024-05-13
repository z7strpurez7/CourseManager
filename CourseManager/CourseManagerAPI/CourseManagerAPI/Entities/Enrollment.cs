namespace CourseManagerAPI.Entities
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public string UserId { get; set; }
        public UserEntity User { get; set; }

        public int CourseId { get; set; }
        public CourseEntity Course {get;set;}

        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; }  //ACTIVE, COMPLETED, CANCELLED
    }
}
