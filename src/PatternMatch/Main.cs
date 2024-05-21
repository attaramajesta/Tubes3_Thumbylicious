using System;
using System.Collections.Generic;
using System.Drawing;
using PatternMatch;
using PatternMatching;

public class PatternMatcher
{
    public static void Main(string[] args)
    {
        DatabaseManager.generateBiodata();

        Console.WriteLine("Enter the total number of pixels (30, 60, 90, 120, 240, 480, 10000):");
        int totalPixels = int.Parse(Console.ReadLine());

        (int Width, int Height) = GetDimensionsForPixels(totalPixels);

        var parser = new PatternMatching.Parser();

        string filePath = "input/Jojo.png";
        Image input = Image.FromFile(filePath);
        string ascii = parser.ConvertImageToAscii(input, Width, Height);

        Dictionary<string, string> fingerprintsDatabase = DatabaseManager.FetchFingerprintsFromDatabase(Width, Height);

        // Cari exact match dulu pake KMP dan BM
        Console.WriteLine("\nKMP and BM:");
        bool exist = false;
        foreach (var fingerprint in fingerprintsDatabase)
        {
            bool isMatchKMP = KMP.KMPSearch(ascii, fingerprint.Value);
            bool isMatchBM = BM.search(ascii.ToCharArray(), fingerprint.Value.ToCharArray());
            if (isMatchKMP || isMatchBM)
            {
                Console.WriteLine($"Exact match found with {fingerprint.Key} with similarity 100%.");
                exist = true;
                break;
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
            DatabaseManager.showBiodata(similarFingerprints[i].Key);
        }
        
        if (exist == false)
        {
            DatabaseManager.inputData(filePath);
        }



    }


    private static (int, int) GetDimensionsForPixels(int totalPixels)
    {
        return totalPixels switch
        {
            30 => (5, 6),
            60 => (6, 10),
            90 => (9, 10),
            120 => (10, 12),
            240 => (15, 16),
            480 => (20, 24),
            10000 => (100, 100),
            _ => throw new ArgumentException("Invalid number of pixels. Please choose from the given options.")
        };
     }
}
