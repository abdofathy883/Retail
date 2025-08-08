# Retail – Backend API for E-Commerce & POS Platform

Retail is a modular ASP.NET Core backend that powers a unified e-commerce storefront and point-of-sale (POS) admin system. It provides role-based APIs for managing products, vendors, categories, customer orders, and cashier-placed transactions.

---

## 🧠 Project Overview

This backend serves two Angular frontends:
- A public-facing **e-commerce UI** for customers
- An internal **admin/POS dashboard** for staff

The system handles:
- Product management with variants (color, size, etc.)
- Order and invoice tracking
- Secure authentication with role-based policies
- Real-time POS order submission (manual input)

---

## 🚀 Key Features

- **JWT Authentication**: Secure login for admin, cashier, and customer roles
- **Product Management**: Full CRUD for categories, products, variants, and vendors
- **E-Commerce Flow**: Cart, wishlist, order placement, and basic payment simulation
- **POS Module**: API for cashier-created orders (terminal input; no hardware integration yet)
- **Invoicing**: Auto-generated invoices tied to completed orders
- **Image Handling**: Upload service with validation and automatic `.webp` conversion
- **AutoMapper Integration**: Clean mapping between DTOs and domain models
- **Role-Based Routing**: All routes protected using policies and middleware

---

## 🛠️ Technology Stack

| Area               | Technology               |
|--------------------|--------------------------|
| Language           | C#                       |
| Framework          | ASP.NET Core 8.0         |
| ORM                | Entity Framework Core    |
| Auth               | ASP.NET Identity + JWT   |
| Mapping            | AutoMapper               |
| Database           | SQL Server               |
| Image Processing   | ImageSharp + WebP        |

---

## 🔐 Roles & Permissions

| Role          | Permissions                                   |
|---------------|-----------------------------------------------|
| SuperAdmin    | Full access to all operations, non-deletable  |
| Admin         | Partial access to all operations              |
| Manager       | Manages POS module & Cachiers                 |
| Cachier       | POS module: create orders                     |
| Customer      | Browse, add to cart, place orders             |

---

## 🔗 Related Repositories

- [E-Commerce Frontend (StoreFront)](https://github.com/abdofathy883/StoreFront)
- [Admin & POS Frontend (BackStore)](https://github.com/abdofathy883/BackStore)

---

## 📄 License

This project is developed for prototyping purposes. Contact the maintainer for commercial use or extensions.
