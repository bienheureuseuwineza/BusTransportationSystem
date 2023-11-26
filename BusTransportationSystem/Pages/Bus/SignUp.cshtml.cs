using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace BusTransportationSystem.Pages.Bus
{
    public class SignUpModel : PageModel { 

    string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";
    public User user = new User();
    public List<User> userList = new List<User>();
    public string message = "";

    public void OnGet()
        {
        
        }
        public void OnPost() 
        {
            user.firstname = Request.Form["firstname"];
            user.lastname = Request.Form["lastname"];
            user.gender = Request.Form["gender"];
            user.email = Request.Form["email"];
            user.role = Request.Form["role"];
            user.dob = Request.Form["dob"];
            user.password = Request.Form["password"];
            string confirmpassword = Request.Form["password2"];
            if (string.IsNullOrEmpty(user.firstname) || string.IsNullOrEmpty(user.lastname) ||
           string.IsNullOrEmpty(user.gender) || string.IsNullOrEmpty(user.email) ||
           string.IsNullOrEmpty(user.dob) || string.IsNullOrEmpty(user.password) ||
           string.IsNullOrEmpty(confirmpassword))
            {
                message = "All fields are required";
                return;
            }

            
            if (user.password != confirmpassword)
            {
                message = "Passwords do not match";
                return;
            }
            using (SqlConnection con = new SqlConnection(connString))
            {
                string qry = "INSERT INTO [User] (firstname, lastname, gender, email, dob, password) " +
                     "VALUES (@firstname, @lastname, @gender, @email, @dob, @password); " +
                     "SELECT SCOPE_IDENTITY();";
                con.Open();

                 using (SqlCommand cmd = new SqlCommand(qry, con))
                 {
                     cmd.Parameters.AddWithValue("@firstname", user.firstname);
                     cmd.Parameters.AddWithValue("@lastname", user.lastname);
                     cmd.Parameters.AddWithValue("@gender", user.gender);
                     cmd.Parameters.AddWithValue("@email", user.email);
                     cmd.Parameters.AddWithValue("@dob", user.dob);
                     cmd.Parameters.AddWithValue("@password", user.password);

                   
                int newUserId = Convert.ToInt32(cmd.ExecuteScalar());

                if (newUserId > 0)
                {
                    message = "User created with ID: " + newUserId;
                    Response.Redirect("/Bus/LogIn"); 
                    return;
                }
                else
                {
                    message = "User not created";
                   
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
