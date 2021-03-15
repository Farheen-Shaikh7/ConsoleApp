using System;
using System.Data.SqlClient;
using Dapper;

namespace ExecuteScalarEx
{
    class Beer
    {
        public int BeerId { get; set; }
        public string Name { get; set; }
        public string Style { get; set; }
        public string Abv { get; set; }
        public string Volume { get; set; }

        public override string ToString()
        {
            return $"{BeerId}  {Name} {Style} {Abv} {Volume}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var cs = @"Server = localhost\MSSQLSERVER02; Database = master; Trusted_Connection = True;";
            using var con = new SqlConnection(cs);
            con.Open();
 
            Console.WriteLine("Insert Data:\n");

            Console.WriteLine("Enter BeerID:");
            var beerid = Convert.ToDecimal(Console.ReadLine());
            Console.Clear();

            Console.WriteLine("Enter Beer Name:");
            var beername = Console.ReadLine();
            Console.Clear();

            Console.WriteLine("Enter Beer Style:");
            var beerstyle =Console.ReadLine();
            Console.Clear();

            Console.WriteLine("Enter Beer ABV:");
            var beerabv = Convert.ToDecimal(Console.ReadLine());
            Console.Clear();

            Console.WriteLine("Enter Beer Volume:");
            var beervolume = Convert.ToDecimal(Console.ReadLine());
            Console.Clear();

            var query = "INSERT INTO Beers(BeerId, Name, Style, Abv, Volume) VALUES(@beerid, @beername ,@beerstyle, @beerabv,@beervolume)";
            var dp = new DynamicParameters();
            dp.Add("@beerid", beerid);
            dp.Add("@beername", beername);
            dp.Add("@beerstyle", beerstyle);
            dp.Add("@beerabv", beerabv);
            dp.Add("@beervolume", beervolume);
            int res = con.Execute(query, dp);
            if (res > 0)
            {
                Console.WriteLine("row inserted");
            }

            Console.WriteLine("View Data:\n");

            Console.WriteLine("Enter min Abv:");
            var abv_min = Convert.ToDecimal(Console.ReadLine());
            Console.Clear();

            Console.WriteLine("Enter Max Abv:");
            var abv_max = Convert.ToDecimal(Console.ReadLine());
            Console.Clear();

            var beers = con.Query<Beer>(@"SELECT * FROM Beers where abv>=@min and abv<=@max;",new {max=abv_max,min=abv_min}).AsList();
            beers.ForEach(Beer => Console.WriteLine(Beer));

            Console.WriteLine("Delete Data:\n");

            Console.WriteLine("Enter BeerID:");
            var beer_id = Convert.ToDecimal(Console.ReadLine());
            Console.Clear();

            int delRows = con.Execute(@"DELETE FROM Beers WHERE BeerID = @Id", new { Id = beer_id });
            if (delRows > 0)
            {
                Console.WriteLine("Beer deleted");
            }

        }
    }
}