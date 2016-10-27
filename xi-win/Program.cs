using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace xi_win
{
    class Program
    {
        static void Main(string[] args)
        {
            var core = new CoreCommunication();
            core.StartInputLoop();

            var result = core.SendRawCommand("{\"id\":0,\"method\":\"new_tab\",\"params\":[]}");

            core.StopInputLoop();
        }
    }
}
