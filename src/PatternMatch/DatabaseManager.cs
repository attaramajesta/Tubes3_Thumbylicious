using System;
using System.Drawing;
using System.IO;
using MySql.Data.MySqlClient;
using PatternMatching;

namespace PatternMatch
{
    public class DatabaseManager
    {
        private static string connectionString = "Server=localhost; Port=1234; Database=tubes3; User ID=root; Password=bbee2e7;";

        public static void Insert(string ascii, string name)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection to database established successfully.");

                    // Query to insert fingerprint data
                    string query = "INSERT INTO sidik_jari (berkas_citra, nama) VALUES (@berkas_citra, @nama)";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    // Adding parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@berkas_citra", ascii);
                    command.Parameters.AddWithValue("@nama", name);

                    Console.WriteLine("Inserting fingerprint data...");

                    // Execute the command
                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine($"{rowsAffected} row(s) inserted.");
                }
                catch (MySqlException mysqlEx)
                {
                    Console.WriteLine($"MySQL error: {mysqlEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        public static void LoadFingerprints(string folderPath)
        {
            var parser = new PatternMatching.Parser();

            foreach (string filePath in Directory.GetFiles(folderPath, "*.jpg"))
            {
                try
                {
                    Image img = Image.FromFile(filePath);
                    string ascii = parser.ConvertImageToAscii(img, img.Width, img.Height);
                    string name = Path.GetFileNameWithoutExtension(filePath);
                    Insert(ascii, name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while processing file {filePath}: {ex.Message}");
                }
            }
        }
    }
}