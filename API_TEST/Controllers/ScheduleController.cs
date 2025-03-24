using API_TEST.Models;
using API_TEST.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_TEST.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleServices scheduleServices;

        public ScheduleController(IScheduleServices scheduleServices)
        {
            this.scheduleServices = scheduleServices;
        }

        [HttpPost]
        public IActionResult AddSchedule([FromBody] ScheduleWithTimeSlotRequest request)
        {
            string? result = scheduleServices.AddSchedule(request.ScheduleRequest, request.TimeSlotRequest);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok("Schedule added successfully");
        }

        [HttpPut("{scheduleId}")]
        public IActionResult UpdateSchedule(int scheduleId, [FromBody] ScheduleWithTimeSlotRequest request)
        {
            string? result = scheduleServices.UpdateSchedule(scheduleId, request.ScheduleRequest, request.TimeSlotRequest);

            if (result == "Schedule not found")
            {
                return NotFound(result);
            }

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok("Schedule updated successfully");
        }

        [HttpDelete("{scheduleId}")]
        public IActionResult DeleteSchedule(int scheduleId)
        {
            string? result = scheduleServices.DeleteSchedule(scheduleId);

            if (result != null)
            {
                return BadRequest(result);
            }

            return Ok("Schedule deleted successfully");

        }

        [HttpGet]
        public IActionResult GetSchedules()
        {
            List<ScheduleWithTimeSlotRespone> schedules = scheduleServices.GetSchedules();

            return Ok(schedules);
        }
    }
}
