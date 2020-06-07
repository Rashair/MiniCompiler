using MiniCompiler.Extensions;
using QUT.Gppg;
using System;
using System.Collections.Generic;

namespace MiniCompiler.Syntax
{
    public abstract class SyntaxNode :
        IEquatable<SyntaxNode>
    {
        protected SyntaxNode parent;
        protected LexLocation location;

        // TODO
        protected List<Token> TokensBefore;

        // TODO
        protected List<Token> TokensAfter;

        public SyntaxNode(LexLocation loc = null)
        {
            Location = loc;
        }

        public SyntaxNode Parent { private get; set; }

        public SyntaxTree Tree { get; set; }

        public LexLocation Location
        {
            get => location;
            set
            {
                location = value?.Copy() ?? new LexLocation();
            }
        }

        public abstract int Count { get; }

        public abstract SyntaxNode this[int i] { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is SyntaxNode other)
            {
                return Equals(other);
            }

            return false;
        }

        public bool Equals(SyntaxNode other)
        {
            if (other == null)
            {
                return false;
            }

            if (GetType() == other.GetType())
            {
                return Count == other.Count && parent.GetType() == other.parent.GetType();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return 19 * this.GetType().GetHashCode();
        }

        public bool IsLeaf()
        {
            return Count == 0;
        }

        public void AddBefore(Token token)
        {
            TokensBefore.Add(token);
        }

        public void AddAfter(Token token)
        {
            TokensAfter.Add(token);
        }
    }
}