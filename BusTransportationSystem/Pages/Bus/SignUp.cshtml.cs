using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace BusTransportationSystem.Pages.Bus
{
    public class SignUpModel : PageModel { 

    string connString = "Data Source=LAPTOP-E65QRG1A\\SQLEXPRESS;Initial Catalog=BusSystem;Integrated Security=True";
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
            using (SqlConnection con = new SqlConnection(connString))
            {
                string qry = "INSERT INTO User VALUES(@firstname, @lastname, @gender, @email, @role,@dob ,@password)";
                con.Open();

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@firstname", user.firstname);
                    cmd.Parameters.AddWithValue("@lastname", user.lastname);
                    cmd.Parameters.AddWithValue("@gender", user.gender);
                    cmd.Parameters.AddWithValue("@email", user.email);
                    cmd.Parameters.AddWithValue("@role", user.role);
                    cmd.Parameters.AddWithValue("@dob", user.dob);
                    cmd.Parameters.AddWithValue("@password", user.password);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        message = "user created";

                    }
                    else { message = " user not created"; }

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
