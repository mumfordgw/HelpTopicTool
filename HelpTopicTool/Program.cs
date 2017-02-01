using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace HelpTopicTool
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "";
            string[] lines = {""};
            bool quiet = false;
            bool debug = false;


            // command line arguments
            System.Console.WriteLine("Number of command line parameters = {0}", args.Length);
            foreach (string s in args)
            {
                if (string.Equals(s, "-q", StringComparison.OrdinalIgnoreCase))
                {
                    quiet = true;
                }
                if (string.Equals(s, "-Debug", StringComparison.OrdinalIgnoreCase))
                {
                    debug = true;
                }
            }

            // get the filename and read the file into the lines array
            try
            {
                if (debug) Console.WriteLine("Enter name of help topic file");
                // fileName = System.Console.ReadLine();
                fileName = @"C:\Users\mumfordgw\Documents\Synchronised Files\VME\vcon\topiclistedited.txt";
                lines = System.IO.File.ReadAllLines(fileName);
            } catch (Exception Ex)
            {
                Console.WriteLine("{0} Error encountered opening and reading input file {1}", Ex, fileName);
            }

            // process each line
            Regex _regex = new Regex(" {2,}");
            string id = "";
            string file = "";

            foreach (string line in lines)
            {
                //Console.WriteLine("\t" + line);                
                string normalisedLine = _regex.Replace(line, " ");
                char delim = ' ';
                string[] substrings = normalisedLine.Split(delim);
                id = substrings[1];
                file = substrings[2];
                Console.WriteLine("id={0}, file={1}", id, file);
            }
            //Console.WriteLine("Press any key to exit.");
            //System.Console.ReadKey();


        }
    }
}
