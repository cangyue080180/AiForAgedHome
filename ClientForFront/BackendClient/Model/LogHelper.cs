using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendClient.Model
{
    public class LogHelper
    {
        private static bool isDebug = true;

        public static void Debug(string msg)
        {
            if (isDebug)
            {
                Console.WriteLine($"{DateTime.Now.ToShortTimeString()}: {msg}");
            }
        }
    }
}
