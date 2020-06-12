using MiniCompiler.Syntax.Abstract;
using MiniCompiler.Syntax.Variables.Scopes;
using QUT.Gppg;

namespace MiniCompiler.Syntax.IOStream
{
    public class Write : SingleChildNode
    {
        public Write(LexLocation loc = null)
        {
            Location = loc;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
