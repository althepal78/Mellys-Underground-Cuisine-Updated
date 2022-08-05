using DAL.Context;

namespace Mellys_Underground_Cuisine.Services
{
    public interface ITimeService
    {
        void DeleteOldMenus();
    }
    public class TimeService : ITimeService
    {

        private readonly ILogger<TimeService> _logger;
        private readonly AppDbContext _db;
        

        public TimeService(ILogger<TimeService> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }
      
        public void DeleteOldMenus()
        {
            var menus = _db.Menu.ToList();
            foreach (var date in menus)
            {
                if (date.DateColumn.Date < DateTime.Now.Date)
                {
                    Console.WriteLine(date.DateColumn);
                    _db.Menu.Remove(date);
                    _db.SaveChanges();
                }
            }
        }
    }
}
