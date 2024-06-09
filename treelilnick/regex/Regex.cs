using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using treelilnick.connector;

namespace treelilnick.regex
{
    public class AlayRegex
    {
        // Generates a regex pattern to match an "alay" version of the provided name
        private static string CreateAlayRegex(string realName)
        {
            // Dictionary for character substitutions with regex patterns
            var substitution = new Dictionary<char, string>
        {
            {'a', "[a4]?"},
            {'e', "[e3]?"},
            {'i', "[i1]?"},
            {'o', "[o0]?"},
            {'u', "[u]?"},
            {'b', "[b68]"},
            {'g', "[g69]"},
            {'j', "[7j]"},
            {'l', "[l1]"},
            {'s', "[s5]"},
            {'z', "[z2]"},
            {' ', "\\s+"}
        };

            // Build regex pattern by substituting each character in the real name
            string regexPattern = "";
            foreach (char c in realName.ToLower())
            {
                if (substitution.ContainsKey(c))
                {
                    regexPattern += substitution[c];
                }
                else
                {
                    regexPattern += c;
                }
            }

            return regexPattern;
        }

        // Searches for a name in the list that matches the "alay" version of the provided real name
        public static Pair<string, string> SearchAlay(string realName, List<Pair<string, string>> listAlay)
        {
            // Create regex pattern from real name
            Regex regex = new Regex("^" + CreateAlayRegex(realName) + "$", RegexOptions.IgnoreCase);

            // Search for a matching name in the list
            foreach (Pair<string, string> nameAndNIK in listAlay)
            {
                // Return the matching name and NIK
                if (regex.IsMatch(nameAndNIK.First))
                {
                    return nameAndNIK;
                }
            }

            // Throw an exception if no match is found
            throw new Exception("No name match");
        }
    }
}
