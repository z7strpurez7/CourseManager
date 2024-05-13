using CourseManagerAPI.Context;
using CourseManagerAPI.DTOs.Log;
using CourseManagerAPI.Entities;
using CourseManagerAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CourseManagerAPI.Services
{

    public class LogService : ILogService
    {
        private readonly courseDbContext _context;

        public LogService(courseDbContext context)
        {
            _context = context;
        }

        public async Task SaveNewLog(string UserName, string Description)
        {

            
                var newLog = new Log()
                {
                    UserName = UserName,
                    Description = Description
                };
                await _context.Logs.AddAsync(newLog);
                await _context.SaveChangesAsync();
            
         
        }
        public async Task<IEnumerable<GetLogDto>> GetLogsAsync()
        {
          
                var logs = await _context.Logs.Select(q => new GetLogDto
                {
                    CreatedAt = q.CreatedAt,
                    UserName = q.UserName,
                    Description = q.Description
                }).OrderByDescending(q => q.CreatedAt).ToListAsync();
                return logs;
           
        }

        public async Task<IEnumerable<GetLogDto>> GetMyLogsAsync(ClaimsPrincipal User)
        {
         
                var logs = await _context.Logs
                .Where(q => q.UserName == User.Identity.Name)
                .Select(q => new GetLogDto
                {
                    CreatedAt = q.CreatedAt,
                    UserName = q.UserName,
                    Description = q.Description
                }).OrderByDescending(q => q.CreatedAt).ToListAsync();

                return logs;
           
        
        }


    }
}
