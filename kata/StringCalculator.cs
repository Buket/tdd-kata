using System;
using System.Collections.Generic;
using System.Linq;

namespace kata
{
    public class StringCalculator
    {
        private readonly char[] _delimeters;
        private Stack<char> _number;
        private Stack<char> _delimeter;

        public StringCalculator(char[] delimeters = null)
        {
            if (delimeters == null || delimeters.Length == 0)
                _delimeters = new [] { ',' };
            else
                _delimeters = delimeters;
        }
        public int Add(string numbers)
        {
            if (string.IsNullOrEmpty(numbers))
                return 0;

            if (!_delimeters.All(delimetr => numbers.Contains(delimetr)))
            {
                return Int32.Parse(numbers);
            }

            var stringNumbers = numbers.Split(_delimeters, StringSplitOptions.RemoveEmptyEntries);
            var delimetrOccurence = 0;


            var sum = 0;
            foreach (var stringNumber in stringNumbers)
            {
                if (delimetrOccurence > 1)
                {
                    throw new ArgumentException();
                }
                if (_delimeters.Any(delimetr => stringNumber.Contains(delimetr)))
                {
                    delimetrOccurence++;
                    continue;
                }

                sum += Int32.Parse(stringNumber);
            }
            
            return sum;
        }
    }
}