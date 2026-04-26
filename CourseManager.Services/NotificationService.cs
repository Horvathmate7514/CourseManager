using CourseManager.DataContext.Context;
using CourseManager.DataContext.Entities;
using CourseManager.Services.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseManager.Services
{
    public interface INotificationService
    {
        Task<List<NotificationDto>> GetNotificationsAsync(int? userId, int? courseId);
    }

    public class NotificationService : INotificationService
    {
        private readonly NeptunDbContext _context;

        public NotificationService(NeptunDbContext context)
        {
            _context = context;
        }

        public async Task<List<NotificationDto>> GetNotificationsAsync(int? userId, int? courseId)
        {
            var query = _context.Notifications.AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(n => n.UserId == userId.Value);
            }

            if (courseId.HasValue)
            {
                query = query.Where(n => n.CourseId == courseId.Value);
            }

            return await query
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    UserId = n.UserId,
                    CourseId = n.CourseId,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt
                })
                .ToListAsync();
        }
    }
}