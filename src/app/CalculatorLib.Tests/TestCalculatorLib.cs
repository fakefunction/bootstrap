using System.Diagnostics;
using CalculatorLib.Lib;
using NUnit.Framework;

namespace CalculatorLib.Tests
{
    [TestFixture]
    public class TestCalculatorLib
    {
        [Test]
        public static void TestAddWithNUnit()
        {
            Trace.WriteLine("TestAddWithNUnit");
            Assert.AreEqual(4, Lib.CalculatorLib.Add(3, 1));
            Assert.AreEqual(0, Lib.CalculatorLib.Add(0, 0));
        }
    }
}