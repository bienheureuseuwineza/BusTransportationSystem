using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus.ManageVehicle
{
    public class EditModel : PageModel
    {

        string connString = "Data Source=HOLLYUWINEZA\\SQLEXPRESS;Initial Catalog=BUSMANAGEMENTSYSTEM;Integrated Security=True";

        string connString = "Data Source=DESKTOP-SED41CT\\SQLEXPRESS01;Initial Catalog=BusSystem;Integrated Security=True";


        public Vehicle vehicleInfo = new Vehicle();

        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string qry = "SELECT * FROM Vehicle WHERE Vehicle_id = @VehicleId";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@VehicleId", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                vehicleInfo.VehicleId = reader.GetInt32(0);
                                vehicleInfo.VehicleName = reader.GetString(1);
                                vehicleInfo.DriverId = reader.GetInt32(2);
                                vehicleInfo.VehicleTypes = reader.GetString(3);
                                vehicleInfo.Seats = reader.GetInt32(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        public void OnPost()
        {
            // Edit an existing vehicle
            vehicleInfo.VehicleId = int.Parse(Request.Form["id"]);
            vehicleInfo.VehicleName = Request.Form["vehicleName"];
            vehicleInfo.DriverId = int.Parse(Request.Form["driverId"]);
            vehicleInfo.VehicleTypes = Request.Form["vehicleTypes"];
            vehicleInfo.Seats = int.Parse(Request.Form["seats"]);

            if (vehicleInfo.VehicleName.Length == 0 || vehicleInfo.DriverId <= 0 ||
                vehicleInfo.VehicleTypes.Length == 0 || vehicleInfo.Seats <= 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connString))
                {
                    string qry = "UPDATE Vehicle SET Vehicle_name = @VehicleName, Driver_id = @DriverId, " +
                                 "vehicle_types = @VehicleTypes, seats = @Seats " +
                                 "WHERE Vehicle_id = @VehicleId";
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@VehicleName", vehicleInfo.VehicleName);
                        cmd.Parameters.AddWithValue("@DriverId", vehicleInfo.DriverId);
                        cmd.Parameters.AddWithValue("@VehicleTypes", vehicleInfo.VehicleTypes);
                        cmd.Parameters.AddWithValue("@Seats", vehicleInfo.Seats);
                        cmd.Parameters.AddWithValue("@VehicleId", vehicleInfo.VehicleId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            successMessage = "Vehicle updated successfully";
                        }
                        else
                        {
                            errorMessage = "Failed to update vehicle";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }
    }
}