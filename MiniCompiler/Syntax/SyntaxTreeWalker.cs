using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCompiler.Syntax
{
    public class SyntaxTreeWalker
    {
        private readonly SyntaxTree tree;

        public SyntaxTreeWalker(SyntaxTree tree)
        {
            this.tree = tree;
        }

        public void Walk(Action<int, SyntaxNode> action)
        {
            RecursiveWalk(action, tree.CompilationUnit, 0);
        }

        private void RecursiveWalk(Action<int, SyntaxNode> action, SyntaxNode node, int level)
        {
            action(level, node);

            foreach (var child in node)
            {
                RecursiveWalk(action, child, level + 1);
            }
        }
    }
}
