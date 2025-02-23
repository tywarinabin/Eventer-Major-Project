using AttendenceApp.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace AttendenceApp.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee?> GetByEmployeeIdAsync(string employeeId);
    }

    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly MyAppContext _context;

        public EmployeeRepository(MyAppContext context) : base(context) { _context = context; }

        public async Task<Employee?> GetByEmployeeIdAsync(string employeeId) =>
            await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeID == employeeId);
    }
}
