using API_TEST.Data;
using API_TEST.Models;
using API_TEST.Repositories;
using AutoMapper;

namespace API_TEST.Services
{
    public interface IScheduleServices
    {
        string? AddSchedule(ScheduleRequest scheduleRequest, TimeSlotRequest timeSlotRequest);
        string? UpdateSchedule(int scheduleId, ScheduleRequest scheduleRequest, TimeSlotRequest timeSlotRequest);
        string? DeleteSchedule(int scheduleId);
        List<ScheduleWithTimeSlotRespone> GetSchedules();
    }
    public class ScheduleServices : IScheduleServices
    {
        private readonly IRepositoryWrapper repositoryWrapper;
        private readonly IMapper mapper;

        public ScheduleServices(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            this.repositoryWrapper = repositoryWrapper;
            this.mapper = mapper;
        }


        private bool IsScheduleConflict(int mediaId, DateTime newStartTime, DateTime newEndTime,
                                RecurrenceFrequency? newFrequency, int? newInterval, DateTime? newEndRecurrenceDate, TimeSlotRequest newTimeSlotRequest)
        {
            List<TimeSlot> existingTimeSlots = repositoryWrapper.TimeSlots.FindByCondition(ts =>
                                                ts.MediaId == mediaId &&
                                                ts.EffectiveDate <= newEndTime &&
                                                (ts.ExpiryDate == null || ts.ExpiryDate >= newStartTime))
                                                .ToList();

            foreach (TimeSlot timeSlot in existingTimeSlots)
            {
                foreach (DayOfWeek dayOfWeek in newTimeSlotRequest.DaysOfWeek)
                {
                    if (timeSlot.DaysOfWeek.Contains(dayOfWeek))
                    {
                        TimeSpan existingStartTime = timeSlot.StartTime;
                        TimeSpan existingEndTime = timeSlot.EndTime;

                        TimeSpan newSlotStartTime = newTimeSlotRequest.StartTime;
                        TimeSpan newSlotEndTime = newTimeSlotRequest.EndTime;

                        if ((newSlotStartTime < existingEndTime && newSlotEndTime > existingStartTime))
                        {
                            return true; // Trùng lặp TimeSlot trong ngày
                        }
                    }
                }
            }

            if (newFrequency != null && newEndRecurrenceDate != null)
            {
                DateTime currentStart = newStartTime;
                DateTime currentEnd = newEndTime;

                while (currentStart <= newEndRecurrenceDate)
                {
                    if (existingTimeSlots.Any(ts =>
                        ts.EffectiveDate <= currentEnd &&
                        (ts.ExpiryDate == null || ts.ExpiryDate >= currentStart) &&
                        ts.DaysOfWeek.Any(d => newTimeSlotRequest.DaysOfWeek.Contains(d)) &&
                        newTimeSlotRequest.StartTime < ts.EndTime &&
                        newTimeSlotRequest.EndTime > ts.StartTime))
                    {
                        return true; // Trùng lặp lịch định kỳ
                    }

                    // Tính toán thời gian kế tiếp
                    switch (newFrequency)
                    {
                        case RecurrenceFrequency.Daily:
                            currentStart = currentStart.AddDays(newInterval ?? 1);
                            currentEnd = currentEnd.AddDays(newInterval ?? 1);
                            break;
                        case RecurrenceFrequency.Weekly:
                            currentStart = currentStart.AddDays((newInterval ?? 1) * 7);
                            currentEnd = currentEnd.AddDays((newInterval ?? 1) * 7);
                            break;
                        case RecurrenceFrequency.Monthly:
                            currentStart = currentStart.AddMonths(newInterval ?? 1);
                            currentEnd = currentEnd.AddMonths(newInterval ?? 1);
                            break;
                        case RecurrenceFrequency.Yearly:
                            currentStart = currentStart.AddYears(newInterval ?? 1);
                            currentEnd = currentEnd.AddYears(newInterval ?? 1);
                            break;
                    }
                }
            }

            return false;
        }

        public string? AddSchedule(ScheduleRequest scheduleRequest, TimeSlotRequest timeSlotRequest)
        {
            Schedule schedule = mapper.Map<Schedule>(scheduleRequest);

            // Kiểm tra media có tồn tại không
            Media? media = repositoryWrapper.Media.FindByCondition(x => x.MediaId == scheduleRequest.MediaId).FirstOrDefault();
            if (media == null)
            {
                return "Media not found";
            }

            // Kiểm tra trùng lặp thời gian
            if (IsScheduleConflict(scheduleRequest.MediaId, scheduleRequest.StartTime, scheduleRequest.EndTime, scheduleRequest.Frequency, scheduleRequest.Interval, scheduleRequest.EndRecurrenceDate, timeSlotRequest))
            {
                return "Schedule conflicts with existing schedules";
            }

            // Tạo đối tượng Schedule
            repositoryWrapper.Schedules.Create(schedule);
            repositoryWrapper.Save();

            // Tạo đối tượng TimeSlot
            TimeSlot timeSlot = mapper.Map<TimeSlot>(timeSlotRequest);
            timeSlot.MediaId = scheduleRequest.MediaId;
            repositoryWrapper.TimeSlots.Create(timeSlot);
            repositoryWrapper.Save();

            return null;
        }

        public string? UpdateSchedule(int scheduleId, ScheduleRequest scheduleRequest, TimeSlotRequest timeSlotRequest)
        {
            Schedule? schedule = repositoryWrapper.Schedules.FindByCondition(x => x.ScheduleId == scheduleId).FirstOrDefault();
            if (schedule == null)
            {
                return "Schedule not found";
            }

            // Kiểm tra media có tồn tại không
            Media? media = repositoryWrapper.Media.FindByCondition(x => x.MediaId == scheduleRequest.MediaId).FirstOrDefault();
            if (media == null)
            {
                return "Media not found";
            }

            // Kiểm tra trùng lặp thời gian
            if (IsScheduleConflict(scheduleRequest.MediaId, scheduleRequest.StartTime, scheduleRequest.EndTime, scheduleRequest.Frequency, scheduleRequest.Interval, scheduleRequest.EndRecurrenceDate, timeSlotRequest))
            {
                return "Schedule conflicts with existing schedules";
            }

            // Cập nhật thông tin Schedule
            schedule = mapper.Map(scheduleRequest, schedule);
            repositoryWrapper.Schedules.Update(schedule);
            repositoryWrapper.Save();

            // Cập nhật thông tin TimeSlot
            TimeSlot? timeSlot = repositoryWrapper.TimeSlots.FindByCondition(x => x.MediaId == scheduleRequest.MediaId).FirstOrDefault();
            if (timeSlot == null)
            {
                return "TimeSlot not found";
            }

            timeSlot = mapper.Map(timeSlotRequest, timeSlot);
            repositoryWrapper.TimeSlots.Update(timeSlot);
            repositoryWrapper.Save();

            return null;
        }

        public string? DeleteSchedule(int scheduleId)
        {
            Schedule? schedule = repositoryWrapper.Schedules.FindByCondition(x => x.ScheduleId == scheduleId).FirstOrDefault();
            if (schedule == null)
            {
                return "Schedule not found";
            }

            repositoryWrapper.Schedules.Delete(schedule);
            repositoryWrapper.Save();

            return null;
        }

        public List<ScheduleWithTimeSlotRespone> GetSchedules()
        {
            List<ScheduleWithTimeSlotRespone> schedules = repositoryWrapper.Schedules.FindAll().ToList().Select(schedule =>
            {
                List<TimeSlot> timeSlots = repositoryWrapper.TimeSlots.FindByCondition(ts => ts.MediaId == schedule.MediaId).ToList();
                return new ScheduleWithTimeSlotRespone()
                {
                    MediaId = schedule.MediaId,
                    StartTime = schedule.StartTime.ToString(),
                    EndTime = schedule.EndTime.ToString(),
                    Frequency = schedule.Frequency.ToString(),
                    Interval = schedule.Interval ?? 0,
                    EndRecurrenceDate = schedule.EndRecurrenceDate ?? DateTime.MinValue,
                    DaysOfWeek = timeSlots.SelectMany(ts => ts.DaysOfWeek).Distinct().ToList(),
                    TimeSlots = timeSlots
                };
            }).ToList();

            return schedules;
        }
    }
}
