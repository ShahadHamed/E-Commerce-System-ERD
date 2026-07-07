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


        //7.Cancel an Order


        //Main method to run the application
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
                Console.WriteLine();
                Console.WriteLine(" 0  - Exit");
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