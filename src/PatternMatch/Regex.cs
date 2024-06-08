using System;
using System.Collections.Generic;

namespace PatternMatch
{
    public class Regex
    {
        private static readonly Dictionary<string, string> regexPatterns = new Dictionary<string, string>
        {
            {"A", "[Aa4]?"}, {"B", "[Bb]"}, {"C", "[Cc]"}, {"D", "[Dd]"}, {"E", "[Ee3]?"},
            {"F", "[Ff]"}, {"G", "[Gg6]"}, {"H", "[Hh]"}, {"I", "[Ii1]?"}, {"J", "[Jj]"},
            {"K", "[Kk]"}, {"L", "[Ll]"}, {"M", "[Mm]"}, {"N", "[Nn]"}, {"O", "[Oo0]?"},
            {"P", "[Pp]"}, {"Q", "[Qq]"}, {"R", "[Rr]"}, {"S", "[Ss5]"}, {"T", "[Tt]"},
            {"U", "[Uu]?"}, {"V", "[Vv]"}, {"W", "[Ww]"}, {"X", "[Xx]"}, {"Y", "[Yy]"},
            {"Z", "[Zz2]"}, {" ", "[ ]"}
        };

        public string Alter(string text)
        {
            text = text.ToUpper();
            foreach (var kvp in regexPatterns)
            {
                text = text.Replace(kvp.Key, kvp.Value);
            }
            return text;
        }
    }
}
