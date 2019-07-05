using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kata
{
    public class StringCalculator
    {
        private readonly string[] _skippedSymbols;
        private StringBuilder _digits;
        
        private StringBuilder _separatorsBuffer;
        private readonly List<string> _separators;
        private Stack<string> _separatorStack_;
        
        private List<int> _negartiveNumbers;
        private static int _callCount;
        
        public int GetCalledCount()
        {
            return _callCount;
        }

        public event Action<string, int> AddOccured; 
        
        public StringCalculator(string[] delimeters = null, string[] skippedSymbols = null)
        {
            if (delimeters == null || delimeters.Length == 0)
            {
                _separators = new List<string>{ "," };
            }
            else
            {
                _separators = new List<string>(delimeters.Select(_ => _.ToString()));
            }

            _skippedSymbols = skippedSymbols ?? new[] {"/"};
            _digits = new StringBuilder();
            _separatorStack_= new Stack<string>();
            _negartiveNumbers = new List<int>();
            _separatorsBuffer = new StringBuilder();
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
                if (/*IsSeparator(c.ToString())*/IsSeparatorPart(c.ToString()))
                {
                    sum = AccamulatePositive(sum);
                    _digits.Clear();
                    
                    if (_separatorStack_.Any())
                        throw new ArgumentException();
                    
                    _separatorsBuffer.Append(c);
                    continue;
                }
                
                //it digit
                /*save from _separatorBuffer to _separators, flush buffer*/
                if (_separatorsBuffer.Length >0 && !IsValidSeparator(_separatorsBuffer))
                {
                    throw new ArgumentException();
                }

                if (_separatorsBuffer.Length > 0)
                {
                    _separatorStack_.Push(_separatorsBuffer.ToString());
                    _separatorsBuffer.Clear();   
                }
                
                _digits.Append(c);
                if (_separatorStack_.Any())
                    _separatorStack_.Pop();
            }
            
            
            sum = AccamulatePositive(sum);

            if (_negartiveNumbers.Any())
            {
                throw new NegativeNotAllowed(_negartiveNumbers);
            }

            this.AddOccured?.Invoke(numbers, sum);
            return sum;
        }

        private bool IsValidSeparator(StringBuilder separatorsBuffer)
        {
            var s = separatorsBuffer.ToString();
            return _separators.Any(_ => _.Equals(s));
        }


        private int AccamulatePositive(int sum)
        {
            if (_digits.Length <= 0) return sum;
            
            var number = Int32.Parse(_digits.ToString());


            if (number > 1000)
                return sum;
            
            if (number < 0)
            {
                _negartiveNumbers.Add(number);
            }

            sum += number;

            return sum;
        }

        private bool IsSeparator(string c)
        {
            return _separators.Contains(c);
        }
        
        private bool IsSeparatorPart(string c)
        {
            return _separators.Any(_ => _.Contains(c));
        }
    }
}