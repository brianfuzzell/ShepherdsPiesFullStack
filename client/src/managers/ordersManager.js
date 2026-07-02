import { apiFetch } from "./apiClient";

export const getOrders = async (date) => {
  const query = date ? `?date=${date}` : "";
  const response = await apiFetch(`/orders${query}`);

  if (!response.ok) {
    throw new Error("Failed to load orders");
  }

  return response.json();
};

export const getOrderById = async (id) => {
  const response = await apiFetch(`/orders/${id}`);

  if (!response.ok) {
    throw new Error("Failed to load order");
  }

  return response.json();
};

export const createOrder = async (order) => {
  const response = await apiFetch("/orders", {
    method: "POST",
    body: JSON.stringify(order),
  });

  if (!response.ok) {
    throw new Error("Failed to create order");
  }

  return response.json();
};

export const updateOrder = async (id, order) => {
  const response = await apiFetch(`/orders/${id}`, {
    method: "PUT",
    body: JSON.stringify(order),
  });

  if (!response.ok) {
    throw new Error("Failed to update order");
  }

  return response.json();
};