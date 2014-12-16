using System.IO;
using System.Xml.Serialization;
using Planets.Model;

namespace Planets.Controller.Subcontrollers
{
    public static class PlayfieldSerializer
    {
        public static void SerializeToFile(this Playfield pf, string filepath)
        {
            var xs = new XmlSerializer(typeof(Playfield));
            if (!Directory.Exists(Path.GetDirectoryName(filepath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            var fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write);
            xs.Serialize(fs, pf);
            fs.Close();
        }

        public static void DeserializeFromFile(this Playfield pf, string filepath)
        {
            var xs = new XmlSerializer(typeof(Playfield));
            if (!Directory.Exists(Path.GetDirectoryName(filepath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filepath));
            var fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            var obj = xs.Deserialize(fs);
            var read = obj as Playfield;
            if (read != null)
            {
                read.BOT.Iterate(pf.BOT.Add);
                pf.CurrentPlayer = read.CurrentPlayer;
            }
            fs.Close();
        }
    }
}
