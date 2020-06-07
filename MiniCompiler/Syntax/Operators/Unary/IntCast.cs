namespace MiniCompiler.Syntax.Operators.Unary
{
    public class IntCast : UnaryOperator
    {
        public override bool CanUse(Type typeA)
        {
            return true;
        }

        public override Type GetResultType(Type type)
        {
            return Type.Int;
        }
    }
}