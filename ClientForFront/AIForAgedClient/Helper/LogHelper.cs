using System;

namespace AIForAgedClient.Helper
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
