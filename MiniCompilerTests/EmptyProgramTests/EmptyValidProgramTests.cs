using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class EmptyValidProgramTests : BaseTests
    {
        private const int expectedResult = 0;


        public EmptyValidProgramTests()
            : base(@".\EmptyProgramTests\Valid")
        {
        }

        [TestMethod]
        public void TestOneLineProgram()
        {
            // Arrange
            string name = GetCaseName(GetCaller());
            string program = GetPath(name);

            // Act
            int result = Compiler.Main(GetArgs(program));

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TestThreeLineProgram()
        {
            // Arrange
            string name = GetCaseName(GetCaller());
            string program = GetPath(name);

            // Act
            int result = Compiler.Main(GetArgs(program));

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TestWhitespaceInsideBraces()
        {
            // Arrange
            string name = GetCaseName(GetCaller());
            string program = GetPath(name);

            // Act
            int result = Compiler.Main(GetArgs(program));

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TestWhitespaceBeforeProgram()
        {
            // Arrange
            string name = GetCaseName(GetCaller());
            string program = GetPath(name);

            // Act
            int result = Compiler.Main(GetArgs(program));

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TestWhitespaceAfterProgramBeforeBraces()
        {
            // Arrange
            string name = GetCaseName(GetCaller());
            string program = GetPath(name);

            // Act
            int result = Compiler.Main(GetArgs(program));

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void TestWhitespaceAfterBraces()
        {
            // Arrange
            string name = GetCaseName(GetCaller());
            string program = GetPath(name);

            // Act
            int result = Compiler.Main(GetArgs(program));

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
