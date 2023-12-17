using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Globalization;

namespace BusTransportationSystem.Pages.Bus
{
    public class UserManagementModel : PageModel
    {

		/*string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";

        string connString = "Data Source=DESKTOP-SED41CT\\SQLEXPRESS01;Initial Catalog=BusSystem;Integrated Security=True";*/
		string connString = "Data Source = LAPTOP - E65QRG1A\\SQLEXPRESS;Initial Catalog = BusSystem; Integrated Security = True";

		public User user = new User();
        public List<User> userList = new List<User>();
        public string message = "";
        public void OnGet()
        {
            RetrieveUserData();
        }
        public void OnPost()
        {
            user.firstname = Request.Form["firstname"];
            user.lastname = Request.Form["lastname"];
            user.gender = Request.Form["gender"];
            user.email = Request.Form["email"];
            user.role = Request.Form["role"];
            user.dob = DateTime.Parse(Request.Form["dob"]);

            user.pasword = Request.Form["password"];
            string confirmpassword = Request.Form["password2"];
            if (string.IsNullOrEmpty(user.firstname) || string.IsNullOrEmpty(user.lastname) ||
        string.IsNullOrEmpty(user.gender) || string.IsNullOrEmpty(user.email) || string.IsNullOrEmpty(user.pasword) ||
        string.IsNullOrEmpty(confirmpassword))
            {
                message = "All fields are required";
                return;
            }

            if (user.pasword != confirmpassword)
            {
                message = "Passwords do not match";
                return;
            }
            using (SqlConnection con = new SqlConnection(connString))
            {
                string qry = "INSERT INTO [User] (firstname, lastname, gender, email, role, dob,  pasword) " +
                     "VALUES (@firstname, @lastname, @gender, @email, @role, @dob, @password); " +
                     "SELECT SCOPE_IDENTITY();";
                con.Open();

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    cmd.Parameters.AddWithValue("@firstname", user.firstname);
                    cmd.Parameters.AddWithValue("@lastname", user.lastname);
                    cmd.Parameters.AddWithValue("@gender", user.gender);
                    cmd.Parameters.AddWithValue("@email", user.email);
                    cmd.Parameters.AddWithValue("@role", user.role);
                    cmd.Parameters.AddWithValue("@dob", user.dob);
                    cmd.Parameters.AddWithValue("@password", user.pasword);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        message = "user created";

                    }
                    else { message = " user not created"; }

                }
                con.Close();
            }
            RetrieveUserData();
        }
        private void RetrieveUserData()
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                string qry = "SELECT * FROM [User]";
                con.Open();

                using (SqlCommand cmd = new SqlCommand(qry, con))
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        User user = new User
                        {
                            id = Convert.ToInt32(reader["id"]),
                            firstname = reader["firstname"].ToString(),
                            lastname = reader["lastname"].ToString(),
                            gender = reader["gender"].ToString(),
                            email = reader["email"].ToString(),
                            role = reader["role"].ToString(),
                             //dob = reader["dob"].ToString(),
                            dob = reader["dob"] != DBNull.Value ? Convert.ToDateTime(reader["dob"]) : (DateTime?)null,
                            pasword = reader["pasword"].ToString()
                        };

                        userList.Add(user);
                    }

                    reader.Close();
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
            public string? pasword { get; set; }

        }
    }
}
