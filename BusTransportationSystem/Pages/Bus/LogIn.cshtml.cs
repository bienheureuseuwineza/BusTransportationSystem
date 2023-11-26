using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus
{
    public class LogInModel : PageModel
    {
        string connString = "Data Source=LAPTOP-E65QRG1A\\SQLEXPRESS;Initial Catalog=BusSystem;Integrated Security=True";
        public User user = new User();
        public List<User> userList = new List<User>();
        public string message = "";
        public void OnGet()
        {
        }
        public void OnPost() 
        {

            user.email = Request.Form["email"];
            user.password = Request.Form["password"];

         
            using (SqlConnection con = new SqlConnection(connString))
            {
                string qry = "SELECT email, password FROM [User] WHERE email = @email AND password = @password";
                con.Open();

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {

                    cmd.Parameters.AddWithValue("@email", user.email);
                    cmd.Parameters.AddWithValue("@password", user.password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            
                            Response.Redirect("/Bus/Dashboard");
                        }
                        else
                        {
                            
                            message = "Invalid email or password";
                        }
                    }


                }
                con.Close();
            }
        }
        public class User
        {
            public int? id { get; set; }
            public string? firstname { get; set; }
            public string? lastname { get; set; }
            public string? gender { get; set; }
            public string? email { get; set; }
            public string? role { get; set; }
            public string? dob { get; set; }
            public string? password { get; set; }
        }
    }
}
