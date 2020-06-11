using MiniCompiler.Syntax.Abstract;
using QUT.Gppg;

namespace MiniCompiler.Syntax.General
{
    public class WhileLoop : SiblingsNode
    {
        public WhileLoop(LexLocation loc = null)
        {
            Location = loc;
        }
    }
}
