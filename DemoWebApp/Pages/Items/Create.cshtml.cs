using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
namespace DemoWebApp.Pages.Items
{
    public class CreateModel : PageModel
    {
        public ItemDetails itemDetails  = new ItemDetails();
        public string errorMessage =  "";
        public string sucessMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            //setting up the values for the pulic object for saving
            itemDetails.item_name = Request.Form["item_name"];
            itemDetails.item_code = Request.Form["item_code"];
            itemDetails.item_category = Request.Form["item_category"];
            itemDetails.item_description = Request.Form["item_description"];
            itemDetails.service_type = Request.Form["service_type"];
            if (itemDetails.item_name.Length == 0)
            {
                errorMessage = "Please fill Item Name";
                return;
            }
            else if (itemDetails.item_code.Length == 0)
            {
                errorMessage = "Please fill item Code";
                return;
            }
            //check item code is alaready exists
            else if (!checkItemCodeExists(itemDetails.item_code)) {
                errorMessage = "Item Code Already Exists";
                return;
            }
            else if (itemDetails.item_category.Length == 0)
            {
                errorMessage = "Please select Item Category";
                return;
            }
            else if (itemDetails.item_description.Length == 0)
            {
                errorMessage = "Please fill Item Description";
                return;
            }
            else if (itemDetails.service_type.Length == 0)
            {
                errorMessage = "Please select Service Type";
                return;
            }
       
            try {
                //creating the connection string
                string connectionString = "Data Source=localhost;Initial Catalog=demoWebApp;Integrated Security=True";
                using (SqlConnection connetion  =new SqlConnection(connectionString) ) {
                    connetion.Open();

                    string sqlQuarry = "INSERT INTO demo_items" +
                                       "(item_name,item_code,item_category,item_description,service_type)" +
                                       "VALUES (@item_name,@item_code,@item_category,@item_description,@service_type);";

                    using (SqlCommand command = new SqlCommand(sqlQuarry,connetion)) {
                        command.Parameters.AddWithValue("@item_name",itemDetails.item_name);
                        command.Parameters.AddWithValue("@item_code", itemDetails.item_code);
                        command.Parameters.AddWithValue("@item_category", itemDetails.item_category);
                        command.Parameters.AddWithValue("@item_description", itemDetails.item_description);
                        command.Parameters.AddWithValue("@service_type", itemDetails.service_type);
                        command.ExecuteNonQuery();

                    }
                
                }

            }
            catch(Exception ex) {
            errorMessage = ex.Message;
                return;
            }

            //save data to the DB
            sucessMessage = "Item Created Sucessfully";
            itemDetails.item_name = "";
            itemDetails.item_code = "";
            itemDetails.item_category = "";
            itemDetails.item_description = "";
            itemDetails.service_type = "";
            Response.Redirect("/Items/Index");
        }

        private bool checkItemCodeExists(string item_code)
        {
            try
            {
                int item_count;
                //creating the connection string
                string connectionString = "Data Source=localhost;Initial Catalog=demoWebApp;Integrated Security=True";

                //innitiate sql connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuarry = "SELECT COUNT(item_code) FROM demo_items WHERE item_code =@code";
                    using (SqlCommand cmd = new SqlCommand(sqlQuarry, connection))
                    {
                        cmd.Parameters.AddWithValue("@code", item_code);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                item_count = reader.GetInt32(0);

                                if (item_count == 0) { return true; }
                                else { return false; }


                            }
                            
                        }

                    }
                }

                return false;

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;

            }
        }
    }
}
