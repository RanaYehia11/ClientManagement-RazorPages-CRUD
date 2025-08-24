using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace MyStore.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errorMessage = "";
        public string successMessage = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            // Get data from the form

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

                    string sql = "INSERT INTO clients (Name, Email, Phone, Address, Created_At) " +
                                 "VALUES (@Name, @Email, @Phone, @Address, @CreatedAt)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Name", clientInfo.Name);
                        command.Parameters.AddWithValue("@Email", clientInfo.Email);
                        command.Parameters.AddWithValue("@Phone", clientInfo.Phone);
                        command.Parameters.AddWithValue("@Address", clientInfo.Address);
                        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            // clear form
            clientInfo.Name = "";
            clientInfo.Email = "";
            clientInfo.Phone = "";
            clientInfo.Address = "";

            successMessage = "New Client Added Correctly";

            // redirect back to list
            Response.Redirect("/Clients/Index");
        }

    }
}
