using QUT.Gppg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCompiler.Syntax.General
{
    class EmptyNode : SyntaxNode
    {
        public EmptyNode(LexLocation loc = null) : base(loc) { }
    }
}
