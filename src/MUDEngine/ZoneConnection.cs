﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using ModernMUD;

namespace MUDEngine
{
    /// <summary>
    /// Zone connection -- used for saving and loading zone-to-zone connections.
    /// Generates exits from one place to another after zones have been loaded.
    /// </summary>
    [Serializable]
    public class ZoneConnection
    {
        public int FirstRoomNumber { set; get; }
        public int SecondRoomNumber { set; get; }
        public Exit.Direction FirstToSecondDirection { set; get; }

        /// <summary>
        /// Loads zone connections.
        /// </summary>
        /// <returns></returns>
        public static List<ZoneConnection> Load()
        {
            string filename = FileLocation.AreaDirectory + FileLocation.ZoneConnectionFile;
            string blankFilename = FileLocation.BlankAreaFileDirectory + FileLocation.ZoneConnectionFile;
            Stream stream = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<ZoneConnection>));
                try
                {
                    stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch (FileNotFoundException)
                {
                    Log.Info("Zone connection file not found. Using empty version.");
                    File.Copy(blankFilename, filename);
                    stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None);
                }
                List<ZoneConnection> connections = (List<ZoneConnection>)serializer.Deserialize(stream);
                stream.Close();
                return connections;
            }
            catch( Exception ex )
            {
                Log.Error("Exception in ZoneConnection.Load(): " + ex);
                return new List<ZoneConnection>();
            }
        }

        public static void Save(List<ZoneConnection> connections)
        {
            string filename = FileLocation.AreaDirectory + FileLocation.ZoneConnectionFile;
            XmlSerializer serializer = new XmlSerializer(typeof(List<ZoneConnection>));
            Stream stream = new FileStream( filename, FileMode.Create, FileAccess.Write, FileShare.None );
            serializer.Serialize( stream, connections );
            stream.Close();
        }

    }
}
