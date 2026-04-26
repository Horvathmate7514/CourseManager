using CourseManager.DataContext.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.DataContext.Entities
{
    public class User
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserRole Role { get; set; }

        public StudyType StudyType { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Course> RegisteredCourses { get; set; } = new List<Course>();
        public ICollection<Course> TaughtCourses { get; set; } = new List<Course>();

        public ICollection<Notification> Notifications { get; set; }

    }
}
