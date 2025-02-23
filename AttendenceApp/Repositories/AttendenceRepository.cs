using AttendenceApp.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace AttendenceApp.Repositories
{
    public interface IAttendanceRepository : IRepository<Attendance>
    {
        Task<IEnumerable<Attendance>> GetAttendanceByEventIdAsync(int eventId);
    }

    public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
    {
        private readonly MyAppContext _context;

        public AttendanceRepository(MyAppContext context) : base(context) { _context = context; }

        public async Task<IEnumerable<Attendance>> GetAttendanceByEventIdAsync(int eventId) =>
            await _context.Attendances.Where(a => a.EventId == eventId).ToListAsync();
    }

}
