using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus.ManageVehicle
{
    public class IndexModel : PageModel
    {


		string connString = "Data Source = LAPTOP - E65QRG1A\\SQLEXPRESS;Initial Catalog = BusSystem; Integrated Security = True";

		public List<Vehicle> VehicleList = new List<Vehicle>();

        public void OnGet()
        {
            // Display the list of Vehicles
            VehicleList.Clear();

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    // Retrieve Vehicles
                    string vehicleQuery = "SELECT * FROM Vehicle";
                    con.Open();
                    using (SqlCommand vehicleCmd = new SqlCommand(vehicleQuery, con))
                    {
                        using (SqlDataReader vehicleReader = vehicleCmd.ExecuteReader())
                        {
                            while (vehicleReader.Read())
                            {
                                Vehicle vehicle = new Vehicle
                                {
                                    VehicleId = vehicleReader.GetInt32(0),
                                    VehicleName = vehicleReader.GetString(1),
                                    DriverId = vehicleReader.GetInt32(2),
                                    VehicleTypes = vehicleReader.GetString(3),
                                    Seats = vehicleReader.GetInt32(4)
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

	public class Vehicle
	{
		public int VehicleId { get; set; }
		public string VehicleName { get; set; }
		public int DriverId { get; set; }
		public string VehicleTypes { get; set; }
		public int Seats { get; set; }
	}
}