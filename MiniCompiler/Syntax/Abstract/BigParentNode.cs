using QUT.Gppg;
using System.Collections;
using System.Collections.Generic;

namespace MiniCompiler.Syntax.Abstract
{
    public abstract class BigParentNode : SyntaxNode,
        IEnumerable<SyntaxNode>
    {
        protected List<SyntaxNode> children;

        public BigParentNode(LexLocation loc = null)
            : base(loc)
        {
            children = new List<SyntaxNode>();
        }

        public override int Count => children.Count;

        public override SyntaxNode this[int i]
        {
            get => children[i];
            set
            {
                children[i] = value;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + 31 * children.Count;
        }

        public IEnumerator<SyntaxNode> GetEnumerator()
        {
            return children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return children.GetEnumerator();
        }

        public SyntaxNode Add(SyntaxNode node)
        {
            node.Parent = this;
            node.Tree = Tree;
            children.Add(node);

            return this;
        }

        public void SetChildren(List<SyntaxNode> children)
        {
            this.children = children;
        }
    }
}