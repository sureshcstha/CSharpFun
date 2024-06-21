
using CSharpFun.Data;
using CSharpFun.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CSharpFun
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // string connectionString = "Server=(localdb)\\local;Database=DotNetCourseDatabase;TrustServerCertificate=true;Trusted_Connection=true;";

            // IDbConnection dbConnection = new SqlConnection(connectionString);

            DataContextDapper dapper = new DataContextDapper();
            DataContextEF entityFramework = new DataContextEF();


            string sqlCommand = "SELECT GETDATE()";

            DateTime rightNow = dapper.LoadDataSingle<DateTime>(sqlCommand);

            Console.WriteLine(rightNow);

            Computer myComputer = new Computer()
            {
                Motherboard = "X690",
                HasWifi = true,
                HasLTE = false,
                ReleaseDate = DateTime.Now,
                Price = 943.87m,
                VideoCard = "RTX 2060"
            };

            entityFramework.Add(myComputer);
            entityFramework.SaveChanges();

            string sql = @"INSERT INTO TutorialAppSchema.Computer (
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
            + "')";

            // Console.WriteLine(sql);

            // int result = dapper.ExecuteSqlWithRowCount(sql);
            bool result = dapper.ExecuteSql(sql);

            // Console.WriteLine(result);


            string sqlSelect = @"
            SELECT 
                Computer.ComputerId, 
                Computer.Motherboard, 
                Computer.HasWifi, 
                Computer.HasLTE, 
                Computer.ReleaseDate, 
                Computer.Price, 
                Computer.VideoCard
            FROM TutorialAppSchema.Computer";

            IEnumerable<Computer> computers = dapper.LoadData<Computer>(sqlSelect);


            Console.WriteLine("Dapper");
            foreach (Computer computer in computers) 
            {
                Console.WriteLine("'" + computer.ComputerId
                    + "','" + computer.Motherboard
                    + "','" + computer.HasWifi
                    + "','" + computer.HasLTE
                    + "','" + computer.ReleaseDate
                    + "','" + computer.Price
                    + "','" + computer.VideoCard
                    + "'");
            }


            IEnumerable<Computer>? computersEf = entityFramework.Computer?.ToList<Computer>();

            Console.WriteLine("Entity Framework");
            if (computersEf != null)
            {

                foreach (Computer computer in computersEf)
                {
                    Console.WriteLine("'" + computer.ComputerId
                        + "','" + computer.Motherboard
                        + "','" + computer.HasWifi
                        + "','" + computer.HasLTE
                        + "','" + computer.ReleaseDate
                        + "','" + computer.Price
                        + "','" + computer.VideoCard
                        + "'");
                }
            }
        }
    }
}