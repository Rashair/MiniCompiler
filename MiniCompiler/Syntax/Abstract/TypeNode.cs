namespace MiniCompiler.Syntax.Abstract
{
    public abstract class TypeNode : SiblingsNode
    {
        public virtual Type Type { get; protected set; }
    }
}