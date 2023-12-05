using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus.ClientRide
{
    public class ClientRideModel : PageModel
    {
        string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";

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
                    string query = "INSERT INTO Client_Ride (trip_id, user_id, payment_status, checkin, tripdate) " +
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
                successMessage = "Client ride registered successfully.";
            }
            else
            {
                errorMessage = "Registration failed.";
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

        /* string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";
        public Client driver = new Client();
        public List<Client> userList = new List<Client>();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            // Fetch and display existing drivers
            // Drivers = FetchDriversFromDb();
        }
        private bool RegisterClientRide(int trip_id, int user_id, string payment_status, string checkin, DateTime tripdate)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    string query = "INSERT INTO Client_Ride (trip_id, user_id, payment_status, checkin, tripdate) VALUES (@trip_id, @user_id, @payment_status, @checkin, @tripdate)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@trip_id", trip_id);
                        cmd.Parameters.AddWithValue("@user_id", user_id);
                        cmd.Parameters.AddWithValue("@payment_status", payment_status);
                        cmd.Parameters.AddWithValue("@checkin", checkin);
                        cmd.Parameters.AddWithValue("@tripdate", tripdate);

                        int result = cmd.ExecuteNonQuery();

                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception
                return false;
            }
        }


        public void OnPostRegisterDriver()
        {
            var tripId = Request.Form["trip_id"];
            var userId = Request.Form["user_id"];
            var paymentStatus = Request.Form["payment_status"];
            var checkIn = Request.Form["checkin"];
            var tripDate = DateTime.Parse(Request.Form["tripdate"]);

            // Call the RegisterClientRide function with the correct parameters
            bool isRegistered = RegisterClientRide(tripId, userId, paymentStatus, checkIn, tripDate);

            if (isRegistered)
            {
                successMessage = "Client ride registered successfully.";
            }
            else
            {
                errorMessage = "Registration failed.";
            }
        }


        // Add methods for Edit and Delete operations
        public class Client
        {
            public int? Id { get; set; }
            public string? trip_id { get; set; }
            public string? user_id { get; set; }
            public string? payment_status { get; set; }
            public string? checkin { get; set; }
            public string? tripdate { get; set; }

        }*/
    }

}

