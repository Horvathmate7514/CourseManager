using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.DataContext.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } 
        public int CourseId { get; set; }
        public Course Course { get; set; } 

        public string Message { get; set; } 
        public DateTime CreatedAt { get; set; } 
    }
}
