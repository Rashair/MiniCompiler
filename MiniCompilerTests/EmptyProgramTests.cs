using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests
{
    [TestClass]
    public class EmptyProgramTests : BaseTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            // Arrange
            string program = Path.GetFullPath("./TestCases/empty-prog-1.txt");
           
            // Act
            int result = Compiler.Main(GetArgs(program));

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestThreeLineProgram()
        {
            // Arrange
            string program = Path.GetFullPath("./TestCases/empty-prog-2.txt");

            // Act
            int result = Compiler.Main(GetArgs(program));

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestWhitespaceInsideBraces()
        {
            // Arrange
            string program = Path.GetFullPath("./TestCases/empty-prog-3.txt");

            // Act
            int result = Compiler.Main(GetArgs(program));

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestWhitespaceBeforeProgram()
        {
            // Arrange
            string program = Path.GetFullPath("./TestCases/empty-prog-4.txt");

            // Act
            int result = Compiler.Main(GetArgs(program));

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestWhitespaceAfterProgramBeforeBraces()
        {
            // Arrange
            string program = Path.GetFullPath("./TestCases/empty-prog-5.txt");

            // Act
            int result = Compiler.Main(GetArgs(program));

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void TestWhitespaceAfterBraces()
        {
            // Arrange
            string program = Path.GetFullPath("./TestCases/empty-prog-6.txt");

            // Act
            int result = Compiler.Main(GetArgs(program));

            Assert.AreEqual(0, result);
        }
    }
}
