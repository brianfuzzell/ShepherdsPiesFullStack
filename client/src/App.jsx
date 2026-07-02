import { useEffect, useState } from "react";
import { Route, Routes } from "react-router-dom";
import Container from "react-bootstrap/Container";
import Spinner from "react-bootstrap/Spinner";
import Button from "react-bootstrap/Button";
import Login from "./components/Login";
import OrderList from "./components/OrderList";
import OrderDetail from "./components/OrderDetail";
import { getProfile, logout } from "./managers/authManager";

function App() {
  const [currentEmployee, setCurrentEmployee] = useState(null);
  const [isCheckingSession, setIsCheckingSession] = useState(true);

  useEffect(() => {
    getProfile()
      .then(setCurrentEmployee)
      .finally(() => setIsCheckingSession(false));
  }, []);

  const handleLogout = async () => {
    await logout();
    setCurrentEmployee(null);
  };

  if (isCheckingSession) {
    return (
      <Container className="text-center mt-5">
        <Spinner animation="border" />
      </Container>
    );
  }

  if (!currentEmployee) {
    return <Login onLogin={setCurrentEmployee} />;
  }

  return (
    <Container className="mt-4">
      <div className="d-flex justify-content-between align-items-center">
        <h1>Shepherd's Pies</h1>
        <div className="text-end">
          <p className="mb-1">
            Logged in as {currentEmployee.firstName} {currentEmployee.lastName}{" "}
            ({currentEmployee.role})
          </p>
          <Button variant="secondary" onClick={handleLogout}>
            Log Out
          </Button>
        </div>
      </div>
      <Routes>
        <Route path="/" element={<OrderList />} />
        <Route
          path="/orders/new"
          element={<OrderDetail currentEmployee={currentEmployee} />}
        />
        <Route
          path="/orders/:id"
          element={<OrderDetail currentEmployee={currentEmployee} />}
        />
      </Routes>
    </Container>
  );
}

export default App;