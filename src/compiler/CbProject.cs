using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace HappyCspp.Compiler
{
    public class CbProject
    {
        string projFile;

        public CbProject(string projFile)
        {
            this.projFile = projFile;
        }

        internal void UpdateSourceFiles(IEnumerable<string> files)
        {
            XmlDocument doc = new XmlDocument();

            using (FileStream fs = new FileStream(this.projFile, FileMode.Open, FileAccess.Read))
            {
                doc.Load(fs);
            }

            XmlElement projNode = doc.DocumentElement["Project"];

            // Remove all units (source files)
            XmlNodeList nodeList = projNode.GetElementsByTagName("Unit");
            XmlNode[] nodes = new XmlNode[nodeList.Count];

            int i = 0;
            foreach (XmlNode node in nodeList)
            {
                nodes[i++] = node;
            }

            foreach (XmlNode node in nodes)
            {
                projNode.RemoveChild(node);
            }

            // Add all source files
            foreach (string file in files)
            {
                var unit = doc.CreateElement("Unit");
                unit.SetAttribute("filename", "cpp/" + file);
                projNode.AppendChild(unit);
            }

            // Save XML using CB default indentation style
            var xwSettings = new XmlWriterSettings()
            {
                Encoding = new UTF8Encoding(true),
                Indent = true,
                IndentChars = "\t"
            };

            using (FileStream fs = new FileStream(this.projFile, FileMode.Create, FileAccess.Write))
            using (XmlWriter xtw = XmlWriter.Create(fs, xwSettings))
            {
                doc.Save(xtw);
                xtw.WriteRaw(Environment.NewLine);
            }
        }
    }
}
