using SwipeBox.DAL.Context;
using SwipeBox.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeBox.DAL.Repositories
{
    public class EFMeetingRepository : IMeetingRepository
    {
        private ISwipeBoxContext m_context;

        public EFMeetingRepository()
        {
            m_context = new SwipeBoxContext();
        }

        public IQueryable<Meeting> Get
        {
            get { return m_context.Meetings; }
        }

        public Meeting Delete(int meetingId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="meetingToSave"></param>
        /// <returns></returns>
        public bool Save(Meeting meetingToSave)
        {
            var retVal = true;

            if (meetingToSave.MeetingId == 0)
            {
                m_context.Meetings.Add(meetingToSave);
            }
            else
            {
                var meetingEntry = m_context.Meetings.Find(meetingToSave.MeetingId);
                if (meetingEntry != null)
                {
                    meetingEntry.MeetingDate = meetingToSave.MeetingDate;
                    meetingEntry.ClientId = meetingToSave.ClientId;
                }
                else
                {
                    retVal = false;
                }
            }

            m_context.SaveChanges();
            return retVal;
        }

        public void Dispose()
        {
            m_context.Dispose();
        }
    }
}
