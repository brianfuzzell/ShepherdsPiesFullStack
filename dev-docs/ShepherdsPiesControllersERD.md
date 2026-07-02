<!-- Last updated: 2026-06-30 -->
<!-- Last change: DBML export of the ERD in ARCHITECTURE.md, for use with dbdiagram.io -->

# Shepherd's Pies - ERD (DBML)

DBML translation of the data model in dev-docs/ARCHITECTURE.md. Paste the block below
into https://dbdiagram.io/ to render/export a diagram image. ARCHITECTURE.md's Mermaid
diagram remains the source of truth, if the data model changes, update there first and
re-sync this file.

```dbml
Table Employee {
  Id varchar [pk]
  FirstName varchar
  LastName varchar
  UserName varchar
  Email varchar
}

Table Order {
  Id int [pk, increment]
  OrderType varchar [note: 'DineIn or Delivery']
  TableNumber int [note: 'nullable, DineIn only']
  EmployeeId varchar [ref: > Employee.Id, note: 'order taker, required']
  DeliveryEmployeeId varchar [ref: > Employee.Id, note: 'nullable, Delivery only']
  OrderDate datetime
  IsCancelled boolean [note: 'soft delete flag']
}

Table Pizza {
  Id int [pk, increment]
  OrderId int [ref: > Order.Id]
  SizeId int [ref: > Size.Id]
  CheeseOptionId int [ref: > CheeseOption.Id]
  SauceOptionId int [ref: > SauceOption.Id]
}

Table Size {
  Id int [pk, increment]
  Name varchar [note: 'Small, Medium, Large']
  Price decimal [note: '10.00, 12.00, 15.00']
}

Table CheeseOption {
  Id int [pk, increment]
  Name varchar [note: 'Buffalo Mozzarella, Four Cheese, Vegan, None']
}

Table SauceOption {
  Id int [pk, increment]
  Name varchar [note: 'Marinara, Arrabbiata, Garlic White, None']
}

Table Topping {
  Id int [pk, increment]
  Name varchar
  Price decimal [note: '0.50 each']
}

Table PizzaTopping {
  Id int [pk, increment]
  PizzaId int [ref: > Pizza.Id]
  ToppingId int [ref: > Topping.Id]
}
```