using System;
using System.IO;
using System.Xml.Serialization;
using Planets.Model;
using System.Xml;

namespace Planets.Controller.Subcontrollers
{
    public static class PlayfieldSerializer
    {
        public static void SerializeToFile(this Playfield pf, string filepath)
        {
            // Create XML serializer
            var xs = new XmlSerializer(typeof(Playfield));

            // Check if directory exists
            if (!Directory.Exists(Path.GetDirectoryName(filepath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filepath));

            // Try making file
            using (var fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                // Serialize playfield to filestream
                xs.Serialize(fs, pf);
            }
        }

        public static void DeserializeFromFile(this Playfield pf, string filepath)
        {
            // Create XML serializer
            var xs = new XmlSerializer(typeof(Playfield));

            // Open file stream
            object obj;
            using (var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read))
            {
                // Deserialize from filestream
                obj = xs.Deserialize(fs);
            }

            // Check if exists
            var read = obj as Playfield;
            if (read != null)
            {
                read.BOT.Iterate(pf.BOT.Add);
                pf.CurrentPlayer = read.CurrentPlayer;
            }
            else
            {
                throw new ArgumentException("File does not exist");
            }
        }

		public static void WriteScoreToXml(int score, string filepath)
		{
			XmlDocument scoreDocument = new XmlDocument();

			// Ensure score file exists.
			try {
				scoreDocument.Load(filepath);
			} catch (FileNotFoundException) {
				XmlNode docNode = scoreDocument.CreateXmlDeclaration("1.0", "UTF-8", null);
				scoreDocument.AppendChild(docNode);
				scoreDocument.Save(filepath);

				scoreDocument.Load(filepath);
			}

			// Add score to the XML file.
			XmlNode rootNode = scoreDocument.DocumentElement;
			XmlNode scoreNode = scoreDocument.CreateElement("score");

			scoreNode.InnerText = score.ToString();
			rootNode.AppendChild(scoreNode);

			scoreDocument.Save(filepath);
		}
    }
}
