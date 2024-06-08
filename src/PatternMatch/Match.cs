using System;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using PatternMatch;
using PatternMatching;

public class PatternMatcher
{
    public List<Tuple<DataTable, double>> Match(int totalPixels, string filePath, string algorithm)
    {
        List<Tuple<DataTable, double>> results = new List<Tuple<DataTable, double>>();

        (int Width, int Height) = GetDimensionsForPixels(totalPixels);

        var parser = new Parser();

        Image input = Image.FromFile(filePath);
        string ascii = parser.ConvertImageToAscii(input, Width, Height);

        Dictionary<string, string> fingerprintsDatabase = DatabaseManager.FetchFingerprintsFromDatabase(Width, Height);

        bool exist = false;
        if (algorithm == "BM")
        {
            foreach (var fingerprint in fingerprintsDatabase)
            {
                bool isMatchBM = BM.Search(ascii.ToCharArray(), fingerprint.Value.ToCharArray());
                if (isMatchBM)
                {
                    Console.WriteLine($"Exact match found with {fingerprint.Key} with similarity 100%.");
                    exist = true;
                    break;
                }
            }
        }
        else if (algorithm == "KMP")
        {
            foreach (var fingerprint in fingerprintsDatabase)
            {
                bool isMatchKMP = KMP.KMPSearch(ascii, fingerprint.Value);
                if (isMatchKMP)
                {
                    Console.WriteLine($"Exact match found with {fingerprint.Key} with similarity 100%.");
                    exist = true;
                    break;
                }
            }
        }

        if (!exist)
        {
            Console.WriteLine("No match fingerprint found");
        }

        // Find similar matches (using Levenshtein Distance, threshold: 80%)
        Console.WriteLine("\nLevenshtein Distance:");
        List<KeyValuePair<string, double>> similarFingerprints = new List<KeyValuePair<string, double>>();

        foreach (var fingerprint in fingerprintsDatabase)
        {
            double similarity = LevenshteinDistance.Similarity(ascii, fingerprint.Value);
            similarFingerprints.Add(new KeyValuePair<string, double>(fingerprint.Key, similarity));
        }

        similarFingerprints.Sort((x, y) => y.Value.CompareTo(x.Value));

        Regex regex = new Regex();
        for (int i = 0; i < Math.Min(5, similarFingerprints.Count); i++)
        {
            // Console.WriteLine($"Text: {similarFingerprints[i].Key}");
            string alteredText = regex.Alter(similarFingerprints[i].Key);
            // Console.WriteLine($"Altered: {alteredText}");
            DataTable biodata = DatabaseManager.showBiodata(alteredText);
            double similarityValue = similarFingerprints[i].Value * 100;

            if (biodata.Rows.Count > 0)
            {
                foreach (DataRow row in biodata.Rows)
                {
                    row["nama"] = similarFingerprints[i].Key;
                }
            }

            results.Add(new Tuple<DataTable, double>(biodata, similarityValue));
        }

        if (!exist)
        {
            DatabaseManager.inputData(filePath);
        }

        return results;
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
