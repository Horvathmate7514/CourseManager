using System;

namespace CourseManager.Services.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}