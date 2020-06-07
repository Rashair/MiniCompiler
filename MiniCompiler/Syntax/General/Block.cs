using MiniCompiler.Syntax.Abstract;
using QUT.Gppg;

namespace MiniCompiler.Syntax.General
{
    public class Block : BigParentNode
    {
        public Block()
        {
        }

        public Block(LexLocation location) : base(location)
        {
        }
    }
}