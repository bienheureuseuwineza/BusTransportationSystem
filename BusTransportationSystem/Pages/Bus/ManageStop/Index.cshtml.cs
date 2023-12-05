using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus.ManageStops
{
    public class IndexModel : PageModel
    {

        string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";

        string connString = "Data Source=DESKTOP-SED41CT\\SQLEXPRESS01;Initial Catalog=BusSystem;Integrated Security=True";

        public List<Stop> StopList = new List<Stop>();

        public void OnGet()
        {
            // Display the list of Stops
            StopList.Clear();

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    // Retrieve Stops
                    string stopQuery = "SELECT * FROM Stops";
                    con.Open();
                    using (SqlCommand stopCmd = new SqlCommand(stopQuery, con))
                    {
                        using (SqlDataReader stopReader = stopCmd.ExecuteReader())
                        {
                            while (stopReader.Read())
                            {
                                Stop stop = new Stop
                                {
                                    Id = stopReader.GetInt32(0),
                                    StopsDestination = stopReader.GetString(1),
                                    TripId = stopReader.GetInt32(2)
                                };
                                StopList.Add(stop);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error" + ex.Message);
            }
        }
    }

    public class Stop
    {
        public int Id { get; set; }
        public string StopsDestination { get; set; }
        public int TripId { get; set; }
    }
}