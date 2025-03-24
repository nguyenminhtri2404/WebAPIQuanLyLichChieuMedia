using API_TEST.Data;
using API_TEST.Models;
using AutoMapper;

namespace API_TEST.Mapping
{
    public class Mappers : Profile
    {
        public Mappers()
        {
            CreateMap<MediaRequest, Media>();
            CreateMap<Media, MediaRespone>().ReverseMap();

            CreateMap<TimeSlotRequest, TimeSlot>();

            CreateMap<ScheduleRequest, Schedule>();

            CreateMap<Schedule, ScheduleWithTimeSlotRespone>().ReverseMap();
        }
    }
}
