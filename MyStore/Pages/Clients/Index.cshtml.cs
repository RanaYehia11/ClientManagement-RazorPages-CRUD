using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> listClients = new List<ClientInfo>(); // list to store multiple clients
        public void OnGet()
        {
            try
            {
                string connectionString = "Server=localhost,1433;Initial Catalog=myStore;User Id=sa;Password=Rana@1234;TrustServerCertificate=True;";


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM clients"; // SQL query to select all clients
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Read each record from the database
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.Id = "" + reader.GetInt32(0); // assuming first column is Id
                                clientInfo.Name = reader.GetString(1); // assuming second column is Name
                                clientInfo.Email = reader.GetString(2); // assuming third column is Email
                                clientInfo.Phone = reader.GetString(3); // assuming fourth column is Phone
                                clientInfo.Address = reader.GetString(4); // assuming fifth column is Address
                                clientInfo.Created_At = reader.GetDateTime(5).ToString(); // assuming sixth column is Created_At
                                listClients.Add(clientInfo); // add client info to the list
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());

            }
        }
    }


    public class ClientInfo  // store Data in ClientInfo from Database
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Created_At { get; set; }


    }
}