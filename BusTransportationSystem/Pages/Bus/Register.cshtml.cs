using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages
{
    public class RegisterModel : PageModel
    {

        //string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";

        //string connString = "Data Source=DESKTOP-SED41CT\\SQLEXPRESS01;Initial Catalog=BusSystem;Integrated Security=True";
        string connString = "Data Source = LAPTOP-E65QRG1A\\SQLEXPRESS;Initial Catalog=BusSystem; Integrated Security = True";
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
            user.dob = DateTime.Parse(Request.Form["dob"]);
            user.password = Request.Form["password"];
            string confirmpassword = Request.Form["password2"];
            if (string.IsNullOrEmpty(user.firstname) || string.IsNullOrEmpty(user.lastname) ||

        string.IsNullOrEmpty(user.gender) || string.IsNullOrEmpty(user.email) || string.IsNullOrEmpty(user.password) ||
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
            var passwordHasher = new PasswordHasher<User>(); // User is your user model
            user.password = passwordHasher.HashPassword(null, user.password);

            using (SqlConnection con = new SqlConnection(connString))
            {

                string qry = "INSERT INTO [User] (firstname, lastname, gender, email,role, dob, password) " +
                     "VALUES (@firstname, @lastname, @gender, @email,'Client', @dob, @password); " +

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


                    int newUserId = cmd.ExecuteNonQuery();

                    if (newUserId > 0)
                    {
                        message = "User created with ID: " + newUserId;
                        Response.Redirect("/Index");
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
            public DateTime? dob { get; set; }
            public string? password { get; set; }

        }
    }
}