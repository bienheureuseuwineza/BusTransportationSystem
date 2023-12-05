using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus.ManageTrip
{
    public class EditModel : PageModel
    {
        string connString = "Data Source=DESKTOP-SED41CT\\SQLEXPRESS01;Initial Catalog=BusSystem;Integrated Security=True";

        public Trip tripInfo = new Trip();

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string qry = "SELECT * FROM Trip WHERE id = @TripId";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@TripId", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tripInfo.TripId = reader.GetInt32(0);
                                tripInfo.DriverId = reader.GetInt32(1);
                                tripInfo.VehicleId = reader.GetInt32(2);
                                tripInfo.InitDestination = reader.GetString(3);
                                tripInfo.FinalDestination = reader.GetString(4);
                                tripInfo.Price = reader.GetFloat(5);
                                tripInfo.TripDate = reader.GetDateTime(6);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        public void OnPost()
        {
            // Edit an existing trip
            tripInfo.TripId = int.Parse(Request.Form["id"]);
            tripInfo.DriverId = int.Parse(Request.Form["driverId"]);
            tripInfo.VehicleId = int.Parse(Request.Form["vehicleId"]);
            tripInfo.InitDestination = Request.Form["initDestination"];
            tripInfo.FinalDestination = Request.Form["finalDestination"];
            tripInfo.Price = float.Parse(Request.Form["price"]);
            tripInfo.TripDate = DateTime.Parse(Request.Form["tripDate"]);

            if (tripInfo.DriverId <= 0 || tripInfo.VehicleId <= 0 ||
                string.IsNullOrEmpty(tripInfo.InitDestination) || string.IsNullOrEmpty(tripInfo.FinalDestination) ||
                tripInfo.Price <= 0 || tripInfo.TripDate == DateTime.MinValue)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string qry = "UPDATE Trip SET driver_id = @DriverId, vehicle_id = @VehicleId, " +
                                 "init_destination = @InitDestination, final_destination = @FinalDestination, " +
                                 "price = @Price, trip_date = @TripDate " +
                                 "WHERE id = @TripId";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@DriverId", tripInfo.DriverId);
                        cmd.Parameters.AddWithValue("@VehicleId", tripInfo.VehicleId);
                        cmd.Parameters.AddWithValue("@InitDestination", tripInfo.InitDestination);
                        cmd.Parameters.AddWithValue("@FinalDestination", tripInfo.FinalDestination);
                        cmd.Parameters.AddWithValue("@Price", tripInfo.Price);
                        cmd.Parameters.AddWithValue("@TripDate", tripInfo.TripDate);
                        cmd.Parameters.AddWithValue("@TripId", tripInfo.TripId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            successMessage = "Trip updated successfully";
                        }
                        else
                        {
                            errorMessage = "Failed to update trip";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }
    }
}