using MiniCompiler.Syntax.General;
using QUT.Gppg;

namespace MiniCompiler.Syntax.IOStream
{
    class SimpleString : EmptyNode
    {
        public SimpleString(string value, LexLocation loc = null)
        {
            Value = value;
            Location = loc;
        }

        public string Value { get; }
    }
}
