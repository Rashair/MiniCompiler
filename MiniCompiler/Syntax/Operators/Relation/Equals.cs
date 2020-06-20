﻿using MiniCompiler.Syntax.Variables;

namespace MiniCompiler.Syntax.Operators.Relation
{
    public class Equals : BinaryOperator
    {
        public override bool CanUse(MiniType typeA, MiniType typeB = MiniType.Unknown)
        {
            switch (typeA)
            {
                case MiniType.Int:
                    return typeB == MiniType.Int || typeB == MiniType.Double;

                case MiniType.Double:
                    return typeB == MiniType.Int || typeB == MiniType.Double;

                case MiniType.Bool:
                    return typeB == MiniType.Bool;
            }

            return false;
        }

        public override MiniType GetResultTypeBinary(MiniType typeA, MiniType typeB)
        {
            return MiniType.Bool;
        }

        public override void Visit(SyntaxVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}