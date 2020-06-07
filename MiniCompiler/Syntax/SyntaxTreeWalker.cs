using System;

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

            for (int i = 0; i < node.Count; ++i)
            {
                RecursiveWalk(action, node[i], level + 1);
            }
        }
    }
}