﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kata
{
    public class StringCalculator
    {
        private readonly char[] _skippedSymbols;
        private readonly char[] _separators;
        private StringBuilder _digits;
        private Stack<char> _separatorStack;

        public StringCalculator(char[] delimeters = null, char[] skippedSymbols = null)
        {
            if (delimeters == null || delimeters.Length == 0)
                _separators = new [] { ',' };
            else
                _separators = delimeters;

            _skippedSymbols = skippedSymbols ?? new[] {'/'};
            _digits = new StringBuilder();
            _separatorStack = new Stack<char>();
        }
        public int Add(string numbers)
        {
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
                    sum = AccamulateSum(sum);
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
            
            
            sum = AccamulateSum(sum);

            return sum;
        }

        private int AccamulateSum(int sum)
        {
            if (_digits.Length > 0)
            {
                sum += Int32.Parse(_digits.ToString());
            }

            return sum;
        }

        private bool IsSeparator(char c)
        {
            return _separators.Contains(c);
        }
    }
}