using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Npgsql;

namespace DateBaseTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1.Вывести таблицу");
            Console.WriteLine("2.Внести данные в таблицу");
            int choise = Int32.Parse(Console.ReadLine());
            
            Sql sql = new Sql();

            switch (choise)
            {
                case 1:
                    Console.Clear();
                    var display = sql.Display();
                    break;
                case 2:
                    Console.Clear();
                    var insert = sql.Insert();
                    break;
                default:
                    Console.WriteLine("Введено некорректное значение");
                    break;
            }
        }
    }

    public class Sql
    {
        public async Task Display()
        {
            string connection = "Host=localhost;Username=postgres;Password=220502446688;Database=restaraunt";
            NpgsqlConnection nc = new NpgsqlConnection(connection);
            try
            {
                nc.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            await using var cmd = new NpgsqlCommand("SELECT * FROM database", nc);
            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                Console.Write(reader.GetString(0) + "\t \t");
                Console.Write(reader.GetString(1) + "\t \t");
                Console.Write(reader.GetString(2) + "\t \t");
                Console.Write(reader.GetDate(ordinal: 3) + "\t \t");
                Console.WriteLine(reader.GetTimeSpan(ordinal: 4) + "\t \t");
            }
        }


        public async Task Insert() { 
            
            string connection = "Host=localhost;Username=postgres;Password=220502446688;Database=restaraunt";
            NpgsqlConnection nc = new NpgsqlConnection(connection);
            try
            {
                nc.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Введите имя");
            string fn = Console.ReadLine();
            Console.WriteLine("Введите фамилию");
            string ln = Console.ReadLine();
            Console.WriteLine("Введите номер телефона в формате 7...");
            string pn = Console.ReadLine();
            Console.WriteLine("Введите  дату прибытия в формате MM:DD:YYYY");
            DateTime ad = DateTime.Parse(Console.ReadLine());
            Console.WriteLine("Введите время прибытия");
            TimeSpan at = TimeSpan.Parse(Console.ReadLine());

            using (var cmd = new NpgsqlCommand("INSERT INTO table (firstName, lastName, phoneNumber, arrivalDate, arrivalTime) VALUES (@p1, @p2, @p3, @p4, @p5)", nc))
            {
                cmd.Parameters.AddWithValue("p1", fn);
                cmd.Parameters.AddWithValue("p2", ln);
                cmd.Parameters.AddWithValue("p3", pn);
                cmd.Parameters.AddWithValue("p4", ad);
                cmd.Parameters.AddWithValue("p5", at);
                int ncmd = cmd.ExecuteNonQuery();
                Console.WriteLine($"Было изменено {ncmd} элементов");
            }
            //await using var newCmd = new NpgsqlCommand("INSERT INTO datebase VALUES('Ilya', 'Voxsie', '79625575555','2021-10-03', '16:30:00')", nc);
            //newCmd.ExecuteNonQuery(); 
        }
    }
}