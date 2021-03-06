﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Unary
{
    [TestClass]
    public class DoubleCastValidTests : UnaryValidTests
    {
        [TestMethod]
        public void TestOneLineProgram()
        {
            Invoke();
        }

        [TestMethod]
        public void TestMultilineStatement()
        {
            Invoke();
        }

        [TestMethod]
        public void TestUsedManyTimesInRow()
        {
            Invoke();
        }

        [TestMethod]
        public void TestMultilineParentheses()
        {
            Invoke();
        }
    }
}