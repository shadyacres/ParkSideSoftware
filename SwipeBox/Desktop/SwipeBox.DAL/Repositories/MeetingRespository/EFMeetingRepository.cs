// <copyright file="EFCLientRepository.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>EF meetings repository</summary>

using SwipeBox.DAL.Context;
using SwipeBox.Shared.Entities;
using System;
using System.Linq;

namespace SwipeBox.DAL.Repositories
{
    /// <summary>
    /// Entity Framework meetings repository
    /// </summary>
    public class EFMeetingRepository : IMeetingRepository
    {
        private ISwipeBoxContext m_context;

        /// <summary>
        /// Initializes a new instance of the EFMeetingRepository
        /// </summary>
        public EFMeetingRepository()
        {
            m_context = new SwipeBoxContext();
        }

        /// <summary>
        /// Gets a list of meetings
        /// </summary>
        public IQueryable<Meeting> Get
        {
            get { return m_context.Meetings; }
        }

        /// <summary>
        /// Delete a meeting from the database - not implemented
        /// </summary>
        /// <param name="meetingId">The meeting id</param>
        /// <returns>Throws not implemented exception</returns>
        public Meeting Delete(int meetingId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save a meeting
        /// </summary>
        /// <param name="meetingToSave">The meeting to save</param>
        /// <returns>true if saved false if not</returns>
        public bool Save(Meeting meetingToSave)
        {
            var retVal = true;

            // Doesn't already exist - Insert
            if (meetingToSave.MeetingId == 0)
            {
                m_context.Meetings.Add(meetingToSave);
            }
            else
            {
                // Already exists - update
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

        /// <summary>
        /// Dispose underlying context
        /// </summary>
        public void Dispose()
        {
            m_context.Dispose();
        }
    }
}
