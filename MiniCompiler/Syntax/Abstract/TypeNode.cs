using MiniCompiler.Syntax.Variables;

namespace MiniCompiler.Syntax.Abstract
{
    public abstract class TypeNode : SiblingsNode<TypeNode, TypeNode>
    {
        public virtual MiniType Type { get; protected set; }

        protected override bool IsNodeEqual(SyntaxNode node)
        {
            if (base.IsNodeEqual(node))
            {
                var other = node as TypeNode;
                return other.Type == Type;
            }

            return false;
        }

        public override bool HasValue => true;

        protected override int GetNodeHash()
        {
            return CombineHashCode(base.GetNodeHash(), Type);
        }
    }
}