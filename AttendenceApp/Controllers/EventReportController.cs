using AttendenceApp.DatabaseContext;
using AttendenceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace AttendenceApp.Controllers
{
    public class EventReportController : Controller
    {
        private readonly MyAppContext _context;

        public EventReportController(MyAppContext context)
        {
            _context = context;
        }

        // GET: ReportEvent
        [HttpGet]
        public IActionResult ReportEvent(int eventId)
        {
            var eventDetails = _context.Events.FirstOrDefault(e => e.Id == eventId);
            if (eventDetails == null)
            {
                return NotFound("Event not found.");
            }

            var viewModel = new EventReportViewModel
            {
                EventId = eventDetails.Id,
                Event = eventDetails
            };

            return View(viewModel);
        }

        // POST: ReportEvent
        [HttpPost]
        public IActionResult ReportEvent(EventReportViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.ErrorMessage = "Invalid data. Please check your inputs.";
                viewModel.Event = _context.Events.FirstOrDefault(e => e.Id == viewModel.EventId);
                return View(viewModel);
            }

            var eventEntity = _context.Events.Find(viewModel.EventId);
            var employeeEntity = _context.Employees.FirstOrDefault(x => x.EmployeeID == viewModel.EmployeeId.ToString());

            if (eventEntity == null)
            {
                viewModel.ErrorMessage = "Event not found. Please select a valid event.";
                viewModel.Event = _context.Events.FirstOrDefault(e => e.Id == viewModel.EventId);
                return View(viewModel);
            }

            if (employeeEntity == null)
            {
                viewModel.ErrorMessage = "Employee not found. Please enter a valid Employee ID.";
                viewModel.Event = _context.Events.FirstOrDefault(e => e.Id == viewModel.EventId);
                return View(viewModel);
            }

            var eventReport = new EventReport
            {
                EventId = viewModel.EventId,
                EmployeeId = viewModel.EmployeeId,
                ReportType = viewModel.ReportType,
                Description = viewModel.Description,
                SubmittedOn = DateTime.UtcNow,
                Event = eventEntity,
                Employee = employeeEntity
            };

            try
            {
                _context.EventReports.Add(eventReport);
                _context.SaveChanges();

                viewModel.SuccessMessage = "Report submitted successfully!";
                TempData["SuccessMessage"] = "Report submitted successfully!";
                return RedirectToAction("OpenParticipation","Employee", new { eventReport.EventId,title=eventReport.Event.Name });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                viewModel.ErrorMessage = "An error occurred while submitting the report. Please try again.";
                viewModel.Event = _context.Events.FirstOrDefault(e => e.Id == viewModel.EventId);
                return View(viewModel);
            }
        }
        public IActionResult Reports(string statusFilter = "", int eventFilter = 0)
        {
            // Get all reports with related event data
            var reportsQuery = _context.EventReports
                .Include(r => r.Event)
                .AsQueryable();

            // Apply status filter if specified
            if (!string.IsNullOrEmpty(statusFilter))
            {
                reportsQuery = reportsQuery.Where(r => r.Status.ToLower() == statusFilter.ToLower());
            }

            // Apply event filter if specified
            if (eventFilter > 0)
            {
                reportsQuery = reportsQuery.Where(r => r.EventId == eventFilter);
            }

            // Get all events for the filter dropdown
            ViewBag.Events = _context.Events
                .OrderBy(e => e.Name)
                .ToList();

            var reports = reportsQuery
                .OrderByDescending(r => r.SubmittedOn)
                .ToList();

            return View(reports);
        }

        // GET: Reports/Details/5
        public IActionResult ViewReport(int id)
        {
            var report = _context.EventReports
                .Include(r => r.Event)
                .FirstOrDefault(r => r.Id == id);

            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }
        // GET: Reports/Resolve/5
        [HttpGet]
        public async Task<IActionResult> Resolve(int id)
        {
            try
            {
                var report = await _context.EventReports
                    .Include(r => r.Event)
                    .Include(r => r.Employee)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (report == null)
                {
                    return NotFound();
                }

                var viewModel = new EventReportViewModel
                {
                    Id = report.Id,
                    EventId = report.EventId,
                    Event = report.Event,
                    EmployeeId = report.EmployeeId,
                    ReportType = report.ReportType,
                    Description = report.Description,
                    Status = report.Status,
                    SubmittedOn = report.SubmittedOn
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { errorMessage = "Error loading report details" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Resolve(int id, EventReportViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            try
            {
                var report = await _context.EventReports.FindAsync(id);
                if (report == null)
                {
                    return NotFound();
                }

                // Only update the status field
                report.Status = "Resolved";
               

                _context.Update(report);
                await _context.SaveChangesAsync();

                return RedirectToAction("Reports");
            }
            catch (Exception ex)
            {
               
                ModelState.AddModelError("", "An error occurred while updating the report status.");
                return View(model);
            }
        }
    }
}
