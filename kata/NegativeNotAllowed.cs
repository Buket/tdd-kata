using System;
using System.Collections.Generic;
using System.Linq;

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
}