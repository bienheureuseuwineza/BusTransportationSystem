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
        string connString = "Data Source=.;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";
        public User user = new User();
        public List<User> userList = new List<User>();
        public string message = "";
        public void OnGet()
        {
        }
        public IActionResult OnPost() 
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
                            string role = reader.GetString(1);

                            // Store role in session
                            if (HttpContext != null && HttpContext.Session != null)
                            {
                                HttpContext.Session.SetString("role", role);
                            }
                            if (string.Equals(role, "ADMIN", StringComparison.OrdinalIgnoreCase))
                            {
                              
                                return RedirectToPage("/Bus/AdminDashboard");
                            }
                            else
                            {
                                return RedirectToPage("/Bus/UserDashboard");
                            }


                        }
                        else
                        {
                            
                            message = "Invalid email or password";
                        }
                    }
                    return Page();

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
