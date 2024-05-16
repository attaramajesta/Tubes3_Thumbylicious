using System;
using System.Collections.Generic;
using System.Drawing;
using PatternMatching;

public class PatternMatcher
{
    public static void Main(string[] args)
    {
        var parser = new PatternMatching.Parser();

        Image input = Image.FromFile("database/fingerprint1.jpg");
        string ascii = parser.ConvertImageToAscii(input);

        // Belum ada MySQL jadi hardcode dulu
        Image img2 = Image.FromFile("database/fingerprint2.jpg");
        string ascii2 = parser.ConvertImageToAscii(img2);

        Image img3 = Image.FromFile("database/fingerprint3.jpg");
        string ascii3 = parser.ConvertImageToAscii(img3);

        // Load ke dictionary (method loader harus ditambahin nanti)
        Dictionary<string, string> fingerprintsDatabase = new Dictionary<string, string>
        {
            { "John Doe", ascii2 },
            { "Jane Doe", ascii3 }
        };

        // Cari exact match dulu pake KMP dan BM
        Console.WriteLine("\nKMP and BM:");
        foreach (var fingerprint in fingerprintsDatabase)
        {
            bool isMatchKMP = KMP.KMPSearch(ascii, fingerprint.Value);
            bool isMatchBM = BM.search(ascii.ToCharArray(), fingerprint.Value.ToCharArray());

            if (isMatchKMP || isMatchBM)
            {
                Console.WriteLine($"Exact match found with {fingerprint.Key} with similarity 100%.");
            }
            else
            {
                Console.WriteLine($"No exact match found for {fingerprint.Key}.");
            }
        }

        // Cari similar match (bentuknya list maks 5 orang) pake Levenshtein Distance (tuning: 80%)
        Console.WriteLine("\nLevenshtein Distance:");
        List<KeyValuePair<string, double>> similarFingerprints = new List<KeyValuePair<string, double>>();

        foreach (var fingerprint in fingerprintsDatabase)
        {
            double similarity = LevenshteinDistance.Similarity(ascii, fingerprint.Value);
            similarFingerprints.Add(new KeyValuePair<string, double>(fingerprint.Key, similarity));
        }

        similarFingerprints.Sort((x, y) => y.Value.CompareTo(x.Value));

        for (int i = 0; i < Math.Min(5, similarFingerprints.Count); i++)
        {
            Console.WriteLine($"{similarFingerprints[i].Key}: {similarFingerprints[i].Value * 100}% similarity");
        }
    }
}