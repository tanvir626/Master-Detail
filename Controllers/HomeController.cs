using Master_Detail.Models;
using Microsoft.AspNetCore.Mvc;

namespace Master_Detail.Controllers
{
    public class HomeController : Controller
    {
        private readonly MasterdetaildbContext db;

        public HomeController(MasterdetaildbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SaveTableData([FromBody] PostDataModel model)
        {
            try
            {
                // Ensure that the model is not null
                if (model == null || model.TableData == null || string.IsNullOrEmpty(model.UserName))
                {
                    return Json(new { success = false, message = "Invalid input data." });
                }

                // Create new Ordertable entry
                var user = new Ordertable
                {
                    Username = model.UserName
                };

                // Add to the database and save changes
                db.Ordertables.Add(user);
                db.SaveChanges(); // This will generate the OrderId for the newly created user

                // Retrieve the newly generated OrderId
                int neworderid = user.Orderid;

                // Loop through the table data and create Orderdetail records
                foreach (var row in model.TableData)
                {
                    // Ensure the Quantity and UnitPrice are valid integers
                    int quantity = row.Quantity != 0 ? row.Quantity : 0;
                    int unitPrice = row.UnitPrice != 0 ? row.UnitPrice : 0;

                    var newOrderDetail = new Orderdetail
                    {
                        orderid = neworderid,
                        ProductName = row.ProductName,
                        Quantity = quantity,
                        UnitPrice = unitPrice
                    };

                    // Add the new OrderDetail to the database
                    db.Orderdetails.Add(newOrderDetail);
                }

                // Save the changes to the Orderdetails table
                db.SaveChanges();

                // Return success response
                return Json(new { success = true, message = "Data saved successfully!" });
            }
            catch (Exception ex) // Catch block with ex to handle exceptions
            {
                // Return the error message in the response
                return Json(new { success = false, message = "Error saving data.", error = ex.Message });
            }
        }


        // Model to wrap incoming data
        public class PostDataModel
        {
            public string UserName { get; set; } // User name
            public List<TableRow> TableData { get; set; } // Table data
        }

        public class TableRow
        {
            public int orderid { get; set; }
            public string ProductName { get; set; }  // ProductName
            public int Quantity { get; set; }   // Quantity (string, assuming it could come as string from the client)
            public int UnitPrice { get; set; }  // UnitPrice (string, assuming it could come as string from the client)
        }
    }
}
