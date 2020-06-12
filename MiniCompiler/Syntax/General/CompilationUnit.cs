using MiniCompiler.Syntax.Abstract;
using QUT.Gppg;

namespace MiniCompiler.Syntax.General
{
    public class CompilationUnit : SingleChildNode<SyntaxNode>
    {
        public CompilationUnit(LexLocation location = null)
        {
            Location = location;
        }

        public CompilationUnit(SyntaxTree tree, LexLocation location = null) :
            this(location)
        {
            this.Tree = tree;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}