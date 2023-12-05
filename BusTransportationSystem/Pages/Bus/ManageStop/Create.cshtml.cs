using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus.ManageStops
{
    public class CreateModel : PageModel
    {

        string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";

        string connString = "Data Source=DESKTOP-SED41CT\\SQLEXPRESS01;Initial Catalog=BusSystem;Integrated Security=True";


        public Stop newStop = new Stop();

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            // Add a new stop
            newStop.StopsDestination = Request.Form["destination"];
            newStop.TripId = int.Parse(Request.Form["tripId"]);

            if (newStop.StopsDestination.Length == 0 || newStop.TripId <= 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string qry = "INSERT INTO Stops (stops_destination, trip_id) " +
                                 "VALUES (@Destination, @TripId)";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@Destination", newStop.StopsDestination);
                        cmd.Parameters.AddWithValue("@TripId", newStop.TripId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            successMessage = "Stop added successfully";
                        }
                        else
                        {
                            errorMessage = "Failed to add stop";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            newStop.StopsDestination = "";
            newStop.TripId = 0;
        }
    }
}