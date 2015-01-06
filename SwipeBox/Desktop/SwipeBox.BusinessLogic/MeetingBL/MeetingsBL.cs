using SwipeBox.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SwipeBox.BusinessLogic.MeetingBL
{
    public class MeetingsBL : IMeetingsBL
    {
        private IMeetingRepository m_meetingsRepo;

        public MeetingsBL()
        {
            m_meetingsRepo = new EFMeetingRepository();
        }

        public IEnumerable<Shared.Entities.Meeting> GetTodaysMeetings()
        {
            return m_meetingsRepo.Get.Include("Client").Where(m => DbFunctions.TruncateTime(m.MeetingDate) == DbFunctions.TruncateTime(DateTime.Now));
        }
    }
}
