using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;

namespace HappyCspp.Compiler
{
    partial class CsWalker
    {
        private string ExprSyntax(ExpressionSyntax expr, bool isLeftValue = false)
        {
            TypeInfo dummyType;
            return this.ExprSyntax(expr, out dummyType, isLeftValue);
        }

        private string ExprSyntax(ExpressionSyntax expr, out TypeInfo exprType, bool isLeftValue = false)
        {
            if (expr == null)
            {
                exprType = new TypeInfo();
                return null;
            }
				
            if (expr is TypeSyntax)
            {
                string name = this.SyntaxType(expr as TypeSyntax);
                foreach (var variable in this.memberModel.Variables)
                {
                    if (variable.Name == name)
                    {
                        exprType = variable.TypeInfo;
                        return name;
                    }
                }

                foreach (var member in this.typeModel.Members)
                {
                    if (member.Name == name)
                    {
                        exprType = member.Type;
                        return member.Name;
                    }
                }

                exprType = this.semantic.GetTypeInfo(expr);
                if (exprType.Type == null)
                {
                    // Should be a part of NS
                    return name;
                }
                else
                {
                    return typeWrapper.Wrap(exprType, false);
                }
            }
            else if (expr is ParenthesizedExpressionSyntax)
            {
                return this.ExprParenthesizedSyntax(expr as ParenthesizedExpressionSyntax, out exprType);
            }
            else if (expr is PrefixUnaryExpressionSyntax)
            {
                return this.ExprPrefixUnarySyntax(expr as PrefixUnaryExpressionSyntax, out exprType);
            }
            else if (expr is AwaitExpressionSyntax)
            {
                throw Util.NewSyntaxNotSupportedException(expr);
            }
            else if (expr is PostfixUnaryExpressionSyntax)
            {
                return this.ExprPostfixUnarySyntax(expr as PostfixUnaryExpressionSyntax, out exprType);
            }
            else if (expr is MemberAccessExpressionSyntax)
            {
                this.memberAccessDepth++;
                string ret = this.ExprMemberAccessSyntax(expr as MemberAccessExpressionSyntax, isLeftValue, out exprType);
                this.memberAccessDepth--;

                return ret;
            }
            else if (expr is ConditionalAccessExpressionSyntax)
            {
            }
            else if (expr is MemberBindingExpressionSyntax)
            {
            }
            else if (expr is ElementBindingExpressionSyntax)
            {
                this.ExprElementBindingSyntax(expr as ElementBindingExpressionSyntax);
            }
            else if (expr is ImplicitElementAccessSyntax)
            {
            }
            else if (expr is BinaryExpressionSyntax)
            {
                return this.ExprBinarySyntax(expr as BinaryExpressionSyntax, out exprType);
            }
            else if (expr is AssignmentExpressionSyntax)
            {
                return this.ExprAssignmentSyntax(expr as AssignmentExpressionSyntax, out exprType);
            }
            else if (expr is ConditionalExpressionSyntax)
            {
                return this.ExprConditionalSyntax(expr as ConditionalExpressionSyntax, out exprType);
            }
            else if (expr is ThisExpressionSyntax)
            {
                exprType = this.semantic.GetTypeInfo(expr);
                return "this";
            }
            else if (expr is BaseExpressionSyntax)
            {
                throw Util.NewSyntaxNotSupportedException(expr);
            }
            else if (expr is LiteralExpressionSyntax)
            {
                return this.ExprLiteralSyntax(expr as LiteralExpressionSyntax, out exprType);
            }
            else if (expr is MakeRefExpressionSyntax)
            {
            }
            else if (expr is RefTypeExpressionSyntax)
            {
            }
            else if (expr is RefValueExpressionSyntax)
            {
            }
            else if (expr is CheckedExpressionSyntax)
            {
            }
            else if (expr is DefaultExpressionSyntax)
            {
                return this.ExprDefaultSyntax(expr as DefaultExpressionSyntax, out exprType);
            }
            else if (expr is TypeOfExpressionSyntax)
            {
            }
            else if (expr is SizeOfExpressionSyntax)
            {
                
            }
            else if (expr is InvocationExpressionSyntax)
            {
                return this.ExprInvocationSyntax(expr as InvocationExpressionSyntax, out exprType);
            }
            else if (expr is ElementAccessExpressionSyntax)
            {
                return ExprElementAccessSyntax(expr as ElementAccessExpressionSyntax, isLeftValue, out exprType);
            }
            else if (expr is CastExpressionSyntax)
            {
                return this.ExprCastSyntax(expr as CastExpressionSyntax, out exprType);
            }
            else if (expr is AnonymousMethodExpressionSyntax)
            {
                return this.ExprAnonymousMethodSyntax(expr as AnonymousMethodExpressionSyntax);
            }
            else if (expr is SimpleLambdaExpressionSyntax)
            {
                return this.ExprSimpleLambdaSyntax(expr as SimpleLambdaExpressionSyntax);
            }
            else if (expr is ParenthesizedLambdaExpressionSyntax)
            {
                return this.ExprParenthesizedLambdaSyntax(expr as ParenthesizedLambdaExpressionSyntax);
            }
            else if (expr is InitializerExpressionSyntax)
            {
                return this.ExprInitializerSyntax(expr as InitializerExpressionSyntax, true, out exprType);
            }
            else if (expr is ObjectCreationExpressionSyntax)
            {
                return this.ExprObjectCreationSyntax(expr as ObjectCreationExpressionSyntax, out exprType);
            }
            else if (expr is AnonymousObjectCreationExpressionSyntax)
            {
            }
            else if (expr is ArrayCreationExpressionSyntax)
            {
                return this.ExprArrayCreationSyntax(expr as ArrayCreationExpressionSyntax, out exprType);
            }
            else if (expr is ImplicitArrayCreationExpressionSyntax)
            {
            }
            else if (expr is StackAllocArrayCreationExpressionSyntax)
            {
            }
            else if (expr is QueryExpressionSyntax)
            {
            }
            else if (expr is OmittedArraySizeExpressionSyntax)
            {
                exprType = new TypeInfo();
                return null;
            }
            else if (expr is InterpolatedStringExpressionSyntax)
            {
            }

            Console.WriteLine("{0} >> {1}", expr.GetType().Name, expr.ToString());
            throw Util.NewSyntaxNotSupportedException(expr);
        }

        private string ExprLiteralSyntax(LiteralExpressionSyntax literalExpression, out TypeInfo exprType)
        {
            // E.g. "xyz" @"xyz" 1 2.0f 3.8d
            var literal = literalExpression.Token;
            if (literal.Value == null)
            {
                exprType = new TypeInfo();
                return "nullptr";
            }

            exprType = this.semantic.GetTypeInfo(literalExpression);

            string txt = literal.Text;

            if (literal.Value is string)
            {
                if (txt.StartsWith("@"))
                {
                    // Compile to C++ raw string syntax
                    txt = literal.Value as string;

                    int c = 0;
                    string delimiter;
                    while (txt.Contains(")" + (delimiter = new string('_', c))))
                    {
                        c++;
                    }

                    txt = string.Format("R\"{0}({1}){0}\"", delimiter, txt);
                }

                return config.PreferWideChar ? "L" + txt : txt;
            }
            else
            {
                // double x = 0.618d ==> the d or D is C++ is not needed (and double is default if suffix is omitted)
                return txt.EndsWith("d", StringComparison.OrdinalIgnoreCase) ? txt.Substring(0, txt.Length - 1) : txt;
            }
        }

        private string ExprParenthesizedSyntax(ParenthesizedExpressionSyntax parenthesizedExpression, out TypeInfo exprType)
        {
            // E.g. (x)
            return "(" + this.ExprSyntax(parenthesizedExpression.Expression, out exprType) + ")";
        }

        private string ExprPrefixUnarySyntax(PrefixUnaryExpressionSyntax prefixUnaryExpression, out TypeInfo exprType)
        {
            // E.g. +x
            return prefixUnaryExpression.OperatorToken.Text + this.ExprSyntax(prefixUnaryExpression.Operand, out exprType);
        }

        private string ExprPostfixUnarySyntax(PostfixUnaryExpressionSyntax postfixUnaryExpression, out TypeInfo exprType)
        {
            // E.g. -x
            return this.ExprSyntax(postfixUnaryExpression.Operand, out exprType) + postfixUnaryExpression.OperatorToken.Text;
        }

        private string ExprMemberAccessSyntax(MemberAccessExpressionSyntax memberAccessExpression, bool isLeftValue, out TypeInfo exprType)
        {
            // E.g. this.foo(); variableA.memberB.memberC; classA.staticMemberB; enumType.enumValue
            if (memberAccessExpression.Expression is LiteralExpressionSyntax)
            {
                // E.g. "foo".X ; 5.ToString
                throw Util.NewSyntaxNotSupportedException(memberAccessExpression);
            }

            ExpressionSyntax leftExpr = memberAccessExpression.Expression;
            string left = this.ExprSyntax(leftExpr, out exprType);
            string op;
            if (left == "this")
            {
                op = "->";
            }
            else if (exprType.Type == null)
            {
                op = "::";
            }
            else
            {
                op = exprType.Type.IsValueType || exprType.Type.Name == "String" ? "." : "->";
            }

            string right = memberAccessExpression.Name.Identifier.Text;
            ImmutableArray<ISymbol> members = ImmutableArray<ISymbol>.Empty;

            if (exprType.Type == null)
            {
                // Left is NS, right must be NS or type
                exprType = this.semantic.GetTypeInfo(memberAccessExpression);
                if (exprType.Type != null)
                {
                    // It's a type
                    return typeWrapper.Wrap(exprType, false);
                }
                else
                {
                    // It's a NS
                    return left + "::" + right;
                }
            }
            else if (exprType.Type is IArrayTypeSymbol)
            {
                members = App.ArrayType.GetMembers(right);
            }
            else if (exprType.Type is INamespaceOrTypeSymbol)
            {
                members = (exprType.Type as INamespaceOrTypeSymbol).GetMembers(right);
            }
            else
            {
                Util.NewSyntaxNotSupportedException(memberAccessExpression);
            }
				
            foreach (var member in members)
            {
                string alias = Util.GetSymbolAlias(config.PreferWideChar, member.GetAttributes());

                if (member.IsStatic)
                    op = "::";

                // Properties: compile to get_X / set_X
                // Members with aliases: compiled to alias
                // Properties with alias: compiled to alias as-is. No get_/set_ prefixes
                if (member is IFieldSymbol)
                {
                    right = alias ?? member.Name;
                }
                else if (member is IPropertySymbol)
                {
                    if (!isLeftValue || this.memberAccessDepth > 1)
                    {
                        right = (alias ?? Consts.GetterPrefix + member.Name) + "()";
                    }
                    else
                    {
                        right = (alias ?? Consts.SetterPrefix + member.Name) + "("; // x.Prop = Y; will be compiled to x.set_Prop(Y);
                    }
                }
                else if (member is IMethodSymbol)
                {
                    right = alias ?? member.Name;
                }
                else
                {
                    throw Util.NewSyntaxNotSupportedException(memberAccessExpression);
                }

                break;
            }

            exprType = this.semantic.GetTypeInfo(memberAccessExpression.Name);
            return string.IsNullOrEmpty(left) ? right : (left + op + right);
        }

        private string ExprDefaultSyntax(DefaultExpressionSyntax defaultExpression, out TypeInfo exprType)
        {
            // E.g. default(T)
            exprType = this.semantic.GetTypeInfo(defaultExpression);

            if (!exprType.Type.IsValueType)
            {
                return "nullptr";
            }
            else
            {
                switch (exprType.Type.SpecialType)
                {
                    case SpecialType.System_Boolean:
                        return "false";
                    case SpecialType.System_Byte:
                    case SpecialType.System_SByte:
                    case SpecialType.System_Char:
                    case SpecialType.System_Int16:
                    case SpecialType.System_UInt16:
                    case SpecialType.System_Int32:
                    case SpecialType.System_UInt32:
                    case SpecialType.System_Int64:
                    case SpecialType.System_UInt64:
                    case SpecialType.System_Single:
                    case SpecialType.System_Double:
                    case SpecialType.System_Enum:
                        return "0";
                    default:
                        // Call default constructor on the struct
                        // Trick:
                        // C# doesn't allow default constructor definition on structs
                        // But we alwalys generate one for struct in C++, which is: memset(this, 0, sizeof(*this))
                        // Thus whenever a struct is created, its members are zeroed - same as C# semantic
                        // Because of this, any imported struct must have a default constructor too. Otherwise, import as class
                        // Note:
                        // We cannot use {} to zero members for all structs. It only works with POD structs
                        return typeWrapper.Wrap(exprType, false) + "()";
                }
            }
        }

        private string ExprAssignmentSyntax(AssignmentExpressionSyntax assignmentExpression, out TypeInfo exprType)
        {
            // E.g. x = y, x += y, x %= y, etc.
            string op = assignmentExpression.OperatorToken.Text;
            string left = this.ExprSyntax(assignmentExpression.Left, out exprType, true);
            string right = this.ExprSyntax(assignmentExpression.Right, false);

            if (op == "=>")
            {
                // Only support x= syntax like +=, -=, etc.
                throw Util.NewSyntaxNotSupportedException(assignmentExpression);
            }

            bool leftIsProperty = left.EndsWith("(") || left.EndsWith(", ");
            if (!leftIsProperty)
            {
                return string.Format("{0} {1} {2}", left, op, right);
            }

            switch (op)
            {
                case "=":
                    return left + right + ")";
                default:
                    string leftAtRight = this.ExprSyntax(assignmentExpression.Left, out exprType, false);
                    string opop = op.Substring(0, op.Length - 1); // e.g. += --> +
                    return string.Format("{0}{1} {2} {3})", left, leftAtRight, opop, right); // a.PropX += 1 --> a.set_PropX(a.get_PropX() + 1)
            }
        }

        private string ExprCastSyntax(CastExpressionSyntax castExpression, out TypeInfo exprType)
        {
            // E.g. (int)3.5f
            exprType = this.semantic.GetTypeInfo(castExpression);
            return string.Format("({0}){1}", typeWrapper.Wrap(exprType, true), this.ExprSyntax(castExpression.Expression));
        }

        private string ExprArrayCreationSyntax(ArrayCreationExpressionSyntax arrayCreationExpression, out TypeInfo exprType)
        {
            // E.g. new int[x]
            exprType = this.semantic.GetTypeInfo(arrayCreationExpression.Type);

            // Get size information from the first rank (only first rank can define size in C# syntax)
            var firstRank = arrayCreationExpression.Type.RankSpecifiers.First();

            List<string> sizes = new List<string>();
            if (firstRank.Sizes.First() is OmittedArraySizeExpressionSyntax)
            {
                // Get sizes from initializers
                if (arrayCreationExpression.Initializer != null)
                {
                    var initializer = arrayCreationExpression.Initializer;
                    do
                    {
                        sizes.Add(initializer.Expressions.Count.ToString());
                        initializer = initializer.Expressions.First() as InitializerExpressionSyntax;
                    }
                    while(initializer != null);
                }
                else
                {
                    // An initializer is expected if the size is not defined when creating an array
                    throw Util.NewSyntaxNotSupportedException(arrayCreationExpression);
                }
            }
            else
            {
                // Get user defined sizes
                foreach (var size in firstRank.Sizes)
                {
                    sizes.Add(this.ExprSyntax(size));
                }
            }

            TypeInfo dummyType;

            return string.Format("new_<{0}>({1}){2}",
                this.SyntaxArrayType(arrayCreationExpression.Type),
                string.Join(", ", sizes),
                arrayCreationExpression.Initializer != null ? "->init({ " + this.ExprInitializerSyntax(arrayCreationExpression.Initializer, false, out dummyType) + " })" : null);
        }

        private string ExprInitializerSyntax(InitializerExpressionSyntax initializerExpression, bool withBrankets, out TypeInfo exprType)
        {
            // E.g. x, y, z
            List<string> list = new List<string>();

            foreach (var expr in initializerExpression.Expressions)
            {
                if (expr is InitializerExpressionSyntax)
                {
                    list.Add(this.ExprInitializerSyntax(expr as InitializerExpressionSyntax, withBrankets, out exprType));
                }
                else
                {
                    list.Add(this.ExprSyntax(expr));
                }
            }

            exprType = this.semantic.GetTypeInfo(initializerExpression);

            if (withBrankets)
            {
                return list.Count == 0 ? "{ }" : "{ " + string.Join(", ", list) + " }";
            }
            else
            {
                return list.Count == 0 ? null : string.Join(", ", list);
            }
        }

        private string ExprBinarySyntax(BinaryExpressionSyntax binaryExpression, out TypeInfo exprType)
        {
            // E.g. x + y; x == y
            exprType = this.semantic.GetTypeInfo(binaryExpression);

            TypeInfo leftType, rightType;

            string left = this.ExprSyntax(binaryExpression.Left, out leftType);
            string right = this.ExprSyntax(binaryExpression.Right, out rightType);
            string op = binaryExpression.OperatorToken.Text;

            if (!(op == "==" || op == "!=") || (leftType.Type.IsValueType && rightType.Type.IsValueType))
            {
                // The operator is not == or !=, or both side are value type
                goto DEFAULT_RETURN;
            }

            if (leftType.Type.IsValueType != rightType.Type.IsValueType)
            {
                // One side is reference type but the other is not:
                // There is an operator defined in the reference type, as we don't allow operators defined on value types comparing with reference types
                goto DEFAULT_RETURN;
            }
                
            // The problem:
            // Reference types will be wrapped in a template class called sptr to manage memory automatically
            // So when comparing instances of reference types, we are actually comparing instances of sptr
            // We have defined all possible operators in sptr template, but == and != becomes a problem:
            // Given binary syntax a == b or a != b:
            // 1) It could mean to check the reference (the memory address) of a and b is same or not
            // 2) It could also mean to call the operator == or != on a or b if the operator is defined
            // Thus it's not possible to define operators in sptr to handle both cases
            // Infact the operators defined in sptr only handle 2nd case
            // For 1st case, we should compare sptr.ptr instead

            if (right == "nullptr")
            {
                left = left + ".ptr";
                goto DEFAULT_RETURN;
            }

            if (left == "nullptr")
            {
                right = right + ".ptr";
                goto DEFAULT_RETURN;
            }

            string opName = op == "==" ? "op_Equality" : "op_Inequality";

            var leftMembers = leftType.Type.GetMembers();
            foreach (var member in leftMembers)
            {
                if (member is IMethodSymbol)
                {
                    IMethodSymbol method = member as IMethodSymbol;
                    if (method.MethodKind == MethodKind.UserDefinedOperator && method.Name == opName)
                    {
                        // The left type has op_Equality or op_Inequality
                        goto DEFAULT_RETURN;
                    }
                }
            }

            var rightMembers = rightType.Type.GetMembers();
            foreach (var member in rightMembers)
            {
                if (member is IMethodSymbol)
                {
                    IMethodSymbol method = member as IMethodSymbol;
                    if (method.MethodKind == MethodKind.UserDefinedOperator && method.Name == opName)
                    {
                        // The right type has op_Equality or op_Inequality
                        goto DEFAULT_RETURN;
                    }
                }
            }

            // No operator defined in both sides, compare sptr.ptr only
            return string.Format("{0}.ptr {1} {2}.ptr", left, op, right);

            DEFAULT_RETURN:
            return string.Format("{0} {1} {2}", left, op, right);
        }

        private string ExprInvocationSyntax(InvocationExpressionSyntax invocationExpression, out TypeInfo exprType)
        {
            // E.g. X(y, z)
            string left = this.ExprSyntax(invocationExpression.Expression, out exprType);

            return string.Format("{0}({1})",
                left,
                this.SyntaxArgumentList(invocationExpression.ArgumentList));
        }

        private string ExprObjectCreationSyntax(ObjectCreationExpressionSyntax objectCreationExpression, out TypeInfo exprType)
        {
            // E.g. new X(y)
            exprType = this.semantic.GetTypeInfo(objectCreationExpression);

            if (exprType.Type.IsValueType)
            {
                return string.Format("{0}({1})", // new X(y) ==> X(y)
                    typeWrapper.Wrap(exprType, false),
                    this.SyntaxArgumentList(objectCreationExpression.ArgumentList));
            }
            else
            {
                string type = typeWrapper.Wrap(exprType, true); // _<foo>, or _string/_wstring/_array

                return string.Format("new{0}({1})", // new X(y) ==> new_<X>(y)
                    type.StartsWith("_<") ? type : "_<" + type.Substring(1) + ">",
                    this.SyntaxArgumentList(objectCreationExpression.ArgumentList));
            }
        }

        private string ExprElementBindingSyntax(ElementBindingExpressionSyntax elementBindingExpression)
        {
            // TODO: ExprElementBindingSyntax
            return null;
        }

        private string ExprElementAccessSyntax(ElementAccessExpressionSyntax elementAccessExpression, bool isLeftValue, out TypeInfo exprType)
        {
            // E.g. x[index1, index2]
            TypeInfo objType = this.semantic.GetTypeInfo(elementAccessExpression.Expression);
            string obj = this.ExprSyntax(elementAccessExpression.Expression);

            exprType = this.semantic.GetTypeInfo(elementAccessExpression);
            string args = this.SyntaxSeparatedSyntaxList(elementAccessExpression.ArgumentList.Arguments, (s) => this.SyntaxArgument(s));

            if (objType.Type.Kind == SymbolKind.ArrayType || Util.IsAttributeDefined(objType.Type.GetAttributes(), "ImportedAttribute"))
            {
                if (objType.Type.IsValueType)
                {
                    // Compile to x[index] as is
                    return string.Format("{0}[1]", obj, args);
                }
                else
                {
                    // Compile to x.IndexOf<ReturnType>(index)
                    return string.Format("{0}.{1}<{2}>({3})", obj, Consts.IndexOf, typeWrapper.Wrap(exprType, true), args);
                }
            }
            else
            {
                // C# indexer, compile to x.get_IndexOf(index) or x.set_IndexOf(index)
                string op = objType.Type.IsValueType ? "." : "->";

                if (!isLeftValue || this.memberAccessDepth > 1)
                {
                    return string.Format("{0}{1}{2}{3}({4})", obj, op, Consts.GetterPrefix, Consts.IndexOf, args);
                }
                else
                {
                    return string.Format("{0}{1}{2}{3}({4}, ", obj, op, Consts.SetterPrefix, Consts.IndexOf, args);
                }
            }
        }

        private string ExprSizeOfSyntax(SizeOfExpressionSyntax sizeOfExpression, out TypeInfo exprType)
        {
            // exprType is useless here since we don't support member access for primitive values except string
            exprType = new TypeInfo();// App.Compilation.GetTypeByMetadataName("xint");

            return string.Format("sizeof({0})", typeWrapper.Wrap(sizeOfExpression.Type, false));
        }

        private string ExprAnonymousMethodSyntax(AnonymousMethodExpressionSyntax anonymousMethodExpression)
        {
            // TODO: WIP... support functional programming
            // E.g. var x = delegate(){}
            string parameters = this.SyntaxParameterList(anonymousMethodExpression.ParameterList, false);

            this.tempStringBuilder.Length = 0;
            this.StatementBlockSyntax("[&]" + parameters, this.tempWriter, ";", (anonymousMethodExpression.Body as BlockSyntax));

            return Environment.NewLine + this.tempStringBuilder.ToString();
        }

        private string ExprSimpleLambdaSyntax(SimpleLambdaExpressionSyntax simpleLambdaExpression)
        {
            // TODO: WIP... support functional programming
            // E.g. var x = (x) => x.y;
            string parameter = this.SyntaxParameter(simpleLambdaExpression.Parameter, false);
            this.memberModel.PushScope();
            string body = this.ExprSyntax(simpleLambdaExpression.Body as ExpressionSyntax);
            this.memberModel.PopScope();
            return string.Format("[&]({0}) {{ return {1}; }}", parameter, body);
        }

        private string ExprParenthesizedLambdaSyntax(ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpression)
        {
            // TODO: WIP... support functional programming
            // E.g. var x = (x) => { return x.y; };
            string parameters = this.SyntaxParameterList(parenthesizedLambdaExpression.ParameterList, false);

            this.tempStringBuilder.Length = 0;
            this.StatementBlockSyntax("[&]" + parameters, this.tempWriter, ";", (parenthesizedLambdaExpression.Body as BlockSyntax));

            return Environment.NewLine + this.tempStringBuilder.ToString();
        }

        private string ExprConditionalSyntax(ConditionalExpressionSyntax conditionalExpression, out TypeInfo exprType)
        {
            // E.g. var x = b ? y : z;
            return string.Format("{0} ? {1} : {2}",
                this.ExprSyntax(conditionalExpression.Condition),
                this.ExprSyntax(conditionalExpression.WhenTrue, out exprType),
                this.ExprSyntax(conditionalExpression.WhenFalse, out exprType)
            );
        }
    }
}
