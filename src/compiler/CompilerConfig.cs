using System;
using System.IO;
using System.Xml.Serialization;

namespace HappyCspp.Compiler
{
    [XmlRoot]
    public class CompilerConfig
    {
        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public bool PreferWideChar { get; set; }

        [XmlArray, XmlArrayItem("Define")]
        public string[] Defines { get; set; }

        [XmlArray, XmlArrayItem("Include")]
        public string[] Includes { get; set; }

        [XmlArray, XmlArrayItem("Library")]
        public string[] Libraries { get; set; }

        [XmlElement]
        public string Compiler { get; set; }

        [XmlArray, XmlArrayItem("Option")]
        public string[] CompilerOptions { get; set; }

        [XmlElement]
        public string Linker { get; set; }

        [XmlArray, XmlArrayItem("Option")]
        public string[] LinkerOptions { get; set; }

        [XmlElement]
        public string LibLinker { get; set; }

        [XmlArray, XmlArrayItem("Option")]
        public string[] LibLinkerOptions { get; set; }

        public static void Serialize(CompilerConfig config, string xmlFile)
        {
            XmlSerializer xs = new XmlSerializer(typeof(CompilerConfig));

            using (FileStream fs = new FileStream(xmlFile, FileMode.Create, FileAccess.Write))
            {
                xs.Serialize(fs, config);
            } 
        }

        public static CompilerConfig Deserialize(string xmlFile)
        {
            XmlSerializer xs = new XmlSerializer(typeof(CompilerConfig));

            using (FileStream fs = new FileStream(xmlFile, FileMode.Open, FileAccess.Read))
            {
                return (CompilerConfig)xs.Deserialize(fs);
            }
        }
    }
}
