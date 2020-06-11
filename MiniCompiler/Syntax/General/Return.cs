using QUT.Gppg;

namespace MiniCompiler.Syntax.General
{
    public class Return : EmptyNode
    {
        public Return(LexLocation loc = null)
        {
            Location = loc;
        }
    }
}
