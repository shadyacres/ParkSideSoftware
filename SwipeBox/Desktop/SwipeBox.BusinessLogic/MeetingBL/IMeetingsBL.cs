// <copyright file="IMeetingsBL.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Interface for meeetings business logic</summary>

using SwipeBox.Shared.Entities;
using System.Collections.Generic;

namespace SwipeBox.BusinessLogic.MeetingBL
{
    /// <summary>
    ///  inteface for meetings business logic
    /// </summary>
    public interface IMeetingsBL
    {
        /// <summary>
        /// Gets a list of todays' meetings
        /// </summary>
        /// <returns>List of today's meetings</returns>
        IEnumerable<Meeting> GetTodaysMeetings();
    }
}
