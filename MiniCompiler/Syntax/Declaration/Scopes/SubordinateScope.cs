using System.Collections.Generic;

namespace MiniCompiler.Syntax.Declaration.Scopes
{
    public class SubordinateScope : IScope
    {
        private readonly IScope parentScope;
        private readonly HashSet<string> variables;

        public SubordinateScope(IScope parentScope)
        {
            this.parentScope = parentScope;
            this.variables = new HashSet<string>();
        }

        public bool AddToScope(string id)
        {
            return variables.Add(id);
        }

        public bool IsInScope(string id)
        {
            return variables.Contains(id) || parentScope.IsInScope(id);
        }

        public IScope GetParentScope()
        {
            return parentScope;
        }
    }
}
