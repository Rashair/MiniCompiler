using System.Collections.Generic;

namespace MiniCompiler.Syntax.Variables.Scopes
{
    public class SubordinateScope : IScope
    {
        private readonly IScope parentScope;
        private readonly Dictionary<string, VariableDeclaration> variables;

        public SubordinateScope(IScope parentScope)
        {
            this.parentScope = parentScope;
            this.variables = new Dictionary<string, VariableDeclaration>();
        }

        public bool IsPresent(string name)
        {
            return variables.ContainsKey(name);
        }

        public void AddToScope(VariableDeclaration variable)
        {
            variables.Add(variable.Name, variable);
        }

        public bool TryGetVariable(string id, ref VariableDeclaration variable)
        {
            return variables.TryGetValue(id, out variable) || parentScope.TryGetVariable(id, ref variable);
        }

        public IScope GetParentScope()
        {
            return parentScope;
        }
    }
}