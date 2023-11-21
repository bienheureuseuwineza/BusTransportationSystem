using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus
{
    public class CreateModel : PageModel
    {
        string connString = "Data Source=JOSEPHUS-ML;Initial Catalog=BusTransportationDB;Integrated Security=True;Encrypt=False";

        public Bus newBus = new Bus();
       
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }
        public void OnPost()
        {
            // Add a new bus

            newBus.BusName = Request.Form["busName"];
            newBus.DriverName = Request.Form["driverName"];
            newBus.Types = Request.Form["busType"];
            newBus.NumberOfSeats = int.Parse(Request.Form["numberOfSeats"]);

            if(newBus.BusName.Length ==0 || newBus.DriverName.Length ==0 ||
                newBus.Types.Length ==0 || newBus.NumberOfSeats <=0)
            {
                errorMessage = "All fields are required";
                return;
            }
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string qry = "INSERT INTO Bus (bus_name, driver_name, types, number_of_seats) " +
                                 "VALUES (@BusName, @DriverName, @Types, @NumberOfSeats)";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@BusName", newBus.BusName);
                        cmd.Parameters.AddWithValue("@DriverName", newBus.DriverName);
                        cmd.Parameters.AddWithValue("@Types", newBus.Types);
                        cmd.Parameters.AddWithValue("@NumberOfSeats", newBus.NumberOfSeats);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            successMessage = "Bus added successfully";
                        }
                        else
                        {
                            errorMessage = "Failed to add bus";
                        }
                    }
                }

                //OnGet(); // Refresh the buses list after addition

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            newBus.BusName = "";
            newBus.DriverName = "";
            newBus.Types = "";
            newBus.NumberOfSeats  = 0;
        }
    }
   
}
