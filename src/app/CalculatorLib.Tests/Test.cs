using System.Diagnostics;
using CalculatorLib.Lib;
using NUnit.Framework;

namespace CalculatorLib.Tests
{
    [TestFixture]
    public class Test
    {
        [Test]
        public static void TestAddWithNUnit()
        {
            Trace.WriteLine("TestAddWithNUnit");
            Assert.AreEqual(4, SomeLib.Add(3, 1));
            Assert.AreEqual(0, SomeLib.Add(0, 0));
        }
    }
}