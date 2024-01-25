using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace DemoWebApp.Pages.Items
{
    public class EditModel : PageModel
    {
        public ItemDetails itemDetails = new ItemDetails();
        public string errorMessage = "";
        public string sucessMessage = "";
        public void OnGet()
        {
            string item_id =Request.Query["id"];

            try {

                //creating the connection string
                string connectionString = "Data Source=localhost;Initial Catalog=demoWebApp;Integrated Security=True";

                //innitiate sql connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuarry = "SELECT * FROM demo_items WHERE id =@id";
                    using (SqlCommand cmd = new SqlCommand(sqlQuarry, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", item_id);

                        using (SqlDataReader reader = cmd.ExecuteReader()) 
                        {

                            if (reader.Read())
                            {
                                itemDetails.id = "" + reader.GetInt32(0);
                                itemDetails.item_name = reader.GetString(1);
                                itemDetails.item_code = reader.GetString(2);
                                itemDetails.item_category = reader.GetString(3);
                                itemDetails.item_description = reader.GetString(4);
                                itemDetails.service_type = reader.GetString(5);
                                itemDetails.created_at = reader.GetDateTime(6).ToString();

                            }
                        
                        }

                    }
                }



            }
            catch (Exception ex) { 
            errorMessage = ex.Message;
                return;
            
            }

        }

        public void OnPost() {

            //setting up the values for the pulic object got from the request
            itemDetails.id = Request.Form["id"];
            itemDetails.item_name = Request.Form["item_name"];
            itemDetails.item_code = Request.Form["item_code"];
            itemDetails.item_category = Request.Form["item_category"];
            itemDetails.item_description = Request.Form["item_description"];
            itemDetails.service_type = Request.Form["service_type"];
            //validation check
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
                using (SqlConnection connetion = new SqlConnection(connectionString))
                {
                    connetion.Open();
                    //create quarry string
                    string sqlQuarry = "UPDATE demo_items" +
                                       " SET item_name =@item_name,item_code =@item_code,item_category =@item_category,item_description =@item_description,service_type =@service_type" +
                                       " WHERE id =@id;";

                    using (SqlCommand command = new SqlCommand(sqlQuarry, connetion))
                    {
                        //set values for quarry got from the object
                        command.Parameters.AddWithValue("@id", itemDetails.id);
                        command.Parameters.AddWithValue("@item_name", itemDetails.item_name);
                        command.Parameters.AddWithValue("@item_code", itemDetails.item_code);
                        command.Parameters.AddWithValue("@item_category", itemDetails.item_category);
                        command.Parameters.AddWithValue("@item_description", itemDetails.item_description);
                        command.Parameters.AddWithValue("@service_type", itemDetails.service_type);
                        command.ExecuteNonQuery();

                    }

                }


            }

            catch (Exception ex)
            {
                //if fails retuern with error message 
                errorMessage = ex.Message;
                return;

            }
            //redirect to the listing page
            Response.Redirect("/Items/Index");

        }
    }
}
