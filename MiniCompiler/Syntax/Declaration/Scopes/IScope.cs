namespace MiniCompiler.Syntax.Declaration.Scopes
{
    public interface IScope
    {
        bool AddToScope(string id);

        bool IsInScope(string id);

        IScope GetParentScope();
    }
}
