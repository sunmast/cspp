using System;
using System.IO;
using System.Xml;

namespace HappyCspp.Compiler
{
    public class CsProject
    {
        public string Name { get; private set; }

        public string[] ReferencedLibraries { get; private set; }

        public string[] SourceFiles { get; private set; }

        public string DestinationFolder { get; private set; }

        public string DefaultNamespace { get; private set; }

        public CsProject(string projectFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(projectFile);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("msbuild", "http://schemas.microsoft.com/developer/msbuild/2003");

            string dir = Path.GetDirectoryName(projectFile);

            this.Name = doc.SelectSingleNode("//msbuild:AssemblyName", nsmgr).InnerText;

            // TODO: Load referenced libraries automatically
            this.ReferencedLibraries = this.Name == "corelib"
                ? new string[]{ }
                : new string[]{ Path.Combine(dir, "bin", "Debug", "corelib.dll") };

            var nodesCompile = doc.SelectNodes("//msbuild:Compile", nsmgr);
            this.SourceFiles = new string[nodesCompile.Count];

            for (int i = 0; i < nodesCompile.Count; i++)
            {
                this.SourceFiles[i] = Path.Combine(dir, nodesCompile[i].Attributes["Include"].Value.Replace('\\', '/'));
            }

            this.DestinationFolder = Path.Combine(dir, "cpp");

            this.DefaultNamespace = doc.SelectSingleNode("//msbuild:RootNamespace", nsmgr).InnerText;
        }
    }
}
