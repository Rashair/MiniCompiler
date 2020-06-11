using MiniCompiler.Syntax.Abstract;
using QUT.Gppg;

namespace MiniCompiler.Syntax.General
{
    public class IfCond : TrinityNode
    {
        public IfCond(LexLocation loc = null)
        {
            Location = loc;
        }
    }
}
