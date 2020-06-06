namespace MiniCompiler.Syntax.Declaration.Scopes
{
    public class EmptyScope : IScope
    {
        public bool AddToScope(string id, Type type)
        {
            return false;
        }

        public bool TryGetType(string id, ref Type type)
        {
            return false;
        }

        public IScope GetParentScope()
        {
            return this;
        }
    }
}
