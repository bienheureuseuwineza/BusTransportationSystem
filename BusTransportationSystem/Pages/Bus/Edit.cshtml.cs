using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus
{
    public class EditModel : PageModel
    {
        string connString = "Data Source=JOSEPHUS-ML;Initial Catalog=BusTransportationDB;Integrated Security=True;Encrypt=False";

        public Bus busInfo = new Bus();

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {

                using (SqlConnection con = new SqlConnection(connString))
                {
                    string qry = "SELECT * FROM Bus WHERE bus_id = @BusId";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@BusId",id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                busInfo.BusId = reader.GetInt32(0);
                                busInfo.BusName = reader.GetString(1);
                                busInfo.DriverName = reader.GetString(2);
                                busInfo.Types = reader.GetString(3);
                                busInfo.NumberOfSeats = reader.GetInt32(4);
                                
                                
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

            // Edit an existing bus
            busInfo.BusId = int.Parse(Request.Form["id"]);
            busInfo.BusName = Request.Form["busName"];
            busInfo.DriverName = Request.Form["driverName"];
            busInfo.Types = Request.Form["busType"];
            busInfo.NumberOfSeats = int.Parse(Request.Form["numberOfSeats"]);

            if (busInfo.BusName.Length == 0 || busInfo.DriverName.Length == 0 ||
                busInfo.Types.Length == 0 || busInfo.NumberOfSeats <= 0)
            {
                errorMessage = "All fields are required";
                return;
            }
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string qry = "UPDATE Bus SET bus_name = @BusName, driver_name = @DriverName, " +
                                 "types = @Types, number_of_seats = @NumberOfSeats WHERE bus_id = @BusId";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@BusName", busInfo.BusName);
                        cmd.Parameters.AddWithValue("@DriverName", busInfo.DriverName);
                        cmd.Parameters.AddWithValue("@Types", busInfo.Types);
                        cmd.Parameters.AddWithValue("@NumberOfSeats", busInfo.NumberOfSeats);
                        cmd.Parameters.AddWithValue("@BusId", busInfo.BusId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            successMessage = "Bus updated successfully";
                        }
                        else
                        {
                            errorMessage = "Failed to update bus";
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
