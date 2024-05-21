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

        // Fetch data sidik_jari dari database ke dictionary
        public static Dictionary<string, string> FetchFingerprintsFromDatabase(int width, int height)
        {
            Dictionary<string, string> fingerprintsDatabase = new Dictionary<string, string>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT nama, berkas_citra FROM sidik_jari";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    var parser = new PatternMatching.Parser();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nama = reader["nama"].ToString();
                            string berkas_citra = reader["berkas_citra"].ToString();

                            Image image = Image.FromFile(berkas_citra);
                            string ascii = parser.ConvertImageToAscii(image, width, height);

                            Console.WriteLine($"Fingerprint data found: {nama}");
                            fingerprintsDatabase.Add(nama, ascii);
                        }
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"MySQL error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                return fingerprintsDatabase;
            }
        }

        // Insert nama dan ascii ke table sidik_jari
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

        // Generate images dari folder ke database, nama diambil dari nama file
        public static void LoadFingerprints(string folderPath)
        {
            var parser = new PatternMatching.Parser();
            var fileTypes = new string[] { "*.jpg", "*.png", "*.bmp" };

            foreach (var fileType in fileTypes)
            {
                foreach (string filePath in Directory.EnumerateFiles(folderPath, fileType))
                {
                    try
                    {
                        string name = Path.GetFileNameWithoutExtension(filePath);
                        Insert(filePath, name);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred while processing file {filePath}: {ex.Message}");
                    }
                }
            }
        }

        // Hapus semua data di table sidik_jari
        public static void ClearDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection to database established successfully.");

                    // Query to delete all data
                    string query = "DELETE FROM sidik_jari";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    Console.WriteLine("Clearing database...");

                    // Execute the command
                    int rowsAffected = command.ExecuteNonQuery();

                    Console.WriteLine($"{rowsAffected} row(s) deleted.");
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
        
        // Generate biodata random ke database
        public static void generateBiodata(string folderPath) {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection to database established successfully.");

                    // Inisialisasi array nama
                    string[] names = new string[303];

                    // List nama untuk digunakan sebagai basis
                    string[] firstNames = {
                        "James", "Mary", "John", "Patricia", "Robert", "Jennifer", "Michael", "Linda", "William", "Elizabeth",
                        "David", "Barbara", "Richard", "Susan", "Joseph", "Jessica", "Thomas", "Sarah", "Charles", "Karen",
                        "Christopher", "Nancy", "Daniel", "Lisa", "Matthew", "Betty", "Anthony", "Margaret", "Mark", "Sandra",
                        "Donald", "Ashley", "Steven", "Kimberly", "Paul", "Emily", "Andrew", "Donna", "Joshua", "Michelle",
                        "Kenneth", "Dorothy", "Kevin", "Carol", "Brian", "Amanda", "George", "Melissa", "Edward", "Deborah"
                    };

                    string[] lastNames = {
                        "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
                        "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin",
                        "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
                        "Walker", "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores",
                        "Green", "Adams", "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell", "Carter", "Roberts"
                    };

                    // Pseudo-random number generator
                    Random rand = new Random();

                    // Generate 303 unique names
                    for (int i = 0; i < 303; i++)
                    {
                        // Combine first and last names randomly
                        string firstName = firstNames[rand.Next(firstNames.Length)];
                        string lastName = lastNames[rand.Next(lastNames.Length)];
                        string fullName = firstName + " " + lastName;

                        // Ensure the name is unique
                        while (Array.Exists(names, name => name == fullName))
                        {
                            firstName = firstNames[rand.Next(firstNames.Length)];
                            lastName = lastNames[rand.Next(lastNames.Length)];
                            fullName = firstName + " " + lastName;
                        }

                        // Add the unique name to the array
                        names[i] = fullName;
                    }

                    string[] placesOfBirth = { "Jakarta", "Bandung", "Jogja", "Bali", "Surabaya", "Medan"};
                    string[] genders = { "Laki-Laki", "Perempuan" };
                    string[] bloodTypes = { "A", "B", "AB", "O" };
                    string[] addresses = { "Jl. in aja dulu", "Jl. jalan yuk", "Jl. doang jadian kagak", "Jl. i saja hidup ini", "Jl. lagi sama mantan"};
                    string[] religions = { "Islam", "Protestan", "Katolik", "Buddha", "Hindu", "Konghucu" };
                    string[] maritalStatuses = { "Belum Menikah", "Menikah", "Cerai" };
                    string[] jobs = { "Polisi", "Mahasiswa", "Programmer", "Tidak bekerja", "Wirausahawan", "Pegawai Negeri Sipil" };
                    string[] nationalities = { "Indonesia", "Jepang", "Korea", "Amerika" };

                    // Inisialisasi jumlah baris yang terpengaruh
                    int rowsAffected = 0;

                    foreach (var name in names)
                    {
                        string nik = rand.Next(10000000, 99999999).ToString();
                        string placeOfBirth = placesOfBirth[rand.Next(placesOfBirth.Length)];
                        DateTime dateOfBirth = new DateTime(rand.Next(1950, 2003), rand.Next(1, 13), rand.Next(1, 29));
                        string gender = genders[rand.Next(genders.Length)];
                        string bloodType = bloodTypes[rand.Next(bloodTypes.Length)];
                        string address = addresses[rand.Next(addresses.Length)];
                        string religion = religions[rand.Next(religions.Length)];
                        string maritalStatus = maritalStatuses[rand.Next(maritalStatuses.Length)];
                        string job = jobs[rand.Next(jobs.Length)];
                        string nationality = nationalities[rand.Next(nationalities.Length)];

                        // Query to insert biodata
                        string query = "INSERT INTO biodata (NIK, nama, tempat_lahir, tanggal_lahir, jenis_kelamin, golongan_darah, alamat, agama, status_perkawinan, pekerjaan, kewarganegaraan) " +
                                    "VALUES (@NIK, @nama, @tempat_lahir, @tanggal_lahir, @jenis_kelamin, @golongan_darah, @alamat, @agama, @status_perkawinan, @pekerjaan, @kewarganegaraan)";
                        MySqlCommand command = new MySqlCommand(query, connection);

                        // Adding parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@NIK", nik);
                        command.Parameters.AddWithValue("@nama", name);
                        command.Parameters.AddWithValue("@tempat_lahir", placeOfBirth);
                        command.Parameters.AddWithValue("@tanggal_lahir", dateOfBirth);
                        command.Parameters.AddWithValue("@jenis_kelamin", gender);
                        command.Parameters.AddWithValue("@golongan_darah", bloodType);
                        command.Parameters.AddWithValue("@alamat", address);
                        command.Parameters.AddWithValue("@agama", religion);
                        command.Parameters.AddWithValue("@status_perkawinan", maritalStatus);
                        command.Parameters.AddWithValue("@pekerjaan", job);
                        command.Parameters.AddWithValue("@kewarganegaraan", nationality);

                        // Eksekusi perintah
                        rowsAffected += command.ExecuteNonQuery();
                    }

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

        // Rename nama file image dari folder
        public static void renameFingerprint(string folderPath)
        {
            List<string> names = new List<string>
            {
                "Thomas Miller", "Edward Mitchell", "Thomas Davis", "Christopher Walker", "Kimberly King",
                "Edward Roberts", "Donna Clark", "Kimberly Hernandez", "Susan Taylor", "Richard Hernandez",
                "Robert Torres", "Michelle Jones", "Dorothy Flores", "Linda Lopez", "William Lee",
                "Richard King", "Mary Smith", "Margaret Hall", "Carol Perez", "Kevin Smith",
                "Brian Adams", "Jessica Hill", "Mary Nelson", "Karen Wilson", "Steven Baker",
                "Jessica Brown", "Brian White", "Lisa Nelson", "Lisa Adams", "Thomas Nguyen",
                "Donald Johnson", "Paul Jones", "Edward Rodriguez", "Emily Clark", "Karen Green",
                "Thomas Garcia", "Thomas King", "Carol White", "George Hernandez", "Edward Gonzalez",
                "Ashley Hill", "Joseph Young", "Richard Wright", "Lisa Smith", "Kimberly Johnson",
                "Steven Lopez", "Susan Lee", "Karen Allen", "Andrew Torres", "Edward Torres",
                "Deborah Hill", "Anthony Nguyen", "Linda Taylor", "Carol Moore", "Karen Hernandez",
                "William Jackson", "Joseph Clark", "Elizabeth Robinson", "Melissa Green", "Amanda Moore",
                "Michelle Perez", "Kenneth Scott", "Joseph Brown", "Brian Thompson", "Kevin Martinez",
                "Michael Wright", "Emily Walker", "Daniel King", "William Lopez", "Linda Scott",
                "William Johnson", "Jessica Young", "Jennifer Nelson", "Paul Rodriguez", "Lisa Garcia",
                "Emily Jones", "Sandra Hill", "Joshua Martinez", "Susan Rivera", "Patricia Thompson",
                "Mark Hall", "Melissa Baker", "Steven Thomas", "Emily Harris", "Joseph Torres",
                "Donald Davis", "Amanda Sanchez", "Robert Martin", "James Nelson", "Deborah Green",
                "Lisa Jackson", "Andrew King", "Nancy Brown", "George Brown", "Betty Robinson",
                "Emily Rivera", "Richard Williams", "John Adams", "Donna Thomas", "Richard Lewis",
                "Mark Martinez", "Richard Torres", "Kimberly Martin", "Deborah Brown", "Mark Green",
                "Richard Hill", "Amanda Allen", "Michael Sanchez", "Joseph Hill", "Deborah Mitchell",
                "Emily Flores", "Joshua Rivera", "Andrew Thompson", "Christopher Scott", "Charles Anderson",
                "Deborah Lewis", "Daniel Torres", "Joseph Baker", "Jennifer Ramirez", "Charles Brown",
                "John Roberts", "Thomas Lopez", "Thomas Lewis", "Susan Mitchell", "Sandra Jones",
                "David Campbell", "Richard Smith", "Patricia Martinez", "Joseph Anderson", "Kevin Rodriguez",
                "Matthew Lopez", "Joshua Hill", "Joseph Johnson", "Emily Nelson", "Paul Nguyen",
                "William Lewis", "Patricia Davis", "Mary Thompson", "Jennifer Garcia", "Mary Baker",
                "Kenneth Rodriguez", "Betty Nelson", "Kimberly Roberts", "Susan Hall", "James Hernandez",
                "Thomas Walker", "Daniel Jones", "Sarah Clark", "William Perez", "Betty Lewis",
                "Thomas Brown", "Ashley Nguyen", "Andrew Lopez", "Donna Walker", "Ashley Martinez",
                "Amanda Wright", "Kenneth Smith", "Jennifer Harris", "Michael Nelson", "Barbara Lee",
                "Andrew Hall", "Richard Garcia", "Richard Martinez", "Paul Scott", "Richard Martin",
                "Kenneth Lopez", "Steven Johnson", "Joshua Smith", "William Campbell", "Patricia Walker",
                "Karen Rivera", "Linda Walker", "Margaret Jackson", "Christopher Ramirez", "Kenneth Brown",
                "Deborah Sanchez", "Carol Campbell", "Matthew Thompson", "Melissa Robinson", "Barbara Taylor",
                "Brian Gonzalez", "Karen Williams", "Sarah Taylor", "Lisa Robinson", "George Sanchez",
                "Sandra Walker", "Ashley Walker", "Robert Davis", "James Nguyen", "Michelle Clark",
                "Michelle Miller", "William White", "Melissa Campbell", "Edward Nelson", "Karen Hill",
                "Nancy Lewis", "Andrew Thomas", "Joseph Nelson", "Elizabeth Hall", "Kenneth Green",
                "Margaret Wilson", "Anthony Ramirez", "Amanda Jones", "Dorothy Martin", "Dorothy Rivera",
                "Andrew Brown", "Mary Anderson", "John Ramirez", "Deborah Gonzalez", "Edward Baker",
                "Melissa Lee", "Linda White", "Susan Ramirez", "Mark Lopez", "Kevin Hall",
                "Michelle Brown", "Thomas Perez", "Kevin Carter", "Christopher Hill", "Thomas Rodriguez",
                "Linda Harris", "Sandra Torres", "Brian Smith", "Brian Baker", "Amanda Johnson",
                "Mary Green", "Betty Rivera", "Dorothy Walker", "Elizabeth Jackson", "Christopher Nguyen",
                "Steven Walker", "Thomas Flores", "Elizabeth Young", "Nancy Mitchell", "Melissa Thomas",
                "Sarah Johnson", "Melissa Smith", "Daniel Rivera", "Elizabeth Anderson", "Daniel Thomas",
                "Elizabeth Clark", "Margaret Campbell", "Donna Allen", "Richard Davis", "Barbara Carter",
                "Brian Allen", "Thomas Mitchell", "Joshua Brown", "Robert Allen", "Ashley King",
                "Anthony Wilson", "William Torres", "Jennifer Hernandez", "Michael Lee", "Joseph Hall",
                "Jessica Martinez", "Robert Gonzalez", "Barbara Anderson", "Dorothy Jones", "Edward Johnson",
                "Daniel Gonzalez", "Deborah Thompson", "Robert Hall", "Jennifer Hall", "Sandra Harris",
                "Daniel Perez", "Joseph Thomas", "Barbara Brown", "Jessica Robinson", "Deborah Jackson",
                "Matthew Hall", "Kenneth Mitchell", "Kimberly Wright", "Ashley Lewis", "Linda Baker",
                "Daniel Flores", "Susan Lewis", "Kimberly Williams", "Donald Lopez", "Nancy Gonzalez",
                "Sarah Carter", "Kimberly Allen", "David Martinez", "Carol Nguyen", "Mary Allen",
                "Michelle Flores", "Daniel Walker", "Carol Jackson", "Ashley Hernandez", "Jennifer Thompson",
                "Melissa Lopez", "Donna Perez", "Donald Roberts", "Donna Moore", "Donna Lopez",
                "David Young", "Michael Jackson", "James Lewis", "Anthony Thomas", "Daniel Allen",
                "William Clark", "Thomas Clark", "Linda Miller"
            };
    
            string[] files = Directory.GetFiles(folderPath);

            if (files.Length > names.Count)
            {
                Console.WriteLine("More files than names provided.");
                return;
            }

            for (int i = 0; i < files.Length; i++)
            {
                string filePath = files[i];
                string extension = Path.GetExtension(filePath);
                string newFileName = Path.Combine(folderPath, names[i] + extension);

                try
                {
                    File.Move(filePath, newFileName);
                    Console.WriteLine($"Renamed {filePath} to {newFileName}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error renaming file {filePath}: {ex.Message}");
                }
            }
        }
    }
}