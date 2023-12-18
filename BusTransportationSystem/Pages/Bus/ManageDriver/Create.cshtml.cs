using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus.ManageDriver
{
    public class CreateModel : PageModel
    {


		/*string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";

        string connString = "Data Source=DESKTOP-SED41CT\\SQLEXPRESS01;Initial Catalog=BusSystem;Integrated Security=True";*/
		string connString = "Data Source = LAPTOP - E65QRG1A\\SQLEXPRESS;Initial Catalog = BusSystem; Integrated Security = True";

		public Driver newDriver = new Driver(); 


        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            // Add a new driver
            newDriver.FirstName = Request.Form["firstName"];
            newDriver.LastName = Request.Form["lastName"];
            newDriver.Category = Request.Form["driverCategory"];
            newDriver.Phone = Request.Form["phone"];
            newDriver.DateOfBirth = DateTime.Parse(Request.Form["dob"]);

            if (newDriver.FirstName.Length == 0 || newDriver.LastName.Length == 0 ||
                newDriver.Category.Length == 0 || newDriver.Phone.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string qry = "INSERT INTO Driver (firstname, lastname, D_category, phone, dob) " +
                                 "VALUES (@FirstName, @LastName, @Category, @Phone, @DateOfBirth)";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", newDriver.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", newDriver.LastName);
                        cmd.Parameters.AddWithValue("@Category", newDriver.Category);
                        cmd.Parameters.AddWithValue("@Phone", newDriver.Phone);
                        cmd.Parameters.AddWithValue("@DateOfBirth", newDriver.DateOfBirth);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            successMessage = "Driver added successfully";
                        }
                        else
                        {
                            errorMessage = "Failed to add driver";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            newDriver.FirstName = "";
            newDriver.LastName = "";
            newDriver.Category = "";
            newDriver.Phone = "";
            newDriver.DateOfBirth = DateTime.MinValue;
        }
    }
}
