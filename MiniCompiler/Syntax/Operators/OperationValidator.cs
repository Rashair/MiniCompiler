namespace MiniCompiler.Syntax.Operators
{
    public static class OperationValidator
    {
        public static bool CanUseOperator(Operator op, Type typeA, Type typeB)
        {
            return op.CanUse(typeA, typeB);
        }

        public static bool CanUseOperator(Operator op, Type typeA)
        {
            return op.CanUse(typeA);
        }
    }
}
