using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> input = new List<string>();

            // first read input till there are nonempty items, means they are not null and not ""
            // also add read item to list do not need to read it again    
            string line;
            while ((line = Console.ReadLine()) != null && line != "")
            {
                input.Add(line);
            }



            Console.Out.WriteLine("Display filenames to a file:");
        }
    }
}
