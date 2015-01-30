using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SwipeBox.Shared.Entities
{
    [DataContract]
    public class Meeting
    {
        /// <summary>
        /// Meeting ID
        /// </summary>
        [DataMember]
        public int MeetingId { get; set; }

        /// <summary>
        /// Time the meeting occured
        /// </summary>
        [DataMember]
        public DateTime MeetingDate { get; set; }

        /// <summary>
        /// The Id of the client
        /// </summary>
        [DataMember]
        public int ClientId { get; set; }

        /// <summary>
        /// The client attending the meeting
        /// </summary>
        [DataMember]
        public virtual Client Client { get; set; }
    }
}
