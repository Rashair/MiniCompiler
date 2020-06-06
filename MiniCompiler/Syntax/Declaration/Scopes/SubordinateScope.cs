using System.Collections.Generic;

namespace MiniCompiler.Syntax.Declaration.Scopes
{
    public class SubordinateScope : IScope
    {
        private readonly IScope parentScope;
        private readonly Dictionary<string, Type> variables;

        public SubordinateScope(IScope parentScope)
        {
            this.parentScope = parentScope;
            this.variables = new Dictionary<string, Type>();
        }

        public bool AddToScope(string id, Type type)
        {
            if (!variables.ContainsKey(id))
            {
                variables.Add(id, type);
                return true;
            }

            return false;
        }

        public bool TryGetType(string id, ref Type type)
        {
            return variables.TryGetValue(id, out type) || parentScope.TryGetType(id, ref type);
        }

        public IScope GetParentScope()
        {
            return parentScope;
        }
    }
}
