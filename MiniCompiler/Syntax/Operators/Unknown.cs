namespace MiniCompiler.Syntax.Operators
{
    public class Unknown : Operator
    {
        public override bool CanUse(Type typeA)
        {
            return false;
        }

        public override bool CanUse(Type typeA, Type typeB)
        {
            return false;
        }
    }
}
