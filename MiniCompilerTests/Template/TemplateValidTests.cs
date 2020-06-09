using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCompilerTests.Template
{
    public class TemplateValidTests : ValidTests
    {
        protected override string PathSuffix => Path.Combine(base.PathSuffix, "Template");
    }
}
