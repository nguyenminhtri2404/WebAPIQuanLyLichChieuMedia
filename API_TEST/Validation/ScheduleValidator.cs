using API_TEST.Data;
using FluentValidation;

namespace API_TEST.Validation
{
    public class ScheduleValidator : AbstractValidator<Schedule>
    {
        public ScheduleValidator()
        {
            RuleFor(x => x.MediaId).NotEmpty().WithMessage("MediaId is required");

            //Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc
            RuleFor(x => x.StartTime).LessThan(x => x.EndTime).WithMessage("StartTime must be less than EndTime");
        }
    }
}
