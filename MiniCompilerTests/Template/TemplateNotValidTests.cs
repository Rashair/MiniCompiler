namespace MiniCompilerTests.Template
{
    public class TemplateNotValidTests : NotValidTests 
    {
        protected override string PathSuffix => Path.Combine(base.PathSuffix, "Template");
    }
}
