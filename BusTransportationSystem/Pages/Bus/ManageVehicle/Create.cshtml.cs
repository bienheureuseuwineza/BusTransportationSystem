using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus.ManageVehicle
{
    public class CreateModel : PageModel
    {

        //string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";

        string connString = "Data Source=DESKTOP-SED41CT\\SQLEXPRESS01;Initial Catalog=BusSystem;Integrated Security=True";


        public Vehicle newVehicle = new Vehicle();

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {

            //var userRole = HttpContext.Session.GetString("role");

            //if (userRole != null && userRole.Equals("ADMIN"))
            //{

            //}

            //else
            //{
            //    // Redirect unauthorized users or show an error message
            //    Response.Redirect("/Bus/UnauthorizedAccess");
                
            //    // errorMessage = "Unauthorized access";

            //}
            // Add a new vehicle
            newVehicle.VehicleName = Request.Form["vehicleName"];
            newVehicle.DriverId = int.Parse(Request.Form["driverId"]);
            newVehicle.VehicleTypes = Request.Form["vehicleTypes"];
            newVehicle.Seats = int.Parse(Request.Form["seats"]);

            if (newVehicle.VehicleName.Length == 0 || newVehicle.VehicleTypes.Length == 0 || newVehicle.Seats <= 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {


                    string qry = "INSERT INTO Vehicle (Vehicle_name, Driver_id, vehicle_types, seats) " +

                                 "VALUES (@VehicleName, @DriverId, @VehicleTypes, @Seats)";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@VehicleName", newVehicle.VehicleName);
                        cmd.Parameters.AddWithValue("@DriverId", newVehicle.DriverId);
                        cmd.Parameters.AddWithValue("@VehicleTypes", newVehicle.VehicleTypes);
                        cmd.Parameters.AddWithValue("@Seats", newVehicle.Seats);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            successMessage = "Vehicle added successfully";
                        }
                        else
                        {
                            errorMessage = "Failed to add vehicle";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            newVehicle.VehicleName = "";
            newVehicle.DriverId = 0;
            newVehicle.VehicleTypes = "";
            newVehicle.Seats = 0;
        }
    }
}