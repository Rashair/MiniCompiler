namespace MiniCompiler.Syntax.Operators.Unary
{
    public class UnaryMinus : UnaryOperator
    {
        public override bool CanUse(Type typeA)
        {
            switch (typeA)
            {
                case Type.Int:
                    return true;
            }

            return false;
        }
    }
}
