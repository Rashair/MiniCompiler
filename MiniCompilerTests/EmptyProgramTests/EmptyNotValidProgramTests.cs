using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCompilerTests.EmptyProgramTests
{
    [TestClass]
    public class EmptyNotValidProgramTests : BaseTests
    {
        private const int expectedResult = 2;

        public EmptyNotValidProgramTests()
           : base(@".\EmptyProgramTests\NotValid")
        {
        }


        [TestMethod]
        public void TestNoProgram()
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
        public void TestProgramNoBraces()
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
        public void TestProgramNoOpeningBrace()
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
        public void TestProgramNoClosingBrace()
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
        public void TestProgramWrongBraceOrder()
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
        public void TestBracesBeforeProgram()
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
