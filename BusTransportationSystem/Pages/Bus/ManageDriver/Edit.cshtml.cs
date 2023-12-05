using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus.ManageDriver
{
    public class EditModel : PageModel
    {

        string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";

        string connString = "Data Source=DESKTOP-SED41CT\\SQLEXPRESS01;Initial Catalog=BusSystem;Integrated Security=True";


        public Driver driverInfo = new Driver();

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string qry = "SELECT * FROM Driver WHERE id = @DriverId";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@DriverId", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                driverInfo.Id = reader.GetInt32(0);
                                driverInfo.FirstName = reader.GetString(1);
                                driverInfo.LastName = reader.GetString(2);
                                driverInfo.Category = reader.GetString(3);
                                driverInfo.Phone = reader.GetString(4);
                                driverInfo.DateOfBirth = reader.GetDateTime(5);
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
            // Edit an existing driver
            driverInfo.Id = int.Parse(Request.Form["id"]);
            driverInfo.FirstName = Request.Form["firstName"];
            driverInfo.LastName = Request.Form["lastName"];
            driverInfo.Category = Request.Form["driverCategory"];
            driverInfo.Phone = Request.Form["phone"];
            driverInfo.DateOfBirth = DateTime.Parse(Request.Form["dob"]);

            if (driverInfo.FirstName.Length == 0 || driverInfo.LastName.Length == 0 ||
                driverInfo.Category.Length == 0 || driverInfo.Phone.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string qry = "UPDATE Driver SET firstname = @FirstName, lastname = @LastName, " +
                                 "D_category = @Category, Phone = @Phone, dob = @DateOfBirth " +
                                 "WHERE id = @DriverId";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", driverInfo.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", driverInfo.LastName);
                        cmd.Parameters.AddWithValue("@Category", driverInfo.Category);
                        cmd.Parameters.AddWithValue("@Phone", driverInfo.Phone);
                        cmd.Parameters.AddWithValue("@DateOfBirth", driverInfo.DateOfBirth);
                        cmd.Parameters.AddWithValue("@DriverId", driverInfo.Id);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            successMessage = "Driver updated successfully";
                        }
                        else
                        {
                            errorMessage = "Failed to update driver";
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