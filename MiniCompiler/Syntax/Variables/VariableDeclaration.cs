using MiniCompiler.Syntax.Abstract;
using MiniCompiler.Syntax.Variables.Scopes;
using QUT.Gppg;

namespace MiniCompiler.Syntax.Variables
{
    public class VariableDeclaration : TypeNode
    {
        public string Name { get; private set; }
        public IScope Scope { get; private set; }

        public VariableDeclaration(string name, IScope scope, Type type, LexLocation loc = null)
        {
            Name = name;
            Scope = scope;
            scope.AddToScope(this);
            Type = type;
            Location = loc;
        }

        protected override bool IsNodeEqual(SyntaxNode node)
        {
            if (base.IsNodeEqual(node))
            {
                var other = (VariableDeclaration)node;
                return Name == other.Name && Scope.Equals(other.Scope);
            }

            return false;
        }

        protected override int GetNodeHash()
        {
            return CombineHashCode(base.GetNodeHash(), Name, Scope);
        }

        public override string ToString()
        {
            return $"{base.ToString()}: {Type} {Name};";
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}