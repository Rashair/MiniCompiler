using QUT.Gppg;

namespace MiniCompiler.Extensions
{
    public static class LexLocationExtensions
    {
        public static LexLocation Copy(this LexLocation location)
        {
            return new LexLocation(location.StartLine, location.StartColumn, location.EndLine, location.EndColumn);
        }
    }
}