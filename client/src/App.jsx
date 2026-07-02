import { useEffect, useState } from "react";
import Container from "react-bootstrap/Container";
import Spinner from "react-bootstrap/Spinner";
import Button from "react-bootstrap/Button";
import Login from "./components/Login";
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
      <h1>Shepherd's Pies</h1>
      <p>
        Logged in as {currentEmployee.firstName} {currentEmployee.lastName} (
        {currentEmployee.role})
      </p>
      <Button variant="secondary" onClick={handleLogout}>
        Log Out
      </Button>
    </Container>
  );
}

export default App;