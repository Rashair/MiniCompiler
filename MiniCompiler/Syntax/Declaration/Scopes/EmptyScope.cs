namespace MiniCompiler.Syntax.Declaration.Scopes
{
    public class EmptyScope : IScope
    {
        public bool AddToScope(string id)
        {
            return false;
        }

        public bool IsInScope(string id)
        {
            return false;
        }

        public IScope GetParentScope()
        {
            return this;
        }
    }
}
