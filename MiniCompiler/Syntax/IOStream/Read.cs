using MiniCompiler.Syntax.Abstract;
using QUT.Gppg;

namespace MiniCompiler.Syntax.IOStream
{
    public class Read : SingleChildNode
    {
        public Read(LexLocation loc = null)
        {
            Location = loc;
        }
    }
}
