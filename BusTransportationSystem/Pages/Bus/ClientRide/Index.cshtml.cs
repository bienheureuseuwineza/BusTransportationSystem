using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus.ClientRide
{
    public class IndexModel : PageModel
    {
		/*string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";*/
		string connString = "Data Source=DESKTOP-SED41CT\\SQLEXPRESS01;Initial Catalog=BusSystem;Integrated Security=True";

		public List<Client> VehicleList = new List<Client>();

        public void OnGet()
        {
            // Display the list of Vehicles
            VehicleList.Clear();

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    // Retrieve Vehicles
                    string vehicleQuery = "SELECT * FROM [Client Rides]";
                    con.Open();
                    using (SqlCommand vehicleCmd = new SqlCommand(vehicleQuery, con))
                    {
                        using (SqlDataReader vehicleReader = vehicleCmd.ExecuteReader())
                        {
                            while (vehicleReader.Read())
                            {
                                Client vehicle = new Client
                                {
                                    id = vehicleReader.GetInt32(0),
                                    trip_id = vehicleReader.GetInt32(1),
                                    user_id = vehicleReader.GetInt32(2),
                                    payment_status = vehicleReader.GetString(3),
                                    checkin = vehicleReader.GetString(4),
                                    tripDate = vehicleReader.GetDateTime(5)
                                 
                                };
                                VehicleList.Add(vehicle);
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

    public class Client
    {
        public int id { get; set; }
        public int trip_id { get; set; }
      
        public int user_id { get; set; }
        public string? payment_status { get; set; }
        public string? checkin { get; set; }
        public DateTime tripDate { get; set; }
       
    }
}

