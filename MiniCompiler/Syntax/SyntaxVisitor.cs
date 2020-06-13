using MiniCompiler.Extensions;
using MiniCompiler.Syntax.Abstract;
using MiniCompiler.Syntax.General;
using MiniCompiler.Syntax.HelperClasses;
using MiniCompiler.Syntax.IOStream;
using MiniCompiler.Syntax.Operators.Assignment;
using MiniCompiler.Syntax.Operators.Bitwise;
using MiniCompiler.Syntax.Operators.Logical;
using MiniCompiler.Syntax.Operators.Math;
using MiniCompiler.Syntax.Operators.Relation;
using MiniCompiler.Syntax.Operators.Unary;
using MiniCompiler.Syntax.Variables;
using MiniCompiler.Syntax.Variables.Scopes;
using System.Collections.Generic;
using static MiniCompiler.Compiler;

namespace MiniCompiler.Syntax
{
    public class SyntaxVisitor
    {
        private readonly SyntaxTree tree;

        private const string TTY = "[mscorlib]System.Console";
        private readonly string PrintStr = $"call void {TTY}::Write(string)";
        private readonly string PrintInt = $"call void {TTY}::Write({Type.Int.ToCil()})";
        private readonly string PrintBool = $"call void {TTY}::Write({Type.Bool.ToCil()})";
        private readonly string PrintDouble = $"call void {TTY}::Write({Type.Double.ToCil()})";

        private const string tmpPrefix = "@_tmp";
        private readonly Dictionary<Type, Usage> tmpVariables;
        
        private readonly Stack<int> ifLabels;
        private int lastIfLabel;

        private int lastWhileLabel;

        private int lastAndLabel;
        private int lastOrlabel;

        public SyntaxVisitor(SyntaxTree tree)
        {
            this.tree = tree;
            tmpVariables = new Dictionary<Type, Usage>
            {
                { Type.Int,    new Usage{ Used = 0, Max = 0} },
                { Type.Double, new Usage{ Used = 0, Max = 0}  },
                { Type.Bool,   new Usage{ Used = 0, Max = 0}  }
            };
            ifLabels = new Stack<int>();
            ifLabels.Push(0);
        }

        public void Visit()
        {
            GenProlog();
            tree.Visit(this);
            GenEpilog();
        }

        private static void GenProlog()
        {
            EmitCode(".assembly extern mscorlib { auto }");
            EmitCode(".assembly MiniCompiler { }");
            EmitCode($".module MiniCompiler.exe");

            /* From Expert .NET 2.0 IL Assembler */
        }

        private static void GenEpilog()
        {
        }

        public void Visit(CompilationUnit compilationUnit)
        {
            EmitCode(".method static void Program()");
            EmitCode("{");
            EmitCode(".entrypoint");

            EmitCode(".try");
            EmitCode("{");
            EmitCode();


            compilationUnit.Child.Visit(this);

            EmitCode("leave Return");
            EmitCode("}");

            EmitCode("catch [mscorlib]System.Exception");
            EmitCode("{");
            EmitCode("callvirt instance string [mscorlib]System.Exception::get_Message()");
            EmitCode(PrintStr);
            EmitCode("leave Return");
            EmitCode("}");

            EmitCode("Return: ret");
            EmitCode("}");
        }

        public void Visit(Block block)
        {
            EmitCode("{");

            for (int i = 0; i < block.Count; ++i)
            {
                block[i].Visit(this);
                PopIfHasValue(block[i]);
            }

            EmitCode("}");
        }

        public void Visit(VariableDeclaration declare)
        {
            EmitCode(".locals init ({0} {1})", declare.Type.ToCil(), declare.Name);
        }

        public void Visit(Write write)
        {
            var node = write.Child;
            if (node.Type == Type.Double)
            {
                EmitCode("call class [mscorlib]System.Globalization.CultureInfo " +
                    "[mscorlib]System.Globalization.CultureInfo::get_InvariantCulture()");
                EmitCode("ldstr \"{0:0.000000}\"");

                node.Visit(this);

                EmitCode("box [mscorlib]System.Double");
                EmitCode("call string [mscorlib]System.String:" +
                    ":Format(class [mscorlib]System.IFormatProvider, string, object)");
                EmitCode(PrintStr);
                return;
            }

            node.Visit(this);
            EmitCode($"call void {TTY}::Write({node.Type.ToCil()})");
        }

        public void Visit(SimpleString simpleString)
        {
            EmitCode("ldstr {0}", simpleString.Value);
        }

        public void Visit(VariableReference variableReference)
        {
            EmitCode("ldloc {0}", variableReference.Declaration.Name);
        }

        public void Visit(Assign assign)
        {
            assign.Right.Visit(this);
            var left = assign.Left as VariableReference;
            ConvertToDoubleIfNeeded(left.Type, assign.Right.Type);

            EmitCode("stloc {0}", left.Declaration.Name);
            EmitCode("ldloc {0}", left.Declaration.Name);
        }

        public void Visit(Value value)
        {
            EmitCode("ldc.{0} {1}", value.Type.ToPrimitive(), value.Val);
        }

        public void Visit(IfCond ifCond)
        {
            ifCond.Left.Visit(this);
            int label = lastIfLabel++;
            if (ifCond.HasElse)
            {
                ifLabels.Push(label);
                EmitCode($"brfalse ELSE_{label}");

                ifCond.Middle.Visit(this);
                PopIfHasValue(ifCond.Middle);

                EmitCode($"br OUT_IF_ELSE_{label}");
                ifCond.Right.Visit(this);
            }
            else
            {
                EmitCode($"brfalse OUT_IF_ELSE_{label}");
                ifCond.Middle.Visit(this);
                PopIfHasValue(ifCond.Middle);
            }
            EmitCode($"OUT_IF_ELSE_{label}: ");
        }

        public void Visit(ElseCond elseCond)
        {
            EmitCode($"ELSE_{ifLabels.Pop()}: ");
            elseCond.Child.Visit(this);
            PopIfHasValue(elseCond.Child);
        }

        public void Visit(WhileLoop whileLoop)
        {
            int label = lastWhileLabel++;
            EmitCode($"WHILE_{label}: ");
            whileLoop.Left.Visit(this);

            EmitCode($"brfalse OUT_WHILE_{label}");
            whileLoop.Right.Visit(this);
            PopIfHasValue(whileLoop.Right);
            EmitCode($"br WHILE_{label}");

            EmitCode($"OUT_WHILE_{label}: ");
        }

        public void Visit(Read read)
        {

        }

        public void Visit(Equals bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitCode("ceq");
        }


        public void Visit(NotEquals bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitCode("ceq");
            EmitCode("ldc.{0} 0", Type.Bool.ToPrimitive());
            EmitCode("ceq");
        }


        public void Visit(Greater bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitCode("cgt");
        }

        public void Visit(GreaterOrEqual bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(right, left);

            EmitCode("clt");
        }


        public void Visit(Less bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitCode("clt");
        }

        public void Visit(LessOrEqual bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(right, left);

            EmitCode("cgt");
        }


        public void Visit(Add bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitCode("add");
        }

        public void Visit(Subtract bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitCode("sub");
        }

        public void Visit(Multiplies bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitCode("mul");
        }

        public void Visit(Divides bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitCode("div");
        }

        public void Visit(BitOr bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitCode("or");
        }
        public void Visit(BitAnd bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitCode("and");
        }

        public void Visit(UnaryMinus uno)
        {
            uno.Left.Visit(this);
            EmitCode("neg");
        }

        public void Visit(LogicNegation uno)
        {
            uno.Left.Visit(this);
            EmitCode("ldc.{0} 1", Type.Bool.ToPrimitive());
            EmitCode("xor");
        }

        public void Visit(BitNegation uno)
        {
            uno.Left.Visit(this);
            EmitCode("not");
        }

        public void Visit(IntCast uno)
        {
            uno.Left.Visit(this);
            EmitCode("conv." + Type.Int.ToPrimitive());
        }

        public void Visit(DoubleCast uno)
        {
            uno.Left.Visit(this);
            EmitCode("conv." + Type.Double.ToPrimitive());
        }

        public void Visit(And and)
        {
            and.Left.Visit(this);
            int label = lastAndLabel++;
            EmitCode("dup");
            EmitCode($"brfalse AND_OUT_{label}");
            
            and.Right.Visit(this);
            EmitCode("and");

            EmitCode($"AND_OUT_{label}: ");
        }

        public void Visit(Or or)
        {
            or.Left.Visit(this);
            int label = lastOrlabel++;
            EmitCode("dup");
            EmitCode($"brtrue OR_OUT_{label}");

            or.Right.Visit(this);
            EmitCode("or");

            EmitCode($"OR_OUT_{label}: ");
        }

        public void Visit(Return @return)
        {
            EmitCode("leave Return");
        }

        private void PopIfHasValue(SyntaxNode node)
        {
            if (node.HasValue)
            {
                EmitCode("pop");
            }
        }

        private void ConvertToDoubleIfNeeded(Type a, Type b)
        {
            if(a == Type.Double && b != Type.Double)
            {
                EmitCode("conv." + Type.Double.ToPrimitive());
            }
        }

        private void PrepareBinaryOperation(TypeNode left, TypeNode right)
        {
            left.Visit(this);
            ConvertToDoubleIfNeeded(right.Type, left.Type);

            right.Visit(this);
            ConvertToDoubleIfNeeded(left.Type, right.Type);
        }



        private string GetTmp(Type type)
        {
            var usage = tmpVariables[type];
            ++usage.Used;
            var name = $"{tmpPrefix}_{type.ToCil()}_{usage.Used}";
            if (usage.Used > usage.Max)
            {
                EmitCode(".locals init ({0} {1})", type.ToCil(), $"{name}");
                ++usage.Max;
            }

            return name;
        }

        private void FreeTmp(Type type)
        {
            tmpVariables[type].Used -= 1;
        }

        private void VisitAllChildren(SyntaxNode node)
        {
            for (int i = 0; i < node.Count; ++i)
            {
                node[i].Visit(this);
            }
        }
    }
}