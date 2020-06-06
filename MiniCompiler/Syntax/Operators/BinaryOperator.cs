using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCompiler.Syntax.Operators
{
    public abstract class BinaryOperator : Operator
    {
        public override bool CanUse(Type typeA)
        {
            return false;
        }
    }
}
