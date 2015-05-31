// <copyright file="Meeting.cs" company="Park Side Software">
// Copyright (c) 29/04/2015 All Right Reserved
// </copyright>
// <author>Daniel Blackmore</author>
// <date>29/04/2015</date>
// <summary>Meeting entity</summary>

using System;
using System.Runtime.Serialization;

namespace SwipeBox.Shared.Entities
{
    /// <summary>
    /// Meeting entity
    /// </summary>
    public class Meeting
    {
        /// <summary>
        /// Meeting ID
        /// </summary>
        public int MeetingId { get; set; }

        /// <summary>
        /// Time the meeting occured
        /// </summary>
        public DateTime MeetingDate { get; set; }

        /// <summary>
        /// The Id of the client
        /// </summary>
        public int ClientId { get; set; }

        /// <summary>
        /// The client attending the meeting
        /// </summary>
        public virtual Client Client { get; set; }
    }
}
