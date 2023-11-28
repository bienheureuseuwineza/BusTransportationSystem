using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace BusTransportationSystem.Pages
{
    public class LoginModel : PageModel
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
                string qry = "SELECT email, password FROM [User] WHERE email = @email";
                con.Open();

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@email", user.email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            string storedPasswordHash = reader.GetString(1);

                            var passwordHasher = new PasswordHasher<User>();
                            var result = passwordHasher.VerifyHashedPassword(null, storedPasswordHash, user.password);

                            if (result == PasswordVerificationResult.Success)
                            {
                                string role = reader.GetString(1);
								//HttpContext.Session.SetString("username", user.email);


								// Redirect to different page based on role
								if (role.Equals("ADMIN"))
                                {
                                    return RedirectToPage("/Bus/AdminDashboard");
                                }
                                else
                                {
                                    return RedirectToPage("/Index");
                                }
                            }
                            else
                            {
                                message = "Invalid email or password";
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

            return Page();
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