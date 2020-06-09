using System.IO;

namespace MiniCompilerTests
{
    public class TemplateValidTests : ValidTests
    {
        protected override string PathSuffix => Path.Combine(base.PathSuffix, "Template");
    }
}
