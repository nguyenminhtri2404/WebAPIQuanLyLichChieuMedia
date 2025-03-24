using API_TEST.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace API_TEST.Repositories
{
    public interface IRepositoryWrapper
    {
        IRepositoryBase<Media> Media { get; }
        IRepositoryBase<Schedule> Schedules { get; }
        IRepositoryBase<TimeSlot> TimeSlots { get; }

        void Save();
        IDbContextTransaction Transaction();
    }
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly MyDBContext dbContext;
        private IRepositoryBase<Media> media;
        private IRepositoryBase<Schedule> schedule;
        private IRepositoryBase<TimeSlot> timeSlot;

        public RepositoryWrapper(MyDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IRepositoryBase<Media> Media
        {
            get
            {
                if (media == null)
                {
                    media = new RepositoryBase<Media>(dbContext);
                }
                return media;
            }
        }

        public IRepositoryBase<Schedule> Schedules
        {
            get
            {
                if (schedule == null)
                {
                    schedule = new RepositoryBase<Schedule>(dbContext);
                }
                return schedule;
            }
        }

        public IRepositoryBase<TimeSlot> TimeSlots
        {
            get
            {
                if (timeSlot == null)
                {
                    timeSlot = new RepositoryBase<TimeSlot>(dbContext);
                }
                return timeSlot;
            }
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public IDbContextTransaction Transaction()
        {
            return dbContext.Database.BeginTransaction();
        }
    }
}
