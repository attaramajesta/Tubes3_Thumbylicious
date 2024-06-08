using System;
using System.Data;
using System.Collections.Generic;
using System.Drawing;
using PatternMatch;
using PatternMatching;

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter the total number of pixels (30, 60, 90, 120, 240, 480, 10000):");
        int totalPixels;
        while (!int.TryParse(Console.ReadLine(), out totalPixels) || !IsValidPixelOption(totalPixels))
        {
            Console.WriteLine("Invalid input. Please enter a valid number of pixels (30, 60, 90, 120, 240, 480, 10000):");
        }

        string filePath = "input/Jojo.BMP";

        Console.WriteLine("Enter the Algorithm you want to use (KMP or BM):");
        string algorithm = Console.ReadLine();
        while (algorithm != "KMP" && algorithm != "BM")
        {
            Console.WriteLine("Invalid input. Please enter the Algorithm you want to use (KMP or BM):");
            algorithm = Console.ReadLine();
        }

        PatternMatcher matcher = new PatternMatcher();
        List<Tuple<DataTable, double>> results = matcher.Match(totalPixels, filePath, algorithm);

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
    }

    private static bool IsValidPixelOption(int totalPixels)
    {
        return totalPixels == 30 || totalPixels == 60 || totalPixels == 90 ||
               totalPixels == 120 || totalPixels == 240 || totalPixels == 480 || totalPixels == 10000;
    }
}
