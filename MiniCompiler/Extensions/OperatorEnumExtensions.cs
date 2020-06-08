using MiniCompiler.Syntax.Operators;
using System;
using System.Linq;
using System.Reflection;

namespace MiniCompiler.Extensions
{
    public static class OperatorEnumExtensions
    {
        public delegate T ObjectActivator<T>(params object[] args);

        public static Operator CreateOperator(this OperatorEnum op)
        {
            var className = op.ToString();
            var type = GetTypeByName(className);
            if (type == null)
            {
                return new UnknownOperator();
            }

            return (Operator)Activator.CreateInstance(type, true);
        }

        private static System.Type GetTypeByName(string className)
        {
            System.Type[] assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
            return assemblyTypes.FirstOrDefault(type => type.Name == className);
        }
    }
}
