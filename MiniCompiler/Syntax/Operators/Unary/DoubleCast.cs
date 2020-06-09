namespace MiniCompiler.Syntax.Operators.Unary
{
    public class DoubleCast : UnaryOperator
    {
        public override bool CanUse(Type typeA)
        {
            return typeA != Type.Unknown;
        }

        public override Type GetResultType(Type type)
        {
            return Type.Double;
        }
    }
}