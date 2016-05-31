using System;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace HappyCspp.Compiler
{
    public abstract class CppCompiler
    {
        protected CompilerConfig Config { get; private set; }

        public CppCompiler(CompilerConfig config)
        {
            this.Config = config;
        }

        public void Compile(bool isLibrary, string[] cppFiles, string outputFile)
        {
            string[] objFiles = this.Compile(cppFiles);
            this.Link(isLibrary, objFiles, outputFile);
        }

        protected abstract string[] Compile(string[] inputs);

        protected abstract void Link(bool isLibrary, string[] objFiles, string outputFile);

        protected void Execute(string program, string arguments)
        {
            Process p = Process.Start(program, arguments);
            p.WaitForExit();
            if (p.ExitCode != 0)
            {
                throw new Exception(string.Format("{0} {1} failed with exit code {2}.", program, arguments, p.ExitCode));
            }
        }
    }

    public class GccCompiler : CppCompiler
    {
        private string compileCmd, linkCmd, libLinkCmd;

        public GccCompiler(CompilerConfig config) : base(config)
        {
            this.compileCmd =
                string.Concat(config.CompilerOptions.Select(x => " -" + x)) +
                string.Concat(config.Includes.Select(x => " -I" + x)) +
                " -c {0} -o obj/" + config.Name + "/{0}.o";

            this.linkCmd =
                string.Concat(config.LinkerOptions.Select(x => " " + x)) +
                string.Concat(config.Libraries.Select(x => " " + x)) +
                " {0} -o bin/" + config.Name + "/{1}" +
                (Environment.OSVersion.Platform.ToString().StartsWith("Win") ? ".exe" : "");

            this.libLinkCmd =
                string.Concat(config.LibLinkerOptions.Select(x => " -" + x)) +
                " bin/" + config.Name + "/{1}.a" +
                string.Concat(config.Libraries.Select(x => " " + x)) + " {0}";
        }

        protected override string[] Compile(string[] inputs)
        {
            foreach (string input in inputs)
            {
                string cmd = string.Format(this.compileCmd, "cpp/" + input);
                this.Execute(this.Config.Compiler, cmd);
            }

            return inputs.Select(x => "obj/" + this.Config.Name + "/cpp/" + x + ".o").ToArray();
        }

        protected override void Link(bool isLibrary, string[] objFiles, string outputFile)
        {
            string cmd = string.Format(isLibrary ? this.libLinkCmd : this.linkCmd, string.Join(" ", objFiles), outputFile);
            this.Execute(isLibrary ? this.Config.LibLinker : this.Config.Linker, cmd);
        }
    }

    public class LlvmCompiler : CppCompiler
    {
        public LlvmCompiler(CompilerConfig config) : base(config)
        {
        }

        protected override string[] Compile(string[] inputs)
        {
            return null;
        }

        protected override void Link(bool isLibrary, string[] objFiles, string outputFile)
        {}
    }

    public class MsvcCompiler : CppCompiler
    {
        public MsvcCompiler(CompilerConfig config) : base(config)
        {
        }

        protected override string[] Compile(string[] inputs)
        {
            return null;
        }

        protected override void Link(bool isLibrary, string[] objFiles, string outputFile)
        {}
    }
}
