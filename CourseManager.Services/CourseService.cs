using AutoMapper;
using CourseManager.DataContext.Context;
using CourseManager.DataContext.Dtos;
using CourseManager.DataContext.Entities;
using CourseManager.DataContext.Enums;
using CourseManager.Services.Dtos;
using Microsoft.EntityFrameworkCore; // Ez KELL az Include és az Async metódusokhoz!
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManager.Services
{
    public interface ICourseService
    {
        Task<CourseDto> CreateCourseAsync(CreateCourseDto dto);
        Task<CourseDto> GetCourseByIdAsync(int id);
        Task<CourseDto> UpdateCourseAsync(int id, UpdateCourseDto dto);
        Task DeleteCourseAsync(int id);



        Task<List<UserDto>> GetStudentsAsync(int courseId);


        Task ChangeCourseAsync(CourseChangeDto dto);

        Task AddScheduleAsync(int courseId, ScheduleCreateDto dto);
        Task ModifyScheduleAsync(int courseId, ScheduleModifyDto dto);
    }

    public class CourseService : ICourseService
    {
        private readonly NeptunDbContext _context;
        private readonly IMapper _mapper;

        public CourseService(NeptunDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CourseDto> CreateCourseAsync(CreateCourseDto dto)
        {
            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Code == dto.SubjectCode);
            if (subject == null)
            {
                throw new Exception("Nincs ilyen tantárgy");
            }

            var course = _mapper.Map<Course>(dto);
            course.SubjectId = subject.Id;
            course.Subject = subject;
            course.Teachers = new List<User>();

            if (dto.TeacherIds != null && dto.TeacherIds.Any())
            {
                var teachers = await _context.Users
                    .Where(u => dto.TeacherIds.Contains(u.Id) && u.Role == UserRole.Teacher)
                    .ToListAsync();

                foreach (var teacher in teachers)
                {
                    course.Teachers.Add(teacher);
                }
            }

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            return _mapper.Map<CourseDto>(course);
        }

        public async Task<CourseDto> GetCourseByIdAsync(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Teachers)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) throw new Exception("Kurzus nem található!");

            return _mapper.Map<CourseDto>(course);
        }

        public async Task<CourseDto> UpdateCourseAsync(int id, UpdateCourseDto dto)
        {
            var course = await _context.Courses
                .Include(c => c.Subject)
                .Include(c => c.Teachers)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null) throw new Exception("Kurzus nem található!");

            _mapper.Map(dto, course);
            await _context.SaveChangesAsync();

            return _mapper.Map<CourseDto>(course);
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null) throw new Exception("Kurzus nem található!");
            await _context.SaveChangesAsync();
        }


        public async Task<List<UserDto>> GetStudentsAsync(int courseId)
        {
            var course = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null) throw new Exception("Kurzus nem található!");
            return _mapper.Map<List<UserDto>>(course.Students);
        }


        public async Task ChangeCourseAsync(CourseChangeDto dto)
        {
            var student = await _context.Users
                .Include(u => u.RegisteredCourses) 
                .FirstOrDefaultAsync(u => u.Id == dto.StudentId && u.Role == UserRole.Student);

            if (student == null)
                throw new Exception("Hallgató nem található.");

            var fromCourse = student.RegisteredCourses.FirstOrDefault(c => c.Id == dto.FromCourseId);
            if (fromCourse == null)
                throw new Exception("A hallgató nincs feljelentkezve a leadni kívánt kurzusra.");

            var toCourse = await _context.Courses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == dto.ToCourseId);

            if (toCourse == null)
                throw new Exception("A célkurzus nem található.");


            if (toCourse.Students.Count >= toCourse.maxStudents)
                throw new Exception("A célkurzus betelt, nincs szabad hely!");

            if (student.StudyType != toCourse.StudyType)
                throw new Exception("A képzési forma nem egyezik! Nappalis hallgató csak nappalis kurzust vehet fel.");

            if (fromCourse.SubjectId != toCourse.SubjectId)
                throw new Exception("Csak azonos tantárgyhoz tartozó kurzusok között lehet átjelentkezni!");

            if (student.RegisteredCourses.Any(c => c.Id == dto.ToCourseId))
                throw new Exception("A hallgató már fel van véve a célkurzusra.");


            student.RegisteredCourses.Remove(fromCourse);

            student.RegisteredCourses.Add(toCourse);

            await _context.SaveChangesAsync();
        }

        public async Task AddScheduleAsync(int courseId, ScheduleCreateDto dto)
        {
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null) throw new Exception("Kurzus nem található!");
            var schedule = _mapper.Map<Schedule>(dto);
            schedule.CourseId = courseId;
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
        }



        public async Task ModifyScheduleAsync(int courseId, ScheduleModifyDto dto)
        {
            var schedules = await _context.Schedules.Where(s => s.CourseId == courseId).ToListAsync();
            if (schedules == null || !schedules.Any()) throw new Exception("Órarend nem található!");
             _mapper.Map(dto, schedules); 
            await _context.SaveChangesAsync();
        }
    }
}