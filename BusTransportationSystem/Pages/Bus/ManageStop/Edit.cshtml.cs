using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus.ManageStops
{
    public class EditModel : PageModel
    {

        string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";

        string connString = "Data Source=DESKTOP-SED41CT\\SQLEXPRESS01;Initial Catalog=BusSystem;Integrated Security=True";


        public Stop stopInfo = new Stop();

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string qry = "SELECT * FROM Stops WHERE id = @StopId";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@StopId", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                stopInfo.Id = reader.GetInt32(0);
                                stopInfo.StopsDestination = reader.GetString(1);
                                stopInfo.TripId = reader.GetInt32(2);
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
            // Edit an existing stop
            stopInfo.Id = int.Parse(Request.Form["id"]);
            stopInfo.StopsDestination = Request.Form["destination"];
            stopInfo.TripId = int.Parse(Request.Form["tripId"]);

            if (stopInfo.StopsDestination.Length == 0 || stopInfo.TripId <= 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string qry = "UPDATE Stops SET stops_destination = @Destination, trip_id = @TripId " +
                                 "WHERE id = @StopId";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@Destination", stopInfo.StopsDestination);
                        cmd.Parameters.AddWithValue("@TripId", stopInfo.TripId);
                        cmd.Parameters.AddWithValue("@StopId", stopInfo.Id);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            successMessage = "Stop updated successfully";
                        }
                        else
                        {
                            errorMessage = "Failed to update stop";
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