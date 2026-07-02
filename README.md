# Shepherd's Pies - full-stack project
### NSS Book 4 - Building More Complex APIs with C# Controllers

NOTE: this project shares the name and description with an earlier client-side project I built. However, I had to build this latest full-stack project from scratch without using the code from the earlier project.

### Agentic Development
Nashville Software School provides Agentic Development training and supports using an AI development framework, [Tandem](https://github.com/Valerie-Freeman/tandem) paired with Claude Code, on select projects. This project was built using Tandem and Claude Code, but I own the decisions and code.

## Project Description
Giuseppe Shepherd learned how to make the perfect pizza as a child from his nonna. Since then it's been his dream to open his own restaurant, but he needs help creating the order management system for his new business.

### Orders
Giuseppe's (Joe, to his friends) restaurant has a dining room and delivery service available, and an order can either be placed for a particular table number or for delivery. Each order at the restaurant can have multiple pizzas on it.

Each order can potentially have two employees assigned to it for different purposes - Joe can tell if an order is for delivery if an employee has been assigned as the deliverer for that order. Orders are also _always_ associated with the employee that took the order (at a table or over the phone).  

Joe needs to see what the total cost for the order will be based on the total cost of all of the pizzas on that order. He also needs to see if the customer left a tip. Joe's restaurant is located in a magical place with no sales tax. For record-keeping purposes, Joe also needs to know the date and time an order was placed.    

### Pizza
Eventually there will be other items available, but for the restaurant opening Joe is going to only serve pizzas. The pizzas come in three sizes - small, medium, and large. Each pizza on an order can have a cheese type, a sauce type, and then any number of toppings chosen from a list on the menu.
