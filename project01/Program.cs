using Microsoft.EntityFrameworkCore;
using project01.Models;

namespace project01
{
    public class Program
    {

        public static ECommerceContext context = new ECommerceContext();


        //1.Register a New User--------------------------------------------
        public static void RegisterUser()
        {
            Console.WriteLine("\n**--Registering a new user...");

            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter email: ");
            string email = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            string passwordHash = password; // hash the password here

            Console.Write("Enter full name: ");
            string fullName = Console.ReadLine();

            Console.Write("Enter phone number: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Enter address: ");
            string address = Console.ReadLine();

            // Create user
            context.Users.Add(new User
            {
                Name = username,
                email = email,
                passwordHash = password,
                fullName = fullName,
                phoneNumber = phoneNumber,
                address = address,
                registrationDate = DateTime.Now,
                isActive = true
            });

            // Save changes to the database
            context.SaveChanges();

            // Get the last saved user
            User saved = context.Users.OrderBy(c => c.userId).Last();

            // Display confirmation
            Console.WriteLine($"User registered successfully! Assigned UserID: {saved.userId}");
        }


        //2.Add a New Category----------------------------------------
        public static void NewCategory()
        {
            Console.WriteLine("\n**--Adding a new category...");

            Console.Write("Enter Category Name: ");
            string categoryName = Console.ReadLine();

            Console.Write("Enter Category Description: ");
            string description = Console.ReadLine();

            Console.Write("Enter Image URL: ");
            string imageUrl = Console.ReadLine();

            // Create category
            context.Categories.Add(new Category
            {
                categoryName = categoryName,
                description = description,
                imageUrl = imageUrl
            });

            // Save changes to the database
            context.SaveChanges();

            // Get the last saved category
            Category saved = context.Categories.OrderBy(c => c.categoryId).Last();

            // Display confirmation
            Console.WriteLine($"Category added successfully! Assigned CategoryID: {saved.categoryId}");
        }


        //3.Add a New Product to a Category----------------------------------------
        public static void NewProduct()
        {
            Console.WriteLine("\n**--Adding a new product to a category...");

            // Display all categories
            List<Category> categories = context.Categories.ToList();

            Console.WriteLine("Available Categories:");
            foreach (Category c in categories)
            {
                Console.WriteLine($"{c.categoryId} - {c.categoryName}");
            }

            // Read category selection
            Console.Write("\nEnter Category ID: ");
            int categoryId = int.Parse(Console.ReadLine());

            // Check category exists
            Category category = context.Categories.FirstOrDefault(c => c.categoryId == categoryId);

            if (category == null)
            {
                Console.WriteLine("Category not found!");
                return;
            }

            // Read product details
            Console.Write("Enter Product Name: ");
            string productName = Console.ReadLine();

            Console.Write("Enter Product Description: ");
            string description = Console.ReadLine();

            Console.Write("Enter Product Price: ");
            double price = double.Parse(Console.ReadLine());

            Console.Write("Enter Stock Quantity: ");
            int stockQuantity = int.Parse(Console.ReadLine());

            Console.Write("Enter Image URL: ");
            string imageUrl = Console.ReadLine();


            // Create product
            context.Products.Add(new Product
            {
                productName = productName,
                description = description,
                price = price,
                stockQuantity = stockQuantity,
                imageUrl = imageUrl,
                categoryId = categoryId,
                createdAt = DateTime.Now,
                isAvailable = true
            });

            // Save to database
            context.SaveChanges();

            // Get the last saved product
            Product saved = context.Products.OrderBy(p => p.productId).Last();

            // Display confirmation
            Console.WriteLine($"Product added successfully! Assigned ProductID: {saved.productId}");
        }


        //4.Place an Order for a Product----------------------------------------
        public static void PlaceOrder()
        {
            Console.WriteLine("\n**--Placing an order for a product...");

            Console.WriteLine("Users:");
            foreach (User u in context.Users)
                Console.WriteLine($"  ID: {u.userId}  |  {u.Name}");

            Console.Write("Enter User ID: ");
            int userId = int.Parse(Console.ReadLine());

            User user = context.Users.FirstOrDefault(u => u.userId == userId);
            if (user == null)
            {
                Console.WriteLine("User not found!");
                return;
            }

            Console.Write("Enter shipping Address: ");
            string shippingAddress = Console.ReadLine();

            Console.Write("Enter payment Method(CreditCard / Cash): ");
            string paymentMethod = Console.ReadLine();

            // Create order
            context.Orders.Add(new Order
            {
                userId = userId,
                shippingAddress = shippingAddress,
                paymentMethod = paymentMethod,
                orderDate = DateTime.Now,
                status = "Pending",
                totalAmount = 0
            });

            // Save changes to the database
            context.SaveChanges();

            // Get the last saved order
            Order saved = context.Orders.OrderBy(o => o.orderId).Last();

            // Display confirmation
            Console.WriteLine($"Order placed successfully! Assigned OrderID: {saved.orderId}");

            // Add products to the order
            bool addItems = true;
            while (addItems)
            {
                Console.WriteLine("\nAvailable products:");

                foreach (Product p in context.Products.Where(p => p.isAvailable && p.stockQuantity > 0).ToList())
                    Console.WriteLine($"  ID: {p.productId}  |  {p.productName}  |  {p.price:C}  |  Stock: {p.stockQuantity}");

                Console.Write("Enter product ID to add (0 to finish): ");
                int productId = int.Parse(Console.ReadLine());

                //if user enters 0, exit the loop
                if (productId == 0) 
                    break; 

                Product product = context.Products.FirstOrDefault(p => p.productId == productId);

                if (product == null)
                {
                    Console.WriteLine("Product not found!");
                    continue;
                }

                // Read quantity
                Console.Write("Enter Quantity: ");
                int quantity = int.Parse(Console.ReadLine());

                if (quantity <= 0)
                {
                    Console.WriteLine("Invalid quantity!");
                    continue;
                }

                if (product.stockQuantity < quantity)
                {
                    Console.WriteLine("Not enough stock!");
                    continue;
                }

                // Create OrderItem
                context.OrderItems.Add(new OrderItem
                {
                    orderId = saved.orderId,
                    productId = productId,
                    quantity = quantity,
                    unitPrice = (decimal)product.price
                });

                // Calculate total amount
                saved.totalAmount += (decimal)product.price * quantity;

                // Reduce stock quantity
                product.stockQuantity -= quantity;


                Console.WriteLine("Product added to order successfully!");


                Console.Write("\nAdd another product? (y/n): ");
                string answer = Console.ReadLine();

                if (answer.ToLower() != "y")
                    break;
            }

            // Save OrderItems, updated stock and total amount
            context.SaveChanges();

            // Display final order summary
            Console.WriteLine("\nOrder completed successfully!");
            Console.WriteLine($"Order ID: {saved.orderId}");
            Console.WriteLine($"Total Amount: {saved.totalAmount}");
        }


        //5.Write a Product Review----------------------------------------
        public static void WriteReview()
        {
            Console.WriteLine("\n**--Writing a review for a product...");

            // Display all users
            Console.WriteLine("Users:");
            foreach (User u in context.Users)
                Console.WriteLine($"  ID: {u.userId}  |  {u.Name}");

            Console.Write("Enter User ID: ");
            int userId = int.Parse(Console.ReadLine());

            User user = context.Users.FirstOrDefault(u => u.userId == userId);

            if (user == null)
            {
                Console.WriteLine("User not found!");
                return;
            }

            // Display all available products
            Console.WriteLine("\nAvailable products:");

            foreach (Product p in context.Products.Where(p => p.isAvailable).ToList())
                Console.WriteLine($"  ID: {p.productId}  |  {p.productName}");

            Console.Write("Enter Product ID to review: ");
            int productId = int.Parse(Console.ReadLine());

            Product product = context.Products.FirstOrDefault(p => p.productId == productId);

            if (product == null)
            {
                Console.WriteLine("Product not found!");
                return;
            }

            // Read review details
            Console.Write("Enter Rating (1-5): ");
            int rating = int.Parse(Console.ReadLine());

            if (rating < 1 || rating > 5)
            {
                Console.WriteLine("Invalid rating!");
                return;
            }

            Console.Write("Enter Comment: ");
            string comment = Console.ReadLine();

            // Create review
            context.Reviews.Add(new Review
            {
                userId = userId,
                productId = productId,
                rating = rating,
                comment = comment,
                reviewDate = DateTime.Now
            });

            // Save changes to the database
            context.SaveChanges();

            Review saved = context.Reviews.OrderBy(r => r.reviewId).Last();

            // Display confirmation
            Console.WriteLine($"Review submitted successfully! Review ID: {saved.reviewId}");
        }


        //6.Update Product Price and Availability----------------------------------------
        public static void UpdateProduct()
        {
            Console.WriteLine("\n**--Updating product price and availability...");
           
            // Display all products
            Console.WriteLine("Available products:");

            foreach (Product p in context.Products)
                Console.WriteLine($"  ID: {p.productId}  |  {p.productName}  |  Price: {p.price:C}  |  Available: {p.isAvailable}");

            Console.Write("Enter Product ID to update: ");
            int productId = int.Parse(Console.ReadLine());

            Product product = context.Products.FirstOrDefault(p => p.productId == productId);

            if (product == null)
            {
                Console.WriteLine("Product not found!");
                return;
            }

            // Read new price
            Console.Write("Enter new Price: ");
            double newPrice = double.Parse(Console.ReadLine());

            // Read availability
            Console.Write("Is the product available? (y/n): ");
            string availabilityInput = Console.ReadLine();

            bool isAvailable = availabilityInput.ToLower() == "y";

            // Update product
            product.price = newPrice;
            product.isAvailable = isAvailable;

            // Save changes to the database
            context.SaveChanges();

            // Display confirmation
            Console.WriteLine($"Product updated successfully! New Price: {product.price:C}, Available: {product.isAvailable}");
        }


        //7.Cancel an Order----------------------------------------     
        public static void CancelOrder()
        {
            Console.WriteLine("\n**--Cancelling an order...");

            // Display all orders
            Console.WriteLine("Orders:");

            foreach (Order o in context.Orders)
                Console.WriteLine($"  ID: {o.orderId}  |  User ID: {o.userId}  |  Status: {o.status}");

            Console.Write("Enter Order ID to cancel: ");
            int orderId = int.Parse(Console.ReadLine());

            // Find the order
            Order order = context.Orders.FirstOrDefault(o => o.orderId == orderId);
            
            if (order == null)
            {
                Console.WriteLine("Order not found!");
                return;
            }

            // Check if already cancelled
            if (order.status == "Cancelled")
            {
                Console.WriteLine("This order is already cancelled.");
                return;
            }

            // Load all OrderItems for this order
            List<OrderItem> orderItems = context.OrderItems
                                                .Where(i => i.orderId == orderId)
                                                .ToList();

            // Restore stock quantity
            foreach (OrderItem item in orderItems)
            {
                // find the related product
                Product product = context.Products.FirstOrDefault(p => p.productId == item.productId);
                
                // Restore the stock quantity
                if (product != null)
                {
                    product.stockQuantity += item.quantity;
                }
            }

            // Update order status
            order.status = "Cancelled";

            // Save changes to the database
            context.SaveChanges();

            // Display confirmation
            Console.WriteLine($"Order cancelled successfully! Order ID: {order.orderId}, New Status: {order.status}");
        }


        //8.Delete a Review----------------------------------------     
        public static void DeleteReview()
        {
            Console.WriteLine("\n**--Deleting a review...");

            // Display all reviews
            Console.WriteLine("Reviews:");

            foreach (Review r in context.Reviews)
                Console.WriteLine($"  ID: {r.reviewId}  |  User ID: {r.userId}  |  Product ID: {r.productId}  |  Rating: {r.rating}  |  Comment: {r.comment}");

            Console.Write("Enter Review ID to delete: ");
            int reviewId = int.Parse(Console.ReadLine());

            // Find the review
            Review review = context.Reviews.FirstOrDefault(r => r.reviewId == reviewId);

            if (review == null)
            {
                Console.WriteLine("Review not found!");
                return;
            }

            // Delete the review
            context.Reviews.Remove(review);

            // Save changes to the database
            context.SaveChanges();

            // Display confirmation
            Console.WriteLine($"Review deleted successfully! Review ID: {review.reviewId}");
        }


        //9. View All Products (Get All)-----------------------------------------
        public static void ViewAllProduct()
        {
            Console.WriteLine("\n**---View All Products");

            // Get all products
            List<Product> products = context.Products.ToList();

            if (products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }

            foreach (Product product in products)
            {
                Console.WriteLine("------------------------------------");
                Console.WriteLine($"Product ID : {product.productId}");
                Console.WriteLine($"Name       : {product.productName}");
                Console.WriteLine($"Price      : {product.price}");
                Console.WriteLine($"Stock      : {product.stockQuantity}");
                Console.WriteLine($"Available  : {product.isAvailable}");
            }

            Console.WriteLine("------------------------------------");
        }


        //10. Filter Products by Category and Price Range-----------------------------------------
        public static void FilterProductsByCategoryAndPrice()
        {
            Console.WriteLine("\n**---Filter Products by Category and Price Range");
            
            // Display categories
            Console.WriteLine("Categories:");

            foreach (Category c in context.Categories)
            {
                Console.WriteLine($"ID: {c.categoryId} | {c.categoryName}");
            }

            // Read filter values
            Console.Write("\nEnter Category ID: ");
            int categoryId = int.Parse(Console.ReadLine());

            Console.Write("Enter Minimum Price: ");
            double minPrice = double.Parse(Console.ReadLine());

            Console.Write("Enter Maximum Price: ");
            double maxPrice = double.Parse(Console.ReadLine());

            // Filter and sort
            List<Product> products = context.Products
                .Where(p => p.categoryId == categoryId
                         && p.price >= minPrice
                         && p.price <= maxPrice)
                .OrderBy(p => p.price)
                .ToList();

            if (products.Count == 0)
            {
                Console.WriteLine("No products found.");
                return;
            }

            // Display results
            Console.WriteLine("\nFiltered Products:");

            foreach (Product product in products)
            {
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"Product ID : {product.productId}");
                Console.WriteLine($"Name       : {product.productName}");
                Console.WriteLine($"Price      : {product.price}");
                Console.WriteLine($"Stock      : {product.stockQuantity}");
                Console.WriteLine($"Available  : {product.isAvailable}");
            }

            Console.WriteLine("--------------------------------");
        }


        //11.Get Category with All Its Products (Include)-----------------------------------------
        public static void GetCategoryWithProducts()
        {
            Console.WriteLine("\n**---Get Category with All Its Products");
            
            // Display categories
            Console.WriteLine("Categories:");

            foreach (Category c in context.Categories)
            {
                Console.WriteLine($"ID: {c.categoryId} | {c.categoryName}");
            }

            // Read category ID
            Console.Write("\nEnter Category ID: ");
            int categoryId = int.Parse(Console.ReadLine());

            // Get category with products
            Category category = context.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.categoryId == categoryId);

            if (category == null)
            {
                Console.WriteLine("Category not found.");
                return;
            }

            // Display category and its products
            Console.WriteLine("\nCategory Details");
            Console.WriteLine($"\nCategory: {category.categoryName}");
            Console.WriteLine($"Description: {category.description}");

            // Display products
            Console.WriteLine("Products:");

            if (category.Products.Count == 0)
            {
                Console.WriteLine("No products in this category.");
                return;
            }

            foreach (Product product in category.Products)
            {
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"Product ID : {product.productId}");
                Console.WriteLine($"Name       : {product.productName}");
                Console.WriteLine($"Price      : {product.price}");
                Console.WriteLine($"Stock      : {product.stockQuantity}");
                Console.WriteLine($"Available  : {product.isAvailable}");
            }

            Console.WriteLine("--------------------------------");
        }

        //12.  View Order History with Full Details(ThenInclude)-----------------------------------------
        public static void ViewOrderHistory()
        {
            Console.WriteLine("\n**---View Order History with Full Details");
            
            // Display users
            Console.WriteLine("Users:");

            foreach (User u in context.Users)
            {
                Console.WriteLine($"ID: {u.userId} | {u.Name}");
            }

            // Read user ID
            Console.Write("\nEnter User ID: ");
            int userId = int.Parse(Console.ReadLine());
           
            // Get orders with details
            List<Order> orders = context.Orders
                .Where(o => o.userId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToList();

            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found for this user.");
                return;
            }

            // Display orders and their items
            foreach (Order order in orders)
            {
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"Order ID: {order.orderId}");
                Console.WriteLine($"Order Date: {order.orderDate}");
                Console.WriteLine($"Status: {order.status}");
                Console.WriteLine($"Total Amount: {order.totalAmount}");

                Console.WriteLine("Order Items:");
                foreach (OrderItem item in order.OrderItems)
                {
                    Console.WriteLine($"  Product: {item.Product.productName} | Quantity: {item.quantity} | Unit Price: {item.unitPrice}");
                }
            }
            Console.WriteLine("--------------------------------");
        }

        //13.ProductSummaryReport-----------------------------------------
        public static void ProductSummaryReport()
        {
            Console.WriteLine("\n**--Product Summary Report...");

            // Get product summary report
            var report = context.Products
                .Select(p => new
                {
                    ProductName = p.productName,
                    CategoryName = p.category.categoryName,
                    ReviewCount = p.Reviews.Count(),
                    AvgRating = p.Reviews.Any() ? p.Reviews.Average(r => r.rating) : 0,
                    Stock = p.stockQuantity
                })
                .ToList();

            Console.WriteLine("\nProduct Summary Report");

            foreach (var item in report)
            {
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"Product Name : {item.ProductName}");
                Console.WriteLine($"Category     : {item.CategoryName}");
                Console.WriteLine($"Reviews      : {item.ReviewCount}");
                Console.WriteLine($"Avg Rating   : {item.AvgRating:F2}");
                Console.WriteLine($"Stock        : {item.Stock}");
            }
        }


        //Main method to run the application------------------------------------
        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n========================================");
                Console.WriteLine("        E-Commerce System");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Register User");
                Console.WriteLine("2. Add Category");
                Console.WriteLine("3. Add Product");
                Console.WriteLine("4. Place Order");
                Console.WriteLine("5. WriteReview");
                Console.WriteLine("6. Update Product");
                Console.WriteLine("7. Cancel Order");
                Console.WriteLine("8. Delete Review");
                Console.WriteLine("9. View All Products");
                Console.WriteLine("10. Filter Products by Category and Price Range");
                Console.WriteLine("11. Get Category with All Its Products");
                Console.WriteLine("12. View Order History with Full Details");
                Console.WriteLine("13. Product Summary Report");
                Console.WriteLine("0. Exit");
                Console.WriteLine("========================================");
                Console.Write("Select option: ");

                int option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        RegisterUser();
                        break;

                    case 2:
                        NewCategory();
                        break;

                    case 3:
                        NewProduct();
                        break;

                    case 4:
                        PlaceOrder();
                        break;

                    case 5:
                        WriteReview();
                        break;

                    case 6:
                        UpdateProduct();
                        break;

                    case 7:
                        CancelOrder();
                        break;

                    case 8:
                        DeleteReview();
                        break;

                    case 9:
                        ViewAllProduct();
                        break;

                     case 10:
                        FilterProductsByCategoryAndPrice();
                        break;

                     case 11:
                        GetCategoryWithProducts();
                        break;

                     case 12:
                        ViewOrderHistory();
                        break;

                     case 13:
                        ProductSummaryReport();
                        break;

                     case 0:
                        exit = true;
                        break;

                     default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            Console.WriteLine("Goodbye!");
        }
    }
}