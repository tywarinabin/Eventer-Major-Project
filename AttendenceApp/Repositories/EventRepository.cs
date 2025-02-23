
using AttendenceApp.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace AttendenceApp.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Task<IEnumerable<Event>> GetActiveEventsAsync();
    }
    public class EventRepository : Repository<Event>, IEventRepository
    {
        private readonly MyAppContext _context;

        public EventRepository(MyAppContext context) : base(context) { _context = context; }

        public async Task<IEnumerable<Event>> GetActiveEventsAsync() =>
            await _context.Events.Where(e => e.Status == "active").ToListAsync();

    }
}
