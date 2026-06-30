<!-- Last updated: 2026-06-30 -->
<!-- Last change: Added React Bootstrap as the frontend styling choice -->

# Shepherd's Pies - Product Requirements Document

## Problem Statement

Giuseppe Shepherd runs a pizzeria that serves both dine-in and delivery customers, and needs an internal order management system. Employees currently have no system for tracking orders, the pizzas within each order, custom toppings, and which employee took or delivered an order. This project builds that system as an ASP.NET Core Web API with a React front-end, following the controller-based patterns introduced in Book 4 of the NSS curriculum.

This is an NSS bootcamp project: the primary purpose is demonstrating mastery of controller organization, authentication/authorization, interfaces, and server-side validation, not shipping a production system for an actual restaurant.

## Target Users

Pizzeria employees only. There is no customer-facing/public ordering portal. Two roles are assumed for this project:

- **Employee**: can create orders, view orders, update orders (add/remove pizzas, assign a delivery employee), and update pizzas (size, cheese, sauce, toppings).
- **Manager**: everything an Employee can do, plus cancel/delete orders.

This role split is a working assumption to satisfy the role-based access learning objective. Confirm against your assignment rubric/instructor guidance before building the Identity roles, and adjust here if the actual requirement differs.

## Core Requirements

### Planning Artifacts
- Entity Relationship Diagram covering Orders, Employees, Pizzas, Cheese Options, Sauce Options, Toppings (and the pizza-topping join entity)
- Wireframes for key views: login, order list (filtered by day), order detail/create, pizza builder
- User stories written as GitHub Issues
- GitHub Project board to track issue status
- A feature branch per user story/issue

### API Functionality
1. **Create an order** - dine-in (table number) or delivery, associated with the employee taking it
2. **View all orders** - filterable by day, defaults to today, ordered newest first
3. **View order detail** - includes the full list of pizzas on that order
4. **Update an order** - add/remove pizzas, assign a delivery employee
5. **Update a pizza** - change size, cheese, sauce, add/remove toppings
6. **Cancel/delete an order**

### Business Logic
- Pizza sizes: Small $10, Medium $12, Large $15
- Each pizza has exactly one cheese option (Buffalo Mozzarella, Four Cheese, Vegan, None) and one sauce option (Marinara, Arrabbiata, Garlic White, None)
- Each topping adds $0.50: sausage, pepperoni, mushroom, onion, green pepper, black olive, basil, extra cheese
- Delivery orders add a flat $5.00 surcharge
- No sales tax
- Order total = sum of pizza prices (+ delivery surcharge if applicable)

### Authentication & Authorization
- Employees log in via ASP.NET Core Identity
- At least one endpoint enforces role-based restriction (e.g., only a Manager can cancel an order)

### Validation
- Server-side validation via data annotations on models/DTOs (required fields, valid enum-style values for size/cheese/sauce, etc.)

## Technical Stack

- **Backend**: ASP.NET Core Web API (C#), controller-based (not Minimal API), following Book 4 patterns: constructor-injected `DbContext`, attribute routing, DTOs to shape responses, Mapping from Models to DTOs with AutoMapper
- **ORM / Database**: Entity Framework Core with the Npgsql provider, PostgreSQL
- **Auth**: ASP.NET Core Identity, cookie-based, with role-based authorization (`[Authorize(Roles = "...")]`)
- **Frontend**: React, built from scratch, calling the API via `fetch`, styled with React Bootstrap
- **Validation**: Data Annotations (`[Required]`, `[Range]`, etc.) on models/DTOs
- **Source control**: GitHub repository, feature branches, GitHub Issues, GitHub Project board

### Stack Decisions

- **PostgreSQL + Npgsql** chosen to match the NSS curriculum's standard database setup for this book.
- **Controller-based API** (not Minimal API) is the explicit focus of Book 4; this is a deliberate departure from earlier projects to practice that pattern.
- **Cookie-based ASP.NET Identity** matches the auth pattern used elsewhere in this curriculum track for employee-only internal tools.
- **React from scratch** rather than a provided starter, consistent with how earlier full-stack projects in the program were built.
- **React Bootstrap** chosen for styling so effort goes toward functional usability (per the PRD's priority order) rather than hand-rolled CSS, while still getting a clean, usable UI.

## Scope

### In Scope (v1)
- ERD, wireframes, GitHub Issues, and Project board completed before/alongside coding
- All 6 required API endpoints (create, list/filter, detail, update order, update pizza, cancel)
- Pricing and delivery surcharge logic
- ASP.NET Identity login with Employee/Manager roles and at least one role-protected endpoint
- React client covering: login, order list (filtered by day, newest first), order detail/create, pizza builder, cancel order
- Server-side validation on order/pizza inputs

### Out of Scope (future)
- Sales tax calculation
- Customer-facing self-order portal
- Payment processing
- Order status tracking beyond cancel (e.g., "in progress," "out for delivery")
- Ingredient/inventory management
- Reporting or analytics dashboards
- UI polish/styling beyond functional usability

### Priority Order (given the 2-day timeline)
If time runs short before the deadline, prioritize in this order: (1) correct API behavior for all 6 endpoints and pricing logic, (2) auth/role enforcement, (3) front-end completeness and polish. A functionally correct API with a rough front-end is preferable to a polished UI over a broken or incomplete API.

## Success Criteria
- All 6 required endpoints work correctly, verified via Postman/Swagger and through the React client
- ERD, wireframes, GitHub Issues, and Project board exist and reflect the build
- Login is required to use the app; at least one action is restricted by role
- Pricing logic (pizza cost, toppings, delivery surcharge, no tax) is correct for all combinations
- Code follows Book 4 patterns: controllers with constructor DI, attribute routing, DTOs, data annotation validation
- Submitted by **Thursday, July 2, 2026, 4:00 PM**

## Learning Goals

- Practice controller-based ASP.NET Core API design, as a contrast to earlier Minimal API projects
- Implement ASP.NET Core Identity with role-based authorization end to end
- Practice designing interfaces as an abstraction layer
- Practice server-side validation with data annotations
- Reinforce EF Core relationship modeling, including multiple foreign keys to the same table (an Order references an Employee both as order-taker and, optionally, as delivery employee)
- Practice a full Git workflow: planning artifacts before code, GitHub Issues, a Project board, and feature branches per story