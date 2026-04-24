var builder = DistributedApplication.CreateBuilder(args);

// Projects.OrderService 
var orderService = builder.AddProject<Projects.OrderService>("OrderService");
var userService = builder.AddProject<Projects.UserService>("UserService");
var productService = builder.AddProject<Projects.ProductService>("ProductService");



builder.Build().Run();
