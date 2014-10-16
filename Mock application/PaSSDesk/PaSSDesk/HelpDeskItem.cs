using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaSSDesk
{
    class HelpDeskItem
    {
        public string HelpDeskNumber { get; set; }
        public string Name { get; set; }
        public int Urgency { get; set; }
        public DateTime Reported { get; set; }
        public string Location { get; set; }
        public string Flag { get; set; }
    }
}
