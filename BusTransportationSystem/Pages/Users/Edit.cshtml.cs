using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Users
{
    public class EditModel : PageModel
    {
        string connString = "Data Source=JOSEPHUS-ML;Initial Catalog=BusTransportationDB;Integrated Security=True;Encrypt=False";

        public User userInfo = new User();

        public void OnGet()
        {

            String userId = Request.Query["id"];
            try
            {

                using (SqlConnection con = new SqlConnection(connString))
                {

                    string qry = "SELECT * FROM UserTable WHERE user_id = @UserId";

                    con.Open();


                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);


                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                userInfo.UserId = reader.GetInt32(0);
                                userInfo.Username = reader.GetString(1);
                                userInfo.Gender = reader.GetString(2);
                                userInfo.Email = reader.GetString(3);
                                userInfo.Role = reader.GetString(4);
                                userInfo.Dob = reader.GetDateTime(5);
                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                String errorMessage = ex.Message;
                return;
            }
        }
    }
}
