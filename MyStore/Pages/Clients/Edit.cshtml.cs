using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
            // Get the client ID from the query string
            string Id =Request.Query["Id"];
            try {
                string connectionString = "Server=localhost,1433;Initial Catalog=myStore;User Id=sa;Password=Rana@1234;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM clients WHERE Id=@Id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Id", Id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientInfo.Id = "" + reader.GetInt32(0);
                                clientInfo.Name = reader.GetString(1);
                                clientInfo.Email = reader.GetString(2);
                                clientInfo.Phone = reader.GetString(3);
                                clientInfo.Address = reader.GetString(4);
                                clientInfo.Created_At = reader.GetDateTime(5).ToString();
                            }
                            else
                            {
                                errorMessage = "Client not found";
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
            // Get data from the form
            clientInfo.Id = Request.Form["id"];
            clientInfo.Name = Request.Form["name"];
            clientInfo.Email = Request.Form["email"];
            clientInfo.Phone = Request.Form["phone"];
            clientInfo.Address = Request.Form["address"];
            if (clientInfo.Name.Length == 0 || clientInfo.Email.Length == 0 || clientInfo.Phone.Length == 0 || clientInfo.Address.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }
            try
            {
                string connectionString = "Server=localhost,1433;Initial Catalog=myStore;User Id=sa;Password=Rana@1234;TrustServerCertificate=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE clients " +
                                 "SET Name=@Name, Email=@Email, Phone=@Phone, Address=@Address " +
                                 "WHERE Id=@Id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", clientInfo.Name);
                        command.Parameters.AddWithValue("@Email", clientInfo.Email);
                        command.Parameters.AddWithValue("@Phone", clientInfo.Phone);
                        command.Parameters.AddWithValue("@Address", clientInfo.Address);
                        command.Parameters.AddWithValue("@Id", clientInfo.Id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            Response.Redirect("/Clients/Index");
        }


    }
}
