﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniCompilerTests.Operators.Relation
{
    [TestClass]
    public class GreaterOrEqualNotValidTests : RelationNotValidTests
    {
        [TestMethod]
        public void TestInvalidType()
        {
            Invoke();
        }

        [TestMethod]
        public void TestMultilineExpression()
        {
            Invoke();
        }
    }
}
