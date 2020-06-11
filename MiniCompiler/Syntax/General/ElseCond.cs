using MiniCompiler.Syntax.Abstract;
using QUT.Gppg;

namespace MiniCompiler.Syntax.General
{
    class ElseCond : SingleChildNode
    {
        public ElseCond(LexLocation loc = null)
        {
            Location = loc;
        }
    }
}
