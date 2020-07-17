using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Advent2019
{
    internal class PasswordTester
    {
        private int minimum;
        private int maximum;
        private List<string> potential_passwords = new List<string>();

        public PasswordTester(int min, int max)
        {
            this.minimum = min;
            this.maximum = max;
        }

        internal int GetPotentialPasswordCount()
        {
            List<string> range_strings = Enumerable.Range(minimum, (maximum - minimum)).Select(x => x.ToString()).ToList();
            //var repeat_chars = new Regex(@"^[0-9]*([0-9])\1[0-9]*$");
            var only_2_repeat_chars = new Regex(@"(([^1]|^)11([^1]|$)|([^2]|^)22([^2]|$)|([^3]|^)33([^3]|$)|([^4]|^)44([^4]|$)|([^5]|^)55([^5]|$)|([^6]|^)66([^6]|$)|([^7]|^)77([^7]|$)|([^8]|^)88([^8]|$)|([^9]|^)99([^9]|$)|([^0]|^)00([^0]|$))");
            List<string> repeat_matches = range_strings.Where(s => only_2_repeat_chars.IsMatch(s)).ToList();
            foreach (string match in repeat_matches)
            {
                int[] digits = match.Select(c => int.Parse(c.ToString())).ToArray();
                int? last_digit = null;
                bool valid = true;
                foreach(var digit in digits)
                {
                    if (digit < (last_digit ??= digit)) { valid = false; break; }
                    else { last_digit = digit; }
                }
                if (valid) { potential_passwords.Add(match); }
            }

            return potential_passwords.Count();
        }
    }
}