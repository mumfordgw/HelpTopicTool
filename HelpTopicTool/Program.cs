using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

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
            System.IO.StreamWriter mapFile, aliasFile, renameFile;
            string currentPath = "";


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

            // get the current directory
            try
            {
                currentPath = Directory.GetCurrentDirectory();
            }
            catch (Exception Ex)
            {
                Console.WriteLine("{0} Error encountered getting current directory", Ex);
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

            // Open the output files

            // map file
            try
            {
                mapFile = new System.IO.StreamWriter(@"C:\Users\mumfordgw\Documents\Synchronised Files\VME\vcon\testmap.h");
            }
            catch (Exception Ex)
            {
                Console.Write("{0} Error encountered opening new map file {1}", Ex, "xxxx");
                return;
            }

            // alias file
            try
            {
                aliasFile = new System.IO.StreamWriter(@"C:\Users\mumfordgw\Documents\Synchronised Files\VME\vcon\testalias.txt");
            }
            catch (Exception Ex)
            {
                Console.Write("{0} Error encountered opening new alias file {1}", Ex, "xxxx");
                return;
            }

            // rename entry
            try
            {
                renameFile = new System.IO.StreamWriter(@"C:\Users\mumfordgw\Documents\Synchronised Files\VME\vcon\testrename.txt");
            }
            catch (Exception Ex)
            {
                Console.Write("{0} Error encountered opening new rename file {1}", Ex, "xxxx");
                return;
            }


            // process each line
            Regex _regex = new Regex(" {2,}");
            string id = "";
            string file = "";

            foreach (string line in lines)
            {
                //Console.WriteLine("\t" + line);                
                TopicHandler thisTopic = new TopicHandler();

                string normalisedLine = _regex.Replace(line, " ");
                char delim = ' ';
                string[] substrings = normalisedLine.Split(delim);
                id = substrings[1];
                file = substrings[2];
                Console.WriteLine("id={0}, file={1}", id, file);
                try
                {
                    thisTopic.setTopic(id, file);
                }
                catch (Exception Ex)
                {
                    Console.WriteLine("{0} error encounterd while storing topic {1}", Ex, id);
                    return;
                }
                string newTopicID = thisTopic.setNewTopic();
                Console.WriteLine("new topic id is " + newTopicID);
                string newURL = thisTopic.setNewURL();
                Console.WriteLine("new URL is " + newURL);

                string newMapEntry = thisTopic.getNewMapEntry();
                Console.WriteLine("new map file entry: " + newMapEntry);
                mapFile.WriteLine(newMapEntry);

                string newAliasEntry = thisTopic.getNewAliasEntry();
                Console.WriteLine("new alias file entry: " + newAliasEntry);
                aliasFile.WriteLine(newAliasEntry);

                string newRenameEntry = thisTopic.getNewRenameEntry();
                Console.WriteLine("new rename file entry: " + newRenameEntry);
                renameFile.WriteLine(newRenameEntry);

            }

            // close files
            // MSDN recommends that the stream is Disposed of rather than flushed and closed.
            //mapFile.Flush();
            //mapFile.Close();
            //aliasFile.Flush();
            //aliasFile.Close();
            //renameFile.Flush();
            //renameFile.Close();
            mapFile.Dispose();
            aliasFile.Dispose();
            renameFile.Dispose();

            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();


        }
    }
}
