using AttendenceApp.DatabaseContext;
using AttendenceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace AttendenceApp.Controllers
{
    public class EventController : BaseController
    {
        private readonly MyAppContext _context;

        public EventController(MyAppContext context)
        {
            _context = context;
        }

        // Show event creation form
        public IActionResult Create()
        {
            return View();
        }

        // Handle event form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Event newEvent)
        {
            // Ensure default values for required properties
            newEvent.Name = newEvent.Name?.Trim() ?? string.Empty;
            newEvent.EventType = newEvent.EventType?.Trim() ?? string.Empty;
            newEvent.Status = newEvent.Status?.Trim() ?? "Upcoming";

            // Validate date logic manually
            if (newEvent.EndDate.HasValue && newEvent.StartDate >= newEvent.EndDate)
            {
                ModelState.AddModelError("EndDate", "End Date must be later than Start Date.");
            }
            else if (newEvent.StartDate < DateTime.Now)
            {
                ViewData["ErrorMessage"] = "You can't create an event for past, Try Again";
                return View(newEvent);
            }

            // Print model validation errors for debugging
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model Validation Errors:");
                foreach (var error in ModelState)
                {
                    foreach (var err in error.Value.Errors)
                    {
                        Console.WriteLine($" - {error.Key}: {err.ErrorMessage}");
                    }
                }
                ViewData["ErrorMessage"] = "Error while creating event: ";
                return View(newEvent); // Return view with validation errors
            }

            try
            {
                // Set creation date
                newEvent.Created = DateTime.UtcNow;

                // Save event to database
                _context.Events.Add(newEvent);
                _context.SaveChanges();

                // Redirect to event list page
                return RedirectToAction("Events", "Event");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = "Error while creating event: " + ex.Message;
                return View(newEvent);
            }
        }


        // Show all events
        public IActionResult Events()
        {
            var events = _context.Events.ToList();
            return View(events);
        }
        public async Task<IActionResult> Details(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return NotFound();
            }
            var parts = title.Split('-');
            if (!int.TryParse(parts[0], out int eventId))
            {
                return NotFound();
            }

            var eventItem = await _context.Events.FindAsync(eventId);
            var eventWithAttendees = await _context.Events
                .Include(e => e.Attendances)
                   .FirstOrDefaultAsync(e => e.Id == eventId);
            ViewBag.ParticipantCount = eventWithAttendees.Attendances.Count;

            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        //Route["/Employee/GoToOpenParticipationEvent/"]
        public IActionResult OpenParticipation(int id)
        {
            //ModelState.AddModelError("EmployeeNotFound", "Employee not Found");
            //TempData["SuccessMessage"] = "Successfully Logged IN";
            //return View(_context.Events.FirstOrDefault(e => e.Id == int.Parse(id[id.Length - 1].ToString())));

            return View(_context.Events.FirstOrDefault((e) => e.Id == id));

        }
        // Controller Action (EventController.cs)
        public async Task<IActionResult> ShowAttendance(int id)
        {
            var eventWithAttendees = await _context.Events
                .Include(e => e.Attendances)
                    .ThenInclude(a => a.Employee)
                .FirstOrDefaultAsync(e => e.Id == id);
            

            if (eventWithAttendees == null)
            {
                return NotFound();
            }

            return View(eventWithAttendees);
        }

    }
}
