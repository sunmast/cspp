using System;
using System.Collections.Generic;
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
            doc.Load(this.projFile);

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
            using (XmlTextWriter xtw = new XmlTextWriter(this.projFile, Encoding.UTF8))
            {
                xtw.Formatting = Formatting.Indented;
                xtw.IndentChar = '\t';
                xtw.Indentation = 1;

                doc.Save(xtw);

                xtw.WriteRaw(Environment.NewLine);
            }
        }
    }
}
