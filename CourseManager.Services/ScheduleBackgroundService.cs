using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CourseManager.DataContext.Context;
using CourseManager.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CourseManager.Services
{
    public class ScheduleBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ScheduleBackgroundService> _logger;

        public ScheduleBackgroundService(IServiceProvider serviceProvider, ILogger<ScheduleBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Schedule Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessSchedules(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Hiba történt a ScheduleBackgroundService futása közben!");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task ProcessSchedules(CancellationToken stoppingToken)
        {
            // Új Scope nyitása, mivel a DbContext "Scoped", a BackgroundService pedig "Singleton"
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<NeptunDbContext>();

            var now = DateTime.Now;

            //  Pontosan a 29 és 30 perc közötti ablakot nézzük, 
            // hogy ne generáljunk percenként újra értesítést ugyanarra az órára.
            var targetTimeStart = now.AddMinutes(29);
            var targetTimeEnd = now.AddMinutes(30);

            var upcomingSchedules = await context.Schedules
                .Include(s => s.Course)
                    .ThenInclude(c => c.Students)
                .Include(s => s.Course)
                    .ThenInclude(c => c.Teachers)
                .Where(s => s.StartTime > targetTimeStart && s.StartTime <= targetTimeEnd)
                .ToListAsync(stoppingToken);

            foreach (var schedule in upcomingSchedules)
            {
                var message = $"Figyelem: A(z) {schedule.Course.CourseCode} kódú kurzus 30 perc múlva kezdõdik.";

                foreach (var student in schedule.Course.Students)
                {
                    var notification = new Notification
                    {
                        UserId = student.Id,
                        CourseId = schedule.CourseId,
                        Message = message,
                        CreatedAt = DateTime.Now
                    };
                    context.Notifications.Add(notification);
                    _logger.LogInformation($"Értesítés logolva a hallgatónak (ID: {student.Id}), kurzus: {schedule.CourseId}");
                }

                foreach (var teacher in schedule.Course.Teachers)
                {
                    var notification = new Notification
                    {
                        UserId = teacher.Id,
                        CourseId = schedule.CourseId,
                        Message = message,
                        CreatedAt = DateTime.Now
                    };
                    context.Notifications.Add(notification);
                    _logger.LogInformation($"Értesítés logolva az oktatónak (ID: {teacher.Id}), kurzus: {schedule.CourseId}");
                }
            }

            if (upcomingSchedules.Any())
            {
                await context.SaveChangesAsync(stoppingToken);
                _logger.LogInformation($"{upcomingSchedules.Count} db kurzushoz generálva értesítések.");
            }
        }
    }
}