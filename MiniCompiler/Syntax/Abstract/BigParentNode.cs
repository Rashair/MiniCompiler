using QUT.Gppg;
using System;
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
                children[i].Parent = this;
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
            if(node == null)
            {
                throw new ArgumentNullException(nameof(node), "You cannot add null child.");
            }

            node.Parent = this;
            node.Tree = Tree;
            children.Add(node);

            return this;
        }

        public void AddChildren(List<SyntaxNode> children)
        {
            foreach (var child in children)
            {
                Add(child);
            }
        }
    }
}