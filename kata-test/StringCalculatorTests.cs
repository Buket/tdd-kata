using System;
using System.Linq;
using kata;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace kata_test
{
    [TestClass]
    public class StringCalculatorTests
    {
        [TestMethod]
        public void StringCalculatorReturn0__WhenNumbersEmptyString()
        {
            var numbers = string.Empty;
            var calculator = new StringCalculator();
            var expected = 0;

            var result = calculator.Add(numbers);
            
            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        public void StringCalculatorReturn3_WhenNumbers_12()
        {
            var numbers = "1,2";
            var calculator = new StringCalculator();
            var expected = 3;

            var result = calculator.Add(numbers);
            
            Assert.IsTrue(result == expected);
        }
        
        [DataTestMethod]
        [DataRow("1,2,3", 6)]
        [DataRow("50", 50)]
        [DataRow("13,", 13)]
        [DataRow(",7", 7)]
        public void StringCalculatorReturn3_WhenNumbers(string numbers, int expected)
        {
            var calculator = new StringCalculator();

            var result = calculator.Add(numbers);
            
            Assert.IsTrue(result == expected);
        }
        
        [TestMethod]
        public void StringCalculatorReturn3_WhenNumbersWithNewLine()
        {
            var numbers = "1\n2";
            var calculator = new StringCalculator(new []{'\n'});
            var expected = 3;

            var result = calculator.Add(numbers);
            
            Assert.IsTrue(result == expected);
        }
        
        [TestMethod]
        public void StringCalculatorReturn3_WhenNumbersWithComma()
        {
            var numbers = "1,2";
            var calculator = new StringCalculator(new []{','});
            var expected = 3;

            var result = calculator.Add(numbers);
            
            Assert.IsTrue(result == expected);
        }
        
        [TestMethod]
        public void StringCalculatorReturn3_WhenNumbersWithNewLineAndComma()
        {
            var numbers = "1\n2,3";
            var calculator = new StringCalculator(new []{'\n', ','});
            var expected = 6;

            var result = calculator.Add(numbers);
            
            Assert.IsTrue(result == expected);
        }
        
        [TestMethod]
        public void StringCalculatorThrowException_WhenNumbersWithNewLineAndCommaSequential()
        {
            var numbers = "1,\n";
            var calculator = new StringCalculator(new []{'\n', ','});

            Assert.ThrowsException<ArgumentException>(() => calculator.Add(numbers));
        }

        [TestMethod]
        public void StringCalculatorWorkCorrect__WhenDiffentDelimeters()
        {
            var numbers = "//;\n1;2";
            var calculator = new StringCalculator(new []{';'}, new []{'/'});
            var expected = 3;

            var result = calculator.Add(numbers);
            
            Assert.AreEqual(expected, result);
        }
        
        [TestMethod]
        public void StringCalculatorThrowException_WhenPassedNegativeNumbers()
        {
            var numbers = "-1,2";
            var calculator = new StringCalculator(new []{','});

            Assert.ThrowsException<NegativeNotAllowed>(() => calculator.Add(numbers));
        }
        
        [TestMethod]
        public void StringCalculatorThrowExceptionWithNegativeNumbcount_WhenPassedNegativeNumbers()
        {
            var numbers = "-1,-2,-3";
            var calculator = new StringCalculator(new []{','});
            var e = Assert.ThrowsException<NegativeNotAllowed>(() => calculator.Add(numbers));
            Assert.AreEqual(3, e.NegativeNumbers.Count);
        }

        [DataTestMethod]
        [DataRow(6)]

        public void GetCalledCountReturnZero__WhenCalled(int calcCallCount)
        {
            var numbers = "1,2";
            var calculator = new StringCalculator(new []{','});
            

            var oldCallCount = calculator.GetCalledCount();
            Enumerable.Range(0, calcCallCount).ToList().ForEach(_ => calculator.Add(numbers));
            var calledCount = calculator.GetCalledCount();

            var nowCalledCount = calledCount - oldCallCount;
            Assert.AreEqual(calcCallCount, nowCalledCount);
            
            
        }
    }
}