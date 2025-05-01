# E-Commerce API - ASP.NET Core Web API 9.0

A modular and scalable **E-Commerce API** built with **ASP.NET Core 9.0**.  
This project provides full backend functionality for users, sellers, products, shopping carts, payments, reviews, and orders.

---

## Features

### Authentication & Users
- JWT-based login and registration
- Role-based access (User / Seller / Admin)
- Email confirmation and reset password
- Profile update and password change

### Product Management
- CRUD operations for products and categories
- Product filtering by category/brand
- Image handling and stock management

### Shopping Cart & Checkout
- Add/update/remove items in cart
- Cart retrieval by user
- Convert cart into order on checkout

### Orders & Shipping
- Place new orders from cart
- Track order status (Pending → Shipped → Delivered)
- Shipping details handling

### Payments
- Payment processing with status tracking
- Refund support
- Payment history per order

### Reviews
- Add reviews for purchased products
- Seller/product rating calculation
- CRUD for reviews

### Seller Panel
- Seller-specific endpoints for managing products and profile
- View sales and orders

---

## Project Structure

```
ECommerce/
├── ECommerce.API/                  # API Project (entry point)
│   ├── Controllers/                # All API controllers
│   │   ├── AdminAccountController.cs
│   │   ├── AuthController.cs
│   │   ├── CartController.cs
│   │   ├── CategorieController.cs
│   │   ├── OrdersController.cs
│   │   ├── PaymentsController.cs
│   │   ├── ProductsController.cs
│   │   ├── ReviewController.cs
│   │   ├── SellersController.cs
│   │   └── UsersController.cs
│   ├── Properties/
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── ECommerce.API.csproj

├── ECommerce.Application/          # Application layer
│   ├── DTOs/                       # Data Transfer Objects
│   ├── Interfaces/
│   │   ├── Repositories/
│   │   │   ├── ICartRepository.cs
│   │   │   ├── ICategoriesRepository.cs
│   │   │   ├── IGenericRepository.cs
│   │   │   ├── IProductRepository.cs
│   │   │   └── IReviewRepository.cs
│   │   └── Services/
│   │       ├── ICartServices.cs
│   │       ├── ICategoriesService.cs
│   │       ├── IJwtService.cs
│   │       ├── IOrderService.cs
│   │       ├── IPaymentService.cs
│   │       ├── IProductService.cs
│   │       └── IReviewService.cs
│   ├── Mapping/                    # AutoMapper configs
│   └── ECommerce.Application.csproj

├── ECommerce.Domain/               # Domain models & logic
│   ├── Entities/
│   │   ├── Cart/                   # Cart.cs, CartItem.cs
│   │   ├── Orders/                 # Order.cs, OrderItem.cs
│   │   ├── Payments/               # Payment.cs, Shipping.cs
│   │   ├── Identity/               # ApplicationUser.cs, Sellers.cs
│   │   ├── Common/                 # BaseEntity.cs
│   │   ├── Enums/                  # OrderStatus.cs
│   │   └── Others/                 # Category.cs, Product.cs, Review.cs
│   └── ECommerce.Domain.csproj

├── ECommerce.Persistence/          # Infrastructure & EF Core setup
│   ├── Config/                     # Seeding roles, data
│   │   ├── ApplicationDbSeedData.cs
│   │   └── RoleSeeder.cs
│   ├── Contexts/                   # ApplicationDbContext.cs
│   ├── Migrations/                 # EF Core migrations
│   ├── Repositories/               # Implementation of data access
│   ├── Services/                   # Implementation of business logic
│   ├── ServiceCollectionExtensions.cs   # DI setup
│   └── ECommerce.Persistence.csproj

├── ECommerce.Tests/                # Unit/Integration tests (WIP)
├── ECommerce.sln                  # Visual Studio solution
├── .gitignore
└── .gitattributes

```

---

## Technologies Used

- ASP.NET Core 9.0  
- Entity Framework Core  
- SQL Server  
- AutoMapper  
- JWT Authentication  
- RESTful API Principles  
- Swagger (Swashbuckle)

---

## Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/yourusername/ECommerce.git
cd ECommerce
```
2. Apply Migrations & Seed Data
```
dotnet ef database update
```
3. Run the API
```
dotnet run
```
4. Open Swagger
```
https://localhost:5001/swagger/index.html
```
