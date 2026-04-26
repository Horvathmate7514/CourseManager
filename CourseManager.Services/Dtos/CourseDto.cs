using CourseManager.DataContext.Entities;
using CourseManager.DataContext.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Services.Dtos
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string CourseCode { get; set; }
        public string Semester { get; set; }
        public int maxStudents { get; set; }
        public List<int> TeacherIds { get; set; }

        public CourseType CourseType { get; set; }

        public StudyType StudyType { get; set; }

        public int Hours { get; set; }


        public bool IsWeeklyHours { get; set; }


        public int SubjectId { get; set; }

        public string SubjectCode { get; set; }



    }


    public class CreateCourseDto
    {
        public string CourseCode { get; set; }

        public string SubjectCode { get; set; }
        public string Semester { get; set; }
        public int maxStudents { get; set; }
        public List<int> TeacherIds { get; set; }
        public CourseType CourseType { get; set; }
        public StudyType StudyType { get; set; }
        public int Hours { get; set; }

        public bool IsWeeklyHours { get; set; }
    }

    public class UpdateCourseDto
    {
        public int MaxStudents { get; set; }
        public int Hours { get; set; }
        public bool IsWeeklyHours { get; set; }
    }

    public class CourseRegisterDto
    {
        public int StudentId { get; set; }

        public List<int> CourseIds { get; set; }

    }

    public class CourseUnregisterDto
    {
        public int StudentId { get; set; }
        public string Semester { get; set; }
    }


    public class CourseChangeDto
    {
        public int StudentId { get; set; }
        public int FromCourseId { get; set; }
        public int ToCourseId { get; set; }
    }
}
