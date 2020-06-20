using MiniCompiler.Syntax.Variables;

namespace MiniCompiler.Extensions
{
    public static class EnumToCilExtensions
    {
        public static string ToCil(this MiniType type)
        {
            switch (type)
            {
                case MiniType.Int:
                    return "int32";

                case MiniType.Double:
                    return "float64";

                case MiniType.Bool:
                    return "bool";

                case MiniType.String:
                    return "string";
            }

            return null;
        }

        public static string ToPrimitive(this MiniType type)
        {
            switch (type)
            {
                case MiniType.Int:
                    return "i4";

                case MiniType.Double:
                    return "r8";

                case MiniType.Bool:
                    return "i4";
            }

            return null;
        }

        public static string ToCSharp(this MiniType type)
        {
            switch (type)
            {
                case MiniType.Int:
                    return "Int32";

                case MiniType.Double:
                    return "Double";

                case MiniType.Bool:
                    return "Boolean";

                case MiniType.String:
                    return "String";
            }

            return null;
        }
    }
}