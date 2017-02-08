﻿using System;
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
            string topicFileName = "";
            string[] lines = {""};
            bool quiet = false;
            bool debug = false;
            System.IO.StreamWriter mapFile, aliasFile, replaceFile;
            string currentPath = "";


            // command line arguments
            System.Console.WriteLine("Number of command line parameters = {0}", args.Length);
            foreach (string s in args)
            {
                bool argProcessed = false;
                if (string.Equals(s, "-q", StringComparison.OrdinalIgnoreCase))
                {
                    quiet = true;
                    argProcessed = true;
                    break;
                }
                if (string.Equals(s, "-Debug", StringComparison.OrdinalIgnoreCase))
                {
                    debug = true;
                    argProcessed = true;
                    break;
                }
                if (s.Length > 2)
                {
                    switch (s.Trim().ToUpper().Substring(0, 3))
                    {
                        case "-D:":
                            currentPath = s.Trim().Substring(3, s.Length - 3);
                            Console.WriteLine("Directory is {0}", currentPath);
                            argProcessed = true;
                            break;
                        case "-T:":
                            topicFileName = s.Trim().Substring(3, s.Length - 3);
                            Console.WriteLine("Topic File is {0}", topicFileName);
                            argProcessed = true;
                            break;
                        default:
                            Console.WriteLine("Unexpected argument {0}", s);
                            Console.WriteLine("Usage is!!!!!");
                            argProcessed = true;
                            return;
                    }
                }
                if (!argProcessed)
                {
                    Console.WriteLine("Unexpected argument {0}", s);
                    Console.WriteLine("Usage is!!!!!");
                    return;
                }

            }

            // check the directory parameter
            if (currentPath == "")
            {
                Console.WriteLine("ERROR - No -D:directory argument supplied");
                return;
            }
            if (!Directory.Exists(currentPath))
            {
                Console.WriteLine("ERROR - Directory {0} does not exist");
                return;

            }

            // check the topicFileName parameter
            if (topicFileName == "")
            {
                Console.WriteLine("ERROR - No -T:topicFile argument supplied");
                return;
            }

            string topicFileFullName = currentPath + topicFileName;
            if (!File.Exists(topicFileFullName))
            {
                Console.WriteLine("ERROR - Topic File {0} does not exist", topicFileFullName);
                return;

            }


            // get the filename and read the file into the lines array
            try
            {
                lines = System.IO.File.ReadAllLines(topicFileFullName);
            } catch (Exception Ex)
            {
                Console.WriteLine("{0} Error encountered opening and reading input file {1}", Ex, topicFileName);
            }

            // Open the output files

            // map file
            string mapFileName = currentPath + "map.h";
            try
            {
                mapFile = new System.IO.StreamWriter(mapFileName);
            }
            catch (Exception Ex)
            {
                Console.Write("{0} Error encountered opening new map file {1}", Ex, mapFileName);
                return;
            }

            // alias file
            string aliasFileName = currentPath + "alias.h";
            try
            {
                aliasFile = new System.IO.StreamWriter(aliasFileName);
            }
            catch (Exception Ex)
            {
                Console.Write("{0} Error encountered opening new alias file {1}", Ex, aliasFileName);
                return;
            }

            // rename file
            string replaceFileName = currentPath + "replace.txt";
            try
            {
                replaceFile = new System.IO.StreamWriter(replaceFileName);
            }
            catch (Exception Ex)
            {
                Console.Write("{0} Error encountered opening new replace file {1}", Ex, replaceFileName);
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

                string newReplaceEntry = thisTopic.getNewRenameEntry();
                Console.WriteLine("new rename file entry: " + newReplaceEntry);
                replaceFile.WriteLine(newReplaceEntry);

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
            replaceFile.Dispose();

            Console.WriteLine("Press any key to exit.");
            System.Console.ReadKey();


        }
    }
}
