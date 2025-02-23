using AttendenceApp.DatabaseContext;
using AttendenceApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AttendenceApp.Controllers
{
    public class EventController : Controller
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
            else if(newEvent.StartDate < DateTime.Now)
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
            var events = _context.Events.ToList(); // Fetch events from DB
            return View(events);
        }
        public IActionResult Details(int id)
        {
            var eventItem = _context.Events.FirstOrDefault(e => e.Id == id);

            if (eventItem == null)
            {
                return NotFound(); // Handle case where event doesn't exist
            }

            return View(eventItem);
        }
    }
}
