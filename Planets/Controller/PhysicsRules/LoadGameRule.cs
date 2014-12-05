using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Planets.Controller.PhysicsRules;
using Planets.Model;

namespace Planets.Controller
{
    class LoadGameRule : AbstractGameRule
    {

        protected override void ExecuteRule(Playfield pf, double ms)
        {
            //XmlSerializer SerializerObj = new XmlSerializer(typeof(Playfield));
            //TextWriter WriteFileStream = new StreamWriter(@"C:\test2.xml");
            //SerializerObj.Serialize(WriteFileStream, pf);

            //WriteFileStream.Close();

            //List<GameObject> henk = new List<GameObject>();
            //pf.BOT.Iterate(g =>
            //{
            //    henk.Add(g);
            //}
            //);
        }
    }
}
