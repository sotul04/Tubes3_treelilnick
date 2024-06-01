using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace treelilnick.preproccess
{
    public class KMP
    {
        public static List<int> ComputeLps(string pattern)
        {
            List<int> lps = [];
            int n = pattern.Length;
            for (int idx = 0; idx < n; idx++)
            {
                lps.Add(0);
            }
            int i = 1, j = 0;
            while (i < n)
            {
                if (pattern[i] == pattern[j])
                {
                    lps[i] = j + 1;
                    i++;
                    j++;
                }
                else if (j > 0)
                {
                    j = lps[j - 1];
                }
                else
                {
                    i++;
                }
            }
            return lps;
        }
        public static bool FindPattern(string text, string pattern)
        {
            List<int> lps = ComputeLps(pattern);
            int n = text.Length;
            int m = pattern.Length;
            int i = 0;
            int j = 0;

            while (i < n)
            {
                if (text[i] == pattern[j])
                {
                    if (j == m - 1)
                    {
                        return true;
                    }
                    i++;
                    j++;
                }
                else if (j > 0)
                {
                    j = lps[j - 1];
                }
                else
                {
                    i++;
                }
            }
            return false;
        }
    }
    public class BayerMoore
    {
        public static List<int> BuildLast(String pattern)
        {
            List<int> last = [];
            for (int i = 0; i < 256; i++)
            {
                last.Add(-1);
            }
            for (int i = 0; i < pattern.Length; i++)
            {
                last[pattern[i]] = i;
            }
            return last;
        }
        public static bool FindPattern(string text, string pattern)
        {
            List<int> last = BuildLast(pattern);
            int n = text.Length;
            int m = pattern.Length;
            int i = m - 1;

            if (i > n - 1)
            {
                return false;
            }

            int j = m - 1;
            do
            {
                if (text[i] == pattern[j]) {
                    if (j == 0)
                    {
                        return true;
                    }
                    else
                    {
                        i--;
                        j--;
                    }
                }
                else
                {
                    int lo = last[text[i]];
                    i = i + m - Math.Min(j, 1 + lo);
                    j = m - 1;
                }
            } while (i < n);

            return false;
        }
    }
    public class ClosestString
    {
        public static int HammingDistance(string s1, string s2) {
            int len = Math.Min(s1.Length, s2.Length);
            int count = 0;
            for (int i = 0; i < len; i++) {
                if (s1[i] != s2[i]) {
                    count++;
                }
            }
            return count;
        }
    }
}