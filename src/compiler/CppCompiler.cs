using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;

namespace HappyCspp.Compiler
{
    public abstract class CppCompiler
    {
        protected CompilerConfig Config { get; private set; }

        protected string[] CppFiles { get; private set; }

        protected string[] ObjFiles { get; private set; }

        protected string TargetFile { get; private set; }

        protected IEnumerable<string> Includes { get; private set; }

        protected IEnumerable<string> Libraries { get; private set; }

        protected CppCompiler(CompilerConfig config)
        {
            this.Config = config;
        }

        public void Build(CsProject csproj, IEnumerable<string> cppFiles)
        {
            this.CppFiles = cppFiles.Select(x => Path.Combine(csproj.CppDirectory, x)).ToArray();
            this.ObjFiles = cppFiles.Select(x => Path.Combine(csproj.ObjDirectory, this.Config.Name, x) + this.ObjExt).ToArray();
            this.TargetFile = Path.Combine(csproj.BinDirectory, this.Config.Name, csproj.TargetName) + (csproj.IsLibrary ? this.LibExt : this.ExeExt);

            this.Includes = this.Config.Includes.Select(x => Path.GetFullPath(Path.Combine(csproj.Directory, x)));
            this.Libraries = this.Config.Libraries.Select(x => Path.GetFullPath(Path.Combine(csproj.Directory, x)));

            this.Compile();

            if (csproj.IsLibrary)
            {
                this.LinkLib();
            }
            else
            {
                this.Link();
            }
        }

        protected abstract string ObjExt { get; }

        protected abstract string LibExt { get; }

        protected abstract string ExeExt { get; }

        protected abstract void Compile();

        protected abstract void Link();

        protected abstract void LinkLib();
    }

    public class GccCompiler : CppCompiler
    {
        public GccCompiler(CompilerConfig config) : base(config)
        {
        }

        protected override string ObjExt { get { return ".o"; } }

        protected override string LibExt { get { return ".a"; } }

        protected override string ExeExt { get { return ""; } }

        protected override void Compile()
        {
            string argsFormat =
                string.Concat(this.Config.CompilerOptions.Select(x => " -" + x)) +
                string.Concat(this.Includes.Select(x => " -I" + x)) +
                " -c {0} -o {1}";

            for (int i = 0; i < this.CppFiles.Length; i++)
            {
                string args = string.Format(argsFormat, this.CppFiles[i], this.ObjFiles[i]);
                Util.RunCommand(this.Config.Compiler, args);
            }
        }

        protected override void Link()
        {
            string argsFormat =
                string.Concat(this.Config.LinkerOptions.Select(x => " -" + x)) +
                " {0}" +
                string.Concat(this.Libraries.Select(x => " " + x)) +
                " -o {1}";

            string args = string.Format(argsFormat, string.Join(" ", this.ObjFiles), this.TargetFile);
            Util.RunCommand(this.Config.Linker, args);
        }

        protected override void LinkLib()
        {
            string argsFormat =
                string.Concat(this.Config.LibLinkerOptions.Select(x => " -" + x)) +
                " {1}" + " {0}" +
                string.Concat(this.Libraries.Select(x => " " + x));

            string args = string.Format(argsFormat, string.Join(" ", this.ObjFiles), this.TargetFile);
            Util.RunCommand(this.Config.LibLinker, args);
        }
    }

    public class MsvcCompiler : CppCompiler
    {
        public MsvcCompiler(CompilerConfig config) : base(config)
        {
        }

        protected override string ObjExt { get { return ".obj"; } }

        protected override string LibExt { get { return ".dll"; } }

        protected override string ExeExt { get { return ".exe"; } }

        protected override void Compile()
        {
        }

        protected override void Link()
        { }

        protected override void LinkLib()
        { }
    }

    public class LlvmCompiler : CppCompiler
    {
        public LlvmCompiler(CompilerConfig config) : base(config)
        {
        }

        protected override string ObjExt { get { return ".o"; } }

        protected override string LibExt { get { return ".a"; } }

        protected override string ExeExt { get { return ""; } }

        protected override void Compile()
        {
        }

        protected override void Link()
        { }

        protected override void LinkLib()
        { }
    }
}
