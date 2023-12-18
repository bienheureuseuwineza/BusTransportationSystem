using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus.ManageTrip
{
    public class CreateModel : PageModel
    {

        // string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";

        //string connString = "Data Source=DESKTOP-SED41CT\\SQLEXPRESS01;Initial Catalog=BusSystem;Integrated Security=True";
        string connString = "Data Source=LAPTOP-E65QRG1A\\SQLEXPRESS;Initial Catalog=BusSystem; Integrated Security=True";


        public Trip newTrip = new Trip();

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            // Add a new trip
            newTrip.DriverId = int.Parse(Request.Form["driverId"]);
            newTrip.VehicleId = int.Parse(Request.Form["vehicleId"]);
            newTrip.InitDestination = Request.Form["initDestination"];
            newTrip.FinalDestination = Request.Form["finalDestination"];
            newTrip.Price = float.Parse(Request.Form["price"]);
            newTrip.TripDate = DateTime.Parse(Request.Form["tripDate"]);

            if (newTrip.DriverId <= 0 || newTrip.VehicleId <= 0 ||
                newTrip.InitDestination.Length == 0 || newTrip.FinalDestination.Length == 0 || newTrip.Price <= 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string qry = "INSERT INTO Trip (driver_id, vehicle_id, init_destination, final_destination, price, trip_date) " +
                                 "VALUES (@DriverId, @VehicleId, @InitDestination, @FinalDestination, @Price, @TripDate)";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@DriverId", newTrip.DriverId);
                        cmd.Parameters.AddWithValue("@VehicleId", newTrip.VehicleId);
                        cmd.Parameters.AddWithValue("@InitDestination", newTrip.InitDestination);
                        cmd.Parameters.AddWithValue("@FinalDestination", newTrip.FinalDestination);
                        cmd.Parameters.AddWithValue("@Price", newTrip.Price);
                        cmd.Parameters.AddWithValue("@TripDate", newTrip.TripDate);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            successMessage = "Trip added successfully";
                        }
                        else
                        {
                            errorMessage = "Failed to add trip";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            newTrip.DriverId = 0;
            newTrip.VehicleId = 0;
            newTrip.InitDestination = "";
            newTrip.FinalDestination = "";
            newTrip.Price = 0;
            newTrip.TripDate = DateTime.MinValue;
        }
    }
}