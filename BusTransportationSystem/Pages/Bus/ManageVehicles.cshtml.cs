using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus
{
   public class ManageVehiclesModel : PageModel
{
        string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";
        public Driver driver = new Driver();
        public List<Driver> userList = new List<Driver>();
        public string Message = "";

    public void OnGet()
    {
        // Fetch and display existing drivers
        // Drivers = FetchDriversFromDb();
    }
        private bool RegisterDriver(string firstName, string lastName, string drivingCategory, string phoneNumber)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    con.Open();
                    string query = "INSERT INTO Drivers (firstname, lastname, d_category, phone) VALUES (@firstname, @lastname, @d_category, @phone)";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@firstname", firstName);
                        cmd.Parameters.AddWithValue("@lastname", lastName);
                        cmd.Parameters.AddWithValue("@d_category", drivingCategory);
                        cmd.Parameters.AddWithValue("@phone", phoneNumber);

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
        var firstName = Request.Form["firstName"];
        var lastName = Request.Form["lastName"];
        var drivingCategory = Request.Form["drivingCategory"];
        var phoneNumber = Request.Form["phoneNumber"];

            // Validate and save the driver
            // bool isRegistered = RegisterDriver(firstName, lastName, drivingCategory, phoneNumber);
            bool isRegistered = RegisterDriver(firstName, lastName, drivingCategory, phoneNumber);


            if (isRegistered)
        {
            Message = "Driver registered successfully.";
        }
        else
        {
            Message = "Registration failed.";
        }
    }

        // Add methods for Edit and Delete operations
        public class Driver
        {
            public int? Id { get; set; }
            public string? firstname { get; set; }
            public string? lastname { get; set; }
            public string? drivingCategory { get; set; }
            public string? phoneNumber { get; set; }
          
        }
    }

}
