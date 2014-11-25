using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planets
{
    public static class Debug
    {
        private static string[] Messages = new string[10];

        private static int size = 10;

        private static int current = 0;

        public static void AddMessage(string message)
        {
            lock (Messages)
            {
                Messages[current] = message;
                current = (current + 1)%size;
            }
        }

        public static IEnumerable<string> LastMessages()
        {
            for (int i = current; i < current + size; i++)
            {
                yield return Messages[i % size];
            }
        } 
    }
}
