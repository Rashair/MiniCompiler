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

        public static string ToCSharp(this Type type)
        {
            switch (type)
            {
                case Type.Int:
                    return "Int32";

                case Type.Double:
                    return "Double";

                case Type.Bool:
                    return "Boolean";

                case Type.String:
                    return "String";
            }

            return null;
        }
    }
}