using MiniCompiler.Syntax.General;

namespace MiniCompiler.Syntax
{
    public class SyntaxVisitor
    {
        private readonly SyntaxTree tree;

        public SyntaxVisitor(SyntaxTree tree)
        {
            this.tree = tree;
        }

        public void Visit()
        {
            // Visit(tree.CompilationUnit);
        }

        public void Visit(CompilationUnit compilationUnit)
        {
            Compiler.EmitCode("// linia {0,3} :  " + Compiler.sourceLines[compilationUnit.Location.EndLine - 1], compilationUnit.Location.EndLine);
            Compiler.EmitCode("ldstr \"\\nEnd of execution\\n\"");
            Compiler.EmitCode("call void [mscorlib]System.Console::WriteLine(string)");
            Compiler.EmitCode("");
        }
    }
}