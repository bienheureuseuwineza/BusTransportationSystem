using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Users
{
    public class IndexModel : PageModel
    {
        string connString = "Data Source=JOSEPHUS-ML;Initial Catalog=BusTransportationDB;Integrated Security=True;Encrypt=False";

        public List<User> UserList = new List<User>();

        public void OnGet()
        {
            // Display the list of users
            UserList.Clear();
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string qry = "SELECT * FROM UserTable";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    UserId = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    Gender = reader.GetString(2),
                                    Email = reader.GetString(3),
                                    Role = reader.GetString(4),
                                    Dob = reader.GetDateTime(5)
                                    
                                };
                                UserList.Add(user);
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
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime Dob { get; set; }
        
    }
}
