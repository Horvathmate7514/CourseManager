namespace CourseManager.DataContext.Entities
{
    public class Schedule
    {

        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

  
        public bool IsWeekly { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;
    }


}