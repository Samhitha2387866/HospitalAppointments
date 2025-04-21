using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;

using NewHospitalManagementSystem.Models;

using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;

namespace Hospital_Appointment_Management_System.Controllers

{

    [Route("api/[controller]")]

    [ApiController]

    public class NotificationsController : ControllerBase

    {

        private readonly ApplicationDBContext _context;

        public NotificationsController(ApplicationDBContext context)

        {

            _context = context;

        }

        // GET: api/Notifications

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()

        {

            return await _context.Notifications.ToListAsync();

        }

        // GET: api/Notifications/patient/5

        [HttpGet("patient/{patientId}")]

        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsByPatientId(int patientId)

        {

            var notifications = await _context.Notifications

                                              .Where(n => n.PatientId == patientId)

                                              .ToListAsync();

            if (notifications == null || !notifications.Any())

            {

                return NotFound();

            }

            return notifications;

        }

    }

}

