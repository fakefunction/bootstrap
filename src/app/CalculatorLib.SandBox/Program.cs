﻿using System;
using CalculatorLib.Lib;

namespace CalculatorLib.SandBox
{
    internal class Program
    {
        private static void Main()
        {
            int r = Calculator.Add(3, 4);
            Console.WriteLine("Result is {0}", r);
            Console.ReadLine();
        }
    }
}