import { apiFetch } from "./apiClient";

export const addPizza = async (orderId, pizza) => {
  const response = await apiFetch(`/orders/${orderId}/pizzas`, {
    method: "POST",
    body: JSON.stringify(pizza),
  });

  if (!response.ok) {
    throw new Error("Failed to add pizza");
  }

  return response.json();
};

export const updatePizza = async (id, pizza) => {
  const response = await apiFetch(`/pizzas/${id}`, {
    method: "PUT",
    body: JSON.stringify(pizza),
  });

  if (!response.ok) {
    throw new Error("Failed to update pizza");
  }

  return response.json();
};

export const deletePizza = async (id) => {
  const response = await apiFetch(`/pizzas/${id}`, {
    method: "DELETE",
  });

  if (!response.ok) {
    throw new Error("Failed to delete pizza");
  }
};