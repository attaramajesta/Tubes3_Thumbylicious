using System;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using PatternMatch;
using PatternMatching;

public class PatternMatcher
{
    public static void Main(string[] args)
    {
        List<Tuple<DataTable, double>> results = new List<Tuple<DataTable, double>>();

        Console.WriteLine("Enter the total number of pixels (30, 60, 90, 120, 240, 480, 10000):");
        int totalPixels = int.Parse(Console.ReadLine());

        (int Width, int Height) = GetDimensionsForPixels(totalPixels);

        var parser = new PatternMatching.Parser();

        string filePath = "input/Jojo.png";
        Image input = Image.FromFile(filePath);
        string ascii = parser.ConvertImageToAscii(input, Width, Height);

        Dictionary<string, string> fingerprintsDatabase = DatabaseManager.FetchFingerprintsFromDatabase(Width, Height);

        Console.WriteLine("Enter the Algorithm you wanted to use (KMP or BM)");
        string algorithm = Console.ReadLine();


        while (algorithm != "KMP" && algorithm != "BM")
        {
            Console.WriteLine("Enter the Algorithm you wanted to use (KMP or BM)");
            algorithm = Console.ReadLine();

        }

        bool exist = false;
        if(algorithm == "BM")
        {
            exist = false;
            foreach (var fingerprint in fingerprintsDatabase)
            {
                bool isMatchBM = BM.search(ascii.ToCharArray(), fingerprint.Value.ToCharArray());
                if (isMatchBM)
                {
                    Console.WriteLine($"Exact match found with {fingerprint.Key} with similarity 100%.");
                    exist = true;
                    break;
                }
            }
        }
        else
        {
            exist = false;
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
            DataTable biodata = DatabaseManager.showBiodata(similarFingerprints[i].Key);
            double similarityValue = similarFingerprints[i].Value * 100;
            results.Add(new Tuple<DataTable, double>(biodata, similarityValue));
        }

        foreach (var entry in results)
        {
            DataTable biodataTable = entry.Item1;
            foreach (DataRow row in biodataTable.Rows)
            {
                Console.WriteLine($"NIK: {row["NIK"]}");
                Console.WriteLine($"Nama: {row["nama"]}");
                Console.WriteLine($"Tempat Lahir: {row["tempat_lahir"]}");
                Console.WriteLine($"Tanggal Lahir: {row["tanggal_lahir"]}");
                Console.WriteLine($"Jenis Kelamin: {row["jenis_kelamin"]}");
                Console.WriteLine($"Golongan Darah: {row["golongan_darah"]}");
                Console.WriteLine($"Alamat: {row["alamat"]}");
                Console.WriteLine($"Agama: {row["agama"]}");
                Console.WriteLine($"Status Perkawinan: {row["status_perkawinan"]}");
                Console.WriteLine($"Pekerjaan: {row["pekerjaan"]}");
                Console.WriteLine($"Kewarganegaraan: {row["kewarganegaraan"]}");
                Console.WriteLine();
            }
            Console.WriteLine("Similarity: " + entry.Item2);
            Console.WriteLine();
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
