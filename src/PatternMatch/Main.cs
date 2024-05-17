using System;
using System.Collections.Generic;
using System.Drawing;
using PatternMatch;
using PatternMatching;

public class PatternMatcher
{
    public static void Main(string[] args)
    {
        // string folderPath = "database";
        // if (!Directory.Exists(folderPath))
        // {
        //     Console.WriteLine($"The folder path {folderPath} does not exist.");
        //     return;
        // }
        // Load fingerprint data from the database
        // DatabaseManager.LoadFingerprints(folderPath);

        Console.WriteLine("Enter the total number of pixels (30, 60, 90, 120, 240, 480, 10000):");
        int totalPixels = int.Parse(Console.ReadLine());

        (int Width, int Height) = GetDimensionsForPixels(totalPixels);

        var parser = new PatternMatching.Parser();

        string filePath = "database/Jojo.jpg";
        Image img1 = Image.FromFile(filePath);
        string ascii = parser.ConvertImageToAscii(img1, Width, Height);
        string name = Path.GetFileNameWithoutExtension(filePath);
        DatabaseManager.Insert(ascii, name);

        // Belum ada MySQL jadi hardcode dulu
        string filePath2 = "database/Jebe.jpg";
        Image img2 = Image.FromFile("database/Jebe.jpg");
        string ascii2 = parser.ConvertImageToAscii(img2, Width, Height);
        string name2 = Path.GetFileNameWithoutExtension(filePath2);
        DatabaseManager.Insert(ascii2, name2);

        string filePath3 = "database/Attara.jpg";
        Image img3 = Image.FromFile("database/Attara.jpg");
        string ascii3 = parser.ConvertImageToAscii(img3, Width, Height);
        string name3 = Path.GetFileNameWithoutExtension(filePath3);
        DatabaseManager.Insert(ascii3, name3);

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
