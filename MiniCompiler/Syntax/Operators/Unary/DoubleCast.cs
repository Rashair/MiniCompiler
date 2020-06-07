namespace MiniCompiler.Syntax.Operators.Unary
{
    public class DoubleCast : UnaryOperator
    {
        public override bool CanUse(Type typeA)
        {
            return true;
        }

        public override Type GetResultType(Type type)
        {
            return Type.Double;
        }
    }
}