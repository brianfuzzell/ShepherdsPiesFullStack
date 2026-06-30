<!-- Last updated: 2026-06-30 -->
<!-- Last change: Initial wireframes for the 4 required views (Step 1 planning artifact) -->

# Shepherd's Pies - Wireframes

Low-fidelity layout sketches for the four views required by the PRD. These are about
content and behavior, not visual polish: what's on screen, what a user can do, and
which API call each action maps to. See dev-docs/ARCHITECTURE.md for the API design
these are built against.

## Login

```
+-----------------------------------+
|          Shepherd's Pies          |
|                                    |
|  Username  [______________]       |
|  Password  [______________]       |
|                                    |
|            [ Log In ]             |
|                                    |
|  (error message area, hidden      |
|   until a failed login attempt)   |
+-----------------------------------+
```

- Submits to `POST /api/login`.
- On success, app stores the employee/role (from the response) and navigates to the
  Order List.
- On failure, shows an inline error and stays on the page (no navigation).

## Order List

```
+------------------------------------------------------------+
| Shepherd's Pies            Logged in as: {Name} ({Role})   |
+------------------------------------------------------------+
| Date: [ 2026-07-02 v ]                    [ + New Order ]  |
+------------------------------------------------------------+
| #  | Type     | Table/Delivery | Taken By | Total | View   |
|----|----------|----------------|----------|-------|--------|
| 12 | Dine-In  | Table 4        | J. Diaz  | 24.50 | [ > ]  |
| 11 | Delivery | -              | A. Patel | 17.00 | [ > ]  |
| 10 | Dine-In  | Table 2        | J. Diaz  | 32.00 | [ > ]  |
+------------------------------------------------------------+
```

- Loads from `GET /api/orders?date=yyyy-MM-dd`, defaulting to today, newest first.
- Changing the date re-queries the same endpoint with the new date.
- "View" opens Order Detail for that order's `id`. "New Order" opens Order
  Detail/Create in create mode.

## Order Detail / Create

```
+------------------------------------------------------------+
| Order #12                                [ Cancel Order ]  |  <- Manager only
+------------------------------------------------------------+
| Type:   (o) Dine-In    ( ) Delivery                        |
| Table #:        [ 4 ]              (Dine-In only)          |
| Delivery Employee: [ -- unassigned --   v ]  (Delivery only)|
| Taken by: J. Diaz                                           |
| Order Date: 2026-07-02 6:42 PM                              |
+------------------------------------------------------------+
| Pizzas                                      [ + Add Pizza ]|
|--------------------------------------------------------------|
| Large, Four Cheese, Marinara, +pepperoni, +olive    $16.00  [Edit] [Remove]
| Medium, Vegan, Garlic White                          $12.00  [Edit] [Remove]
+------------------------------------------------------------+
| Order Total:                                        $28.00 |
+------------------------------------------------------------+
```

- Create mode (no order yet): same layout, Pizzas list starts empty, "Cancel Order"
  is not shown (nothing to cancel yet), a `[ Create Order ]` button replaces it.
  Submits `POST /api/orders`.
- Detail mode (existing order): loads from `GET /api/orders/{id}`, which includes
  its pizzas. "Add Pizza" navigates to the Pizza Builder for this order.
  "Edit"/"Remove" on a pizza call `PUT /api/pizzas/{id}` / `DELETE /api/pizzas/{id}`.
- "Cancel Order" calls `DELETE /api/orders/{id}` and only renders when the logged-in
  user's role is Manager, the same role check the API enforces server-side.
- Order Total is not stored, it's the sum of the pizza prices shown, computed by the
  response DTO (see ARCHITECTURE.md's Key Technical Decisions).

## Pizza Builder

```
+------------------------------------------------------------+
| Build Pizza                            (for Order #12)     |
+------------------------------------------------------------+
| Size:    ( ) Small $10   ( ) Medium $12   (o) Large $15    |
| Cheese:  [ Four Cheese            v ]                       |
| Sauce:   [ Marinara                v ]                       |
| Toppings:                                                    |
|   [x] Pepperoni     [ ] Sausage      [ ] Mushroom            |
|   [x] Olive         [ ] Onion        [ ] Green Pepper        |
|   [ ] Basil         [ ] Extra Cheese                          |
+------------------------------------------------------------+
| Price: $15.00 + ($0.50 x 2 toppings) = $16.00               |
+------------------------------------------------------------+
|                  [ Cancel ]        [ Save Pizza ]            |
+------------------------------------------------------------+
```

- Size/Cheese/Sauce/Topping options are populated from
  `GET /api/sizes`, `/api/cheeseoptions`, `/api/sauceoptions`, `/api/toppings`.
- "Save Pizza" calls `POST /api/orders/{id}/pizzas` when adding a new pizza, or
  `PUT /api/pizzas/{id}` when editing one opened from the Order Detail view.
- The price preview is calculated client-side as the user picks options, but the
  authoritative total always comes from the server response DTO after save.
