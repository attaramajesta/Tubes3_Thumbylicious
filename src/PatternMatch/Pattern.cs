using System;

namespace PatternMatching
{
    public class KMP
    {
        public static bool KMPSearch(string pat, string txt)
        {
            int M = pat.Length;
            int N = txt.Length;

            // Create lps[] that will hold the longest
            // prefix suffix values for pattern
            int[] lps = new int[M];

            // Index for pat[]
            int j = 0;

            // Preprocess the pattern (calculate lps[]
            // array)
            computeLPSArray(pat, M, lps);

            int i = 0;
            while ((N - i) >= (M - j))
            {
                if (pat[j] == txt[i])
                {
                    j++;
                    i++;
                }
                if (j == M)
                {
                    return true;
                }

                else if (i < N && pat[j] != txt[i])
                {
                    if (j != 0)
                        j = lps[j - 1];
                    else
                        i = i + 1;
                }
            }
            return false;
        }

        private static void computeLPSArray(string pat, int M, int[] lps)
        {
            // Length of the previous longest prefix suffix
            int len = 0;
            int i = 1;
            lps[0] = 0;

            // The loop calculates lps[i] for i = 1 to M-1
            while (i < M)
            {
                if (pat[i] == pat[len])
                {
                    len++;
                    lps[i] = len;
                    i++;
                }
                else // (pat[i] != pat[len])
                {
                    // This is tricky. Consider the example.
                    // AAACAAAA and i = 7. The idea is similar
                    // to search step.
                    if (len != 0)
                    {
                        len = lps[len - 1];

                        // Also, note that we do not increment
                        // i here
                    }
                    else // len = 0
                    {
                        lps[i] = len;
                        i++;
                    }
                }
            }
        }
    }

    public class BM
    {
        static int NO_OF_CHARS = 256;

        // A utility function to get maximum of two integers
        static int max(int a, int b) { return (a > b) ? a : b; }

        // The preprocessing function for Boyer Moore's
        // bad character heuristic
        public static bool badCharHeuristic(char[] str, int size, int[] badchar)
        {
            int i;

            // Initialize all occurrences as -1
            for (i = 0; i < NO_OF_CHARS; i++)
                badchar[i] = -1;

            // Fill the actual value of last occurrence
            // of a character
            for (i = 0; i < size; i++)
                badchar[(int)str[i]] = i;

            return true;
        }

        /* A pattern searching function that uses Bad
        Character Heuristic of Boyer Moore Algorithm */
        public static bool Search(char[] txt, char[] pat)
        {
            int m = pat.Length;
            int n = txt.Length;

            int[] badchar = new int[NO_OF_CHARS];

            /* Fill the bad character array by calling
                the preprocessing function badCharHeuristic()
                for given pattern */
            badCharHeuristic(pat, m, badchar);

            int s = 0; // s is shift of the pattern with
                    // respect to text
            while (s <= (n - m))
            {
                int j = m - 1;

                /* Keep reducing index j of pattern while
                    characters of pattern and text are
                    matching at this shift s */
                while (j >= 0 && pat[j] == txt[s + j])
                    j--;

                /* If the pattern is present at current
                    shift, then index j will become -1 after
                    the above loop */
                if (j < 0)
                {
                    return true;
                }

                else
                {
                    /* Shift the pattern so that the bad
                    character in text aligns with the last
                    occurrence of it in pattern. The max
                    function is used to make sure that we get
                    a positive shift. We may get a negative
                    shift if the last occurrence of bad
                    character in pattern is on the right side
                    of the current character. */
                    s += max(1, j - badchar[txt[s + j]]);
                }
            }
            return false;
        }
    }

    public class LevenshteinDistance
    {
        public static int Compute(string str1, string str2)
        {
            int[,] distance = new int[str1.Length + 1, str2.Length + 1];

            // Initialize the first row and column of the matrix
            for (int i = 0; i <= str1.Length; i++)
            {
                distance[i, 0] = i;
            }

            for (int j = 0; j <= str2.Length; j++)
            {
                distance[0, j] = j;
            }

            // Fill in the matrix with minimum edit distances
            for (int i = 1; i <= str1.Length; i++)
            {
                for (int j = 1; j <= str2.Length; j++)
                {
                    int cost = (str1[i - 1] == str2[j - 1]) ? 0 : 1;
                    distance[i, j] = Math.Min(
                        Math.Min(distance[i - 1, j] + 1,     // deletion
                                distance[i, j - 1] + 1),    // insertion
                        distance[i - 1, j - 1] + cost);    // substitution
                }
            }

            // The bottom-right cell contains the Levenshtein distance
            return distance[str1.Length, str2.Length];
        }

        public static double Similarity(string str1, string str2)
        {
            int distance = Compute(str1, str2);
            int maxLength = Math.Max(str1.Length, str2.Length);
            return 1.0 - (double)distance / maxLength;
        }
    }
}
