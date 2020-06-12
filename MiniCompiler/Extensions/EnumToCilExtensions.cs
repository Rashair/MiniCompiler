using MiniCompiler.Syntax.Operators;

namespace MiniCompiler.Extensions
{
    public static class EnumToCilExtensions
    {
        public static string ToCil(this Type type)
        {
            switch (type)
            {
                case Type.Int:
                    return "int32";
                case Type.Double:
                    return "float64";
                case Type.Bool:
                    return "bool";
                case Type.String:
                    return "string";
            }

            return null;
        }

        public static string ToPrimitive(this Type type)
        {
            switch (type)
            {
                case Type.Int:
                    return "i4";
                case Type.Double:
                    return "r8";
                case Type.Bool:
                    return "i4";
            }

            return null;
        }
    }
}