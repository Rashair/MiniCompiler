using MiniCompiler.Syntax.General;
using QUT.Gppg;

namespace MiniCompiler.Syntax.IOStream
{
    public class SimpleString : EmptyNode
    {
        public SimpleString(string value, LexLocation loc = null)
        {
            Value = value;
            Location = loc;
        }

        public override bool ShouldInclude => true;

        public string Value { get; }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
