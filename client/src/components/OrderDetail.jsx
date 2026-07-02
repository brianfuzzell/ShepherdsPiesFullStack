import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import Container from "react-bootstrap/Container";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import Alert from "react-bootstrap/Alert";
import Spinner from "react-bootstrap/Spinner";
import {
  cancelOrder,
  createOrder,
  getOrderById,
  updateOrder,
} from "../managers/ordersManager";
import { getEmployees } from "../managers/employeesManager";
import { deletePizza } from "../managers/pizzasManager";

export default function OrderDetail({ currentEmployee }) {
  const { id } = useParams();
  const navigate = useNavigate();
  const isCreating = id === undefined;

  const [order, setOrder] = useState(null);
  const [employees, setEmployees] = useState([]);
  const [orderType, setOrderType] = useState("DineIn");
  const [tableNumber, setTableNumber] = useState("");
  const [deliveryEmployeeId, setDeliveryEmployeeId] = useState("");
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(!isCreating);

  useEffect(() => {
    if (isCreating) {
      return;
    }

    const loadOrder = async () => {
      try {
        const loadedOrder = await getOrderById(id);
        setOrder(loadedOrder);
        setDeliveryEmployeeId(loadedOrder.deliveryEmployeeId ?? "");
      } catch {
        setError("Failed to load order.");
      } finally {
        setIsLoading(false);
      }
    };

    loadOrder();
  }, [id, isCreating]);

  useEffect(() => {
    const needsEmployeeList = isCreating || order?.orderType === "Delivery";
    if (!needsEmployeeList) {
      return;
    }

    const loadEmployees = async () => {
      try {
        const loadedEmployees = await getEmployees();
        setEmployees(loadedEmployees);
      } catch {
        setError("Failed to load employees.");
      }
    };

    loadEmployees();
  }, [isCreating, order]);

  const handleCreate = async (event) => {
    event.preventDefault();
    setError("");

    if (orderType === "DineIn" && !tableNumber) {
      setError("Table number is required for dine-in orders.");
      return;
    }

    try {
      const newOrder = await createOrder({
        orderType,
        tableNumber: orderType === "DineIn" ? Number(tableNumber) : null,
        employeeId: currentEmployee.id,
      });
      navigate(`/orders/${newOrder.id}`);
    } catch {
      setError("Failed to create order.");
    }
  };

  const handleAssignDeliveryEmployee = async (event) => {
    event.preventDefault();
    setError("");

    try {
      const updatedOrder = await updateOrder(order.id, { deliveryEmployeeId });
      setOrder(updatedOrder);
    } catch {
      setError("Failed to assign delivery employee.");
    }
  };

  const handleRemovePizza = async (pizzaId) => {
    setError("");

    try {
      await deletePizza(pizzaId);
      const updatedOrder = await getOrderById(order.id);
      setOrder(updatedOrder);
    } catch {
      setError("Failed to remove pizza.");
    }
  };

  const handleCancelOrder = async () => {
    setError("");

    try {
      await cancelOrder(order.id);
      navigate("/");
    } catch {
      setError("Failed to cancel order.");
    }
  };

  if (isCreating) {
    return (
      <Container style={{ maxWidth: "500px" }} className="mt-4">
        <Link to="/">&larr; Back to Order List</Link>
        <h2 className="mb-4 mt-2">New Order</h2>
        <Form onSubmit={handleCreate}>
          {error && <Alert variant="danger">{error}</Alert>}
          <Form.Group className="mb-3" controlId="orderType">
            <Form.Label>Order Type</Form.Label>
            <Form.Select
              value={orderType}
              onChange={(event) => setOrderType(event.target.value)}
            >
              <option value="DineIn">Dine-In</option>
              <option value="Delivery">Delivery</option>
            </Form.Select>
          </Form.Group>
          {orderType === "DineIn" && (
            <Form.Group className="mb-3" controlId="tableNumber">
              <Form.Label>Table Number</Form.Label>
              <Form.Control
                type="number"
                value={tableNumber}
                onChange={(event) => setTableNumber(event.target.value)}
                required
              />
            </Form.Group>
          )}
          <Button type="submit">Create Order</Button>
        </Form>
      </Container>
    );
  }

  if (isLoading) {
    return (
      <Container className="text-center mt-5">
        <Spinner animation="border" />
      </Container>
    );
  }

  if (!order) {
    return (
      <Container className="mt-4">
        <Alert variant="danger">{error || "Order not found."}</Alert>
      </Container>
    );
  }

  return (
    <Container style={{ maxWidth: "600px" }} className="mt-4">
      <Link to="/">&larr; Back to Order List</Link>
      <h2 className="mb-4 mt-2">Order #{order.id}</h2>
      {error && <Alert variant="danger">{error}</Alert>}
      <p>
        <strong>Type:</strong> {order.orderType}
        {order.orderType === "DineIn" && ` (Table ${order.tableNumber})`}
      </p>
      <p>
        <strong>Order Taken By:</strong> {order.employeeName}
      </p>
      <p>
        <strong>Delivery Employee:</strong>{" "}
        {order.deliveryEmployeeName ?? "Not assigned"}
      </p>

      {currentEmployee.role === "Manager" && (
        <Button variant="danger" onClick={handleCancelOrder}>
          Cancel Order
        </Button>
      )}

      <h4 className="mt-4">Pizzas</h4>
      {order.pizzas.length === 0 ? (
        <p className="text-muted">No pizzas on this order yet.</p>
      ) : (
        <ul>
          {order.pizzas.map((pizza) => (
            <li key={pizza.id}>
              {pizza.size.name} - {pizza.cheeseOption.name} -{" "}
              {pizza.sauceOption.name} - ${pizza.price.toFixed(2)}{" "}
              <Link to={`/orders/${order.id}/pizzas/${pizza.id}`}>Edit</Link>{" "}
              <Button
                variant="link"
                className="p-0 align-baseline"
                onClick={() => handleRemovePizza(pizza.id)}
              >
                Remove
              </Button>
            </li>
          ))}
        </ul>
      )}
      <Button as={Link} to={`/orders/${order.id}/pizzas/new`} className="mb-3">
        Add Pizza
      </Button>
      <p>
        <strong>Total:</strong> ${order.total.toFixed(2)}
      </p>

      {order.orderType === "Delivery" && (
        <Form onSubmit={handleAssignDeliveryEmployee} className="mt-4">
          <Form.Group className="mb-3" controlId="deliveryEmployee">
            <Form.Label>Assign Delivery Employee</Form.Label>
            <Form.Select
              value={deliveryEmployeeId}
              onChange={(event) => setDeliveryEmployeeId(event.target.value)}
            >
              <option value="">Select an employee</option>
              {employees.map((employee) => (
                <option key={employee.id} value={employee.id}>
                  {employee.firstName} {employee.lastName}
                </option>
              ))}
            </Form.Select>
          </Form.Group>
          <Button type="submit" disabled={!deliveryEmployeeId}>
            Assign
          </Button>
        </Form>
      )}
    </Container>
  );
}
