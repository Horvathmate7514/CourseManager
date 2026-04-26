
using CourseManager.DataContext.Context;
using CourseManager.DataContext.Entities;
using CourseManager.DataContext.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManager.DataContext.Context.Seeders
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(NeptunDbContext context)
        {
            var random = new Random();

            if (await context.Users.CountAsync() < 30)
            {
                var users = new List<User>();

                
                users.Add(new User { Name = "Adminisztrátor", Email = "admin@neptun.hu", Password = "password123", Role = UserRole.Administrator, IsActive = true, StudyType = StudyType.FullTime });

                // 5 Tanár
                for (int i = 1; i <= 5; i++)
                {
                    users.Add(new User { Name = $"Oktató {i}", Email = $"teacher{i}@neptun.hu", Password = "password123", Role = UserRole.Teacher, IsActive = true, StudyType = StudyType.FullTime });
                }

          
                for (int i = 1; i <= 25; i++)
                {
                    var studyType = (i % 4 == 0) ? StudyType.PartTime : StudyType.FullTime; // Minden negyedik levelezős
                    users.Add(new User { Name = $"Hallgató {i}", Email = $"student{i}@neptun.hu", Password = "password123", Role = UserRole.Student, IsActive = true, StudyType = studyType });
                }

                context.Users.AddRange(users);
                await context.SaveChangesAsync();
            }

            // ==========================================
            // 2. TANTÁRGYAK (Subjects) GENERÁLÁSA
            // ==========================================
            if (await context.Subjects.CountAsync() < 10)
            {
                var subjects = new List<Subject>();
                for (int i = 1; i <= 15; i++)
                {
                    subjects.Add(new Subject { Name = $"Dummy Tantárgy {i}", Code = $"SUBJ-{i * 100}", Credit = random.Next(2, 6), isActive = true });
                }
                context.Subjects.AddRange(subjects);
                await context.SaveChangesAsync();
            }

            // ==========================================
            // 3. KURZUSOK (Courses) GENERÁLÁSA
            // ==========================================
            if (await context.Courses.CountAsync() < 30)
            {
                var courses = new List<Course>();
                var allSubjects = await context.Subjects.ToListAsync();
                var allTeachers = await context.Users.Where(u => u.Role == UserRole.Teacher).ToListAsync();

                for (int i = 1; i <= 30; i++)
                {
                    var subj = allSubjects[random.Next(allSubjects.Count)];
                    var type = (CourseType)random.Next(0, 3); // 0=Theory, 1=Practice, 2=Lab
                    var studyType = (i % 3 == 0) ? StudyType.PartTime : StudyType.FullTime;

                    courses.Add(new Course
                    {
                        CourseCode = $"{subj.Code}-C{i}",
                        Semester = "2025/2026/1", 
                        maxStudents = random.Next(10, 30),
                        SubjectId = subj.Id,
                        CourseType = type,      
                        StudyType = studyType,
                        Hours = random.Next(2, 5),
                        IsWeeklyHours = true,
                        // Véletlenszerű tanár hozzárendelése
                        Teachers = new List<User> { allTeachers[random.Next(allTeachers.Count)] }
                    });
                }
                context.Courses.AddRange(courses);
                await context.SaveChangesAsync();
            }

            // ==========================================
            // 4. HALLGATÓK FELIRATKOZTATÁSA (RegisteredCourses)
            // ==========================================
            var allStudents = await context.Users.Include(u => u.RegisteredCourses).Where(u => u.Role == UserRole.Student).ToListAsync();
            var allCourses = await context.Courses.Include(c => c.Students).ToListAsync();

            bool enrollmentsAdded = false;
            foreach (var student in allStudents)
            {
                if (student.RegisteredCourses.Count > 0) continue; // Ha már van kurzusa, békén hagyjuk

                // Kiválasztunk 3-5 random kurzust, ami passzol a diák tagozatához (Nappali/Levelező)
                var suitableCourses = allCourses
                    .Where(c => c.StudyType == student.StudyType && c.Students.Count < c.maxStudents)
                    .OrderBy(x => random.Next())
                    .Take(random.Next(3, 6))
                    .ToList();

                foreach (var c in suitableCourses)
                {
                    student.RegisteredCourses.Add(c);
                    enrollmentsAdded = true;
                }
            }
            if (enrollmentsAdded) await context.SaveChangesAsync();

            // ==========================================
            // 5. ÓRARENDEK (Schedules) GENERÁLÁSA
            // ==========================================
            if (await context.Schedules.CountAsync() < 30)
            {
                var schedules = new List<Schedule>();
                var now = DateTime.Now;

                // Minden kurzushoz csinálunk pár beosztást
                foreach (var course in allCourses)
                {
                    var start = now.AddDays(random.Next(1, 14)).AddHours(random.Next(8, 16)); // Random nap, random óra 8 és 16 között
                    schedules.Add(new Schedule
                    {
                        CourseId = course.Id,
                        StartTime = start,
                        EndTime = start.AddMinutes(90) // Másfél órás órák
                    });
                }
                context.Schedules.AddRange(schedules);
                await context.SaveChangesAsync();
            }
        }
    }
}