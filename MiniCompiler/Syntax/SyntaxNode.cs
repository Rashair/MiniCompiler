using MiniCompiler.Extensions;
using QUT.Gppg;
using System;
using System.Collections.Generic;

namespace MiniCompiler.Syntax
{
    public abstract class SyntaxNode
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

        public SyntaxNode Parent { private get => parent; set => parent = value; }

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
                return IsNodeEqual(other);
            }

            return false;
        }

        protected virtual bool IsNodeEqual(SyntaxNode node)
        {
            if (node == null)
            {
                return false;
            }

            if (GetType() == node.GetType())
            {
                return Count == node.Count &&
                    parent?.GetType() == node.parent?.GetType();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return GetNodeHash();
        }

        protected virtual int GetNodeHash()
        {
            int hash = 17;
            hash = CombineHashCode(hash, GetType(), Count);
            hash = CombineHashCode(hash, parent?.GetType());
            return hash;
        }

        public static int CombineHashCode(int start, object b, object c = null)
        {
            int hash = start * 23 + b?.GetHashCode() ?? 0;
            hash = hash * c?.GetHashCode() ?? 0;
            return hash;
        }

        public override string ToString()
        {
            return $"{GetType().Name}({Location?.StartLine}..{Location?.EndLine})";
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