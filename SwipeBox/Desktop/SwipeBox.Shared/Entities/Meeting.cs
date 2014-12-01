using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeBox.Shared.Entities
{
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
