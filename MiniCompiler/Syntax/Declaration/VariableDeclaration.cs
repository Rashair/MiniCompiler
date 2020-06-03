using QUT.Gppg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCompiler.Syntax.Declaration
{
    public class VariableDeclaration : SyntaxNode
    {
        public VariableDeclaration(LexLocation loc) : base(loc)
        {
        }
    }
}
