namespace MiniCompiler.Syntax.Declaration.Scopes
{
    public interface IScope
    {
        bool AddToScope(string id, Type type);

        bool TryGetType(string id, ref Type type);

        IScope GetParentScope();
    }
}
