using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace HappyCspp.Compiler
{
    partial class CsWalker
    {
        internal void CompileMemberField(MemberFieldModel field)
        {
            this.hWriter.WriteIndention();
            this.hWriter.Append(field.GetModifiers());

            bool isWeakRef = Util.IsAttributeDefined(field.Attributes, "WeakRef");
            string declaration = this.SyntaxVariableDeclaration(field.Declaration.Declaration, field.Variable, true, true, field.IsStatic);

            if (isWeakRef && !declaration.StartsWith("_<"))
            {
                // WeakRef attribute can only be applied to reference types
                throw Util.NewSyntaxNotSupportedException(field.Declaration);
            }

            this.hWriter.Append(isWeakRef ? Consts.WeakPtr + declaration.Substring(1) : declaration);
            this.hWriter.AppendLine(";");

            if (field.Variable.Initializer != null && field.IsStatic)
            {
                this.cppWriter.WriteIndention();
                this.cppWriter.Append(this.SyntaxVariableDeclaration(field.Declaration.Declaration, field.Variable, true, false, !field.IsStatic));
                this.cppWriter.AppendLine(";");
            }
        }

        internal void CompileMemberEventField(MemberEventFieldModel eventField)
        {
            this.hWriter.WriteIndention();

            this.hWriter.Append(eventField.GetModifiers());

            this.hWriter.Append(this.SyntaxVariableDeclaration(eventField.Declaration.Declaration, eventField.Variable, true, true, eventField.IsStatic));
            this.hWriter.AppendLine(";");
        }

        internal void CompileMemberIndexer(MemberIndexerModel indexer)
        {
            BracketedParameterListSyntax indexerParams = indexer.Declaration.ParameterList;

            this.CompileMemberPropertyHeader(indexer, Consts.IndexOf, indexerParams);
            this.CompileMemberPropertyImpl(indexer, Consts.IndexOf, indexerParams);
        }

        internal void CompileMemberEventProperty(MemberEventPropertyModel property)
        {
            string propName = property.Declaration.Identifier.Text;

            this.CompileMemberPropertyHeader(property, propName, null);
            this.CompileMemberPropertyImpl(property, propName, null);
        }

        internal void CompileMemberProperty(MemberPropertyModel property)
        {
            string propName = property.Declaration.Identifier.Text;

            this.CompileMemberPropertyHeader(property, propName, null);
            this.CompileMemberPropertyImpl(property, propName, null);
        }

        internal void CompileMemberConstructor(MemberConstructorModel constructor)
        {
            this.CompileMemberMethodHeader(constructor);
            this.CompileMemberMethodImpl(constructor);
        }

        internal void CompileMemberDestructor(MemberDestructorModel destructor)
        {
            this.CompileMemberMethodHeader(destructor);
            this.CompileMemberMethodImpl(destructor);
        }

        internal void CompileMemberMethod(MemberMethodModel method)
        {
            if (method.IsStatic && method.Name == "Main")
            {
                var parameters = method.Declaration.ParameterList.Parameters;
				if (!method.IsStatic || parameters.Count != 1 || parameters[0].Type.ToString() != "string[]")
                {
					// TODO: check return type should be void or int
					// method.Type.Type.ToString() != "System.Int32" || 
					Console.WriteLine("Main function must be defined as: \"static int Main(string[])\" or \"static void Main(string[])\"");
                    throw Util.NewSyntaxNotSupportedException(method.Declaration);
                }

                this.mainMethod = method;
            }

            this.CompileMemberMethodHeader(method);
            this.CompileMemberMethodImpl(method);
        }

        internal void CompileMemberOperator(MemberOperatorModel op)
        {
            // TODO: support member operators
            //throw Util.NewSyntaxNotSupportedException(op.Declaration);
        }

        internal void CompileMemberConversionOperator(MemberConversionOperatorModel conv)
        {
            // TODO: support member conversion operators
            //throw Util.NewSyntaxNotSupportedException(conv.Declaration);
        }

        private void CompileMemberPropertyHeader<T>(MemberBasePropertyModel<T> property, string propName, BracketedParameterListSyntax indexerParams)
            where T : BasePropertyDeclarationSyntax
        {
            string typeName = this.WrapTypeName(property.Declaration.Type);
            string indexerParamsStr = this.SyntaxBracketedParameterList(indexerParams, true);

            string getterSignature = string.Format(
                "{0} {1}({2})", typeName, Consts.GetterPrefix + propName, indexerParamsStr);

            string setterSignature = string.Format(
                "void {1}({2}{0} value)", typeName, Consts.SetterPrefix + propName, indexerParamsStr == null ? null : indexerParamsStr + ", ");

            bool isAutoPropperty = false;

            if (property.IsAbstract || property.Declaration.Parent is InterfaceDeclarationSyntax)
            {
                foreach (var accessor in property.Declaration.AccessorList.Accessors)
                {
                    if (accessor.Body == null) isAutoPropperty = true;

                    if (accessor.Keyword.Text == "get")
                    {
                        this.hWriter.WriteLine("virtual {0} = 0;", getterSignature);
                    }
                    else if (accessor.Keyword.Text == "set")
                    {
                        this.hWriter.WriteLine("virtual {0} = 0;", setterSignature);
                    }
                    else
                    {
                        throw Util.NewSyntaxNotSupportedException(accessor.Keyword);
                    }
                }
            }
            else
            {
                foreach (var accessor in property.Declaration.AccessorList.Accessors)
                {
                    if (accessor.Body == null) isAutoPropperty = true;

                    if (accessor.Keyword.Text == "get")
                    {
                        this.hWriter.WriteIndention();

                        this.hWriter.Append(property.GetModifiers());
                        this.hWriter.AppendLine(getterSignature + ";");
                    }
                    else if (accessor.Keyword.Text == "set")
                    {
                        this.hWriter.WriteIndention();

                        this.hWriter.Append(property.GetModifiers());
                        this.hWriter.AppendLine(setterSignature + ";");
                    }
                    else
                    {
                        throw Util.NewSyntaxNotSupportedException(accessor.Keyword);
                    }
                }
            }

            if (isAutoPropperty)
            {
                this.typeModel.AutoProperties.Add(propName, property.Declaration.Type);
            }
        }

        private void CompileMemberPropertyImpl<T>(MemberBasePropertyModel<T> property, string propName, BracketedParameterListSyntax indexerParams)
            where T : BasePropertyDeclarationSyntax
        {
            string typeName = this.WrapTypeName(property.Declaration.Type);
            string indexerParamsStr = this.SyntaxBracketedParameterList(indexerParams, false);

            string getterSignature = string.Format(
                "{0} {2}::{1}({3})", typeName, Consts.GetterPrefix + propName, this.typeModel.Name, indexerParamsStr);

            string setterSignature = string.Format(
                "void {2}::{1}({3}{0} value)", typeName, Consts.SetterPrefix + propName, this.typeModel.Name, indexerParamsStr == null ? null : indexerParamsStr + ", ");

            foreach (var accessor in property.Declaration.AccessorList.Accessors)
            {
                if (accessor.Keyword.Text == "get")
                {
                    if (accessor.Body == null)
                    {
                        this.cppWriter.WriteLine("{1} {{ return {0}; }}", Consts.PropStoragePrefix + propName, getterSignature);
                    }
                    else
                    {
                        this.StatementBlockSyntax(getterSignature, this.cppWriter, null, accessor.Body);
                    }
                }
                else if (accessor.Keyword.Text == "set")
                {
                    if (accessor.Body == null)
                    {
                        this.cppWriter.WriteLine("{1} {{ {0} = value; }}", Consts.PropStoragePrefix + propName, setterSignature);
                    }
                    else
                    {
                        this.StatementBlockSyntax(setterSignature, this.cppWriter, null, accessor.Body);
                    }
                }
                else
                {
                    throw Util.NewSyntaxNotSupportedException(accessor.Keyword);
                }
            }
        }

        private void CompileMemberMethodHeader<T>(MemberBaseMethodModel<T> method)
            where T : BaseMethodDeclarationSyntax
        {
            this.hWriter.WriteLine(this.SyntaxTemplateTypeDeclaration(method.TypeParameterList));

            string returnType = this.WrapTypeName(method.ReturnType);
            if (returnType != null) returnType += " ";

            string signature = string.Format("{0}{1}{2}",
                    returnType,
                    method.Name,
                    this.SyntaxParameterList(method.Declaration.ParameterList, true));

            if (method.IsAbstract || method.Declaration.Parent is InterfaceDeclarationSyntax)
            {
                this.hWriter.WriteLine("virtual {0} = 0;", signature);
            }
            else
            {
                this.hWriter.WriteIndention();

                this.hWriter.Append(method.GetModifiers());
                this.hWriter.AppendLine(signature + ";");
            }
        }

        private void CompileMemberMethodImpl<T>(MemberBaseMethodModel<T> method)
            where T : BaseMethodDeclarationSyntax
        {
            string returnType = this.WrapTypeName(method.ReturnType);
            if (returnType != null) returnType += " ";

            string signature = string.Format("{0}{3}::{1}{2}",
                    returnType,
                    method.Name,
                    this.SyntaxParameterList(method.Declaration.ParameterList, false),
                    this.typeModel.Name);

            this.StatementBlockSyntax(signature, this.cppWriter, null, method.Declaration.Body);
        }
    }
}
