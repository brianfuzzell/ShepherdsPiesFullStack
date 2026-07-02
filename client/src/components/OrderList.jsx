import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Container from "react-bootstrap/Container";
import Table from "react-bootstrap/Table";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import Alert from "react-bootstrap/Alert";
import { getOrders } from "../managers/ordersManager";

const todayIsoDate = () => new Date().toISOString().slice(0, 10);

export default function OrderList() {
  const [date, setDate] = useState(todayIsoDate());
  const [orders, setOrders] = useState([]);
  const [error, setError] = useState("");

  useEffect(() => {
    const loadOrders = async () => {
      setError("");
      try {
        const loadedOrders = await getOrders(date);
        setOrders(loadedOrders);
      } catch {
        setError("Failed to load orders.");
      }
    };

    loadOrders();
  }, [date]);

  return (
    <Container className="mt-4">
      <div className="d-flex justify-content-between align-items-center mb-3">
        <h2>Orders</h2>
        <Button as={Link} to="/orders/new">
          New Order
        </Button>
      </div>

      <Form.Group className="mb-3" style={{ maxWidth: "250px" }}>
        <Form.Label>Date</Form.Label>
        <Form.Control
          type="date"
          value={date}
          onChange={(event) => setDate(event.target.value)}
        />
      </Form.Group>

      {error && <Alert variant="danger">{error}</Alert>}

      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Order #</th>
            <th>Type</th>
            <th>Table</th>
            <th>Order Taker</th>
            <th>Total</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order) => (
            <tr key={order.id}>
              <td>
                <Link to={`/orders/${order.id}`}>{order.id}</Link>
              </td>
              <td>{order.orderType}</td>
              <td>{order.tableNumber ?? "-"}</td>
              <td>{order.employeeName}</td>
              <td>${order.total.toFixed(2)}</td>
            </tr>
          ))}
          {orders.length === 0 && (
            <tr>
              <td colSpan={5} className="text-center text-muted">
                No orders for this date.
              </td>
            </tr>
          )}
        </tbody>
      </Table>
    </Container>
  );
}