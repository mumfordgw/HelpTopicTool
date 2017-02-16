using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpTopicTool
{
    class TopicHandler
    {
        private string _topicID;
        private string _fromURL;
        private string _newTopic;
        private string _toURL;
        private string _fromPath;
        private string _toPath;
        private string _fromFile;
        private string _toFile;

        public void setTopic(string topicId, string path)
        {
            uint uintIgnore;

            // make sure that the topic is numeric
            if (!uint.TryParse(topicId, out uintIgnore))
            {
                throw new ApplicationException("non numeric topic id " + topicId);
            }
            else
            {
                _topicID = topicId;
            }

            // make sure that the path looks ok
            // should be something like html\topic.htm
            // e.g. html\70r32..htm

            // first check that URL starts with "html\"
            if (path.Substring(0, 5) != "html\\")
            {
                throw new ApplicationException("Path doesn't start with html\\ " + path);
            }
            else
            {
                // make sure that the path ends with ".htm"
                if (path.Substring(path.Length - 4, 4) != ".htm")
                {
                    throw new ApplicationException("Path doesn't end with .htm " + path);
                }
                else
                {
                    _fromPath = path;
                    _fromFile = path.Substring(5, path.Length - 5);
                }
            }

            // build the path. This will look like the URL but the slashes will be
            // the other way around!
            _fromURL = _fromPath.Replace('\\', '/');


        }

        public string setNewTopic()
        {
            // The new topic id will be something like "VCON00000"
            // where 00000 is a zero filled version of _topicID

            string newTopic;
            UInt32 uintTopicID;
            
            // convert the topic id to numeric form
            uintTopicID = Convert.ToUInt32(_topicID);

            _newTopic = "VCON";
            // D5 pads uintTopicID to 5 digits padded with zeros to the left
            _newTopic += uintTopicID.ToString("D5");

            return _newTopic;

        }

        public string setNewURL()
        {
            // new URL will look like html/_newTopic.htm
            // e.g. html/VCON0001.htm

            _toURL = "html/";
            _toURL += _newTopic;
            _toURL += ".htm";
            return _toURL;
        }

        public string setNewPath()
        {
            // new path will look like html\_newTopic.htm
            // e.g. html\VCON0001.htm

            _toPath = "html\\";
            _toPath += _newTopic;
            _toPath += ".htm";
            return _toPath;
        }

        public string setNewFile()
        {
            // new path will look like _newTopic.htm
            // e.g. VCON0001.htm

            _toFile = _newTopic;
            _toFile += ".htm";
            return _toFile;
        }

        public string getNewMapEntry()
        {
            // create the new map.h file entry
            // will look like #define IDH__topicID _topicID
            // e.g. #define IDH_00001 1
            // can add an optional comment after slash slash but not going to!

            string NewMapEntry = "#define IDH_";
            NewMapEntry += _newTopic + " ";
            NewMapEntry += _topicID;

            return NewMapEntry;
        }

        public string getNewAliasEntry()
        {
            // create new alias file entry
            // will look like IDH__topicID=_toPath ; comment
            // e.g. IDH_VCON00001=html\VCON00001.htm ; Displaying/Changing System Wide Audit Controls (dialog)

            string newAliasEntry = "IDH_";
            newAliasEntry += _newTopic + "=";
            newAliasEntry += _toPath;

            return newAliasEntry;
        }

        public string getNewReplaceEntry()
        {
            // create new topic renaming file entry
            // will look like oldfile newfile
            // e.g. html/18y988.htm html/VCON00001.htm

            string newReplaceFileEntry = _fromURL;
            newReplaceFileEntry += " " + _toURL;

            return newReplaceFileEntry;
        }

        public string getNewRenameEntry()
        {
            // create new topic renaming file entry
            // will look like oldfile newfile
            // e.g. rename-item 18y988.htm VCON00001.htm

            string newRenameFileEntry = "rename-item ";
            newRenameFileEntry += _fromFile;
            newRenameFileEntry += " " + _toFile;

            return newRenameFileEntry;
        }


    }
}
