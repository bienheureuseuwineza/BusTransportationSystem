using BusTransportationSystem.Pages.Bus.ManageTrip;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus.ManageDriver
{
    public class IndexModel : PageModel
    {
        string connString = "Data Source=JOSEPHUS-ML;Initial Catalog=BusSystem;Integrated Security=True;Encrypt=False";

        public List<Driver> DriverList = new List<Driver>();
		

		public void OnGet()
        {
            // Display the list of Drivers
            DriverList.Clear();
            

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                   
                    // Retrieve Drivers
                    string driverQuery = "SELECT * FROM Driver";
                    con.Open();
                    using (SqlCommand driverCmd = new SqlCommand(driverQuery, con))
                    {
                        using (SqlDataReader driverReader = driverCmd.ExecuteReader())
                        {
                            while (driverReader.Read())
                            {
                                Driver driver = new Driver
                                {
                                    Id = driverReader.GetInt32(0),
                                    FirstName = driverReader.GetString(1),
                                    LastName = driverReader.GetString(2),
                                    Category = driverReader.GetString(3),
                                    Phone = driverReader.GetString(4),
                                    DateOfBirth = driverReader.GetDateTime(5)
                                };
                                DriverList.Add(driver);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("error" + ex.Message);
            }
			
		}

    }
    public class Driver
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Category { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
