namespace CourseManager.Services.Dtos
{
    public class ScheduleDto
    {
        public int Id { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int CourseId { get; set; }
    }


    public class ClassTimeDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool IsWeekly { get; set; }
    }


    public class ScheduleCreateDto
    {
        public List<ClassTimeDto> ClassTimes { get; set; }

    }

    public class ScheduleModifyDto
    {
        public List<ClassTimeDto> ClassTimes { get; set; }

    }
}