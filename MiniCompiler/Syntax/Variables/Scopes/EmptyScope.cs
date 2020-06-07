namespace MiniCompiler.Syntax.Variables.Scopes
{
    public class EmptyScope : IScope
    {
        public bool IsPresent(string name)
        {
            return false;
        }

        public void AddToScope(VariableDeclaration variable)
        {
        }

        public bool TryGetVariable(string id, ref VariableDeclaration variable)
        {
            return false;
        }

        public IScope GetParentScope()
        {
            return this;
        }

        public override bool Equals(object obj)
        {
            return obj is EmptyScope;
        }

        public override int GetHashCode()
        {
            return typeof(EmptyScope).GetHashCode();
        }
    }
}