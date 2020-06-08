using MiniCompiler.Syntax.Abstract;
using QUT.Gppg;
using System;

namespace MiniCompiler.Syntax.Variables.Scopes
{
    public class VariableReference : TypeNode
    {
        public VariableReference(VariableDeclaration declar, LexLocation loc = null)
        {
            Declaration = declar;
            Location = loc;
        }

        public VariableDeclaration Declaration { get; }

        public override Type Type
        {
            get => Declaration.Type;
            protected set => throw new InvalidOperationException("You cannot set type of declared variable");
        }

        protected override bool IsNodeEqual(SyntaxNode node)
        {
            if (base.IsNodeEqual(node))
            {
                var other = (VariableReference)node;
                return Declaration.Equals(other.Declaration);
            }

            return false;
        }

        protected override int GetNodeHash()
        {
            return CombineHashCode(base.GetNodeHash(), Declaration);
        }

        public override string ToString()
        {
            return $"{base.ToString()}: {Declaration.Name}";
        }
    }
}