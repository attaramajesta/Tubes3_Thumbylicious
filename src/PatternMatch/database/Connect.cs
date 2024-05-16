// using System;
// using MySql.Data.MySqlClient; 
// //belom work tapi lanjut nanti ya

// namespace PatternMatcher
// {
//     public class Program
//     {
//         public static void Main(string[] args)
//         {
//             var dbCon = DBConnection.Instance();
//             dbCon.Server = "localhost";
//             dbCon.DatabaseName = "database.sql";
//             dbCon.UserName = "root";
//             dbCon.Password = "bbee2e7";
//             if (dbCon.IsConnect())
//             {
//                 string query = "SELECT * FROM biodata";
//                 var cmd = new MySqlCommand(query, dbCon.Connection);
//                 var reader = cmd.ExecuteReader();
//                 while (reader.Read())
//                 {
//                     string someStringFromColumnZero = reader.GetString(0);
//                     string someStringFromColumnOne = reader.GetString(1);
//                     Console.WriteLine(someStringFromColumnZero + "," + someStringFromColumnOne);
//                 }
//                 dbCon.Close();
//             }
//         }
//     }

//     public class DBConnection
//     {
//         private DBConnection()
//         {
//         }

//         private static DBConnection _instance = null;

//         public static DBConnection Instance()
//         {
//             if (_instance == null)
//                 _instance = new DBConnection();
//             return _instance;
//         }

//         public MySqlConnection Connection { get; set; }

//         public string Server { get; set; } = ""; // Initialize with empty string

//         public string DatabaseName { get; set; } = ""; // Initialize with empty string

//         public string UserName { get; set; } = ""; // Initialize with empty string

//         public string Password { get; set; } = ""; // Initialize with empty string

//         public bool IsConnect()
//         {
//             if (Connection == null)
//             {
//                 if (String.IsNullOrEmpty(DatabaseName))
//                     return false;
//                 string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
//                 Connection = new MySqlConnection(connstring);
//                 Connection.Open();
//             }

//             return true;
//         }

//         public void Close()
//         {
//             Connection.Close();
//         }
//     }
// }
