using MiniCompiler.Extensions;
using QUT.Gppg;

namespace MiniCompiler.Syntax
{
    public abstract class SyntaxNode
    {
        protected SyntaxNode parent;
        protected LexLocation location;

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

        public virtual bool ShouldInclude => true;

        public virtual bool HasValue => false;

        public abstract int Count { get; }

        public abstract SyntaxNode this[int i] { get; set; }

        public abstract void Visit(SyntaxVisitor visitor);

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
            var result = GetType().Name;
            if (Location != null)
            {
                result += $"({Location.StartLine}..{Location.EndLine}) ({Location.StartColumn}..{Location.EndColumn})";
            }
            return result;
        }
    }
}