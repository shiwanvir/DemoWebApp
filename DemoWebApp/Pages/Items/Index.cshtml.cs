using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
namespace DemoWebApp.Pages.Items
{
    public class IndexModel : PageModel
    {
        public List<ItemDetails> itemList = new List<ItemDetails>();
        public void OnGet()
        {
            try {
                //creating the connection string
                string connectionString = "Data Source=localhost;Initial Catalog=demoWebApp;Integrated Security=True";

                //innitiate sql connection
                using (SqlConnection connection =new SqlConnection(connectionString) )
                {
                    connection.Open();
                    string sqlQuarry = "SELECT * FROM demo_items ORDER BY id DESC";
                    using(SqlCommand cmd = new SqlCommand(sqlQuarry, connection)) {
                        using ( SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ItemDetails item = new ItemDetails();
                                item.id = "" + reader.GetInt32(0);
                                item.item_name = reader.GetString(1);
                                item.item_code = reader.GetString(2);
                                item.item_category = reader.GetString(3);
                                item.item_description = reader.GetString(4);
                                item.service_type = reader.GetString(5);
                                item.created_at = reader.GetDateTime(6).ToString();
                                itemList.Add(item);
                            }
                        }
                    
                    }
                }
            
            }
            
            
            catch (Exception ex) { 
                Console.WriteLine("Exception"+ex.ToString());
            }
        }
    }
    //get saved items when page loads
    public class ItemDetails{
        public string id;
        public string item_name;
        public string item_code;
        public string item_category;
        public string item_description;
        public string service_type;
        public string created_at;

    
    
    }
}
