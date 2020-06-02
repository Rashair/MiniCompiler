using QUT.Gppg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCompiler.Syntax.General
{
    public class CompilationUnit : SyntaxNode
    {
        public CompilationUnit(LexLocation location) : base(location)
        {
        }

        public CompilationUnit(SyntaxTree tree, LexLocation location)
            : this(location)
        {
            this.tree = tree;
        }
    }
}
