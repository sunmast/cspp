using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HappyCspp.Compiler
{
	public class VariableModel
	{
		public TypeInfo TypeInfo { get; private set; }
        public string Name { get; set; }
		public int ScopeDepth { get; set; }

        public VariableModel (TypeInfo typeInfo, string name, int depth)
		{
			this.TypeInfo = typeInfo;
            this.Name = name;
			this.ScopeDepth = depth;
		}
	}
}
