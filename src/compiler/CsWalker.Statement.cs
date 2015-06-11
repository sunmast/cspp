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
        private void StatementSyntax(StatementSyntax stmt, CodeWriter cw)
        {
            if (stmt == null) return;

            if (stmt is BlockSyntax)
            {
                cw.WriteIndention();
                this.StatementBlockSyntax(null, cw, null, (stmt as BlockSyntax).Statements.ToArray());
            }
            else if (stmt is LocalDeclarationStatementSyntax)
            {
                this.StatementLocalDeclarationSyntax(stmt as LocalDeclarationStatementSyntax, cw);
            }
            else if (stmt is ExpressionStatementSyntax)
            {
                this.StatementExpressionSyntax(stmt as ExpressionStatementSyntax, cw);
            }
            else if (stmt is EmptyStatementSyntax)
            {
				cw.Append (" ;");
            }
            else if (stmt is LabeledStatementSyntax)
            {
                this.StatementLabeledSyntax(stmt as LabeledStatementSyntax, cw);
            }
            else if (stmt is GotoStatementSyntax)
            {
                this.StatementGotoSyntax(stmt as GotoStatementSyntax, cw);
            }
            else if (stmt is BreakStatementSyntax)
            {
                cw.WriteLine("break;");
            }
            else if (stmt is ContinueStatementSyntax)
            {
                cw.WriteLine("continue;");
            }
            else if (stmt is ReturnStatementSyntax)
            {
                this.StatementReturnSyntax(stmt as ReturnStatementSyntax, cw);
            }
            else if (stmt is ThrowStatementSyntax)
            {
                this.StatementThrowSyntax(stmt as ThrowStatementSyntax, cw);
            }
            else if (stmt is YieldStatementSyntax)
            {
                throw Util.NewSyntaxNotSupportedException(stmt);
            }
            else if (stmt is WhileStatementSyntax)
            {
                this.StatementWhileSyntax(stmt as WhileStatementSyntax, cw);
            }
            else if (stmt is DoStatementSyntax)
            {
                this.StatementDoSyntax(stmt as DoStatementSyntax, cw);
            }
            else if (stmt is ForStatementSyntax)
            {
                this.StatementForSyntax(stmt as ForStatementSyntax, cw);
            }
            else if (stmt is ForEachStatementSyntax)
            {
                this.StatementForEachSyntax(stmt as ForEachStatementSyntax, cw);
            }
            else if (stmt is UsingStatementSyntax)
            {
                this.StatementUsingSyntax(stmt as UsingStatementSyntax, cw);
            }
            else if (stmt is FixedStatementSyntax)
            {
                throw Util.NewSyntaxNotSupportedException(stmt);
            }
            else if (stmt is CheckedStatementSyntax)
            {
                throw Util.NewSyntaxNotSupportedException(stmt);
            }
            else if (stmt is UnsafeStatementSyntax)
            {
                throw Util.NewSyntaxNotSupportedException(stmt);
            }
            else if (stmt is LockStatementSyntax)
            {
                this.StatementLockSyntax(stmt as LockStatementSyntax, cw);
            }
            else if (stmt is IfStatementSyntax)
            {
                this.StatementIfSyntax(stmt as IfStatementSyntax, cw);
            }
            else if (stmt is SwitchStatementSyntax)
            {
                this.StatementSwitchSyntax(stmt as SwitchStatementSyntax, cw);
            }
            else if (stmt is TryStatementSyntax)
            {
                this.StatementTrySyntax(stmt as TryStatementSyntax, cw);
            }
            else 
            {
                throw Util.NewSyntaxNotSupportedException(stmt);
            }
        }

        private void StatementBlockSyntax(string textBeforeBlock, CodeWriter cw, string append, params StatementSyntax[] statements)
        {
            if (statements != null && statements.Length == 1 && statements[0] is BlockSyntax)
            {
                this.StatementBlockSyntax(textBeforeBlock, cw, append, (statements[0] as BlockSyntax).Statements.ToArray());
                return;
            }

            this.memberModel.PushScope();
            cw.WriteLine(textBeforeBlock == null ? "{{" : textBeforeBlock + " {{");
            this.depth++;

            foreach (var stmt in statements)
            {
                this.StatementSyntax(stmt, cw);
            }

            this.depth--;
            cw.WriteLine(append == null ? "}}" : "}}" + append);
            this.memberModel.PopScope();
        }

        private void StatementForEachSyntax(ForEachStatementSyntax forEachStatement, CodeWriter cw)
        {
            this.StatementBlockSyntax(
                string.Format("for ({0}& {1} : {2})",
                    this.WrapTypeName(forEachStatement.Type),
                    forEachStatement.Identifier.Text,
                    this.ExprSyntax(forEachStatement.Expression, out dummyType)),
                cw, null,
                forEachStatement.Statement);
        }

        private void StatementUsingSyntax(UsingStatementSyntax usingStatement, CodeWriter cw)
        {
            cw.WriteLine("{{");
            this.depth++;
            this.memberModel.PushScope();

            List<string> list = new List<string>();

            if (usingStatement.Declaration != null && usingStatement.Expression == null)
            {
                // E.g. using (Foo f = new Foo()) {}
                string type = this.WrapTypeName(usingStatement.Declaration.Type);

                foreach (var usingObj in usingStatement.Declaration.Variables)
                {
                    string name = usingObj.Identifier.Text;
                    list.Add(name);

                    cw.WriteLine("{0} {1}{2};", type, name, this.SyntaxEqualsValueClause(usingObj.Initializer));
                }
            }
            else if (usingStatement.Expression != null && usingStatement.Declaration == null)
            {
                // E.g. Foo f = new Foo(); using (f) {}
                if (usingStatement.Expression is NameSyntax)
                {
                    string name = this.SyntaxName(usingStatement.Expression as NameSyntax);
                    list.Add(name);
                }
                else
                {
                    cw.WriteLine("auto _ = {0};", this.ExprSyntax(usingStatement.Expression, out this.dummyType));
                    list.Add("_");
                }
            }

            cw.Write("finally as([&] {{");
            this.depth++;

            for (int i = list.Count - 1; i >= 0; i--)
            {
                cw.Append(" {0}->Dispose();", list[i]);
            }

            this.depth--;
            cw.AppendLine(" }});");

            if (usingStatement.Statement is BlockSyntax)
            {
                foreach (var stmt in (usingStatement.Statement as BlockSyntax).Statements)
                {
                    this.StatementSyntax(stmt, cw);
                }
            }
            else
            {
                this.StatementSyntax(usingStatement.Statement, cw);
            }

            this.memberModel.PopScope();
            this.depth--;
            cw.WriteLine("}}");
        }

        private void StatementLockSyntax(LockStatementSyntax lockStatement, CodeWriter cw)
        {
            // Only support lock(this), lock(typeof(this)), and lock(mutex)
        }

        private void StatementLocalDeclarationSyntax(LocalDeclarationStatementSyntax localDeclaration, CodeWriter cw)
        {
            foreach (var variable in localDeclaration.Declaration.Variables)
            {
				TypeInfo t = this.semantic.GetTypeInfo(localDeclaration.Declaration.Type);
                this.memberModel.Variables.Push(new VariableModel(t, variable.Identifier.Text, this.memberModel.ScopeDepth));

                cw.WriteLine("{0}{1};",
                    this.SyntaxModifiers(localDeclaration.Modifiers),
                    this.SyntaxVariableDeclaration(localDeclaration.Declaration, variable, false, false, false));
            }
        }

        private void StatementExpressionSyntax(ExpressionStatementSyntax expression, CodeWriter cw)
        {
            cw.Write(this.ExprSyntax(expression.Expression, out dummyType));
            cw.AppendLine(";");
        }

        private void StatementLabeledSyntax(LabeledStatementSyntax labeledStatement, CodeWriter cw)
        {
            this.depth--;
            cw.WriteLine(labeledStatement.Identifier.Text + ":");
            this.depth++;

            this.StatementSyntax(labeledStatement.Statement, cw);
        }

        private void StatementGotoSyntax(GotoStatementSyntax gotoStatement, CodeWriter cw)
        {
            if (gotoStatement.CaseOrDefaultKeyword != null)
            {
                throw Util.NewSyntaxNotSupportedException(gotoStatement.CaseOrDefaultKeyword);
            }

            cw.WriteLine("goto {0};", this.ExprSyntax(gotoStatement.Expression, out dummyType));
        }

        private void StatementReturnSyntax(ReturnStatementSyntax returnStatement, CodeWriter cw)
        {
            string str = this.ExprSyntax(returnStatement.Expression, out dummyType);
            cw.WriteLine("return{0};", str == null ? null : " " + str);
        }

        private void StatementThrowSyntax(ThrowStatementSyntax throwStatement, CodeWriter cw)
        {
            string str = this.ExprSyntax(throwStatement.Expression, out dummyType);
            cw.WriteLine("throw{0};", str == null ? null : " " + str);
        }

        private void StatementWhileSyntax(WhileStatementSyntax whileStatement, CodeWriter cw)
        {
            this.StatementBlockSyntax(string.Format("while ({0})", this.ExprSyntax(whileStatement.Condition, out dummyType)), cw, null, whileStatement.Statement);
        }

        private void StatementDoSyntax(DoStatementSyntax doStatement, CodeWriter cw)
        {
            this.StatementBlockSyntax("do", cw, null, doStatement.Statement);
            cw.WriteLine("while ({0});", this.ExprSyntax(doStatement.Condition, out dummyType));
        }

        private void StatementForSyntax(ForStatementSyntax forStatement, CodeWriter cw)
        {
            if (forStatement.Initializers != null && forStatement.Initializers.Count > 0)
            {
                throw Util.NewSyntaxNotSupportedException(forStatement.Initializers);
            }
                
            foreach (var variable in forStatement.Declaration.Variables)
            {
				TypeInfo t = this.semantic.GetTypeInfo(forStatement.Declaration.Type);
                this.memberModel.Variables.Push(new VariableModel(t, variable.Identifier.Text, this.memberModel.ScopeDepth + 1));
            }

            this.StatementBlockSyntax(
                string.Format("for ({0}; {1}; {2})",
                    this.SyntaxSingleLineVariableDeclaration(forStatement.Declaration),
                    this.ExprSyntax(forStatement.Condition, out dummyType),
                    this.SyntaxSeparatedSyntaxList(forStatement.Incrementors, (s) => this.ExprSyntax(s, out dummyType))),
                cw, null,
                forStatement.Statement);
        }

        private void StatementIfSyntax(IfStatementSyntax ifStatement, CodeWriter cw)
        {
            this.StatementBlockSyntax(string.Format("if ({0})", this.ExprSyntax(ifStatement.Condition, out dummyType)), cw, null, ifStatement.Statement);
            this.StatementElseSyntax(ifStatement.Else, cw);
        }

        private void StatementElseSyntax(ElseClauseSyntax elseClause, CodeWriter cw)
        {
            if (elseClause == null) return;

            this.StatementBlockSyntax("else", cw, null, elseClause.Statement);
        }

        private void StatementSwitchSyntax(SwitchStatementSyntax switchStatement, CodeWriter cw)
        {
            cw.WriteLine("switch ({0}) {{", this.ExprSyntax(switchStatement.Expression, out dummyType));

            foreach (var section in switchStatement.Sections)
            {
                foreach (var label in section.Labels)
                {
                    if (label is CaseSwitchLabelSyntax)
                    {
                        cw.WriteLine("case {0}:", this.ExprSyntax((label as CaseSwitchLabelSyntax).Value, out dummyType));
                    }
                    else if (label is DefaultSwitchLabelSyntax)
                    {
                        cw.WriteLine("default:");
                    }
                    else
                    {
                        throw Util.NewSyntaxNotSupportedException(label);
                    }
                }

                this.depth++;

                foreach (var statement in section.Statements)
                {
                    this.StatementSyntax(statement, cw);
                }

                this.depth--;
            }

            cw.WriteLine("}}");
        }

        private void StatementTrySyntax(TryStatementSyntax tryStatement, CodeWriter cw)
        {
            bool hasFinallyBlock = tryStatement.Finally.Block != null;

            if (hasFinallyBlock)
            {
                cw.WriteLine("{{");

                this.depth++;
                this.memberModel.PushScope();

                this.StatementBlockSyntax("finally as([&]", cw, ");", tryStatement.Finally.Block);
            }

            if (tryStatement.Catches.Count > 0)
            {
                this.StatementBlockSyntax("try", cw, null, tryStatement.Block);
            }
            else
            {
                // Try block with no catch blocks, but only a finally block
                // Only need to output statements in try
                foreach (var stmt in tryStatement.Block.Statements)
                {
                    this.StatementSyntax(stmt, cw);
                }
            }

            foreach (var catchClause in tryStatement.Catches)
            {
                if (catchClause.Filter != null)
                {
                    throw Util.NewSyntaxNotSupportedException(catchClause);
                }

                string catchSyntax;

                if (catchClause.Declaration != null)
                {
                    if (catchClause.Declaration.Identifier != null)
                    {
                        // E.g. catch (Exception ex) {}
                        catchSyntax = string.Format("catch ({0}& {1})",
                            this.WrapTypeName(catchClause.Declaration.Type),
                            catchClause.Declaration.Identifier.Text);
                    }
                    else
                    {
                        // E.g. catch (Exception) {}
                        catchSyntax = string.Format("catch ({0}&)",
                            this.WrapTypeName(catchClause.Declaration.Type));
                    }
                }
                else
                {
                    // E.g. catch {}
                    catchSyntax = "catch (...)";
                }

                this.StatementBlockSyntax(catchSyntax, cw, null, catchClause.Block);
            }

            if (hasFinallyBlock)
            {
                this.memberModel.PopScope();
                this.depth--;
                cw.WriteLine("}}");
            }
        }
    }
}
