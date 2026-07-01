<!-- Last updated: 2026-07-01 -->
<!-- Last change: Marked Step 4 (Repository layer) complete: IOrderRepository, IPizzaRepository, IEmployeeRepository, and lookup repositories (Size, CheeseOption, SauceOption, Topping) implemented as async methods against ShepherdsPiesDbContext, registered in Program.cs DI container -->

# Shepherd's Pies - Implementation Roadmap

Generated from: dev-docs/PRD.md (with dev-docs/ARCHITECTURE.md)

Timeline note: submission is due Thursday, July 2, 2026, 4:00 PM.

Workflow note: Steps 1-5 build the shared backend foundation (models, repositories, DTOs) that every endpoint depends on, so they're sequenced straight through. From Step 6 on, each step is a vertical slice: build the controller and its endpoint(s), verify via Swagger/Postman, then immediately build the related React component against it before moving to the next step. This matches working in small, testable increments rather than finishing the whole API before touching the frontend.

## Steps

- [x] **Step 1: Planning artifacts**
  Produce the required pre-coding deliverables: ERD (already drafted in ARCHITECTURE.md, confirm it matches what you'll actually build), wireframes for login, order list, order detail/create, and pizza builder, and GitHub Issues for each user story. The GitHub Project board is already created; link each issue to it as you create it. Create one feature branch per issue as you start its work.

  **Acceptance Criteria**:
  - **Given** the ERD in ARCHITECTURE.md, **When** you review it against the PRD's entity list, **Then** Orders, Employees, Pizzas, Cheese Options, Sauce Options, Toppings, and the pizza-topping join entity are all represented.
  - **Given** the core API functionality list in the PRD, **When** you create GitHub Issues, **Then** each of the 6 API endpoints and each of the 4 frontend views has a corresponding issue, added to the existing Project board.

- [x] **Step 2: Solution scaffolding and database connection**
  Create the ASP.NET Core Web API project and the EF Core + Npgsql setup: connection string, `DbContext` registration, and a working `dotnet ef database update` against a local PostgreSQL instance with no tables yet (a smoke test that the pipeline works before modeling real data).

  **Acceptance Criteria**:
  - **Given** a local PostgreSQL instance, **When** you run an initial empty migration, **Then** it applies without error and the target database exists.

- [x] **Step 3: Domain models, migrations, and seed data**
  Build out the EF Core models for `Size`, `CheeseOption`, `SauceOption`, `Topping`, `Order`, `Pizza`, `PizzaTopping`, and the `Employee` class extending `IdentityUser`. Configure the two foreign keys from `Order` to `Employee` (order-taker and delivery employee) since both point at the same table. Add seed data for sizes ($10/$12/$15), cheese options, sauce options, and toppings ($0.50 each), and migrate.

  **Acceptance Criteria**:
  - **Given** the migrations have run, **When** you query the database directly, **Then** the lookup tables (Size, CheeseOption, SauceOption, Topping) are populated with the values specified in the PRD's business logic section.
  - **Given** the `Order` table, **When** you inspect its schema, **Then** it has two distinct nullable-where-appropriate foreign keys to `Employee` (order-taker required, delivery employee nullable).

- [x] **Step 4: Repository layer**
  Implement `IOrderRepository`, `IPizzaRepository`, `IEmployeeRepository`, and simple read repositories for the lookup tables, per the method signatures in ARCHITECTURE.md (e.g. `GetByDate`, `GetById` with pizzas included, `Add`, `Update`, `Cancel`). Each implementation takes the injected `DbContext`; no controller touches `DbContext` directly.

  **Acceptance Criteria**:
  - **Given** `IOrderRepository.GetByDate(today)`, **When** called against seeded test orders, **Then** it returns only orders placed on that date, newest first.
  - **Given** `IOrderRepository.GetById(id)`, **When** called, **Then** the returned order includes its associated pizzas.

- [ ] **Step 5: DTOs and AutoMapper profiles**
  Define request and response DTOs for Order, Pizza, and the lookup entities, with data annotations for validation. Configure AutoMapper profiles mapping EF models to response DTOs and request DTOs to EF models. Compute order/pizza totals as a mapped property on the response DTO rather than a stored column.

  **Acceptance Criteria**:
  - **Given** a `Pizza` with a size, cheese, sauce, and three toppings, **When** it's mapped to its response DTO, **Then** the computed price equals size price + (toppings count * $0.50).
  - **Given** an `OrderCreateDto` missing a required field, **When** model binding runs, **Then** validation fails before reaching repository code.

- [ ] **Step 6: Auth backend + React shell + Login page**
  Backend: wire up ASP.NET Core Identity with cookie auth (`POST /api/login`, `POST /api/logout`, `GET /api/login/profile`), and seed the `Employee` and `Manager` roles plus at least one account in each role. Verify all three endpoints via Swagger/Postman before moving on. Frontend: scaffold the React app, install `react-bootstrap` and Bootstrap's CSS, add a `fetch` wrapper that sends `credentials: include` on every request, and build the Login page using React Bootstrap form components. On successful login, store the current employee/role (e.g. component state lifted to the app root) so other views can read it.

  **Acceptance Criteria**:
  - **Given** valid credentials, **When** you POST to `/api/login` via Swagger/Postman, **Then** an auth cookie is set and `GET /api/login/profile` returns the employee's name and role.
  - **Given** valid credentials entered on the Login page, **When** submitted, **Then** the app navigates past login and the auth cookie persists across a page refresh.
  - **Given** invalid credentials, **When** submitted via the Login page, **Then** it shows an error and does not navigate away.

- [ ] **Step 7: Orders backend + Order List & Detail/Create views**
  Backend: implement `POST /api/orders` (dine-in with table number, or delivery), `GET /api/orders?date=yyyy-MM-dd` (defaults to today, newest first), `GET /api/orders/{id}` (includes its pizzas), and `PUT /api/orders/{id}` (assign/update the delivery employee on a delivery order), wired through `IOrderRepository` and `IMapper`. Verify via Swagger/Postman. Frontend: build the Order List view (filtered by day, defaults to today, newest first) and the Order Detail/Create view (start a new dine-in or delivery order, view an existing order's pizzas and total, and assign a delivery employee on delivery orders).

  **Acceptance Criteria**:
  - **Given** no `date` query parameter, **When** you call `GET /api/orders`, **Then** it returns today's orders, newest first.
  - **Given** a dine-in order missing a table number, **When** you call `POST /api/orders`, **Then** the API returns 400.
  - **Given** a delivery order with no delivery employee assigned, **When** you call `PUT /api/orders/{id}` with a `DeliveryEmployeeId`, **Then** the order's delivery employee is updated and a subsequent `GET` reflects it.
  - **Given** the Order List view loads, **When** no date filter is changed, **Then** it shows today's orders newest first, matching the API default.
  - **Given** a new dine-in order is created through the UI, **When** the form is submitted, **Then** the new order appears in the Order List and its detail view shows a table number and no delivery surcharge.

- [ ] **Step 8: Reference data + Pizza endpoints + Pizza Builder view**
  Backend: build `SizesController`, `CheeseOptionsController`, `SauceOptionsController`, and `ToppingsController` (thin read-only `GET` endpoints), then `POST /api/orders/{id}/pizzas`, `PUT /api/pizzas/{id}`, and `DELETE /api/pizzas/{id}`. Manually verify pricing across combinations: each size, each topping count, and a delivery order's $5 surcharge. Frontend: build the Pizza Builder view (size, cheese, sauce, toppings, populated from the reference data endpoints) wired to add/update pizzas on an order.

  **Acceptance Criteria**:
  - **Given** a GET request to `/api/sizes` (and the other three lookup routes), **When** called by an authenticated user, **Then** it returns 200 with the seeded lookup values.
  - **Given** a Large pizza with extra cheese and pepperoni, **When** you GET the order detail, **Then** the pizza price is $15.00 + $0.50 + $0.50 = $16.00.
  - **Given** a delivery order with one Medium pizza and no toppings, **When** you GET the order detail, **Then** the order total is $12.00 + $5.00 = $17.00.
  - **Given** the Pizza Builder is used to add a Large pizza with two toppings, **When** saved, **Then** the order detail view's total updates to reflect the new pizza's price.

- [ ] **Step 9: Cancel endpoint + Cancel UI action**
  Backend: implement `DELETE /api/orders/{id}` (cancel, sets `IsCancelled`) restricted to the `Manager` role via `[Authorize(Roles = "Manager")]`. Verify with both an Employee and a Manager account via Swagger/Postman. Frontend: add the cancel-order action to the Order Detail view, visible only when the logged-in user is a Manager.

  **Acceptance Criteria**:
  - **Given** an authenticated Employee (non-Manager), **When** they call `DELETE /api/orders/{id}`, **Then** the API returns 403.
  - **Given** an authenticated Manager, **When** they call `DELETE /api/orders/{id}`, **Then** the order's `IsCancelled` flag is set to true and a subsequent `GET` reflects the cancellation.
  - **Given** a logged-in Employee (non-Manager) viewing an order, **When** the page renders, **Then** no cancel button is shown.
  - **Given** a logged-in Manager viewing an order, **When** they click cancel, **Then** the order is removed from the active Order List.

- [ ] **Step 10: End-to-end verification against success criteria**
  Walk through every success criterion in the PRD: all 6 endpoints via Swagger/Postman and through the React client, login required end to end, at least one role-restricted action, pricing correct across combinations, and Book 4 patterns (constructor DI, attribute routing, DTOs, data annotation validation) followed throughout. Fix anything that fails before submission.

  **Acceptance Criteria**:
  - **Given** a full walkthrough of create → list → detail → update pizza → update order → cancel, **When** performed through the React client, **Then** every step succeeds with no console or server errors.
  - **Given** the PRD's Success Criteria section, **When** checked off one by one, **Then** every item is satisfied before the Thursday, July 2, 2026, 4:00 PM deadline.