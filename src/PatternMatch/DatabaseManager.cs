using System;
using System.Data;
using System.Drawing;
using System.IO;
using MySql.Data.MySqlClient;
using PatternMatching;

namespace PatternMatch
{
    public class DatabaseManager
    {
        private static string connectionString = "Server=localhost; Port={port}; Database=tubes3; User ID=root; Password={password};";

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
                    Console.WriteLine($"An error occurred FetchFingerprintsFromDatabase: {ex.Message}");
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
                    Console.WriteLine($"An error occurred Insert: {ex.Message}");
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
                    string query = "DELETE FROM biodata";
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
                    Console.WriteLine($"An error occurred ClearDatabase: {ex.Message}");
                }
            }
        }

        // Generate biodata random ke database
        public static void generateBiodata()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection to database established successfully.");

                    // Inisialisasi array nama
                    List<string> names = new List<string>
                    {
                        "th0m5 M1llR", "3dw4Rd mtchll", "ThM5 dv5", "cHrIstpHr wLkr", "k1MbRly KNg",
                        "dwrd r0BRt5", "dnN cl4Rk", "k1mbrly hrnndZ", "5sn tylr", "rchard herN4nd2",
                        "rbRt trrS", "michll Jn5", "drthy Fl0re5", "LnD4 LP2", "w1Ll1m L",
                        "rcHRd Kn6", "mRy 5mtH", "mR6Rt hlL", "c4rl pr3z", "kVn 5mth",
                        "br4n 4dm5", "j55C4 hll", "Mry nlsn", "kaRn wl5n", "stv3n b4kR",
                        "j55c Br0wn", "brN wHt3", "lI5 nl50n", "l54 4dm5", "tH0m45 n6Yn",
                        "dnld j0hn50n", "pl jN5", "dwrd rdrG2", "mLY cL4rk", "krn 6r3n",
                        "thM5 64rc", "Thm5 kn6", "crl Wh1t", "6R6 h3rnnDe2", "dwarD 6n2l2",
                        "a5hley hll", "jsph yon6", "Rchrd WR16Ht", "L1S 5mTh", "k1mbrly Jhns0N",
                        "5tV3n lP2", "55n l", "krn alln", "Ndr3w t0rr5", "Dw4rd trR5",
                        "dBR4h hll", "nthNY n6UyN", "lnd4 t4Ylr", "cR0L mr3", "K4r3n Hrnnd2",
                        "WLlAm JcK5n", "jspH cLrk", "l2bth rbnsn", "mls5 6r3n", "mND Mr3",
                        "mch3lL PR2", "KNnTH 5ctT", "j53ph Brwn", "brn thmp5n", "keVn MrtN2",
                        "Mchl wr6hT", "mLY wlkr", "Dnl kN6", "willm l0p2", "lind sctT",
                        "wlLm jHn5n", "J55c y0uN6", "jnnfR n3l50N", "P4Ul rDRIG2", "L15 grc",
                        "mly J0N5", "5ndr hll", "jSha m4rTn2", "Ssn r1ver", "p4trc14 thmp5N",
                        "m4rk h4Ll", "ml155A Bkr", "5Tvn tHm45", "mly hrR5", "j5PH tRRs",
                        "dnld dVs", "amnD 5NcH3z", "rbrt mRTn", "jMeS n3L50n", "dbrh 6R3n",
                        "l15 JAcK50n", "ndrw kN6", "n4nCy brwN", "63r6 bRwn", "bTty rbn5n",
                        "mly rVr4", "R1chrd wll4MS", "jhn 4dM5", "dnn thmS", "rchRd LWs",
                        "mRk maRTn2", "rcH4rd trR5", "KmBrLy mrTN", "dbrh br0wn", "Mrk 6R3n",
                        "rchrD hll", "mnd 4llN", "McHl 5nch2", "j5ph Hll", "dBrh Mtch3Ll",
                        "3m1lY flrs", "j05h r1vr", "4ndRw thmp50n", "ChR5tphr sc0tt", "cHrl5 4ndr5n",
                        "dBrh lW5", "dan3l trr5", "j5ph bkER", "j3NNfR r4m1r2", "cHrL3S brwn",
                        "jHn rbRt5", "tHm5 Lpe2", "thm5 LwS", "S54n Mtchll", "5ndr4 jN5",
                        "dVD c4mPblL", "rChRd 5mth", "ptRc m4rtn2", "j5ph 4NDR5n", "Kvn rdRi6E2",
                        "mtthw lP2", "j5hu H1LL", "j5ph jhn5n", "Mly nEls0N", "pl ngy3n",
                        "wllAm lew5", "ptRc4 dAv5", "MRy ThmpsN", "JnnFr 6rc14", "mry b4kr",
                        "knnth rdrg3z", "beTtY nl5n", "Kmb3Rly rbRT5", "55n hLl", "JM5 h3Rnnd2",
                        "thM4S W4Lk3R", "dnl jn5", "5rH CL4Rk", "w1llM pr2", "bttY Lw5",
                        "thom5 brwn", "5hlY n6yN", "ndrw lp32", "dnn wLkr", "a5hLy Mrtnz",
                        "MaNdA wRgHt", "k3Nnth 5mtH", "j3nnfr HRr5", "mchl nl5n", "BArBr le",
                        "ndr3w Hll", "rChrd grc14", "RcHrd M4rtn2", "pl 5cTt", "Rchard mrt1n",
                        "kNnth lp2", "5tvN j0hn50N", "J5hU 5Mth", "w1lLm cmpbLl", "Ptrc wlkR",
                        "krn rvr", "LNd wlkr", "mr6Rt jcK5n", "Chr5tpH3r RmR32", "kEnntH BrWn",
                        "Dbr4h 5Nch2", "cRoL cMPb3lL", "MtThW th0mp5n", "mlss rBn5n", "b4Rbr tylR",
                        "BRn gnzL2", "krn wllm5", "5rh Tylr", "l5 r0BnSn", "6r63 SNcH2",
                        "sndr WlKr", "5Hly WLk3R", "rbrt dv1s", "j4m35 n6yn", "Mch3llE Cl4rk",
                        "m1Chll mlL3r", "w1ll4m wht", "ml55 CmpBll", "dwrd nel50N", "krn hll",
                        "NNCy lW15", "ndrw tH0M5", "J5ph NL5n", "l1zbth Hll", "KnnTh 6rn",
                        "mr6Rt w1l5n", "nTHny r4m1R2", "MNd jn35", "dRthy M4rtn", "dR0Thy rv3r",
                        "NDrw brwn", "MRy 4ndr5n", "jhN R4mr32", "DBr4h 6N2L2", "dwrd bKr",
                        "mLs5 L", "lnd whT", "55N rMr2", "mRK LoP2", "kvN hll",
                        "Mch3Ll Brwn", "tHmS pr2", "kvn Crt3r", "CHrstphr Hll", "thm45 r0dr6E2",
                        "l1Nd hrrS", "5ndr tORR3s", "BrN 5mtH", "BRn bakr", "m4Nda jhn50n",
                        "maRy 6Rn", "b3tTY R1V3r", "drThy wlk3R", "L2abth j4ck5on", "chR5T0phr n6yn",
                        "st3Vn wlkr", "thm4s FloR5", "3l2bth yN6", "nNcy m1TCh3Ll", "m3li5s thm5",
                        "5rh j0hN5n", "m3lIS54 5mTh", "dnl rVr4", "l2bTH 4ndrsn", "Dnl thm5",
                        "L24bTh clrK", "mr6Rt cmPBlL", "dnn 4lLn", "RcHRD D4v5", "BrBR crtr",
                        "brN 4lLN", "thOm5 mtcheLl", "JSh brWN", "rbrt aLLen", "shL3Y kN6",
                        "aNthNy W1l5n", "Will14m Trr3s", "jnnfr heRnnde2", "mchl L", "jsPh hLl",
                        "js5ca Mrtn32", "rbrt gnzl3z", "Brbra 4ndR5n", "drThy Jn5", "dwArd JHn5N",
                        "d4n3l gN2L32", "d3brH thmpSn", "rB3Rt hll", "Jnnf3r hll", "sndr hrr5",
                        "dnL pr2", "j5Ph thm45", "brbr br0wn", "jSsc rbn50N", "D3b0rh Jcks0n",
                        "mtThw Hll", "Knnth mtchll", "kmb3rlY wrigHT", "5hlY l3w5", "LiND BKr",
                        "d4N3l flrS", "5sn lW5", "kmbrly wllm5", "dnld lopz", "n4ncY 60n2L32",
                        "5rH crTR", "k1mb3rLy 4lln", "Dvd Mrtn32", "CRl n6uyn", "mry 4lln",
                        "mcHLl flR5", "dn1l wlKR", "crl jCk5n", "45hlY Hrnnd2", "jnnfr th0mP5n",
                        "ML55 l0p2", "dnn4 pRz", "dnld rbrTs", "D0nn m0R", "dnn LP2",
                        "davd yN6", "mChl jckSn", "jM5 lW15", "nthnY th0m4S", "dnl 4lLn",
                        "w1ll1m clRk", "th0m5 ClrK", "lnd mllr"
                    };

                    // Pseudo-random number generator
                    Random rand = new Random();

                    string[] placesOfBirth = { "Jakarta", "Bandung", "Jogja", "Bali", "Surabaya", "Medan" };
                    string[] genders = { "Laki-Laki", "Perempuan" };
                    string[] bloodTypes = { "A", "B", "AB", "O" };
                    string[] addresses = { "Jl. in aja dulu", "Jl. jalan yuk", "Jl. doang jadian kagak", "Jl. i saja hidup ini", "Jl. lagi sama mantan" };
                    string[] religions = { "Islam", "Protestan", "Katolik", "Buddha", "Hindu", "Konghucu" };
                    string[] maritalStatuses = { "Belum Menikah", "Menikah", "Cerai" };
                    string[] jobs = { "Polisi", "Mahasiswa", "Programmer", "Tidak bekerja", "Wirausahawan", "Pegawai Negeri Sipil" };
                    string[] nationalities = { "Indonesia", "Jepang", "Korea", "Amerika" };

                    // Inisialisasi jumlah baris yang terpengaruh
                    int rowsAffected = 0;

                    foreach (var name in names)
                    {
                        string nik = rand.Next(00000000, 99999999).ToString();
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
                    Console.WriteLine($"An error occurred generateBiodata: {ex.Message}");
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

        public static void inputData(string sourcePath)
        {
            Console.WriteLine("Data anda belum ada, silahkan masukan biodata anda");

            Console.Write("Masukkan NIK: ");
            string NIK = Console.ReadLine();

            Console.Write("Masukkan Nama: ");
            string nama = Console.ReadLine();

            Console.Write("Masukkan Tempat Lahir: ");
            string tempatLahir = Console.ReadLine();

            Console.Write("Masukkan Tanggal Lahir (YYYY-MM-DD): ");
            string tanggalLahir = Console.ReadLine();

            Console.Write("Masukkan Jenis Kelamin (Laki-Laki/Perempuan): ");
            string jenisKelamin = Console.ReadLine();

            Console.Write("Masukkan Golongan Darah: ");
            string golonganDarah = Console.ReadLine();

            Console.Write("Masukkan Alamat: ");
            string alamat = Console.ReadLine();

            Console.Write("Masukkan Agama: ");
            string agama = Console.ReadLine();

            Console.Write("Masukkan Status Perkawinan (Belum Menikah/Menikah/Cerai): ");
            string statusPerkawinan = Console.ReadLine();

            Console.Write("Masukkan Pekerjaan: ");
            string pekerjaan = Console.ReadLine();

            Console.Write("Masukkan Kewarganegaraan: ");
            string kewarganegaraan = Console.ReadLine();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO biodata (NIK, nama, tempat_lahir, tanggal_lahir, jenis_kelamin, golongan_darah, alamat, agama, status_perkawinan, pekerjaan, kewarganegaraan) " +
                                "VALUES (@NIK, @nama, @tempat_lahir, @tanggal_lahir, @jenis_kelamin, @golongan_darah, @alamat, @agama, @status_perkawinan, @pekerjaan, @kewarganegaraan)";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@NIK", NIK);
                        cmd.Parameters.AddWithValue("@nama", nama);
                        cmd.Parameters.AddWithValue("@tempat_lahir", tempatLahir);
                        cmd.Parameters.AddWithValue("@tanggal_lahir", tanggalLahir);
                        cmd.Parameters.AddWithValue("@jenis_kelamin", jenisKelamin);
                        cmd.Parameters.AddWithValue("@golongan_darah", golonganDarah);
                        cmd.Parameters.AddWithValue("@alamat", alamat);
                        cmd.Parameters.AddWithValue("@agama", agama);
                        cmd.Parameters.AddWithValue("@status_perkawinan", statusPerkawinan);
                        cmd.Parameters.AddWithValue("@pekerjaan", pekerjaan);
                        cmd.Parameters.AddWithValue("@kewarganegaraan", kewarganegaraan);

                        cmd.ExecuteNonQuery();
                        string destinationPath = ("database/" + Path.GetFileName(sourcePath));
                        CopyImage(sourcePath, destinationPath);
                        Console.WriteLine("Data berhasil dimasukkan ke folder database.");
                        Insert(destinationPath, nama);
                        Console.WriteLine("Data berhasil dimasukkan ke SQL database.");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fetch Error: " + ex.ToString());
                }
            }
        }

        public static void CopyImage(string sourcePath, string destinationPath)
        {
            try
            {
                // Cek apakah file sumber ada
                if (File.Exists(sourcePath))
                {
                    // Cek apakah file tujuan sudah ada
                    if (File.Exists(destinationPath))
                    {
                        Console.WriteLine("File sudah ada di folder tujuan.");
                    }
                    else
                    {
                        // Menyalin file
                        File.Copy(sourcePath, destinationPath);
                        Console.WriteLine("File berhasil disalin.");
                    }
                }
                else
                {
                    Console.WriteLine("File sumber tidak ditemukan.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Terjadi kesalahan: " + ex.Message);
            }
        }

        public string GetOutputImage(List<Tuple<DataTable, double>> results)
        {
            if (results == null || results.Count == 0)
            {
                return "GetOutputImage error";
            }

            DataTable firstTable = results[0].Item1;
            if (firstTable.Rows.Count == 0)
            {
                return "GetOutputImage error";
            }

            string nama = firstTable.Rows[0]["nama"].ToString();
            return GetImage(nama);
        }

        public static string GetImage(string nama)
        {
            string imageFilePath = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT berkas_citra FROM sidik_jari WHERE nama = @nama";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@nama", nama);

                try
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        imageFilePath = reader["berkas_citra"].ToString();
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return imageFilePath;
        }

        public static DataTable showBiodata(string nama)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM biodata WHERE nama REGEXP @nama";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@nama", nama);

                DataTable biodataTable = new DataTable();
                try
                {
                    connection.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    biodataTable.Load(reader);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                return biodataTable;
            }
        }
    }
}
