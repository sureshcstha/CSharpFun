
using CSharpFun.Data;
using CSharpFun.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Data;
using System.Text.Json;

namespace CSharpFun
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // string connectionString = "Server=(localdb)\\local;Database=DotNetCourseDatabase;TrustServerCertificate=true;Trusted_Connection=true;";

            // IDbConnection dbConnection = new SqlConnection(connectionString);

            DataContextDapper dapper = new DataContextDapper(config);
            DataContextEF entityFramework = new DataContextEF(config);


            string sqlCommand = "SELECT GETDATE()";

            DateTime rightNow = dapper.LoadDataSingle<DateTime>(sqlCommand);

            Console.WriteLine(rightNow);

            Computer myComputer = new Computer()
            {
                Motherboard = "Z690",
                HasWifi = true,
                HasLTE = false,
                ReleaseDate = DateTime.Now,
                Price = 943.87m,
                VideoCard = "RTX 2060"
            };



/*            string sql = @"INSERT INTO TutorialAppSchema.Computer (
                Motherboard,
                HasWifi,
                HasLTE,
                ReleaseDate, 
                Price, 
                VideoCard
            ) VALUES ('" + myComputer.Motherboard 
                    + "','" + myComputer.HasWifi
                    + "','" + myComputer.HasLTE
                    + "','" + myComputer.ReleaseDate
                    + "','" + myComputer.Price
                    + "','" + myComputer.VideoCard
            + "')";*/

            //File.WriteAllText("log.txt", "\n" + sql + "\n");

            //using StreamWriter openFile = new("log.txt", append: true);

            //openFile.WriteLine("\n" + sql + "\n");

            //openFile.Close();

            string computerJson = File.ReadAllText("Computers.json");

            //Console.WriteLine(computerJson);


            /* Deserialize and Serialize*/

            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };


            IEnumerable<Computer>? computersNewtonsoft = JsonConvert.DeserializeObject<IEnumerable<Computer>>(computerJson); // Newtonsoft.Json.JsonConvert

            IEnumerable<Computer>? computersSystem = System.Text.Json.JsonSerializer.Deserialize<IEnumerable<Computer>>(computerJson); // System.Text.Json.JsonSerializer



            if (computersNewtonsoft != null ) 
            {
                foreach(Computer computer in computersNewtonsoft) 
                {
                    //Console.WriteLine(computer.Motherboard);
                    string sql = @"INSERT INTO TutorialAppSchema.Computer (
                        Motherboard,
                        HasWifi,
                        HasLTE,
                        ReleaseDate, 
                        Price, 
                        VideoCard
                    ) VALUES ('" + EscapeSingleQuote(computer.Motherboard)
                            + "','" + computer.HasWifi
                            + "','" + computer.HasLTE
                            + "','" + computer.ReleaseDate
                            + "','" + computer.Price
                            + "','" + EscapeSingleQuote(computer.VideoCard)
                    + "')";

                    dapper.ExecuteSql(sql); 
                }
            }


/*            if (computersSystem != null)
            {
                foreach (Computer computer in computersSystem)
                {
                    Console.WriteLine(computer.Motherboard);
                }
            }*/

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            string computersCopyNewtonsoft = JsonConvert.SerializeObject(computersNewtonsoft, settings);
            File.WriteAllText("computersCopyNewtonsoft.txt", computersCopyNewtonsoft);

            string computersCopySystem = System.Text.Json.JsonSerializer.Serialize(computersSystem);
            File.WriteAllText("computersCopySystem.txt", computersCopySystem);

        }

        // can't have single ' ; we need to replace with ''
        static string EscapeSingleQuote(string input)
        {
            string output = input.Replace("'", "''");
            return output;
        }
    }
}