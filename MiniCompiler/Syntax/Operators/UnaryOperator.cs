namespace MiniCompiler.Syntax.Operators
{
    public abstract class UnaryOperator : Operator
    {
        public override bool CanUse(Type typeA, Type typeB)
        {
            return false;
        }
    }
}
