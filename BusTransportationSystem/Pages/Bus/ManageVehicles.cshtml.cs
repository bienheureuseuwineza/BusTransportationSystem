using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusTransportationSystem.Pages.Bus
{
    public class ManageVehiclesModel : PageModel
    {
        //string connString = "Data Source=DESKTOP-SED41CT\\SQLEXPRESS01;Initial Catalog=BusSystem;Integrated Security=True";
        string connString = "Data Source = LAPTOP-E65QRG1A\\SQLEXPRESS;Initial Catalog=BusSystem; Integrated Security = True";
        
        public List<TripInfo> DisplayedData { get; set; } = new List<TripInfo>();
        public List<string> InitialDestinations { get; set; } = new List<string>();
        public List<string> FinalDestinations { get; set; } = new List<string>();
        public void OnGet()
        {
            FetchDestinationsFromDB();
            
        }

        private void FetchDestinationsFromDB()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {

                    connection.Open();


                    string initDestinationQuery = "SELECT DISTINCT init_destination FROM Trip";
                    InitialDestinations = FetchDestinations(connection, initDestinationQuery);

                    string finalDestinationQuery = "SELECT DISTINCT final_destination FROM Trip";
                    FinalDestinations = FetchDestinations(connection, finalDestinationQuery);


                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private List<string> FetchDestinations(SqlConnection connection, string query)
        {
            List<string> destinations = new List<string>();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            destinations.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return destinations;
        }


        public void OnPost(string initial_destination, string final_destination)
        {
            FetchDisplayedData(initial_destination, final_destination);
        }
        private void FetchDisplayedData(string initialDestination, string finalDestination)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();

                    string query = @"SELECT CONCAT(D.firstname, ' ', D.lastname) AS DriverName, 
                            T.init_destination, 
                            T.final_destination, 
                            T.price 
                     FROM Trip T
                     INNER JOIN Driver D ON T.driver_id = D.id
                     WHERE T.init_destination = @initialDestination 
                           AND T.final_destination = @finalDestination";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@initialDestination", initialDestination);
                    command.Parameters.AddWithValue("@finalDestination", finalDestination);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        DisplayedData = new List<TripInfo>();

                        while (reader.Read())
                        {
                            TripInfo trip = new TripInfo
                            {
                                DriverName = reader.GetString(0),
                                InitialDestination = reader.GetString(1),
                                FinalDestination = reader.GetString(2),
                                Price = reader.GetDouble(3)
                            };

                            DisplayedData.Add(trip);
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
public class TripInfo
{
    public string DriverName { get; set; }
    public string InitialDestination { get; set; }
    public string FinalDestination { get; set; }
    public double Price { get; set; }
}
   
}
