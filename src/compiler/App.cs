﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HappyCspp.Compiler
{
    static class App
    {
        public static bool PreferWideChar;
        public static CSharpCompilation Compilation;
        public static INamedTypeSymbol ArrayType;

        static void PrintUsage()
        {
            Console.WriteLine("Syntax: cspp <*.csproj file> <prefer wide char>");
            Console.WriteLine("E.g. cspp test.csproj true");
        }

        static int Main(string[] args)
        {
            Console.Title = "Happy CS++";

            if (args.Length < 1)
            {
                PrintUsage();
                return 1;
            }

            string csprojFile = args[0];

            if (!File.Exists(csprojFile))
            {
                Console.WriteLine("Unable to load the C# project file '{0}'!", args[0]);
                PrintUsage();
                return 1;
            }

            string cbpFile = Path.ChangeExtension(csprojFile, ".cbp");
            if (File.Exists(cbpFile))
            {
                Console.WriteLine("Code::Blocks C++ project file '{0}' found", Path.GetFileName(cbpFile));
            }
            else
            {
                cbpFile = null;
            }

            if (args.Length > 1)
            {
                bool.TryParse(args[1], out App.PreferWideChar);
            }

            CsProject csproj = new CsProject(args[0]);

            if (!Directory.Exists(csproj.DestinationFolder))
            {
                Directory.CreateDirectory(csproj.DestinationFolder);
            }

            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
            foreach (string file in csproj.SourceFiles)
            {
                string src = File.ReadAllText(file, Encoding.UTF8);
                syntaxTrees.Add(CSharpSyntaxTree.ParseText(src, CSharpParseOptions.Default, file));
            }

            if (syntaxTrees.Count == 0)
            {
                Console.Error.WriteLine("No *.cs file found in any C# source folder. Nothing to compile!");
                return 2;
            }

            List<PortableExecutableReference> references = new List<PortableExecutableReference>();
            foreach (string lib in csproj.ReferencedLibraries)
            {
                references.Add(MetadataReference.CreateFromFile(lib));
            }

            App.Compilation = CSharpCompilation.Create("noname", syntaxTrees, references);
            App.ArrayType = App.Compilation.GetTypeByMetadataName("System.GenericArray`1") ?? App.Compilation.GetTypeByMetadataName("System.Array");

            AttributeListSyntax assemblyAttributes = null;
            SortedDictionary<string, TypeModel> knownTypes = new SortedDictionary<string, TypeModel>();

            foreach (SyntaxTree syntaxTree in syntaxTrees)
            {
                SemanticModel semanticModel = App.Compilation.GetSemanticModel(syntaxTree);
                SyntaxNode stRoot = syntaxTree.GetRoot();

                var rootNodes = stRoot.ChildNodes();
                List<UsingDirectiveSyntax> usings = new List<UsingDirectiveSyntax>();

                foreach (var node in rootNodes)
                {
                    if (node is UsingDirectiveSyntax)
                    {
                        usings.Add(node as UsingDirectiveSyntax);
                    }
                    else if (node is NamespaceDeclarationSyntax)
                    {
                        foreach (var nsChildNode in node.ChildNodes())
                        {
                            if (nsChildNode is UsingDirectiveSyntax)
                            {
                                usings.Add(nsChildNode as UsingDirectiveSyntax);
                            }
                            else if (nsChildNode is IdentifierNameSyntax)
                            {
                                Console.WriteLine(nsChildNode.ToString());
                            }
                            else if (nsChildNode is QualifiedNameSyntax)
                            { 
                                Console.WriteLine(nsChildNode.ToString());
                            }
                            else
                            {
                                var tm = TypeModel.Create(node as NamespaceDeclarationSyntax, nsChildNode, usings, semanticModel);
                                if (!knownTypes.ContainsKey(tm.FullName))
                                    knownTypes.Add(tm.FullName, tm);
                            }
                        }
                    }
                    else if (node is AttributeListSyntax)
                    {
                        var attributeListSyntax = node as AttributeListSyntax;

                        if (assemblyAttributes == null)
                        {
                            assemblyAttributes = attributeListSyntax;
                        }
                        else
                        {
                            foreach (var attribute in attributeListSyntax.Attributes)
                            {
                                assemblyAttributes.AddAttributes(attribute);
                            }
                        }
                    }
                    else
                    {
                        // Type with no namespace
                        var tm = TypeModel.Create(null, node, usings, semanticModel);
                        if (tm != null && !knownTypes.ContainsKey(tm.FullName))
                            knownTypes.Add(tm.FullName, tm);
                    }
                }
            }

            string headersFile = csproj.Name + ".h";

            foreach (var kvp in knownTypes)
            {
                kvp.Value.Compiler = new CsWalker(kvp.Value);
            }

            List<string> generatedFiles = new List<string>();
            generatedFiles.Add(headersFile);

            using (FileStream hFileStream = new FileStream(Path.Combine(csproj.DestinationFolder, headersFile), FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter hWriter = new StreamWriter(hFileStream, Encoding.UTF8))
            {
                hWriter.WriteLine("#pragma once");
                hWriter.WriteLine();

                hWriter.WriteLine("#include <cspp.h>");

                foreach (string lib in csproj.ReferencedLibraries)
                {
                    hWriter.WriteLine("#include <{0}.h>", Path.GetFileNameWithoutExtension(lib));
                }

                hWriter.WriteLine();

                int i = 0, j = 0;
                string lastNs = null;

                // Forward declare all classes in all headers file
                foreach (var kvp in knownTypes)
                {
                    TypeModel typeModel = kvp.Value;

                    if (typeModel.IsImported)
                        continue;

                    if (i != 0 && lastNs != typeModel.NsName && j > 0)
                    {
                        hWriter.WriteLine(new string('}', j));
                    }

                    if (lastNs != typeModel.NsName && typeModel.NsNameParts != null)
                    {
                        foreach (string n in typeModel.NsNameParts)
                        {
                            hWriter.Write("namespace {0} {{ ", n);
                        }

                        j = typeModel.NsNameParts.Length;

                        hWriter.WriteLine();
                    }

                    if (typeModel is ClassModel)
                    {
                        hWriter.WriteLine("class {0};", (typeModel as ClassModel).ClassDeclaration.Identifier.Text);
                    }
                    else if (typeModel is StructModel)
                    {
                        hWriter.WriteLine("struct {0};", (typeModel as StructModel).StructDeclaration.Identifier.Text);
                    }
                    else if (typeModel is InterfaceModel)
                    {
                        hWriter.WriteLine("class {0};", (typeModel as InterfaceModel).InterfaceDeclaration.Identifier.Text);
                    }
                    else if (typeModel is EnumModel)
                    {
                        hWriter.WriteLine("struct {0};", (typeModel as EnumModel).EnumDeclaration.Identifier.Text);
                    }
                    else if (typeModel is DelegateModel)
                    {
                        // TODO
                        hWriter.WriteLine("class {0};", (typeModel as DelegateModel).DelegateDeclaration.Identifier.Text);
                    }

                    lastNs = typeModel.NsName;
                    i++;
                }

                if (j > 0)
                {
                    hWriter.WriteLine(new string('}', j));
                }

                i = j = 0;
                lastNs = null;

                // Generate type declaratios and implementions
                // Also updates .cbp file for Code::Blocks IDE if it exists
                // Otherwise it only generates .h and .cpp files
                foreach (var kvp in knownTypes)
                {
                    TypeModel typeModel = kvp.Value;

                    if (typeModel.IsImported)
                        continue;

                    if (i != 0 && lastNs != typeModel.NsName && j > 0)
                    {
                        hWriter.WriteLine(new string('}', j));
                    }

                    hWriter.WriteLine();

                    if (lastNs != typeModel.NsName && typeModel.NsNameParts != null)
                    {
                        foreach (string n in typeModel.NsNameParts)
                        {
                            hWriter.Write("namespace {0} {{ ", n);
                        }

                        j = typeModel.NsNameParts.Length;

                        hWriter.WriteLine();
                        hWriter.WriteLine();
                    }

                    CodeWriter.Depth depth = new CodeWriter.Depth();

                    if (typeModel is ClassModel || typeModel is StructModel)
                    {
                        string fileName = typeModel.FullName.Replace("::", ".") + ".cpp";
                        if (fileName.StartsWith(csproj.DefaultNamespace))
                        {
                            // Omit default NS on file name
                            fileName = fileName.Substring(csproj.DefaultNamespace.Length + 1);
                        }

                        string cppFile = Path.Combine(csproj.DestinationFolder, fileName);
                        generatedFiles.Add(fileName);

                        using (FileStream cppFileStream = new FileStream(cppFile, FileMode.Create, FileAccess.Write, FileShare.None))
                        using (StreamWriter cppWriter = new StreamWriter(cppFileStream, Encoding.UTF8))
                        {
                            cppWriter.WriteLine("#include \"{0}\"", headersFile);

                            typeModel.Compiler.Compile(new CodeWriter(hWriter, depth), new CodeWriter(cppWriter, depth), depth);
                        }
                    }
                    else
                    {
                        typeModel.Compiler.Compile(new CodeWriter(hWriter, depth), new NullCodeWriter(), depth);
                    }

                    lastNs = typeModel.NsName;
                    i++;
                }

                if (j > 0)
                {
                    hWriter.WriteLine(new string('}', j));
                }
            }

//            if (cbpFile != null)
//            {
//                CbProject cbp = new CbProject(cbpFile);
//                cbp.UpdateSourceFiles(generatedFiles);
//            }

            Console.WriteLine("Done!");

            return 0;
        }
    }
}
