using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus.ManageTrip
{
    public class IndexModel : PageModel
    {

        //string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";

        //string connString = "Data Source=DESKTOP-SED41CT\\SQLEXPRESS01;Initial Catalog=BusSystem;Integrated Security=True";
        string connString = "Data Source=LAPTOP-E65QRG1A\\SQLEXPRESS;Initial Catalog=BusSystem; Integrated Security=True";


        public List<Trip> TripList = new List<Trip>();

		

		public void OnGet()
        {
            // Display the list of Trips
            TripList.Clear();
			

			//try
            //{
                using (SqlConnection con = new SqlConnection(connString))
                {
                    // Retrieve Trips
                    string tripQuery = "SELECT * FROM Trip";
                    con.Open();
                    using (SqlCommand tripCmd = new SqlCommand(tripQuery, con))
                    {
                    using (SqlDataReader tripReader = tripCmd.ExecuteReader())
                    {
                        while (tripReader.Read())
                        {
                            Trip trip = new Trip
                            {
                                TripId = tripReader.GetInt32(0),
                                DriverId = tripReader.GetInt32(1),
                                VehicleId = tripReader.GetInt32(2),
                                InitDestination = tripReader.GetString(3),
                                FinalDestination = tripReader.GetString(4),
                                Price = tripReader.GetDouble(5),
                                TripDate = tripReader.GetDateTime(6)
                            };
                            TripList.Add(trip);

                        }
                    }
                    }
                }
            //}
/*            catch (Exception ex)
            {
                Console.WriteLine("error" + ex.Message);
            }*/
        }
    }

    public class Trip
    {
        public int TripId { get; set; }
        public int DriverId { get; set; }
        public int VehicleId { get; set; }
		public string VehicleName { get; set; }
		public string InitDestination { get; set; }
        public string FinalDestination { get; set; }
        public double Price { get; set; }
        public DateTime TripDate { get; set; }
    }
	
}