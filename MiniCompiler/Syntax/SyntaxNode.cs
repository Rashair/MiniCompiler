using MiniCompiler.Extensions;
using QUT.Gppg;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MiniCompiler.Syntax
{
    public abstract class SyntaxNode :
        IEquatable<SyntaxNode>,
        IEnumerable<SyntaxNode>
    {
        protected SyntaxNode parent;
        protected LexLocation location;
        // TODO
        protected List<Token> TokensBefore;
        protected List<SyntaxNode> children;
        // TODO
        protected List<Token> TokensAfter;

        public SyntaxNode(LexLocation loc = null)
        {
            children = new List<SyntaxNode>();
            this.location = loc?.Copy() ?? new LexLocation();
        }

        public SyntaxTree Tree { get; set; }

        public LexLocation Location
        {
            get => location;
            set
            {
                location = value.Copy();
            }
        }

        public int Count => children.Count;

        public SyntaxNode this[int i] => children[i];

        public bool Equals(SyntaxNode other)
        {
            if (other == null)
            {
                return false;
            }

            if (GetType() == other.GetType())
            {
                return children.Count == other.children.Count;
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is SyntaxNode other)
            {
                return Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return 19 * this.GetType().GetHashCode() + 31 * children.Count;
        }

        public IEnumerator<SyntaxNode> GetEnumerator()
        {
            return children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return children.GetEnumerator();
        }

        public bool IsLeaf()
        {
            return children.Count == 0;
        }

        public void AddBefore(Token token)
        {
            TokensBefore.Add(token);
        }

        public SyntaxNode Add(SyntaxNode node)
        {
            node.parent = this;
            node.Tree = Tree;
            children.Add(node);

            return this;
        }

        public void AddAfter(Token token)
        {
            TokensAfter.Add(token);
        }

        public void SetChildren(List<SyntaxNode> children)
        {
            this.children = children;
        }
    }
}
