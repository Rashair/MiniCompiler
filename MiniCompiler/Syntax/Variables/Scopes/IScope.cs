namespace MiniCompiler.Syntax.Variables.Scopes
{
    public interface IScope
    {
        bool IsPresent(string name);

        void AddToScope(VariableDeclaration variable);

        bool TryGetVariable(string id, ref VariableDeclaration variable);

        IScope GetParentScope();
    }
}