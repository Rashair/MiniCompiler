using System;

namespace MiniCompiler.Syntax.Abstract
{
    public abstract class TypeNode : SiblingsNode
    {
        public virtual Type Type { get; protected set; }

        protected override bool IsNodeEqual(SyntaxNode node)
        {
            if (base.IsNodeEqual(node))
            {
                var other = node as TypeNode;
                return other.Type == Type;
            }

            return false;
        }

        protected override int GetNodeHash()
        {
            return CombineHashCode(base.GetNodeHash(), Type);
        }
    }
}