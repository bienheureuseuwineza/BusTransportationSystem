using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus.ClientRide
{
    public class ClientRideModel : PageModel
    {
        
        string connString = "Data Source = LAPTOP-E65QRG1A\\SQLEXPRESS;Initial Catalog=BusSystem; Integrated Security = True";
        public ClientRide newClientRide = new ClientRide(); // Changed to ClientRide
        public List<ClientRide> clientRideList = new List<ClientRide>(); // Changed to List of ClientRide

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            // Fetch and display existing client rides
            // clientRideList = FetchClientRidesFromDb(); // This method needs to be implemented
        }

        private bool RegisterClientRide()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    string query = "INSERT INTO [Client Rides] (trip_id, user_id, checkin, tripdate) " +
                                   "VALUES (@TripId, @UserId, @PaymentStatus, @CheckIn, @TripDate)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@TripId", newClientRide.TripId);
                        cmd.Parameters.AddWithValue("@UserId", newClientRide.UserId);
                        cmd.Parameters.AddWithValue("@PaymentStatus", newClientRide.PaymentStatus);
                        cmd.Parameters.AddWithValue("@CheckIn", newClientRide.CheckIn);
                        cmd.Parameters.AddWithValue("@TripDate", newClientRide.TripDate);

                        int result = cmd.ExecuteNonQuery();

                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        public void OnPost()
        {
            newClientRide.TripId = int.Parse(Request.Form["trip_id"]);
            newClientRide.UserId = int.Parse(Request.Form["user_id"]);
            newClientRide.PaymentStatus = Request.Form["payment_status"];
            newClientRide.CheckIn = Request.Form["checkin"];
            newClientRide.TripDate = DateTime.Parse(Request.Form["tripdate"]);

            if (newClientRide.TripId <= 0 || newClientRide.UserId <= 0)
            {
                errorMessage = "Trip ID and User ID are required";
                return;
            }

            bool isRegistered = RegisterClientRide();

            if (isRegistered)
            {
                successMessage = "YOUR RIDE IS READY ";
            }
            else
            {
                errorMessage = "SOMETHING WENT WRONG ";
            }

            // Reset the fields
            newClientRide.TripId = 0;
            newClientRide.UserId = 0;
            newClientRide.PaymentStatus = "";
            newClientRide.CheckIn = "";
            newClientRide.TripDate = DateTime.MinValue;
        }

        public class ClientRide
        {
            public int TripId { get; set; }
            public int UserId { get; set; }
            public string PaymentStatus { get; set; }
            public string CheckIn { get; set; }
            public DateTime TripDate { get; set; }
        }
    }

}

