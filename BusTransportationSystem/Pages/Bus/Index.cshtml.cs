using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus
{
    public class IndexModel : PageModel
    {
        string connString = "Data Source=JOSEPHUS-ML;Initial Catalog=BusTransportationDB;Integrated Security=True;Encrypt=False";

        public List<Bus> BusList = new List<Bus>();

        public void OnGet()
        {
            // Display the list of buses
            BusList.Clear();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string qry = "SELECT * FROM Bus";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Bus bus = new Bus
                                {
                                    BusId = reader.GetInt32(0),
                                    BusName = reader.GetString(1),
                                    DriverName = reader.GetString(2),
                                    Types = reader.GetString(3),
                                    NumberOfSeats = reader.GetInt32(4)
                                    // Add other properties as needed
                                };
                                BusList.Add(bus);
                            }
                        }
                    }
                }

            }
            catch (Exception ex) {

                Console.WriteLine("error"+ex.Message);
            }


            
        }
    }
    public class Bus
    {
        public int BusId { get; set; }
        public string BusName { get; set; }
        public string DriverName { get; set; }
        public string Types { get; set; }
        public int NumberOfSeats { get; set; }
        // Add other properties as needed
    }
}
