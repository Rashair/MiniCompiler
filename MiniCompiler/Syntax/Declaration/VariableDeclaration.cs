using QUT.Gppg;

namespace MiniCompiler.Syntax.Declaration.Scopes
{
    public class VariableDeclaration : SyntaxNode
    {
        public string Name { get; private set; }
        public IScope Scope { get; private set; }
        public string Value { get; set; }

        public VariableDeclaration(string name, IScope scope, LexLocation loc = null) :
            base(loc)
        {
            Name = name;
            Scope = scope;
        }
    }
}
