using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace HappyCspp.Compiler
{
    public class CsProject
    {
        public string TargetName { get; private set; }

        public string[] ReferencedLibraries { get; private set; }

        public string[] SourceFiles { get; private set; }

        public string Directory {get; private set; }

        public string CppDirectory { get; private set; }

        public string BinDirectory { get; private set; }

        public string ObjDirectory { get; private set; }

        public string DefaultNamespace { get; private set; }

        public bool IsLibrary { get; private set; }

        public CsProject(string projectFile)
        {
            JObject json = JObject.Parse(File.ReadAllText(projectFile));

            this.Directory = Path.GetDirectoryName(projectFile);

            this.TargetName = json.GetJsonValue<string>("buildOptions", "outputName") ?? Path.GetFileName(this.Directory);

            // TODO: Load referenced libraries automatically
            this.ReferencedLibraries = this.TargetName == "corelib"
                ? new string[]{ }
                : new string[]{ Path.Combine(this.Directory, "bin", "Debug", "corelib.dll") };

            this.SourceFiles =  System.IO.Directory.GetFiles(this.Directory, "*.cs", SearchOption.AllDirectories);

            this.CppDirectory = Path.Combine(this.Directory, "cpp");
            this.BinDirectory = Path.Combine(this.Directory, "bin");
            this.ObjDirectory = Path.Combine(this.Directory, "obj");

            this.DefaultNamespace = json.GetJsonValue<string>("cspp", "defaultNamespace");

            this.IsLibrary = !json.GetJsonValue<bool>("buildOptions", "emitEntryPoint");
        }
    }
}
