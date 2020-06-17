using MiniCompiler.Syntax.Abstract;
using QUT.Gppg;

namespace MiniCompiler.Syntax.IOStream
{
    public class SimpleString : TypeNode
    {
        public SimpleString(string value, LexLocation loc = null)
        {
            Value = value;
            Type = Type.String;
            Location = loc;
        }

        public string Value { get; }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}