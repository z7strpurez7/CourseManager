namespace CourseManagerAPI.DTOs.Request
{
    public class GetTeachingRequestsDto
    {
        public int Id { get; set; }
        public string SenderUserName { get; set; }
        public string CourseName { get; set; }
        public int CourseId { get; set; }
        public string RequestStatus { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
