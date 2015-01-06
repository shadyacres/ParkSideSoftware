using SwipeBox.DAL.Context;
using SwipeBox.DAL.Repositories;
using SwipeBox.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SwipeBox.Services
{
    public class SwipeBoxService : ISwipeBoxService
    {

        private IMeetingRepository m_meetingRepo;

        public SwipeBoxService()
        {
            m_meetingRepo = new EFMeetingRepository();
        }

        public bool AddMeeting(int clientId)
        {
            try
            {
                var meeting = new Meeting
                {
                    ClientId = clientId,
                    MeetingDate = DateTime.Now
                };

                return m_meetingRepo.Save(meeting);
            }
            catch
            {
                return false;
            }

        }
    }
}
