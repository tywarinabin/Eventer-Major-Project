using AttendenceApp.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendenceApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly MyAppContext _context;

        public EmployeeController(MyAppContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return NotFound();
        }
        //public IActionResult OpenParticipation(Event )
        //{
        //    return View()
        //}
        [HttpGet]
        public async Task<IActionResult> OpenParticipation(int eventId)
        {
            var eventModel = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            if (eventModel == null)
            {
                return NotFound();
            }

            return View(eventModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OpenParticipation(int eventId, int employeeId)
        {
            //Console.WriteLine($"Received eventId: {eventId}, employeeId: {employeeId}");
            //TempData["ErrorMessage"] = $"Received eventId: {eventId}, employeeId: {employeeId}";

            
            var eventModel = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            if (eventModel == null)
            {
                TempData["ErrorMessage"] = "Event not found!";
                return RedirectToAction("Index", "Home"); 
            }
             if (employeeId <= 0 )
            {
                TempData["ErrorMessage"] = $"Employee not found with your Associated ID {employeeId}";
                return RedirectToAction("OpenParticipation", new { eventId }); 
            }
            if (eventModel.EndDate<=DateTime.UtcNow )
            {

                TempData["Success"]= "Event registration is already Closed. Thank You.";
              
                return RedirectToAction("success", "Employee"); 
            }

            var user = await _context.Employees.FirstOrDefaultAsync(u => u.EmployeeID == (employeeId).ToString());
            if (user == null)
            {
                TempData["ErrorMessage"] = $"Employee not found with your Associated ID {employeeId}";
                return RedirectToAction("OpenParticipation", new { eventId });//Query String
            }

            var existingAttendance = await _context.Attendances
                .AnyAsync(a => a.EventId == eventId && a.EmployeeId == user.Id);

            if (existingAttendance)
            {
                TempData["Name"] = user.Name;

                TempData["Success"] = $"This employee  {user.Name} is already registered for the event.";

                return RedirectToAction("success", "Employee");
               
            }

            var attendance = new Attendance
            {
                EventId = eventId,
                EmployeeId = user.Id,
                Timestamp = DateTime.UtcNow
            };

            try
            {
                _context.Attendances.Add(attendance);
                //_context.Events.Update()
                await _context.SaveChangesAsync();

                TempData["Success"] = "Registration successful!";
                TempData["Name"] = user.Name;
                return View("success", user);
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "Error saving registration. Please try again.";
            }
            return RedirectToAction("OpenParticipation", new { eventId }); // 🔹 Redirect instead of View
        }

        [HttpGet]
        public IActionResult GoToOpenParticipationEvent(string id,string title)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Invalid Event ID!";
                return RedirectToAction("EventList");
            }

            if (!int.TryParse(id, out int eventId))
            {
                TempData["ErrorMessage"] = "Invalid Event ID format!";
                return RedirectToAction("EventList");
            }

            var eventModel = _context.Events.FirstOrDefault(e => e.Id == eventId);

            if (eventModel == null)
            {
                TempData["ErrorMessage"] = "Event not found!";
                return RedirectToAction("EventList");
            }

            return View("OpenParticipation", eventModel);
        }

        [Route("/viewemployee/{id}")]
        public IActionResult ViewEmployee(int id)
        {
            var employee = _context.Employees
                .Include(e => e.Attendances)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
        [Route("/registration/success")]
        public IActionResult Success()
        {
           
            return View();
        }

    }
}
