using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kata
{
    public class NegativeNotAllowed : Exception
    {
        private IList<int> negativeNumbers;

        public IList<int> NegativeNumbers => negativeNumbers;

        public NegativeNotAllowed(string msg = "Negative not allowed") : base(msg)
        {
        }
        
        public NegativeNotAllowed(IEnumerable<int> negativeNumbers, string msg = "Negative not allowed") : base(msg)
        {
            this.negativeNumbers = negativeNumbers.Select(_ => _).ToList();
        }
    }
    public class StringCalculator
    {
        private readonly char[] _skippedSymbols;
        private readonly char[] _separators;
        private StringBuilder _digits;
        private Stack<char> _separatorStack;
        private List<int> _negartiveNumbers;
        private int _callCount;

        public StringCalculator(char[] delimeters = null, char[] skippedSymbols = null)
        {
            if (delimeters == null || delimeters.Length == 0)
                _separators = new [] { ',' };
            else
                _separators = delimeters;

            _skippedSymbols = skippedSymbols ?? new[] {'/'};
            _digits = new StringBuilder();
            _separatorStack = new Stack<char>();
            _negartiveNumbers = new List<int>();
        }
        public int Add(string numbers)
        {
            _callCount++;
            if (string.IsNullOrEmpty(numbers))
                return 0;

            if (_skippedSymbols.Any(_ => numbers.Contains(_)))
            {
                numbers = _skippedSymbols.Aggregate(numbers, 
                    (current, skippedSymbol) => 
                        current.Replace(skippedSymbol.ToString(), string.Empty));
            }

            if (!_separators.All(delimetr => numbers.Contains(delimetr)))
            {
                return Int32.Parse(numbers);
            }

            int sum = 0;
            foreach (var c in numbers.ToCharArray())
            {
                if (IsSeparator(c))
                {
                    sum = AccamulatePositive(sum);
                    _digits.Clear();
                    
                    if (_separatorStack.Any())
                        throw new ArgumentException();
                    
                    _separatorStack.Push(c);
                    
                    continue;
                }
                

                _digits.Append(c);
                if (_separatorStack.Any())
                    _separatorStack.Pop();
            }
            
            
            sum = AccamulatePositive(sum);

            if (_negartiveNumbers.Any())
            {
                throw new NegativeNotAllowed(_negartiveNumbers);
            }

            return sum;
        }

        public int GetCalledCount()
        {
            return _callCount;
        }
        
        private int AccamulatePositive(int sum)
        {
            if (_digits.Length > 0)
            {
                var number = Int32.Parse(_digits.ToString());
                if (number < 0)
                {
                    _negartiveNumbers.Add(number);
                }

                sum += number;
            }

            return sum;
        }

        private bool IsSeparator(char c)
        {
            return _separators.Contains(c);
        }
    }
}