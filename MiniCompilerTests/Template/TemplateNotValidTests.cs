using System.IO;

namespace MiniCompilerTests
{
    public class TemplateNotValidTests : NotValidTests 
    {
        protected override string PathSuffix => Path.Combine(base.PathSuffix, "Template");
    }
}
