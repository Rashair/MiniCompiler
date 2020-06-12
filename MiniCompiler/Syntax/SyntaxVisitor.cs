using MiniCompiler.Extensions;
using MiniCompiler.Syntax.Abstract;
using MiniCompiler.Syntax.General;
using MiniCompiler.Syntax.IOStream;
using MiniCompiler.Syntax.Operators.Assignment;
using MiniCompiler.Syntax.Operators.Bitwise;
using MiniCompiler.Syntax.Operators.Logical;
using MiniCompiler.Syntax.Operators.Math;
using MiniCompiler.Syntax.Operators.Relation;
using MiniCompiler.Syntax.Operators.Unary;
using MiniCompiler.Syntax.Variables;
using MiniCompiler.Syntax.Variables.Scopes;
using System.Globalization;
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

        private int ifCount;



        public SyntaxVisitor(SyntaxTree tree)
        {
            this.tree = tree;
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

            EmitCode("ldstr \"\\nEnd of Program execution\\n\"");
            EmitCode(PrintStr);
            EmitCode("");

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
            if (left.Type == Type.Double && assign.Right.Type != Type.Double)
            {
                EmitCode("conv.r8");
            }
            EmitCode("stloc {0}", left.Declaration.Name);
        }

        public void Visit(Value value)
        {
            EmitCode("ldc.{0} {1}", value.Type.ToPrimitive(), value.Val);
        }

        public void Visit(IfCond ifCond)
        {
            ifCond.Left.Visit(this);
            if (ifCond.HasElse)
            {
                EmitCode($"brfalse ELSE_{ifCount}");
                ifCond.Middle.Visit(this);
                ifCond.Right.Visit(this);
            }
            else
            {
                EmitCode($"brfalse OUT_IF_ELSE_{ifCount}");
                ifCond.Middle.Visit(this);
            }
            EmitCode($"OUT_IF_ELSE_{ifCount++}: ");
        }

        public void Visit(ElseCond elseCond)
        {
            EmitCode($"ELSE_{ifCount}: ");
            elseCond.Child.Visit(this);
        }

        public void Visit(And and)
        {

        }

        public void Visit(Or or)
        {

        }

        public void Visit(UnaryMinus unaryMinus)
        {

        }

        public void Visit(GreaterOrEqual greaterOrEqual)
        {

        }

        public void Visit(LogicNegation logicNegation)
        {

        }

        public void Visit(Less less)
        {

        }


        public void Visit(NotEquals notEquals)
        {

        }

        public void Visit(LessOrEqual lessOrEqual)
        {

        }



        public void Visit(Equals equals)
        {

        }

        public void Visit(Greater greater)
        {

        }

        public void Visit(Subtract subtract)
        {

        }

        public void Visit(Multiplies multiplies)
        {

        }

        public void Visit(Divides divides)
        {

        }

        public void Visit(BitOr bitOr)
        {

        }

        public void Visit(Add add)
        {

        }

        public void Visit(BitAnd bitAnd)
        {

        }

        public void Visit(Read read)
        {

        }

        public void Visit(WhileLoop whileLoop)
        {

        }

        public void Visit(IntCast intCast)
        {

        }

        public void Visit(BitNegation bitNegation)
        {

        }

        public void Visit(DoubleCast doubleCast)
        {

        }

        public void Visit(Return @return)
        {
            EmitCode("leave Return");
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