using SwipeBox.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeBox.BusinessLogic.MeetingBL
{
    public interface IMeetingsBL
    {
        IEnumerable<Meeting> GetTodaysMeetings();
    }
}
