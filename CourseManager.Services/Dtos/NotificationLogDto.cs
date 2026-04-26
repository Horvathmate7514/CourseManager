namespace CourseManager.Services.Dtos
{
    public class NotificationLogDto
    {
        
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
    
    }
}