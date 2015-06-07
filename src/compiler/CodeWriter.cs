using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyCspp.Compiler
{
    class CodeWriter
    {
        private TextWriter writer;
        private Depth depth;

        private string Indention
        {
            get
            {
                int count = this.depth.Value + this.DepthAdjust;
                return new string('\t', count < 0 ? 0 : count);
            }
        }

        public int DepthAdjust { get; set; }

        public CodeWriter(TextWriter writter, Depth depth)
        {
            this.writer = writter;
            this.depth = depth;
        }

        public virtual void NewLine()
        {
            this.writer.WriteLine();
        }

        public virtual void WriteIndention()
        {
            this.writer.Write(this.Indention);
        }

        public virtual void Write(string format, params object[] args)
        {
            if (format == null) return;

            this.WriteIndention();
            this.writer.Write(format, args);
        }

        public virtual void WriteLine(string format, params object[] args)
        {
            if (format == null) return;

            this.WriteIndention();
            this.writer.WriteLine(format, args);
        }

        public virtual void Append(string format, params object[] args)
        {
            if (format == null) return;

            this.writer.Write(format, args);
        }

        public virtual void AppendLine(string format, params object[] args)
        {
            if (format == null) return;

            this.writer.WriteLine(format, args);
        }

        public class Depth
        {
            public int Value { get; private set; }

            public static Depth operator ++(Depth d)
            {
                d.Value++;
                return d;
            }

            public static Depth operator --(Depth d)
            {
                d.Value--;
                return d;
            }
        }
    }

    class NullCodeWriter : CodeWriter
    {
        public NullCodeWriter()
            : base(null, null)
        {
        }

        public override void NewLine()
        {
        }

        public override void WriteIndention()
        { 
        }

        public override void Write(string format, params object[] args)
        {
        }

        public override void WriteLine(string format, params object[] args)
        {
        }

        public override void Append(string format, params object[] args)
        {
        }

        public override void AppendLine(string format, params object[] args)
        {
        }
    }
}
