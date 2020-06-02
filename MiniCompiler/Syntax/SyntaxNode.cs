using QUT.Gppg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCompiler.Syntax
{
    public abstract class SyntaxNode
    {
        private readonly LexLocation location;
        protected SyntaxTree tree;
        protected SyntaxNode parent;
        protected List<SyntaxNode> children;

        private SyntaxNode()
        {
            children = new List<SyntaxNode>();
        }

        public SyntaxNode(LexLocation location)
            : this()
        {
            this.location = location;
        }

        public LexLocation Location => location;

        public bool IsLeaf()
        {
            return children.Count == 0;
        }

        public void AddChild(SyntaxNode node)
        {
            node.parent = this;
            node.tree = tree;
            children.Add(node);
        }
    }
}
