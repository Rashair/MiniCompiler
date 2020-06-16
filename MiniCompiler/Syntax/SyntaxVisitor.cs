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
//using static MiniCompiler.Compiler;

namespace MiniCompiler.Syntax
{
    public class SyntaxVisitor
    {
        private readonly SyntaxTree tree;
        private int stackDepth = 0;
        private int maxStackDepth = 8;

        private const string TTY = "[mscorlib]System.Console";
        private readonly string PrintStr = $"call void {TTY}::Write(string)";

        private readonly Stack<int> ifLabels;
        private int lastIfLabel;

        private int lastWhileLabel;

        private int lastAndLabel;
        private int lastOrlabel;

        public SyntaxVisitor(SyntaxTree tree)
        {
            this.tree = tree;
            ifLabels = new Stack<int>();
            ifLabels.Push(0);
        }

        public void Visit()
        {
            GenProlog();
            tree.Visit(this);
            GenEpilog();
        }

        private void GenProlog()
        {
            Emit(".assembly extern mscorlib { auto }");
            Emit(".assembly MiniCompiler { }");
            Emit($".module MiniCompiler.exe");

            /* From Expert .NET 2.0 IL Assembler */
        }

        private void GenEpilog()
        {
            Compiler.EmitAfterFirst(".entrypoint", $".maxstack {maxStackDepth}");
        }

        public void Visit(CompilationUnit compilationUnit)
        {
            Emit(".method static void Program()");
            Emit("{");
            Emit(".entrypoint");

            Emit(".try");
            Emit("{");
            Emit();

            compilationUnit.Child.Visit(this);

            Emit("leave Return");
            Emit("}");

            Emit("catch [mscorlib]System.Exception");
            Emit("{");
            Emit("callvirt instance string [mscorlib]System.Exception::get_Message()");
            Emit(PrintStr);
            Emit("leave Return");
            Emit("}");

            Emit("Return: ret");
            Emit("}");
        }

        public void Visit(Block block)
        {
            Emit("{");

            for (int i = 0; i < block.Count; ++i)
            {
                block[i].Visit(this);
                PopIfHasValue(block[i]);
            }

            Emit("}");
        }

        public void Visit(VariableDeclaration declare)
        {
            Emit($".locals init ({declare.Type.ToCil()} '{declare.Name}')");
        }

        public void Visit(Write write)
        {
            var node = write.Child;
            if (node.Type == Type.Double)
            {
                EmitStackUp("call class [mscorlib]System.Globalization.CultureInfo " +
                    "[mscorlib]System.Globalization.CultureInfo::get_InvariantCulture()");
                EmitStackUp("ldstr \"{0:0.000000}\"");

                node.Visit(this);

                Emit("box [mscorlib]System.Double");
                EmitStackDown("call string [mscorlib]System.String:" +
                    ":Format(class [mscorlib]System.IFormatProvider, string, object)", 2);
                Emit(PrintStr);
                return;
            }

            node.Visit(this);
            EmitStackDown($"call void {TTY}::Write({node.Type.ToCil()})");
        }

        public void Visit(SimpleString simpleString)
        {
            EmitStackUp($"ldstr {simpleString.Value}");
        }

        public void Visit(VariableReference variableReference)
        {
            EmitStackUp($"ldloc '{variableReference.Declaration.Name}'");
        }

        public void Visit(Assign assign)
        {
            assign.Right.Visit(this);
            var left = assign.Left as VariableReference;
            ConvertToDoubleIfNeeded(left.Type, assign.Right.Type);

            Emit($"stloc '{left.Declaration.Name}'");
            Emit($"ldloc '{left.Declaration.Name}'");
        }

        public void Visit(Value value)
        {
            EmitStackUp($"ldc.{value.Type.ToPrimitive()} {value.Val}");
        }

        public void Visit(IfCond ifCond)
        {
            ifCond.Left.Visit(this);
            int label = lastIfLabel++;
            if (ifCond.HasElse)
            {
                ifLabels.Push(label);
                EmitStackDown($"brfalse ELSE_{label}");

                ifCond.Middle.Visit(this);
                PopIfHasValue(ifCond.Middle);

                Emit($"br OUT_IF_ELSE_{label}");
                ifCond.Right.Visit(this);
            }
            else
            {
                EmitStackDown($"brfalse OUT_IF_ELSE_{label}");
                ifCond.Middle.Visit(this);
                PopIfHasValue(ifCond.Middle);
            }
            Emit($"OUT_IF_ELSE_{label}: ");
        }

        public void Visit(ElseCond elseCond)
        {
            Emit($"ELSE_{ifLabels.Pop()}: ");
            elseCond.Child.Visit(this);
            PopIfHasValue(elseCond.Child);
        }

        public void Visit(WhileLoop whileLoop)
        {
            int label = lastWhileLabel++;
            Emit($"WHILE_{label}: ");
            whileLoop.Left.Visit(this);

            EmitStackDown($"brfalse OUT_WHILE_{label}");
            whileLoop.Right.Visit(this);
            PopIfHasValue(whileLoop.Right);
            Emit($"br WHILE_{label}");

            Emit($"OUT_WHILE_{label}: ");
        }

        public void Visit(Read read)
        {

        }

        public void Visit(Equals bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("ceq");
        }


        public void Visit(NotEquals bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            Emit("ceq");
            Emit($"ldc.{Type.Bool.ToPrimitive()} 0");
            EmitStackDown("ceq");
        }


        public void Visit(Greater bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("cgt");
        }

        public void Visit(GreaterOrEqual bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            Emit("clt");
            Emit($"ldc.{Type.Bool.ToPrimitive()} 0");
            EmitStackDown("ceq");
        }


        public void Visit(Less bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("clt");
        }

        public void Visit(LessOrEqual bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            Emit("cgt");
            Emit($"ldc.{Type.Bool.ToPrimitive()} 0");
            EmitStackDown("ceq");
        }


        public void Visit(Add bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("add");
        }

        public void Visit(Subtract bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("sub");
        }

        public void Visit(Multiplies bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("mul");
        }

        public void Visit(Divides bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("div");
        }

        public void Visit(BitOr bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("or");
        }
        public void Visit(BitAnd bin)
        {
            var left = bin.Left;
            var right = bin.Right;

            PrepareBinaryOperation(left, right);

            EmitStackDown("and");
        }

        public void Visit(UnaryMinus uno)
        {
            uno.Left.Visit(this);
            Emit("neg");
        }

        public void Visit(LogicNegation uno)
        {
            uno.Left.Visit(this);
            EmitStackUp($"ldc.{Type.Bool.ToPrimitive()} 1");
            EmitStackDown("xor");
        }

        public void Visit(BitNegation uno)
        {
            uno.Left.Visit(this);
            Emit("not");
        }

        public void Visit(IntCast uno)
        {
            uno.Left.Visit(this);
            Emit("conv." + Type.Int.ToPrimitive());
        }

        public void Visit(DoubleCast uno)
        {
            uno.Left.Visit(this);
            Emit("conv." + Type.Double.ToPrimitive());
        }

        public void Visit(And and)
        {
            and.Left.Visit(this);
            int label = lastAndLabel++;
            EmitStackUp("dup");
            EmitStackDown($"brfalse AND_OUT_{label}");

            and.Right.Visit(this);
            EmitStackDown("and");

            Emit($"AND_OUT_{label}: ");
        }

        public void Visit(Or or)
        {
            or.Left.Visit(this);
            int label = lastOrlabel++;
            EmitStackUp("dup");
            EmitStackDown($"brtrue OR_OUT_{label}");

            or.Right.Visit(this);
            EmitStackDown("or");

            Emit($"OR_OUT_{label}: ");
        }

        public void Visit(Return @return)
        {
            Emit("leave Return");
        }

        private void PopIfHasValue(SyntaxNode node)
        {
            if (node.HasValue)
            {
                EmitStackDown("pop");
            }
        }

        private void ConvertToDoubleIfNeeded(Type a, Type b)
        {
            if (a == Type.Double && b != Type.Double)
            {
                Emit("conv." + Type.Double.ToPrimitive());
            }
        }

        private void Emit(string instr = null)
        {
            Compiler.EmitCode(instr);
        }

        private void EmitStackUp(string instr, int shift = 1)
        {
            Compiler.EmitCode(instr);
            stackDepth += shift;
            if (stackDepth > maxStackDepth)
            {
                maxStackDepth = stackDepth;
            }
        }

        private void EmitStackDown(string instr, int shift = 1)
        {
            Compiler.EmitCode(instr);
            stackDepth -= shift;
        }

        private void PrepareBinaryOperation(TypeNode left, TypeNode right)
        {
            left.Visit(this);
            ConvertToDoubleIfNeeded(right.Type, left.Type);

            right.Visit(this);
            ConvertToDoubleIfNeeded(left.Type, right.Type);
        }
    }
}