using AutoMapper;
using CourseManager.DataContext.Context;
using CourseManager.DataContext.Dtos;
using CourseManager.DataContext.Enums;
using CourseManager.Services.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Services
{

    public interface ISubjectService
    {
        Task<List<SubjectDto>> GetAllSubjectsAsync();
        Task<SubjectDto> GetSubjectByIdAsync(int id);

        Task<SubjectDto> CreateSubjectAsync(CreateSubjectDto dto);

        Task<SubjectDto> UpdateSubjectAsync(int id, UpdateSubjectDto dto);

        Task DeactivateSubjectAsync(int id);
        Task ReactivateSubjectAsync(int id);


        Task RegisterForCourseAsync(int subjectId, CourseRegisterDto dto);
        Task UnRegisterFromSubjectAsync(int subjectId, CourseUnregisterDto dto);

        Task<List<UserDto>> GetStudentsForSubjectAsync(int subjectId, string semester);


    }
    public class SubjectService : ISubjectService
    {
        private readonly NeptunDbContext _context;
        private readonly IMapper _mapper;

        public SubjectService(NeptunDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<SubjectDto>> GetAllSubjectsAsync()
        {
            var subjects = await _context.Subjects.ToListAsync();
            return _mapper.Map<List<SubjectDto>>(subjects);
        }

        public async Task<SubjectDto> GetSubjectByIdAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) throw new Exception("Tantárgy nem található!");
            return _mapper.Map<SubjectDto>(subject);
        }


        public async Task<SubjectDto> CreateSubjectAsync(CreateSubjectDto dto)
        {
            var subject = _mapper.Map<DataContext.Entities.Subject>(dto);
            subject.isActive = true;
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            return _mapper.Map<SubjectDto>(subject);
        }


        public async Task<SubjectDto> UpdateSubjectAsync(int id, UpdateSubjectDto dto)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) throw new Exception("Tantárgy nem található!");
            if (!subject.isActive) throw new Exception("Inaktív tantárgy nem szerkeszthető!");
            _mapper.Map(dto, subject);
            await _context.SaveChangesAsync();
            return _mapper.Map<SubjectDto>(subject);
        }


        public async Task DeactivateSubjectAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) throw new Exception("Tantárgy nem található!");
            subject.isActive = false;
            await _context.SaveChangesAsync();
        }


        public async Task ReactivateSubjectAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) throw new Exception("Tantárgy nem található!");
            subject.isActive = true;
            await _context.SaveChangesAsync();
        }


        public async Task RegisterForCourseAsync(int subjectId, CourseRegisterDto dto)
        {
            var student = await _context.Users
                .Include(u => u.RegisteredCourses)
                .FirstOrDefaultAsync(u => u.Id == dto.StudentId && u.Role == UserRole.Student && u.IsActive);

            if (student == null) throw new Exception("Aktív hallgató nem található!");

            var courses = await _context.Courses
                .Include(c => c.Students)
                .Where(c => dto.CourseIds.Contains(c.Id) && c.SubjectId == subjectId)
                .ToListAsync();

            if (courses.Count != dto.CourseIds.Count)
                throw new Exception("Nincs ilyen kurzus!");

            foreach (var course in courses)
            {
                if (course.Students.Count >= course.maxStudents)
                    throw new Exception($"A(z) {course.CourseCode} kurzus megtelt!");

                if (course.StudyType != student.StudyType)
                    throw new Exception($"A hallgató tagozata ({student.StudyType}) nem egyezik a kurzus tagozatával ({course.StudyType})!");

                if (student.RegisteredCourses.Any(c => c.Id == course.Id))
                    throw new Exception($"A hallgató már felvette a(z) {course.CourseCode} kurzust!");
            }

            foreach (var course in courses)
            {
                student.RegisteredCourses.Add(course);
            }

            await _context.SaveChangesAsync();
        }
        public async Task UnRegisterFromSubjectAsync(int subjectId, CourseUnregisterDto dto)
        {
            var student = await _context.Users
                .Include(u => u.RegisteredCourses)
                .FirstOrDefaultAsync(u => u.Id == dto.StudentId && u.Role == UserRole.Student && u.IsActive);

            if (student == null) throw new Exception("Aktív hallgató nem található!");

            var coursesToRemove = student.RegisteredCourses.Where(c => c.SubjectId == subjectId).ToList();

            if (!coursesToRemove.Any())
                throw new Exception("A hallgató nem vette fel ezt a tárgyat, így leadni sem tudja!");

            foreach (var course in coursesToRemove)
            {
                student.RegisteredCourses.Remove(course);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<UserDto>> GetStudentsForSubjectAsync(int subjectId, string semester)
        {
            var courses = await _context.Courses
                .Include(c => c.Students)
                .Where(c => c.SubjectId == subjectId && c.Semester == semester)
                .ToListAsync();
            var students = courses.SelectMany(c => c.Students).Distinct().ToList();
            return _mapper.Map<List<UserDto>>(students);

        }

        
    }
}
