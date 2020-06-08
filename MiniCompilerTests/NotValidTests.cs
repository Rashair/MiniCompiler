using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiniCompiler;

namespace MiniCompilerTests
{
    public class NotValidTests : BaseTests
    {
        public NotValidTests() :
             base(2, Helpers.Self())
        {
        }

        protected int? ExpectedErrors { get; set; }

        [TestInitialize]
        public override void Init()
        {
            base.Init();
            ExpectedErrors = null;
        }

        protected override void AssertCorrect()
        {
            if (ExpectedErrors.HasValue)
            {
                int actualErrors = Compiler.errors;
                Assert.AreEqual(ExpectedErrors, actualErrors, "Errors count should match expected");
            }
        }
    }
}
