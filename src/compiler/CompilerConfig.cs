using System;
using System.IO;
using Newtonsoft.Json;

namespace HappyCspp.Compiler
{
    public class CompilerConfig
    {
        public string Name { get; set; }

        public bool PreferWideChar { get; set; }

        public string[] Defines { get; set; }

        public string[] Includes { get; set; }

        public string[] Libraries { get; set; }

        public string Compiler { get; set; }

        public string[] CompilerOptions { get; set; }

        public string Linker { get; set; }

        public string[] LinkerOptions { get; set; }

        public string LibLinker { get; set; }

        public string[] LibLinkerOptions { get; set; }

        CompilerConfig(){}

        public static CompilerConfig Load(string configFile)
        {
            string json = File.ReadAllText(configFile);
            CompilerConfig cc = JsonConvert.DeserializeObject<CompilerConfig>(json);

            if (cc.Name == null || cc.Compiler == null || cc.Linker == null)
            {
                throw new InvalidDataException(string.Format("The compiler config {0} is not valid.", configFile));
            }

            return cc;
        }
    }
}
