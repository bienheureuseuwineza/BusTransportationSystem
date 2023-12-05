using BusTransportationSystem.Pages.Bus.ManageTrip;
using BusTransportationSystem.Pages.Bus.ManageVehicle;
using BusTransportationSystem.Pages.Bus.ManageDriver;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace BusTransportationSystem.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

		string connString = "Data Source=JOSEPHUS-ML;Initial Catalog=BusSystem;Integrated Security=True;Encrypt=False";

		public List<Vehicle> VehicleList = new List<Vehicle>();

		public List<Trip> UserTripList = new List<Trip>();
		public List<string> InitDestinations = new List<string>();
		public List<string> FinalDestinations = new List<string>();

		public List<Driver> DriverList = new List<Driver>();

		private string GetVehicleName(int vehicleId)
		{
			using (SqlConnection con = new SqlConnection(connString))
			{
				con.Open();
				string vehicleQuery = "SELECT Vehicle_name FROM Vehicle WHERE Vehicle_id= @VehicleId";
				using (SqlCommand vehicleCmd = new SqlCommand(vehicleQuery, con))
				{
					vehicleCmd.Parameters.AddWithValue("@VehicleId", vehicleId);
					return vehicleCmd.ExecuteScalar()?.ToString();
				}
			}
		}


		[BindProperty(Name = "initDestination")]
		public string InitDestination { get; set; }


		[BindProperty(Name = "finalDestination")]
		public string FinalDestination { get; set; }


		[HttpGet]
		public void OnGet()
		{
			

			// Display the list of Trips
			UserTripList.Clear();

			InitDestinations.Clear();
			FinalDestinations.Clear();

			try
			{
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
									TripDate = tripReader.GetDateTime(6),
									VehicleName = GetVehicleName(tripReader.GetInt32(2)) // Retrieve VehicleName
								};
								UserTripList.Add(trip);

								// Collect unique initial destinations
								if (!InitDestinations.Contains(trip.InitDestination))
								{
									InitDestinations.Add(trip.InitDestination);
								}

								// Collect unique final destinations
								if (!FinalDestinations.Contains(trip.FinalDestination))
								{
									FinalDestinations.Add(trip.FinalDestination);
								}
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

	
}