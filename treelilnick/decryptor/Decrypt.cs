using System;
using System.Collections.Generic;
using System.Text;

namespace treelilnick.decryptor
{
    public class Decrypt
    {
        private static readonly string charlist = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$^&*()_.+-=<>?/ ";
        private static readonly Dictionary<char, int> charordo;
        private static readonly Dictionary<int, char> charnum;
        private static readonly int length;

        static Decrypt()
        {
            charordo = new Dictionary<char, int>();
            charnum = new Dictionary<int, char>();

            // Populate the dictionaries
            for (int i = 0; i < charlist.Length; i++)
            {
                charordo[charlist[i]] = i;
                charnum[i] = charlist[i];
            }

            length = charlist.Length;
        }

        public static string DecryptCipher(string cipher) {
            StringBuilder cstring = new StringBuilder(cipher);
            int size = cstring.Length;
            for (int i = 0; i < size; i++) {
                int coded = charordo[cstring[i]] - size;
                if (coded < 0) coded += length;
                cstring[i] = charnum[coded];
            }
            return cstring.ToString();
        }
    }
}