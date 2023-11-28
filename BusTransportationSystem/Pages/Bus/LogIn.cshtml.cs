using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Http;

namespace BusTransportationSystem.Pages.Bus
{
    public class LogInModel : PageModel
    {
        string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";
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
                            reader.Read();
                            string role = reader.GetString(5);

                            // Store role in session
                            HttpContext.Session.SetString("role", role);

                            // Redirect to different page based on role
                            if (role.Equals("ADMIN"))
                            {
                                Response.Redirect("/Bus/AdminDashboard");
                            }
                            else
                            {
                                Response.Redirect("/Bus/UserDashboard");
                            }


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
