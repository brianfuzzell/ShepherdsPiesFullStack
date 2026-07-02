import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import Container from "react-bootstrap/Container";
import Form from "react-bootstrap/Form";
import Button from "react-bootstrap/Button";
import Alert from "react-bootstrap/Alert";
import Spinner from "react-bootstrap/Spinner";
import { getSizes } from "../managers/sizesManager";
import { getCheeseOptions } from "../managers/cheeseOptionsManager";
import { getSauceOptions } from "../managers/sauceOptionsManager";
import { getToppings } from "../managers/toppingsManager";
import { addPizza, updatePizza } from "../managers/pizzasManager";
import { getOrderById } from "../managers/ordersManager";

export default function PizzaBuilder() {
  const { orderId, pizzaId } = useParams();
  const navigate = useNavigate();
  const isCreating = pizzaId === undefined;

  const [sizes, setSizes] = useState([]);
  const [cheeseOptions, setCheeseOptions] = useState([]);
  const [sauceOptions, setSauceOptions] = useState([]);
  const [toppings, setToppings] = useState([]);

  const [sizeId, setSizeId] = useState("");
  const [cheeseOptionId, setCheeseOptionId] = useState("");
  const [sauceOptionId, setSauceOptionId] = useState("");
  const [toppingIds, setToppingIds] = useState([]);

  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const loadBuilder = async () => {
      try {
        const [loadedSizes, loadedCheeseOptions, loadedSauceOptions, loadedToppings] =
          await Promise.all([
            getSizes(),
            getCheeseOptions(),
            getSauceOptions(),
            getToppings(),
          ]);
        setSizes(loadedSizes);
        setCheeseOptions(loadedCheeseOptions);
        setSauceOptions(loadedSauceOptions);
        setToppings(loadedToppings);

        if (!isCreating) {
          const order = await getOrderById(orderId);
          const existingPizza = order.pizzas.find(
            (pizza) => pizza.id === Number(pizzaId),
          );
          setSizeId(existingPizza.size.id);
          setCheeseOptionId(existingPizza.cheeseOption.id);
          setSauceOptionId(existingPizza.sauceOption.id);
          setToppingIds(existingPizza.toppings.map((topping) => topping.id));
        }
      } catch {
        setError("Failed to load pizza builder.");
      } finally {
        setIsLoading(false);
      }
    };

    loadBuilder();
  }, [orderId, pizzaId, isCreating]);

  const toggleTopping = (toppingId) => {
    setToppingIds((current) =>
      current.includes(toppingId)
        ? current.filter((id) => id !== toppingId)
        : [...current, toppingId],
    );
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setError("");

    const pizza = {
      sizeId: Number(sizeId),
      cheeseOptionId: Number(cheeseOptionId),
      sauceOptionId: Number(sauceOptionId),
      toppingIds,
    };

    try {
      if (isCreating) {
        await addPizza(orderId, pizza);
      } else {
        await updatePizza(pizzaId, pizza);
      }
      navigate(`/orders/${orderId}`);
    } catch {
      setError("Failed to save pizza.");
    }
  };

  if (isLoading) {
    return (
      <Container className="text-center mt-5">
        <Spinner animation="border" />
      </Container>
    );
  }

  return (
    <Container style={{ maxWidth: "500px" }} className="mt-4">
      <h2 className="mb-4">{isCreating ? "Add Pizza" : "Edit Pizza"}</h2>
      {error && <Alert variant="danger">{error}</Alert>}
      <Form onSubmit={handleSubmit}>
        <Form.Group className="mb-3" controlId="size">
          <Form.Label>Size</Form.Label>
          <Form.Select
            value={sizeId}
            onChange={(event) => setSizeId(event.target.value)}
            required
          >
            <option value="">Select a size</option>
            {sizes.map((size) => (
              <option key={size.id} value={size.id}>
                {size.name} - ${size.price.toFixed(2)}
              </option>
            ))}
          </Form.Select>
        </Form.Group>

        <Form.Group className="mb-3" controlId="cheeseOption">
          <Form.Label>Cheese</Form.Label>
          <Form.Select
            value={cheeseOptionId}
            onChange={(event) => setCheeseOptionId(event.target.value)}
            required
          >
            <option value="">Select a cheese</option>
            {cheeseOptions.map((cheeseOption) => (
              <option key={cheeseOption.id} value={cheeseOption.id}>
                {cheeseOption.name}
              </option>
            ))}
          </Form.Select>
        </Form.Group>

        <Form.Group className="mb-3" controlId="sauceOption">
          <Form.Label>Sauce</Form.Label>
          <Form.Select
            value={sauceOptionId}
            onChange={(event) => setSauceOptionId(event.target.value)}
            required
          >
            <option value="">Select a sauce</option>
            {sauceOptions.map((sauceOption) => (
              <option key={sauceOption.id} value={sauceOption.id}>
                {sauceOption.name}
              </option>
            ))}
          </Form.Select>
        </Form.Group>

        <Form.Group className="mb-3">
          <Form.Label>Toppings ($0.50 each)</Form.Label>
          {toppings.map((topping) => (
            <Form.Check
              key={topping.id}
              type="checkbox"
              id={`topping-${topping.id}`}
              label={topping.name}
              checked={toppingIds.includes(topping.id)}
              onChange={() => toggleTopping(topping.id)}
            />
          ))}
        </Form.Group>

        <Button type="submit">{isCreating ? "Add Pizza" : "Save Changes"}</Button>
      </Form>
    </Container>
  );
}