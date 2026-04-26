using CourseManager.DataContext.Enums;

namespace CourseManager.DataContext.Entities
{
    public class Course
    {

        public int Id { get; set; }

        public string CourseCode { get; set; }
        public string Semester { get; set; }

        public int maxStudents { get; set; }

        public CourseType CourseType { get; set; }  

        public StudyType StudyType { get; set; } 

        public int Hours { get; set; }

        public bool IsWeeklyHours { get; set; }


        public int SubjectId { get; set; }
        public Subject Subject { get; set; }


        public ICollection<User> Teachers { get; set; } = new List<User>();
        public ICollection<User> Students { get; set; } = new List<User>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}