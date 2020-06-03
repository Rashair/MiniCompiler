using QUT.Gppg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCompiler.Syntax.General
{
    public class Block : SyntaxNode
    {
        public Block()
        {
        }

        public Block(LexLocation location) : base(location)
        {
        }
    }
}
