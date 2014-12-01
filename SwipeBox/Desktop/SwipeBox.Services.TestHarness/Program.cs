using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwipeBox.Services.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SwipeBoxServiceReference.SwipeBoxServiceClient client = new SwipeBoxServiceReference.SwipeBoxServiceClient())
            {
                Console.WriteLine(client.AddMeeting(1));
            }

            Console.ReadKey();
        }
    }
}
