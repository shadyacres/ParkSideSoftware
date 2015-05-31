// <copyright file="MeetingsBL.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Meetings business logic</summary>

using SwipeBox.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SwipeBox.BusinessLogic.MeetingBL
{
    /// <summary>
    /// meetings business logic
    /// </summary>
    public class MeetingsBL : IMeetingsBL
    {
        private IMeetingRepository m_meetingsRepo;

        /// <summary>
        /// Initializes a new instance of the MEetingsBL class -default repository
        /// </summary>
        public MeetingsBL()
        {
            m_meetingsRepo = new EFMeetingRepository();
        }

        /// <summary>
        /// Gets a list of today's meetings
        /// </summary>
        /// <returns>List of today#s meetings</returns>
        public IEnumerable<Shared.Entities.Meeting> GetTodaysMeetings()
        {
            // Get today's meetings - including client details
            return m_meetingsRepo.Get.Include("Client").Where(m => DbFunctions.TruncateTime(m.MeetingDate) == DbFunctions.TruncateTime(DateTime.Now));
        }
    }
}
